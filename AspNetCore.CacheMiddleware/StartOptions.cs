using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.CacheMiddleware
{
    public class StartOptions
    {
        private static StartOptions _instance = null;
        public static StartOptions Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new StartOptions();
                return _instance;
            }
        }
        public string NamespaceName { get; set; } = "AspNetCore.CacheMiddleware";



        public string CacheAllKey { get; set; } = "1024.1024.1024";
    }
}
