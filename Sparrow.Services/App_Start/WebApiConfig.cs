using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace Sparrow.Services
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
            config.MapHttpAttributeRoutes();
            config.EnableCors();
            //config.Routes.Clear();
            config.Routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "v1/api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "TrackRoute",
                routeTemplate: "v1/api/{controller}/{id}/Album/{albumid}"
            );
        }
    }
}
