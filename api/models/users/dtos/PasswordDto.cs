using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.models.users.dtos
{
    public class PasswordDto
    {
        [Required]
        [MinLength(12, ErrorMessage = "the password should have at least 12 caracters")]
        public string NewPassword { get; set; }

        [Required]
        public string Token { get; set; }
    }
}