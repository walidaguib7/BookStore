using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.utils;

namespace api.models.users.dtos
{
    public class RegisterDto
    {
        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public Roles role { get; set; }
    }
}