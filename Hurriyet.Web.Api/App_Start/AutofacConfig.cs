using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Hurriyet.Caching;

namespace Hurriyet.Web.Api.App_Start
{
    public class AutofacConfig
    {
        public static void RegisterServices()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // referans porojelere autofac moduller eklenip buradan moduller dinamik olarak cagrilabilir. şimdilik manual fakat zaman kalırsa autofac module ceviricem.
            Hurriyet.Data.AutofacConfig.ConfigureServices(builder);
            Hurriyet.Caching.AutofacConfig.ConfigureServices(builder);

            var container = builder.Build();
            var config = GlobalConfiguration.Configuration;
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            INewsCacheManager cacheManager = container.Resolve<INewsCacheManager>();
            cacheManager.InitCache();
        }
    }
}
