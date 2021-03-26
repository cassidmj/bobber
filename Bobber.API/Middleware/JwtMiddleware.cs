using Bobber.API.Options;
using Bobber.API.Repositories;
using Bobber.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobber.API.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AuthenticationOptions _authOptions;

        public JwtMiddleware(RequestDelegate next, IOptions<AuthenticationOptions> appSettings)
        {
            _next = next;
            _authOptions = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IRepository repository)
        {
            if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                string token = authHeader.FirstOrDefault()?.Split(" ").Last();

                if (token != null)
                {
                    await attachUserToContext(context, repository, token);
                }
            }

            await _next(context);
        }

        private async Task attachUserToContext(HttpContext context, IRepository repository, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_authOptions.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                context.Items["User"] = await repository.GetUserAsync(userId);
            }
            catch
            {
                //TODO: log/track exception
            }
        }
    }
}
