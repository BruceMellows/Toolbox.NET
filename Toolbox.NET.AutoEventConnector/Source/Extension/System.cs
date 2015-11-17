﻿// There is no copyright, you can use and abuse this source without limit.
// There is no warranty, you are responsible for the consequences of your use of this source.
// There is no burden, you do not need to acknowledge this source in your use of this source.

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