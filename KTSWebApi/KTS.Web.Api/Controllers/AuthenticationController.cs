using KTS.Web.Api.Filters;
using KTS.Web.Enums;
using KTS.Web.Interfaces;
using KTS.Web.Objects;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Http;

namespace KTS.Web.Api.Controllers
{
    [RoutePrefix("api/auth")]
    [SharedApiKeyFilter]
    public class AuthenticationController : ApiController
    {
        private IDatabaseClient databaseClient;
        private ITokenProvider tokenProvider;

        public AuthenticationController(IDatabaseClient databaseClient, ITokenProvider tokenProvider)
        {
            this.databaseClient = databaseClient;
            this.tokenProvider = tokenProvider;
        }
        
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> LogOn(Credentials credentials)
        {
            var credentialValidationResult = await this.databaseClient.ValidateCredentialsAsync(credentials);
            if (credentialValidationResult.ResultCode == ResultCode.Ok)
            {
                var token = this.tokenProvider.GetToken(credentials.Username, credentialValidationResult.Data);
                return Ok(token);
            }
            return Unauthorized();
        }

        // TODO: Implement LogOff action
        
    }
}
