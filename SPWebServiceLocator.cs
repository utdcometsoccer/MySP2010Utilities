using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using System.Reflection;

namespace MySP2010Utilities
{
    class MyServiceLocatorConfig : IServiceLocatorConfig
    {
        public SPWeb web { get; set; }
        public SPWebPropertyBag propertyBag { get; set; }
        protected int siteCacheInterval { get; set; }
        public DateTime? lastUpdate { get; set; }

        public MyServiceLocatorConfig() { }
        public MyServiceLocatorConfig(SPWeb Web)
        {
            Web.RequireNotNull("Web");
            web = Web;
            propertyBag = new SPWebPropertyBag(web);
        }

        public int GetSiteCacheInterval()
        {
            return siteCacheInterval;
        }

        public List<TypeMapping> GetTypeMappings()
        {
            List<TypeMapping> mappings = new List<TypeMapping>();

            return mappings;
        }

        public DateTime? LastUpdate
        {
            get { return lastUpdate; }
        }

        public void RegisterTypeMapping<TFrom, TTo>(string key) where TTo : TFrom, new()
        {
            updateTime();
            string derivedTypeKey = string.Format("{0}.{1}", typeof(TFrom).AssemblyQualifiedName, key);
            propertyBag[derivedTypeKey] = typeof(TTo).AssemblyQualifiedName;
            propertyBag.Update();
        }

        private void updateTime()
        {
            lastUpdate = DateTime.Now;
        }

        public void RegisterTypeMapping<TFrom, TTo>() where TTo : TFrom, new()
        {
            updateTime();
            propertyBag[typeof(TFrom).AssemblyQualifiedName] = typeof(TTo).AssemblyQualifiedName;
            propertyBag.Update();
        }

        public void RemoveTypeMapping<T>(string key)
        {
            updateTime();
            string derivedTypeKey = createKey<T>(key);
            propertyBag.Remove(derivedTypeKey);
            propertyBag.Update();
        }

        public static string createKey<T>(string key)
        {
            string derivedTypeKey = string.Format("{0}.{1}", typeof(T).AssemblyQualifiedName, key);
            return derivedTypeKey;
        }

        public static string createKey(Type t, string key)
        {
            return string.Format("{0}.{1}", t.AssemblyQualifiedName, key);
        }

        public void RemoveTypeMappings<T>()
        {
            updateTime();
            propertyBag.Remove(typeof(T).AssemblyQualifiedName);
            propertyBag.Update();
        }

        public void SetSiteCacheInterval(int interval)
        {
            siteCacheInterval = interval;
        }

        public SPSite Site
        {
            get
            {
                return web.Site;
            }
            set
            {

            }
        }
    }

    public class SPWebServiceLocator : IServiceLocator
    {
        protected SPWeb web { get; set; }
        protected SPWebPropertyBag propertyBag { get; set; }
        public SPWebServiceLocator(SPWeb Web)
        {
            Web.RequireNotNull("Web");
            web = Web;
            propertyBag = new SPWebPropertyBag(web);

            MyServiceLocatorConfig config = new MyServiceLocatorConfig(web);
            config.RegisterTypeMapping<IServiceLocatorConfig, MyServiceLocatorConfig>();
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public TService GetInstance<TService>(string key)
        {
            string derivedTypeKey = MyServiceLocatorConfig.createKey<TService>(key);
            return getService<TService>(derivedTypeKey);
        }

        private TService getService<TService>(string derivedTypeKey)
        {
            string assemblyQualifiedType = propertyBag[derivedTypeKey];
            Type[] types = new Type[0];
            Type derivedType = Type.GetType(assemblyQualifiedType);
            ConstructorInfo constructorInfo = derivedType.GetConstructor(types);
            TService service = (TService)constructorInfo.Invoke(new object[0]);
            return service;
        }

        private object getService(string derivedTypeKey)
        {
            string assemblyQualifiedType = propertyBag[derivedTypeKey];
            Type[] types = new Type[0];
            Type derivedType = Type.GetType(assemblyQualifiedType);
            ConstructorInfo constructorInfo = derivedType.GetConstructor(types);
            var service = constructorInfo.Invoke(new object[0]);
            handleServiceLocatorConfig<object>(service);
            return service;
        }

        public TService GetInstance<TService>()
        {            
            var service = getService<TService>(typeof(TService).AssemblyQualifiedName);

            handleServiceLocatorConfig<TService>(service);

            return service;
        }

        private void handleServiceLocatorConfig<TService>(TService service)
        {
            MyServiceLocatorConfig config = service as MyServiceLocatorConfig;
            if (null != config)
            {
                config.web = web;
                config.propertyBag = propertyBag;
            }
        }

        public object GetInstance(Type serviceType, string key)
        {
            string derivedTypeKey = MyServiceLocatorConfig.createKey(serviceType, key);
            return getService(derivedTypeKey);
        }

        public object GetInstance(Type serviceType)
        {
            return getService(serviceType.AssemblyQualifiedName);
        }

        public object GetService(Type serviceType)
        {
            return getService(serviceType.AssemblyQualifiedName);
        }
    }
}
