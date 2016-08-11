using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hurriyet.Data.TableConfigurations;

namespace Hurriyet.Data
{
    public class HurriyetDataContext : System.Data.Entity.DbContext
    {
        public HurriyetDataContext():base(System.Configuration.ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString) {

        }
        public virtual int Commit()
        {
            return base.SaveChanges();
        }
        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new NewsConfiguration());
        }
    }
}
