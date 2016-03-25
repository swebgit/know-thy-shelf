using KTS.Web.Enums;
using KTS.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Security.Claims;
using System.Net.Http;
using Autofac.Integration.WebApi;
using KTS.Web.Attributes;

namespace KTS.Web.Api.Filters
{
    public class TokenClaimsFilter : IAutofacActionFilter
    {
        private ITokenProvider tokenProvider { get; set; }

        public TokenClaimsFilter(ITokenProvider tokenProvider)
        {
            this.tokenProvider = tokenProvider;
        }

        public void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            
        }

        public void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<RequiredClaimsAttribute>().Any() ||
                actionContext.ActionDescriptor.GetCustomAttributes<RequiredClaimsAttribute>().Any())
            {
                var requiredClaims = (actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<RequiredClaimsAttribute>().FirstOrDefault()?.RequiredClaims ?? ActivityClaim.None) |
                                     (actionContext.ActionDescriptor.GetCustomAttributes<RequiredClaimsAttribute>().FirstOrDefault()?.RequiredClaims ?? ActivityClaim.None);

                IEnumerable<string> authTokenValues;
                if (actionContext.Request.Headers.TryGetValues("auth-token", out authTokenValues))
                {
                    var authTokenValue = authTokenValues.FirstOrDefault();
                    if (!String.IsNullOrEmpty(authTokenValue))
                    {
                        var tokenClaimsResult = this.tokenProvider.ParseToken(authTokenValue);
                        if (tokenClaimsResult.ResultCode == ResultCode.Ok)
                        {
                            foreach (var claim in tokenClaimsResult.Data.Where(c => c.Type == ClaimTypes.UserData))
                            {
                                ActivityClaim activityClaim;
                                if (Enum.TryParse<ActivityClaim>(claim.Value, out activityClaim) && ((activityClaim & requiredClaims) > 0))
                                {
                                    return;
                                }
                            }
                        }
                    }
                }
                actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Forbidden);
            }
        }
    }
}