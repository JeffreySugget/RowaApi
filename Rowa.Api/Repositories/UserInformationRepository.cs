using Rowa.Api.Entities;
using Rowa.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rowa.Api.Repositories
{
    public class UserInformationRepository : BaseRepository<UserInformation>, IUserInformationRepository
    {
        public UserInformation GetUserInformation(int userId)
        {
            return DatabaseContext.UserInformations.FirstOrDefault(x => x.UserId == userId);
        }
    }
}