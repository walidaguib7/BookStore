using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.models.authors.dtos
{
    public class AuthorsQuery
    {
        public string? First_name { get; set; } = null;

        public string? SortBy { get; set; } = null;

        public bool IsDescending { get; set; } = false;

        public int PageNumber { get; set; } = 1;

        public int Limit { get; set; } = 20;
    }
}