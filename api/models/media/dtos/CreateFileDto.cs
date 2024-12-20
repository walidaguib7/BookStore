using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.models.media.dtos
{
    public class CreateFileDto
    {
        public string file { get; set; }
        public string type { get; set; }
    }
}