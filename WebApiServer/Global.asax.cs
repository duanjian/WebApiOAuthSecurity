using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Security.Cryptography.X509Certificates;
using WebApiServer.Infrastructure;

namespace WebApiServer
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            ResourceServerConfiguration config = new ResourceServerConfiguration
            {
                EncryptionVerificationCertificate = new X509Certificate2(Server.MapPath("~/Certs/localhost.pfx"), "a"),
                IssuerSigningCertificate = new X509Certificate2(Server.MapPath("~/Certs/localhost.cer"))
            };
            GlobalConfiguration.Configuration.MessageHandlers.Add(new OAuth2Handler(config));

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}