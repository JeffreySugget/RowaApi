using System;
using System.Web.Http;
using Rowa.Api.Interfaces;
using Rowa.Api.Models;
using System.Net.Http;
using Rowa.Api.Classes;
using Rowa.Api.Entities;
using Rowa.Api.Filters;
using System.Web.Http.Cors;
using System.Web;
using System.Net;
using System.IO;
using System.IdentityModel.Tokens.Jwt;

namespace Rowa.Api.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private IUserRepository _userRepository;
        private ICommonMethods _commonMethods;
        private IUserInformationRepository _userInformationRepository;
        private ISecretRepository _secretRepository;

        public UserController(IUserRepository userRepository, ICommonMethods commonMethods, IUserInformationRepository userInformationRepository, ISecretRepository secretRepository)
        {
            _userRepository = userRepository;
            _commonMethods = commonMethods;
            _userInformationRepository = userInformationRepository;
            _secretRepository = secretRepository;
        }

        [HttpPost]
        [Route("checkuser")]
        [AllowAnonymous]
        public IHttpActionResult CheckUser([FromBody] UserModel userModel)
        {
            return Ok(CheckUserInDatabase(userModel));
        }

        [HttpPost]
        [Route("resetpassword")]
        [AllowAnonymous]
        public IHttpActionResult ResetPassword([FromBody] UserModel userModel)
        {
            var user = _userRepository.GetUser(userModel.EmailAddress);

            user.Password = _commonMethods.EncryptPassword(userModel.Password);

            try
            {
                _userRepository.Update(user);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"Error updating password: {ex.Message}")
                });
            }

            return Ok();
        }

        [HttpPost]
        [Route("createuser")]
        [AllowAnonymous]
        public IHttpActionResult CreateUser([FromBody] UserModel userModel)
        {
            var newUser = new User
            {
                Email = userModel.EmailAddress,
                Password = _commonMethods.EncryptPassword(userModel.Password)
            };

            try
            {
                _userRepository.Add(newUser);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"Error creating user: {ex.Message}")
                });
            }
            var userInfo = new UserInformation
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                UserId = newUser.Id
            };

            try
            {
                _userInformationRepository.Add(userInfo);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"Error adding user information: {ex.Message}")
                });
            }

            return Ok("Created user");
        }

        [HttpPost]
        [Route("loginuser")]
        [AllowAnonymous]
        public IHttpActionResult LoginUser([FromBody] UserModel userModel)
        {
            if (CheckUserForLogin(userModel))
            {
                var currentUser = new CurrentUser
                {
                    FirstName = userModel.FirstName,
                    Token = JwtManager.GenerateToken(userModel.EmailAddress, _secretRepository.GetSecret())
                };

                UpdateLastLoginDate(userModel.EmailAddress);

                return Ok(currentUser);
            }

            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        private void UpdateLastLoginDate(string email)
        {
            var user = _userRepository.GetUser(email);

            if (user != null)
            {
                user.LastLogonDate = DateTime.Now;
                _userRepository.Update(user);
            }
        }

        private bool CheckUserForLogin(UserModel userModel)
        {
            var user = _userRepository.LoginUser(userModel.EmailAddress, _commonMethods.EncryptPassword(userModel.Password));

            if (user != null)
            {
                return true;
            }

            return false;
        }

        private bool CheckUserInDatabase(UserModel userModel)
        {
            var user = _userRepository.CheckUserForPasswordReset(userModel.EmailAddress, userModel.EmailAddress);

            if (user != null)
            {
                return true;
            }

            return false;
        }
    }
}