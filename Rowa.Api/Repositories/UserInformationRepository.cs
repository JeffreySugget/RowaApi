using Rowa.Api.Entities;
using Rowa.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rowa.Api.Repositories
{
    public class UserInformationRepository : BaseRepository<UserInformation>, IUserInformationRepository
    {
        private readonly IQueries _queries;

        public UserInformationRepository(IQueries queries)
        {
            _queries = queries;
        }

        public UserInformation GetUserInformation(int userId)
        {
            return DatabaseContext.UserInformations.FirstOrDefault(x => x.UserId == userId);
        }

        public IEnumerable<string> GetMembers()
        {
            var listOfString = DatabaseContext.Database.SqlQuery<string>(_queries.GetMembers).ToList();

            return listOfString;
        }
    }
}