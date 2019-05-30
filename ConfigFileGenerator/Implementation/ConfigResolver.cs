using ConfigFileGenerator.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Web;

namespace ConfigFileGenerator.Implementation
{
    public class ConfigResolver : IConfigResolver
    {
        private static IConfigResolver configResolver { get; set; } = null;
        private static readonly object objLock = new object();
        private IDictionary<string, object> confObjects { get; set; }

        private ConfigResolver()
        {
            confObjects = new Dictionary<string, object>();
        }

        public static IConfigResolver Instance
        {
            get
            {
                if(configResolver == null)
                {
                    lock (objLock)
                    {
                        configResolver = configResolver ?? new ConfigResolver();
                    }
                }
                return configResolver;
            }
        }
        
        private TEntity LoadConfigFile<TEntity>(string className)
        {
            string jsonStr = string.Empty;
            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"ConfigFiles/Data/{className}.json")))
                jsonStr = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"ConfigFiles/Data/{className}.json"));
            else
                jsonStr = File.ReadAllText(Path.Combine(HttpRuntime.AppDomainAppPath, $"ConfigFiles/Data/{className}.json"));
            TEntity entity = Newtonsoft.Json.JsonConvert.DeserializeObject<TEntity>(jsonStr);
            return entity;
        }

        public TEntity CheckCache<TEntity>()
            where TEntity : class
        {
            string className = typeof(TEntity).Name;
            if (!confObjects.ContainsKey(className))
                confObjects.Add(className, LoadConfigFile<TEntity>(className));
            return (TEntity)confObjects[className];
        }

        public TResult Resolve<TEntity, TResult>(Expression<Func<TEntity, TResult>> member)
            where TEntity : class
        {
            string className = typeof(TEntity).Name;
            TEntity entity = CheckCache<TEntity>();
            return (TResult)member.Compile().DynamicInvoke(entity);
        }

        public TEntity Resolve<TEntity>()
            where TEntity : class
        {
            string className = typeof(TEntity).Name;
            return CheckCache<TEntity>();
        }
    }
}
