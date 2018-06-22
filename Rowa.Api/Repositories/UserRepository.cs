using Rowa.Api.Entities;
using Rowa.Api.Interfaces;
using Rowa.Api.Models;
using Rowa.Api.Security;
using System.Linq;

namespace Rowa.Api.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly ISecretRepository _secretRepository;
        private ICommonMethods _commonMethods;

        public UserRepository(ISecretRepository secretRepository, ICommonMethods commonMethods)
        {
            _secretRepository = secretRepository;
            _commonMethods = commonMethods;
        }

        public User GetUser(string email)
        {
            return DatabaseContext.Users.FirstOrDefault(x => x.Email == email);
        }

        public UserProfileModel GetUserProfile(string email)
        {
            var userProfile = DatabaseContext.UserInformations.Where(x => x.User.Email == email)
                .Select(x => new UserProfileModel
                {
                    Email = x.User.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    ReadOnly = false,
                    Rank = x.Rank
                }).FirstOrDefault();

            return userProfile;
        }

        public UserProfileModel GetUserProfile(string firstName, string lastName)
        {
            var userProfile = DatabaseContext.UserInformations
                .Where(x => x.FirstName == firstName && x.LastName == lastName)
                .Select(x => new UserProfileModel
                {
                    Email = x.User.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    ReadOnly = true,
                    Rank = x.Rank
                }).FirstOrDefault();

            return userProfile;
        }

        public CurrentUser LoginUser(string email, string password)
        {
            var encryptedPassword = _commonMethods.EncryptPassword(password);

            var user =  DatabaseContext.UserInformations.Where(x => string.Equals(email, x.User.Email) && string.Equals(encryptedPassword, x.User.Password)).Select(x => new
            {
                x.FirstName,
                x.LastName,
                x.User.Email
            }).FirstOrDefault();

            if (user == null)
            {
                return new CurrentUser();
            }

            return new CurrentUser
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = JwtManager.GenerateToken(user.Email, _secretRepository.GetSecret())
            };
        }

        public int GetUserId(string email)
        {
            return DatabaseContext.Users.FirstOrDefault(x => x.Email == email).Id;
        }

        public int GetUserId(string firstName, string lastName)
        {
            return DatabaseContext.UserInformations.FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName).User.Id;
        }

        public UserProfileModel CheckUserForPasswordReset(string username, string emailAddress)
        {
            var userProfile = DatabaseContext.UserInformations.Where(x => x.User.Email == emailAddress).Select(x => new UserProfileModel
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.User.Email
            }).FirstOrDefault();

            return userProfile;
        }
    }
}