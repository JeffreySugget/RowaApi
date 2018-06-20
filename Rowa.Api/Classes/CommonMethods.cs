using Rowa.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Rowa.Api.Classes
{
    public class CommonMethods : ICommonMethods
    {
        public string EncryptPassword(string password)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            byte[] inArray = HashAlgorithm.Create("SHA1").ComputeHash(bytes);
            return Convert.ToBase64String(inArray);
        }

        public string GetEmailFromToken()
        {
            var token = HttpContext.Current.Request.Headers["Authorization"];
            token = token.Replace("Bearer ", "");

            var jwtToken = new JwtSecurityToken(token);
            return jwtToken.Payload["unique_name"].ToString();
        }
    }
}