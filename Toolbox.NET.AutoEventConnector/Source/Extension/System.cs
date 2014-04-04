namespace System
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Toolbox.NET;

    public static class AutoEventConnector
    {
        private static Dictionary<StaticTuple<Type, Type>, List<Tuple<Action<object, object>, Action<object, object>>>> cache = new Dictionary<StaticTuple<Type, Type>, List<Tuple<Action<object, object>, Action<object, object>>>>();

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
                cacheValue.ForEach(x => x.Item1(pub, sub));
            }
        }

        public static void DisconnectFrom(this object sub, object pub)
        {
            var cacheValue = GetCached(pub, sub);
            if (cacheValue != null)
            {
                cacheValue.ForEach(x => x.Item2(pub, sub));
            }
        }

        private static List<Tuple<Action<object, object>, Action<object, object>>> GetCached(object pub, object sub)
        {
            var subType = sub.GetType();
            var subMethods = subType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
            var pubType = pub.GetType();
            var pubEvents = pubType.GetEvents();
            var cacheKey = StaticTuple.Create(pubType, subType);
            List<Tuple<Action<object, object>, Action<object, object>>> cacheValue;
            if (!cache.TryGetValue(cacheKey, out cacheValue))
            {
                cacheValue = new List<Tuple<Action<object, object>, Action<object, object>>>();
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
                                Tuple.Create(
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
    }
}