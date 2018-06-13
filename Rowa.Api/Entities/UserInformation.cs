using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rowa.Api.Entities
{
    public class UserInformation
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public byte[] ProfilePic { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}