using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Rowa.Repository.Interfaces;
using Rowa.Api.Models;

namespace Rowa.Api.Controllers
{
    public class UserController : ApiController
    {
        private IUserRepository _userRepository;
        private ICommonMethods _commonMethods;
        private IUserInformationRepository _userInformationRepository;

        public UserController(IUserRepository userRepository, ICommonMethods commonMethods, IUserInformationRepository userInformationRepository)
        {
            _userRepository = userRepository;
            _commonMethods = commonMethods;
            _userInformationRepository = userInformationRepository;
        }

        [HttpGet]
        public IHttpActionResult GetUser()
        {
            return Ok("This will get a user");
        }

        [HttpPost]
        public IHttpActionResult CreateUser([FromBody] UserModel userModel)
        {
            var newUser = new Repository.Models.User
            {
                Username = userModel.Username,
                Password = _commonMethods.EncryptPassword(userModel.Password)
            };

            _userRepository.Add(newUser);

            var userInfo = new Repository.Models.UserInformation
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                EmailAddress = userModel.EmailAddress,
                UserId = newUser.Id
            };

            _userInformationRepository.Add(userInfo);

            return Ok("Created user");
        }
    }
}