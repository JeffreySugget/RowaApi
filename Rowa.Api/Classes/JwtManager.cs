using Microsoft.IdentityModel.Tokens;
using Rowa.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Rowa.Api.Classes
{
    public class JwtManager : IJwtManager
    {
        private readonly IConfigurationHelper _configurationHelper;

        public JwtManager(IConfigurationHelper configurationHelper)
        {
            _configurationHelper = configurationHelper;
        }

        private const string Secret = "";

        public string GenerateToken(string username)
        {
            var symmetricKey = Convert.FromBase64String(Secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.Now.AddMinutes(Convert.ToInt32(_configurationHelper.JwtExpireTime)), 

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var tempToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(tempToken);

            return token;
        }
    }
}