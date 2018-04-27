using Rowa.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rowa.Api.Interfaces
{
    public interface IUserInformationRepository : IRepository<UserInformation>
    {
        UserInformation GetUserInformation(int userId);

        IEnumerable<string> GetMembers();
    }
}
