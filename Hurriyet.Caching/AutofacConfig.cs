using Autofac;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hurriyet.Caching
{
    public class AutofacConfig
    {
        public static void ConfigureServices(ContainerBuilder container)
        {

            if (ConfigurationManager.AppSettings["cacheType"] == "noSql")
            {
                container.Register<IRedisClientsManager>(c =>
    new RedisManagerPool(ConfigurationManager.AppSettings["redisConnection"])).SingleInstance();
                container.RegisterType<NewsCacheManager>().As<INewsCacheManager>().InstancePerLifetimeScope();
            }
            else if (ConfigurationManager.AppSettings["cacheType"] == "rabbit")
            {
                container.RegisterType<NewsCacheManagerWithRabbitClient>().As<INewsCacheManager>().SingleInstance();
            }
            else
                throw new Exception("web.config cache type bulunamadı.. AppSettings[\"cacheType\"] ");
           
            
        }
    }
}
