using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.CacheMiddleware
{
    public class CacheOpenAttribute: Attribute
    {
        public CacheOpenAttribute() 
        {
            ExpiryTime = 60;
        }

        public CacheOpenAttribute(int expiryTime)
        {
            ExpiryTime = expiryTime;
        }
        /// <summary>
        /// 过期时间 单位:分
        /// </summary>
        public int ExpiryTime { get; set; }
    }
}
