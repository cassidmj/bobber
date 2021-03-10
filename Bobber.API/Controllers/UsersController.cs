using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bobber.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bobber.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest request)
        {
            return Ok($"hello: {request.Email}");
        }
    }
}