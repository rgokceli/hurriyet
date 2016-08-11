using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hurriyet.Data.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        HurriyetDataContext Get();
    }
}
