using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace VivesRental.RestApi.Extensions
{
    public static class SecurityTokenExtensions
    {
        public static bool IsJwtWithValidSecurityAlgorithm(this SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken securityToken) &&
                   securityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                       StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
