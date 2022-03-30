using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.CacheMiddleware
{
    public interface ICurrentCache
    {

        /// <summary>
        /// 根据Key取对应的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// 添加缓存项
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="data">缓存值</param>
        /// <param name="cacheTime">过期时间（分钟）</param>
        void Set<T>(string key, T data, int cacheTime = 30);

        /// <summary>
        /// 缓存容器中是否存在对应的Key,true:存在,false:不存在
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns></returns>
        bool Contains(string key);

        /// <summary>
        /// 从缓存容器中移除对应的Key值项
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);

    }
}
