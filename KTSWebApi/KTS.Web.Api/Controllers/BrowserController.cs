using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;

namespace KTS.Web.Api.Controllers
{
    public class BrowserController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Index()
        {
            var response = new HttpResponseMessage();
            response.Content = new StringContent($"<html><body>KTS API v{Assembly.GetExecutingAssembly().GetName().Version.ToString()}</body></html>");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}
