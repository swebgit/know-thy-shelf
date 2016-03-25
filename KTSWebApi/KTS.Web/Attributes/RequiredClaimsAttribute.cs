using KTS.Web.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KTS.Web.Attributes
{
    public class RequiredClaimsAttribute : Attribute
    {
        public ActivityClaim RequiredClaims { get; set; }

        public RequiredClaimsAttribute(ActivityClaim requiredClaims)
        {
            this.RequiredClaims = requiredClaims;
        }
    }
}