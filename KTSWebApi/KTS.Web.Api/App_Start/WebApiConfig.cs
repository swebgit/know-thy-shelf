using Autofac;
using Autofac.Integration.WebApi;
using KTS.Web.Interfaces;
using KTS.Web.Search.Algolia;
using KTS.Web.Data.DynamoDb;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Routing;
using KTS.Web.Api.Providers;
using KTS.Web.Api.Filters;
using KTS.Web.Api.Controllers;

namespace KTS.Web.Api
{
    public static class WebApiConfig
    {
        public static IContainer Container { get; set; }

        public static void Register(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            // Web API configuration and services

            builder.RegisterWebApiFilterProvider(config);
            builder.Register(c => new TokenClaimsFilter(c.Resolve<ITokenProvider>()))
                   .AsWebApiActionFilterFor<ClaimsEnabledController>()
                   .InstancePerLifetimeScope();

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            
            builder.RegisterType<DynamoDbClient>().As<IDatabaseClient>();
            builder.RegisterType<AlgoliaSearchClient>().As<ISearchClient>();
            builder.RegisterType<JwtProvider>().As<ITokenProvider>();

            builder.RegisterType<TokenClaimsFilter>().PropertiesAutowired();

            // Set the dependency resolver to be Autofac.
            Container = builder.Build();
            
            config.DependencyResolver = new AutofacWebApiDependencyResolver(Container);

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
