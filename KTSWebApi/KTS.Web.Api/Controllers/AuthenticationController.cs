using KTS.Web.Interfaces;
using KTS.Web.Objects;
using System.Threading.Tasks;
using System.Web.Http;

namespace KTS.Web.Api.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthenticationController : ApiController
    {
        private IDatabaseClient databaseClient;

        public AuthenticationController(IDatabaseClient databaseClient)
        {
            this.databaseClient = databaseClient;
        }
        
        // TODO: Implement LogOn action

        // TODO: Implement LogOff action
    }
}
