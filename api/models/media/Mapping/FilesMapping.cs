using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models.media.dtos;

namespace api.models.media.Mapping
{
    public static class FilesMapping
    {
        public static Media ToModel(this CreateFileDto dto)
        {
            return new Media
            {
                Path = dto.file,
                Type = dto.type
            };
        }

        public static FileDto ToDto(this Media model)
        {
            return new FileDto
            {
                Id = model.Id,
                Path = model.Path,
                Type = model.Type
            };
        }
    }
}