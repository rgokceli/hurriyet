using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hurriyet.Model;

namespace Hurriyet.Data.TableConfigurations
{
    public class NewsConfiguration : EntityTypeConfiguration<News>
    {
        public NewsConfiguration()
        {
            // entitiy mappingler buradan yapılabilir.
            ToTable("News", "dbo");

        }
    }
}
