﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ZeiterfassungPierburg
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()

            
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }
        // Don't show the unauthorized access message
        /*
        protected void Application_EndRequest()
        {
            var context = new HttpContextWrapper(Context);
            if (context.Response.StatusCode == 401)
            {
                context.Response.Redirect("~/Shared/Error401");
            }
        }
        */
        
    }
}
