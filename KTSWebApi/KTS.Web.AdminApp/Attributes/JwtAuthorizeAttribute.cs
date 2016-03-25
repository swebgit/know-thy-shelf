using KTS.Web.Attributes;
using KTS.Web.Enums;
using KTS.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace KTS.Web.AdminApp.Attributes
{
    public class JwtAuthorizeAttribute : AuthorizeAttribute
    {
        private ITokenProvider tokenProvider { get; set; }

        private string token { get; set; }
        
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Request.Cookies.AllKeys.Any(k => k.Equals("KTS-AuthToken", StringComparison.OrdinalIgnoreCase)) &&
                this.tokenProvider.ValidateToken(httpContext.Request.Cookies["KTS-AuthToken"].Value))
            {
                this.token = httpContext.Request.Cookies["KTS-AuthToken"].Value;
                return true;
            }
            return false;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (!String.IsNullOrEmpty(this.token))
            {
                filterContext.Controller.TempData["jwt"] = this.token;
                var tokenClaimsResult = this.tokenProvider.ParseToken(this.token);
                if (tokenClaimsResult.ResultCode == ResultCode.Ok)
                {
                    filterContext.Controller.TempData["jwt_claims"] = tokenClaimsResult.Data;
                    filterContext.HttpContext.User = new GenericPrincipal(new JwtIdentity(tokenClaimsResult.Data.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "UNKNOWN"), new string[] { });
                }


                if (filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(RequiredClaimsAttribute), false).Any() ||
                    filterContext.ActionDescriptor.GetCustomAttributes(typeof(RequiredClaimsAttribute), false).Any())
                {
                    var requiredClaims = ((filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(RequiredClaimsAttribute), false).FirstOrDefault() as RequiredClaimsAttribute)?.RequiredClaims ?? ActivityClaim.None) |
                                         ((filterContext.ActionDescriptor.GetCustomAttributes(typeof(RequiredClaimsAttribute), false).FirstOrDefault() as RequiredClaimsAttribute)?.RequiredClaims ?? ActivityClaim.None);
                    
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

            HandleUnauthorizedRequest(filterContext);
        }
    }
}