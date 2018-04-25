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
        [Route("updateprofilepic")]
        [JwtAuthentication]
        public IHttpActionResult UpdateProfilePicture()
        {
            var profilePic = HttpContext.Current.Request.Files["profilePic"];

            var username = _commonMethods.GetUsernameFromToken();

            if (profilePic == null)
            {
                return Ok(HttpStatusCode.NoContent);
            }

            var path = Path.Combine("C:\\ProfilePics", profilePic.FileName);

            profilePic.SaveAs(path);

            AddProfilePicToDatabase(path, username);

            File.Delete(path);

            return Ok();
        }

        [HttpPost]
        [Route("resetpassword")]
        [AllowAnonymous]
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
        [AllowAnonymous]
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
        [AllowAnonymous]
        public IHttpActionResult LoginUser([FromBody] UserModel userModel)
        {
            if (CheckUserForLogin(userModel))
            {
                var currentUser = new CurrentUser
                {
                    Username = userModel.Username,
                    Token = JwtManager.GenerateToken(userModel.Username, _secretRepository.GetSecret())
                };

                UpdateLastLoginDate(userModel.Username);

                return Ok(currentUser);
            }

            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        [HttpGet]
        [Route("getprofilepic")]
        [JwtAuthentication]
        public HttpResponseMessage GetProfilePic()
        {
            var profilePic = _userInformationRepository.GetUserInformation(_userRepository.GetUserId(_commonMethods.GetUsernameFromToken())).ProfilePic;

            var ms = new MemoryStream(profilePic);

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(ms)
            };

            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");

            return response;
        }

        private void AddProfilePicToDatabase(string filePath, string username)
        {
            var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var reader = new BinaryReader(fs);

            var photo = reader.ReadBytes((int)fs.Length);

            reader.Close();
            fs.Close();

            var ui = _userInformationRepository.GetUserInformation(_userRepository.GetUserId(username));
            ui.ProfilePic = photo;

            _userInformationRepository.Update(ui);
        }

        private void UpdateLastLoginDate(string username)
        {
            var user = _userRepository.GetUser(username);

            if (user != null)
            {
                user.LastLogonDate = DateTime.Now;
                _userRepository.Update(user);
            }
        }

        private bool CheckUserForLogin(UserModel userModel)
        {
            var user = _userRepository.LoginUser(userModel.Username, _commonMethods.EncryptPassword(userModel.Password));

            if (user != null)
            {
                return true;
            }

            return false;
        }

        private bool CheckUserInDatabase(UserModel userModel)
        {
            var user = _userRepository.CheckUserForPasswordReset(userModel.Username, userModel.EmailAddress);

            if (user != null)
            {
                return true;
            }

            return false;
        }
    }
}