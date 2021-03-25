using Bobber.API.Models;
using Bobber.API.Options;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Bobber.API.Repositories
{
    public class SqlRepository : IRepository
    {
        private readonly string _connectionString;

        public SqlRepository(IOptions<DatabaseOptions> options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _connectionString = options?.Value.ConnectionString;
        }

        public async Task<User> GetUserAsync(string email)
        {
            string query = @"SELECT * FROM Users WHERE Email = @email";

            using var connection = new SqlConnection(_connectionString);
            User user = await connection.QueryFirstAsync<User>(query, new { email });

            return user;
        }

        public async Task InsertUserAsync(string email, string passwordHash, string firstName, string lastName)
        {
            string query = @"INSERT INTO Users(PasswordHash, Email, FirstName, LastName) VALUES (@passwordHash, @email, @firstName, @lastName)";

            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(query, new { passwordHash, email, firstName, lastName });
        }
    }
}
