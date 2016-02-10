using Autofac;
using Autofac.Integration.WebApi;
using KTS.Web.Api.Interfaces;
using KTS.Web.Api.Providers.Algolia;
using KTS.Web.Api.Providers.DynamoDb;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Routing;

namespace KTS.Web.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            // Web API configuration and services

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<DynamoDbClient>().As<IDatabaseClient>();
            builder.RegisterType<AlgoliaSearchClient>().As<ISearchClient>();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // Web API routes
            config.Routes.IgnoreRoute("SslChallenges", ".well-known/acme-challenge/{*pathInfo}");

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
