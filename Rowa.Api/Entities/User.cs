using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rowa.Api.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime? LastLogonDate { get; set; }

        public bool FirstLogin { get; set; }
    }
}