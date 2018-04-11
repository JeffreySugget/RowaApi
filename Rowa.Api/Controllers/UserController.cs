using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Rowa.Api.Controllers
{
    public class UserController : ApiController
    {
        public IHttpActionResult GetUser()
        {
            return Ok("This will get a user");
        }
    }
}