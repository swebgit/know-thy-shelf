using KTS.Web.Enums;
using KTS.Web.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KTS.Web.Interfaces
{
    public interface ITokenProvider
    {
        string GetToken(string userName, List<ActivityClaim> activityClaims);
        Result<List<Claim>> ParseToken(string token);
        bool ValidateToken(string token);
    }
}
