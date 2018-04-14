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

        public UserController(IUserRepository userRepository, ICommonMethods commonMethods)
        {
            _userRepository = userRepository;
            _commonMethods = commonMethods;
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

            return Ok(_userRepository.Add(newUser));
        }
    }
}