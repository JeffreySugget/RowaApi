﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rowa.Api.Models
{
    public class CurrentUser
    {
        public string Username { get; set; }

        public string Token { get; set; }
    }
}