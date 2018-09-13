using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using Db4objects.Db4o.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace AgentMarket
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            string databasePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~\ObjectDatabase\support.db4o");
            using (IObjectContainer container = Db4oEmbedded.OpenFile(databasePath))
            {
                var users = from ChatUser user in container select user;
                foreach (ChatUser user in users)
                    container.Delete(user);
                var conversations = from Conversation c in container where c.FinishDate == null select c;
                foreach (Conversation c in conversations)
                    container.Delete(c);
            }
        }
    }
}
