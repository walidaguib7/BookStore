using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models.media;
using Microsoft.AspNetCore.Identity;

namespace api.models
{
    public class User : IdentityUser
    {
        public string? First_name { get; set; }
        public string? Last_Name { get; set; }
        public string? Bio { get; set; }
        public string? Gender { get; set; }
        public int? Age { get; set; }
        public int? mediaId { get; set; }
        public Media? media { get; set; }
    }
}