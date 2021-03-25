using Bobber.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bobber.API.Services
{
    public interface IUserService
    {
        Task RegisterUserAsync(RegisterRequest registerRequest);

        Task<AuthenticationResult> AuthenticateUserAsync(AuthenticateRequest authenticateRequest);
    }
}
