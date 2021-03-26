using Bobber.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bobber.API.Repositories
{
    public interface IRepository
    {
        Task InsertUserAsync(string email, string passwordHash, string firstName, string lastName);

        Task<User> GetUserAsync(string email);

        Task<User> GetUserAsync(long id);
    }
}
