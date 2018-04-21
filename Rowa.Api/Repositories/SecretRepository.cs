using Rowa.Api.Entities;
using Rowa.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rowa.Api.Repositories
{
    public class SecretRepository : BaseRepository<Secret>, ISecretRepository
    {
        public string GetSecret()
        {
            return DatabaseContext.Secrets.FirstOrDefault().Jwt;
        }
    }
}