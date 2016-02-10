using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace KTS.Web.Api.Filters
{
    public class SharedApiKeyFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            IEnumerable<string> apiKeyHeaderValues;
            if (actionContext.Request.Headers.TryGetValues("api-key", out apiKeyHeaderValues))
            {
                var apiKeyValue = apiKeyHeaderValues.FirstOrDefault();
                if (!String.IsNullOrEmpty(apiKeyValue) && apiKeyValue == "jws229")
                {
                    return;
                }
            }
            actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Forbidden);
        }
    }
}