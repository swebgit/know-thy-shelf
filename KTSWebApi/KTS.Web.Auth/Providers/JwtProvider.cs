using KTS.Web.Enums;
using KTS.Web.Interfaces;
using KTS.Web.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.ServiceModel.Security.Tokens;
using System.Web;

namespace KTS.Web.Auth.Providers
{
    public class JwtProvider : ITokenProvider
    {
        private static readonly byte[] TOKEN_SECURITY_KEY = GetBytes(ConfigurationManager.AppSettings["TokenSecurityKey"]);
        
        //http://blog.asteropesystems.com/securing-web-api-requests-with-json-web-tokens/

        public string GetToken(string username, List<ActivityClaim> activityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;
            var claims = new ClaimsIdentity(new[]
                {
                    new Claim( ClaimTypes.UserData, "IsValid", ClaimValueTypes.String ),
                    new Claim( ClaimTypes.Name, username, ClaimValueTypes.String )
                });
            claims.AddClaims(activityClaims.Select(c => new Claim(ClaimTypes.UserData, c.ToString(), ClaimValueTypes.String)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                TokenIssuerName = "self",
                AppliesToAddress = "https://api.knowthyshelf.com",
                Lifetime = new Lifetime(now, now.AddYears(10)),
                SigningCredentials = new SigningCredentials(new InMemorySymmetricSecurityKey(TOKEN_SECURITY_KEY),
                  "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256",
                  "http://www.w3.org/2001/04/xmlenc#sha256"),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public Result<List<Claim>> ParseToken(string token)
        {
            var result = new Result<List<Claim>>();

            if (String.IsNullOrEmpty(token))
                return result;

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters()
            {
                ValidAudience = "https://api.knowthyshelf.com",
                IssuerSigningToken = new BinarySecretSecurityToken(TOKEN_SECURITY_KEY),
                ValidIssuer = "self"
            };

            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
            var isValidClaim = principal.Claims.FirstOrDefault();
            if (isValidClaim?.Value == "IsValid" && securityToken.ValidFrom <= DateTime.UtcNow && securityToken.ValidTo >= DateTime.UtcNow)
            {
                result.ResultCode = Enums.ResultCode.Ok;
                result.Data = principal.Claims.ToList();
            }
            return result;
        }

        public bool ValidateToken(string token)
        {
            if (String.IsNullOrEmpty(token))
                return false;

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters()
            {
                ValidAudience = "https://api.knowthyshelf.com",
                IssuerSigningToken = new BinarySecretSecurityToken(TOKEN_SECURITY_KEY),
                ValidIssuer = "self"
            };

            SecurityToken securityToken;
            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
                var isValidClaim = principal.Claims.FirstOrDefault();
                if (isValidClaim?.Value == "IsValid" && securityToken.ValidFrom <= DateTime.UtcNow && securityToken.ValidTo >= DateTime.UtcNow)
                {
                    return true;
                }
            }
            catch (SecurityTokenExpiredException ex)
            {

            }

            return false;
        }

        private static byte[] GetBytes(string input)
        {
            var bytes = new byte[input.Length * sizeof(char)];
            Buffer.BlockCopy(input.ToCharArray(), 0, bytes, 0, bytes.Length);

            return bytes;
        }
    }
}