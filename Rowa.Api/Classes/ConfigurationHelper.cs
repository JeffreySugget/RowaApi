using Rowa.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Rowa.Api.Classes
{
    public class ConfigurationHelper : IConfigurationHelper
    {
        public string JwtExpireTime => ConfigurationManager.AppSettings["JwtExpireTime"];
    }
}