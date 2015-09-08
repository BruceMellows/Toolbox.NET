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

	public static class AutoPropertyCopy
	{
		private static Dictionary<StaticTuple<Type, Type>, List<SinglePropertyCopy>> cache = new Dictionary<StaticTuple<Type, Type>, List<SinglePropertyCopy>>();

		internal static int CacheCount
		{
			get
			{
				return cache.Count;
			}
		}

		public static void CopyProperties(this object src, object tgt)
		{
			if (src != null && tgt != null)
			{
				GetCachedCopier(src, tgt).CopyProperties(src, tgt);
			}
		}

		private static void CopyProperties(this IEnumerable<SinglePropertyCopy> copier, object src, object tgt)
		{
			if (copier != null)
			{
				foreach (var singleCopy in copier)
				{
					singleCopy.CopyProperty(src, tgt);
				}
			}
		}

		private static List<SinglePropertyCopy> GetCachedCopier(object src, object tgt)
		{
			var srcType = src.GetType();
			var tgtType = tgt.GetType();

			var cacheKey = StaticTuple.Create(srcType, tgtType);
			List<SinglePropertyCopy> cacheValue;
			if (!cache.TryGetValue(cacheKey, out cacheValue))
			{
				var srcProperties = GetProperties(srcType, x => x.GetGetMethod()).ToArray();
				var tgtProperties = GetProperties(tgtType, x => x.GetSetMethod()).ToArray();
				var copiedProperties = srcProperties.Select(x => x.Name).Intersect(tgtProperties.Select(x => x.Name)).ToArray();

				cacheValue = copiedProperties
					.Select(x => CreatePropertyCopier(srcProperties, tgtProperties, x))
					.Where(x => x != null)
					.ToList();

				if (!cache.ContainsKey(cacheKey))
				{
					var newCache = cache.ToDictionary(x => x.Key, x => x.Value);
					newCache[cacheKey] = cacheValue;
					cache = newCache;
				}
			}

			return cacheValue;
		}

		private static IEnumerable<PropertyInfo> GetProperties(Type type, Func<PropertyInfo, MethodInfo> getTargetMethod)
		{
			return type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x =>
			{
				var targetMethod = getTargetMethod(x);
				return targetMethod != null && targetMethod.IsPublic;
			});
		}

		private static Type GetEffectiveType(PropertyInfo propertyInfo)
		{
			var effectiveType = propertyInfo.PropertyType;

			return (effectiveType.IsGenericType && effectiveType.GetGenericTypeDefinition() == typeof(Nullable<>))
				? effectiveType.GetGenericArguments().Single()
				: effectiveType;
		}

		private static SinglePropertyCopy CreatePropertyCopier(IEnumerable<PropertyInfo> srcProperties, IEnumerable<PropertyInfo> tgtProperties, string copiedProperty)
		{
			var getProperty = srcProperties.Single(x => copiedProperty == x.Name);
			var getMethod = getProperty.GetGetMethod();
			var effectiveGetType = GetEffectiveType(getProperty);

			var setProperty = tgtProperties.Single(x => copiedProperty == x.Name);
			var setMethod = setProperty.GetSetMethod();
			var effectiveSetType = GetEffectiveType(setProperty);

			return effectiveGetType == effectiveSetType
				? new SinglePropertyCopy(new Action<object, object>((s, t) => setMethod.Invoke(t, new object[] { getMethod.Invoke(s, null) })))
				: null;
		}

		private sealed class SinglePropertyCopy
		{
			public SinglePropertyCopy(Action<object, object> copyProperty)
			{
				this.CopyProperty = copyProperty;
			}

			public Action<object, object> CopyProperty { get; private set; }
		}
	}
}