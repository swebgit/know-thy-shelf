using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KTS.Web.AdminApp.Attributes
{
    public class JwtOptionalAuthorizeAttribute : JwtAuthorizeAttribute
    {
        public JwtOptionalAuthorizeAttribute()
        {
            this.authorizationRequired = false;
        }
    }
}