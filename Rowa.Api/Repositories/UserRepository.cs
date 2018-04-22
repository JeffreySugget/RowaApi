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

        public User GetUser(string username)
        {
            return DatabaseContext.Users.FirstOrDefault(x => x.Username == username);
        }

        public UserProfileModel GetUserProfile(string username)
        {
            var userProfile = DatabaseContext.Database.SqlQuery<UserProfileModel>(_queries.GetUserProfile, 
                new SqlParameter("@Username", username)).FirstOrDefault();

            return userProfile;
        }

        public User LoginUser(string username, string password)
        {
            return DatabaseContext.Users.FirstOrDefault(x => string.Equals(username, x.Username) && string.Equals(password, x.Password));
        }

        public int GetUserId(string username)
        {
            return DatabaseContext.Users.FirstOrDefault(x => x.Username == username).Id;
        }
    }
}