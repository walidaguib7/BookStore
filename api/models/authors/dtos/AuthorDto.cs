using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.models.authors.dtos
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Biography { get; set; }
        public DateOnly Date_of_birth { get; set; }
        public string Country { get; set; }
    }
}