using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.models.users.dtos
{
    public class UserDto
    {

        public string? email { get; set; }
        public string? First_name { get; set; }
        public string? Last_Name { get; set; }
        public string? Username { get; set; }
        public string? Bio { get; set; }
        public string? Gender { get; set; }
        public int? Age { get; set; }
    }
}