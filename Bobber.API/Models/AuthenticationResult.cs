using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bobber.API.Models
{
    public class AuthenticationResult
    {
        public bool IsAuthorized { get; set; }

        public string Jwt { get; set; }
    }
}
