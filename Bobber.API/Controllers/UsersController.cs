using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bobber.API.Models;
using Bobber.API.Repositories;
using Bobber.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bobber.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest request)
        {
            var result = await _userService.AuthenticateUserAsync(request);

            if (result.IsAuthorized)
            {
                return Ok(result);
            }

            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            await _userService.RegisterUserAsync(request);

            return Ok(new { message = "registration successful" });
        }

    }
}