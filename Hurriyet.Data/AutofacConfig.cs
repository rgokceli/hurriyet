using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Hurriyet.Data.Infrastructure;

namespace Hurriyet.Data
{
    public class AutofacConfig
    {
        public static void ConfigureServices(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerLifetimeScope();
            var data = Assembly.Load("Hurriyet.Data");
            builder.RegisterAssemblyTypes(data)
            .Where(t => t.Name.EndsWith("Repository"))
            .AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
