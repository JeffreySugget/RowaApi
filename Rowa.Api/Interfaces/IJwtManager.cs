using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rowa.Api.Interfaces
{
    public interface IJwtManager
    {
        string GenerateToken(string username);
    }
}
