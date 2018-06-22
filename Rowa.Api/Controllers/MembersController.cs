using Rowa.Api.Filters;
using Rowa.Api.Interfaces;
using Rowa.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Rowa.Api.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    [RoutePrefix("api/members")]
    public class MembersController : ApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserInformationRepository _userInformationRepository;

        public MembersController(IUserRepository userRepository, IUserInformationRepository userInformationRepository)
        {
            _userRepository = userRepository;
            _userInformationRepository = userInformationRepository;
        }

        [HttpGet]
        [Route("getmembers")]
        [JwtAuthentication]
        public IHttpActionResult GetMembers()
        {
            var members = _userInformationRepository.GetMembers();

            return Ok(members);
        }

        [HttpGet]
        [Route("updatememberrank")]
        [JwtAuthentication]
        public IHttpActionResult UpdateMemberRank()
        {
            return Ok();
        }
    }
}