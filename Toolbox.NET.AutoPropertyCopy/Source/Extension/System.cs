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
			var cacheValue = GetCached(src, tgt);
			if (cacheValue != null)
			{
				cacheValue.ForEach(x => x.CopyProperty(src, tgt));
			}
		}

		private static List<SinglePropertyCopy> GetCached(object src, object tgt)
		{
			var srcType = src.GetType();
			var tgtType = tgt.GetType();

			var cacheKey = StaticTuple.Create(srcType, tgtType);
			List<SinglePropertyCopy> cacheValue;
			if (!cache.TryGetValue(cacheKey, out cacheValue))
			{
				var srcProperties = srcType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x =>
				{
					var getMethod = x.GetGetMethod();
					return getMethod != null && getMethod.IsPublic;
				}).ToArray();

				var tgtProperties = tgtType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x =>
				{
					var setMethod = x.GetSetMethod();
					return setMethod != null && setMethod.IsPublic;
				}).ToArray();

				var copiedProperties = srcProperties.Select(x => x.Name).Intersect(tgtProperties.Select(x => x.Name)).ToArray();

				cacheValue = new List<SinglePropertyCopy>();
				foreach (var copiedProperty in copiedProperties)
				{
					var getMethod = srcProperties.Single(x => copiedProperty == x.Name).GetGetMethod();
					var setMethod = tgtProperties.Single(x => copiedProperty == x.Name).GetSetMethod();
					cacheValue.Add(new SinglePropertyCopy(new Action<object, object>((s, t) => setMethod.Invoke(t, new object[] { getMethod.Invoke(s, null) }))));
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