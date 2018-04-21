using Rowa.Api.Entities;
using Rowa.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rowa.Api.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUser(string username);

        UserProfileModel GetUserProfile(string username, string password);

        User LoginUser(string username, string password);
    }
}
