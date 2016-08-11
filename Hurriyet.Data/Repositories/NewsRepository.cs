using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hurriyet.Data.Infrastructure;
using Hurriyet.Model;

namespace Hurriyet.Data.Repositories
{
    public class NewsRepository :RepositoryBase<News>,INewsRepository
    {
        public NewsRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) {

        }
    }

    public interface INewsRepository : IRepository<News>
    {

    }
}
