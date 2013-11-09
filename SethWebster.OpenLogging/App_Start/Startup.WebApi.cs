using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Owin; 
 
namespace SethWebster.OpenLogging
{
    public partial class Startup
    {
        public void ConfigureWebApi(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            // Handle circular references for JSON formatting
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
     
            // Web API configuration and services 
            // Configure Web API to use only bearer token authentication. 
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthBearerOptions.AuthenticationType));
            
            // Web API routes 
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            app.UseWebApi(config);
        }
    } 
}