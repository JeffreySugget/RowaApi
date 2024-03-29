﻿using Rowa.Api.Entities;
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
        User GetUser(string email);

        UserProfileModel GetUserProfile(string username);

        UserProfileModel GetUserProfile(string firstName, string lastName);

        CurrentUser LoginUser(string email, string password);

        int GetUserId(string email);

        int GetUserId(string firstName, string lastName);

        UserProfileModel CheckUserForPasswordReset(string email, string emailAddress);
    }
}
