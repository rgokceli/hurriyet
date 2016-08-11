using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Hurriyet.Web.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}
