﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rowa.Api.Models
{
    public class UserProfileModel
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}