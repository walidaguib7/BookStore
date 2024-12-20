using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.models.media.dtos
{
    public class FileDto
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
    }
}