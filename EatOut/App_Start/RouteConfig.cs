using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Neo4jClient;
namespace EatOut
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("list");
           // routes.IgnoreRoute("search");
            //routes.IgnoreRoute("movie");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Start", id = UrlParameter.Optional }
            );

            var url = ConfigurationManager.AppSettings["GraphDBUrl"];
            var user = ConfigurationManager.AppSettings["GraphDBUser"];
            var password = ConfigurationManager.AppSettings["GraphDBPassword"];
            var client = new GraphClient(new Uri(url), user, password);
            client.Connect();

            GraphClient = client;
        }
        public static IGraphClient GraphClient { get; private set; }
    }
}
