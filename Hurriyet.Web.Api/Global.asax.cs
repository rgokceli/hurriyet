using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Hurriyet.Web.Api.App_Start;

namespace Hurriyet.Web.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Server.MapPath("~/App_Data"));
            AutofacConfig.RegisterServices();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
