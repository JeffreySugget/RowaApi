using Rowa.Api.Entities;
using Rowa.Api.Interfaces;
using Rowa.Api.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Rowa.Api.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly IQueries _queries;

        public UserRepository(IQueries queries)
        {
            _queries = queries;
        }

        public User GetUser(string email)
        {
            return DatabaseContext.Users.FirstOrDefault(x => x.Email == email);
        }

        public UserProfileModel GetUserProfile(string username)
        {
            var userProfile = DatabaseContext.Database.SqlQuery<UserProfileModel>(_queries.GetUserProfile, 
                new SqlParameter("@Username", username)).FirstOrDefault();

            return userProfile;
        }

        public User LoginUser(string email, string password)
        {
            return DatabaseContext.Users.FirstOrDefault(x => string.Equals(email, x.Email) && string.Equals(password, x.Password));
        }

        public int GetUserId(string email)
        {
            return DatabaseContext.Users.FirstOrDefault(x => x.Email == email).Id;
        }

        public UserProfileModel CheckUserForPasswordReset(string username, string emailAddress)
        {
            //var userProfile = DatabaseContext.Database.SqlQuery<UserProfileModel>(_queries.GetUserProfile, 
            //    new SqlParameter("@Username", username)).FirstOrDefault();

            var userProfile = DatabaseContext.UserInformations.Where(x => x.User.Email == emailAddress).Select(x => new UserProfileModel
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.User.Email
            }).FirstOrDefault();

            return userProfile;
        }
    }
}