using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rowa.Api.Models
{
    public class UserProfileModel
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool ReadOnly { get; set; }

        public string Rank { get; set; }

        public bool CanChangeRank { get; set; }
    }
}