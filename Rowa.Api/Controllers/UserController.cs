using System;
using System.Web.Http;
using Rowa.Api.Interfaces;
using Rowa.Api.Models;
using System.Net.Http;
using Rowa.Api.Classes;
using Rowa.Api.Entities;

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

        [HttpPost]
        [Route("checkuser")]
        public IHttpActionResult CheckUser([FromBody] UserModel userModel)
        {
            return Ok(CheckUserInDatabase(userModel));
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
            var newUser = new User
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
            var userInfo = new UserInformation
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.EmailAddress,
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

        [HttpPost]
        [Route("loginuser")]
        public IHttpActionResult LoginUser([FromBody] UserModel userModel)
        {
            if (CheckUserInDatabase(userModel))
            {
                return Ok(JwtManager.GenerateToken(userModel.Username));
            }

            throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
        }

        private bool CheckUserInDatabase(UserModel userModel)
        {
            var user = _userRepository.GetUserProfile(userModel.Username, userModel.EmailAddress);

            if (user != null)
            {
                return true;
            }

            return false;
        }
    }
}