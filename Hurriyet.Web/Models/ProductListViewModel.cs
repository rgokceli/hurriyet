using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hurriyet.Model;

namespace Hurriyet.Web.Models
{
    public class NewsListViewModel
    {
        public News[] News{ get; set; }
        public int CurrentPageIndex{ get; set; }

        public bool LastPage { get; set; }

    }
}
