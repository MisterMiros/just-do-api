using API.Settings;
using Microsoft.IdentityModel.Tokens;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Model.Utils
{
    public static class JwtUtils
    {
        public static string GenerateToken(User user, JwtConfig jwtConfig)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtConfig.Issuer,
                signingCredentials: new SigningCredentials(jwtConfig.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256),
                claims: claims
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
