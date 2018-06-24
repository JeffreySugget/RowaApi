using Rowa.Api.Helpers;
using Rowa.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Rowa.Api.Security
{
    public class RestrictedAccessAuthoriseAttribute : AuthorizeAttribute
    {
        private readonly Rank _rank;

        public RestrictedAccessAuthoriseAttribute(Rank rank)
        {
            _rank = rank;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var userRank = string.Empty;
            using (var context = new DatabaseContext())
            {
                var email = GetEmailFromToken();
                var userId = context.Users.FirstOrDefault(x => x.Email == email).Id;

                userRank = context.UserInformations.FirstOrDefault(x => x.UserId == userId).Rank;
            }

            var result = UserIsCorrectRankForPage(userRank);

            if (!result)
            {
                throw new UnauthorizedAccessException();
            }
        }

        private bool UserIsCorrectRankForPage(string userRank)
        {
            switch (_rank)
            {
                case Rank.President:
                    return UserIsOfRank(userRank, "President");
                case Rank.RoadCaptain:
                    return UserIsOfRank(userRank, "Road Captain");
                default:
                    return false;
            }
        }

        private bool UserIsOfRank(string userRank, string requiredRank)
        {
            return userRank == requiredRank;
        }

        // this method is a duplicate of the one in common methods
        // will be like this until I can work out how to use common methods here
        private string GetEmailFromToken()
        {
            var token = HttpContext.Current.Request.Headers["Authorization"];
            token = token.Replace("Bearer ", "");

            var jwtToken = new JwtSecurityToken(token);
            return jwtToken.Payload["unique_name"].ToString();
        }
    }
}