using System;
using System.Collections.Generic;
using Quarks.ObjectExtensions;

namespace Data.Operations
{
	public class CacheInfo : ICacheInfo
	{
		protected internal CacheInfo(string cacheKeyPrefix)
		{
			CacheKeyPrefix = cacheKeyPrefix;
			AbsoluteDuration = TimeSpan.Zero;
			CacheNulls = true;
		}

		string _cacheKeyPrefix;
		public string CacheKeyPrefix
		{
			get { return _cacheKeyPrefix; }
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					throw new Exception("CacheKeyPrefix cannot be null or whitespace");
				_cacheKeyPrefix = value;
				_cacheKey = null;
			}
		}

		string _cacheKey;
		public virtual string CacheKey
		{
			get { return _cacheKey ?? (_cacheKey = buildCacheKey()); }
		}

		string buildCacheKey()
		{
			var segments = new List<object> { CacheKeyPrefix };

			if (VaryBy == null)
				return string.Join("_", segments);

			segments.AddRange(VaryBy.Flatten());

			return string.Join("_", segments);
		}

		public TimeSpan AbsoluteDuration { get; set; }

		object _varyBy;
		public virtual object VaryBy
		{
			get { return _varyBy; }
			set
			{
				_varyBy = value;
				_cacheKey = null;
			}
		}

		public bool CacheNulls { get; set; }

		public bool Disabled
		{
			get { return AbsoluteDuration == TimeSpan.Zero; }
		}
	}
}