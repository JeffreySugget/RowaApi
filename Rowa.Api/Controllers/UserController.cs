using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Rowa.Repository.Interfaces;
using Rowa.Api.Models;
using System.Net.Http;

namespace Rowa.Api.Controllers
{
    [RoutePrefix("api/user")]
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
        [Route("checkuser")]
        public IHttpActionResult CheckUser(string userName, string emailAddress)
        {
            var user = _userRepository.GetUserProfile(userName, emailAddress);

            if (user != null)
            {
                return Ok(true);
            }

            return Ok(false);
        }

        [HttpPost]
        [Route("resetpassword")]
        public IHttpActionResult ResetPassword([FromBody] UserModel userModel)
        {
            var user = _userRepository.GetUser(userModel.Username);

            user.Password = _commonMethods.EncryptPassword(userModel.Password);

            try
            {
                _userRepository.Update(user);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"Error updating password: {ex.Message}")
                });
            }

            return Ok();
        }

        [HttpPost]
        [Route("createuser")]
        public IHttpActionResult CreateUser([FromBody] UserModel userModel)
        {
            var newUser = new Repository.Models.User
            {
                Username = userModel.Username,
                Password = _commonMethods.EncryptPassword(userModel.Password)
            };

            try
            {
                _userRepository.Add(newUser);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"Error creating user: {ex.Message}")
                });
            }
            var userInfo = new Repository.Models.UserInformation
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                EmailAddress = userModel.EmailAddress,
                UserId = newUser.Id
            };

            try
            {
                _userInformationRepository.Add(userInfo);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"Error adding user information: {ex.Message}")
                });
            }

            return Ok("Created user");
        }
    }
}