using KTS.Web.Api.Filters;
using System.Web.Http.Filters;

namespace KTS.Web.Api
{
    public class FilterConfig
    {
        public static void RegisterWebApiFilters(HttpFilterCollection filters)
        {
            //filters.Add(new SharedApiKeyFilter());
        }
    }
}