using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hurriyet.Caching
{
    public class CRUDEventArgs<T>
    {
        public CRUDEventArgs()
        {

        }
        public CRUDEventArgs(T args, CrudType crudType)
        {
            Args = args;
            EventType = crudType;
        }
        public T Args{ get; set; }

        public CrudType EventType { get; set; }

    }

    public enum CrudType
    {
        INSERT,
        UPDATE,
        DELETE
    }
}
