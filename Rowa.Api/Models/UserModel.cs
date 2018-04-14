using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rowa.Api.Models
{
    public class UserModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }
    }
}