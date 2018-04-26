using Rowa.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rowa.Api.Repositories
{
    public class Queries : IQueries
    {
        public string GetUserProfile => @"SELECT Username
                                                ,Email
                                                ,FirstName
                                                ,LastName
                                        FROM [User] u
                                        INNER JOIN UserInformation ui on u.Id = ui.UserId
                                        WHERE Username=@Username";
    }
}