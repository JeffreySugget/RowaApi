using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rowa.Api.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime? LastLogonDate { get; set; }
    }
}