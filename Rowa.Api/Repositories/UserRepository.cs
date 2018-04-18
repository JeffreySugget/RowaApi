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
            //var user = DatabaseContext.Database.SqlQuery<User>(_queries.GetUser, new SqlParameter("@Username", username)).FirstOrDefault();

            return DatabaseContext.Users.FirstOrDefault(x => x.Username == username);
        }

        public UserProfileModel GetUserProfile(string username, string email)
        {
            var userProfile = DatabaseContext.Database.SqlQuery<UserProfileModel>(_queries.GetUserProfile, 
                new SqlParameter("@Username", username), 
                new SqlParameter("@Email", email)).FirstOrDefault();

            return userProfile;
        }
    }
}