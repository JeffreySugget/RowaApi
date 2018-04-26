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
    [RoutePrefix("api/profile")]
    public class ProfileController : ApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserInformationRepository _userInformationRepository;
        private readonly ICommonMethods _commonMethods;

        public ProfileController(IUserRepository userRepository, IUserInformationRepository userInformationRepository, ICommonMethods commonMethods)
        {
            _userInformationRepository = userInformationRepository;
            _userRepository = userRepository;
            _commonMethods = commonMethods;
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

        [HttpGet]
        [Route("getuserprofile")]
        [JwtAuthentication]
        public IHttpActionResult GetUserProfile()
        {
            var username = _commonMethods.GetUsernameFromToken();

            return Ok(_userRepository.GetUserProfile(username));
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
    }
}