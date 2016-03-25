using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace KTS.Web.AdminApp.Controllers
{
    public class BaseController : Controller
    {
        protected string Token
        {
            get
            {
                var jwt = this.TempData["jwt"];
                return jwt is string ? jwt as string : null;
            }
        }

        protected List<Claim> UserClaims
        {
            get
            {
                var userClaims = this.TempData["jwt_claims"];
                return userClaims is List<Claim> ? userClaims as List<Claim> : new List<Claim>();
            }
        }

        protected string Username
        {
            get
            {
                return this.User?.Identity?.Name;
            }
        }
    }
}