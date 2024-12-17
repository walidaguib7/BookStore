using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.models.users.dtos
{
    public class LoginResponse
    {
        public string Id { get; set; }
        public string token { get; set; }
    }
}