using ConfigFileGenerator.Contract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

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
            TEntity entity = Newtonsoft.Json.JsonConvert.DeserializeObject<TEntity>(System.IO.File.ReadAllText($"ConfigFiles/Data/{className}.json"));
            return entity;
        }

        public TResult Resolve<TEntity, TResult>(Expression<Func<TEntity, TResult>> member)
        {
            string className = typeof(TEntity).Name;
            if (!confObjects.ContainsKey(className))
                confObjects.Add(className, LoadConfigFile<TEntity>(className));
            return (TResult)member.Compile().DynamicInvoke(confObjects[className]);
        }
    }
}
