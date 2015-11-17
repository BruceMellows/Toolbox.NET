// Copyright (c) 2014, Bruce Mellows
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
// 1. This file must retain the above copyright notice, this list of conditions
//    and the following disclaimer.
// 2. Redistributions of source code must retain the above copyright notice, this
//    list of conditions and the following disclaimer.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

namespace System
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Toolbox.NET;

    public static class AutoEventConnector
    {
        private static Dictionary<StaticTuple<Type, Type>, List<AttachDetach>> cache = new Dictionary<StaticTuple<Type, Type>, List<AttachDetach>>();

        internal static int CacheCount
        {
            get
            {
                return cache.Count;
            }
        }

        public static void ConnectTo(this object sub, object pub)
        {
            var cacheValue = GetCached(pub, sub);
            if (cacheValue != null)
            {
                cacheValue.ForEach(x => x.Attach(pub, sub));
            }
        }

        public static void DisconnectFrom(this object sub, object pub)
        {
            var cacheValue = GetCached(pub, sub);
            if (cacheValue != null)
            {
                cacheValue.ForEach(x => x.Detach(pub, sub));
            }
        }

        private static List<AttachDetach> GetCached(object pub, object sub)
        {
            var subType = sub.GetType();
            var subMethods = subType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
            var pubType = pub.GetType();
            var pubEvents = pubType.GetEvents();
            var cacheKey = StaticTuple.Create(pubType, subType);
            List<AttachDetach> cacheValue;
            if (!cache.TryGetValue(cacheKey, out cacheValue))
            {
                cacheValue = new List<AttachDetach>();
                foreach (var pubEvent in pubEvents)
                {
                    var addMethod = pubEvent.GetAddMethod();
                    var addMethodName = addMethod.Name;
                    var removeMethod = pubEvent.GetRemoveMethod();
                    var removeMethodName = removeMethod.Name;
                    var targetMethodName = "Handle" + pubType.Name + pubEvent.Name;
                    var handlerMethod = subType.GetMethod(targetMethodName, BindingFlags.Instance | BindingFlags.NonPublic);
                    if (addMethod != null
                        && removeMethod != null
                        && handlerMethod != null
                        && handlerMethod.ReturnType == typeof(void))
                    {
                        var delegateParameters = addMethod.GetParameters().Select(x => x.ParameterType.GetMethod("Invoke")).Single(x => x != null).GetParameters();
                        var parameters = handlerMethod.GetParameters();
                        if (parameters.Length == delegateParameters.Length
                            && parameters.Zip(delegateParameters, (a, b) => (a.ParameterType == b.ParameterType) ? a : null).All(x => x != null))
                        {
                            cacheValue.Add(
                                new AttachDetach(
                                new Action<object, object>((p, s) => addMethod.Invoke(p, new object[] { Delegate.CreateDelegate(pubEvent.EventHandlerType, s, handlerMethod) })),
                                new Action<object, object>((p, s) => removeMethod.Invoke(p, new object[] { Delegate.CreateDelegate(pubEvent.EventHandlerType, s, handlerMethod) }))));
                        }
                    }
                }

                if (!cache.ContainsKey(cacheKey))
                {
                    var newCache = cache.ToDictionary(x => x.Key, x => x.Value);
                    newCache[cacheKey] = cacheValue;
                    cache = newCache;
                }
            }

            return cacheValue;
        }

        private sealed class AttachDetach
        {
            public AttachDetach(Action<object, object> attach, Action<object, object> detach)
            {
                this.Attach = attach;
                this.Detach = detach;
            }

            public Action<object, object> Attach { get; private set; }

            public Action<object, object> Detach { get; private set; }
        }
    }
}