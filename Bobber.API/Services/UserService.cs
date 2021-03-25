using BCrypt.Net;
using Bobber.API.Models;
using Bobber.API.Options;
using Bobber.API.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Bobber.API.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository _repo;
        private readonly string _authSecret;

        public UserService(IRepository repository, IOptions<AuthenticationOptions> options)
        {
            _repo = repository ?? throw new ArgumentNullException(nameof(repository));

            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _authSecret = options?.Value.Secret;
        }

        public async Task<AuthenticationResult> AuthenticateUserAsync(AuthenticateRequest authenticateRequest)
        {
            User user = await _repo.GetUserAsync(authenticateRequest.Email);
            bool isAuthenticated = BCrypt.Net.BCrypt.Verify(authenticateRequest.Password, user.PasswordHash);

            string jwt = "";
            if (isAuthenticated)
            {
                jwt = generateJwtToken(user);
            }

            return new AuthenticationResult { IsAuthorized = isAuthenticated, Jwt = jwt };
        }

        public async Task RegisterUserAsync(RegisterRequest registerRequest)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);

            await _repo.InsertUserAsync(registerRequest.Email, passwordHash, registerRequest.FirstName, registerRequest.LastName);
        }

        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
