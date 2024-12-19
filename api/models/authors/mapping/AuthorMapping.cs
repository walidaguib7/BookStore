using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models.authors.dtos;

namespace api.models.authors.mapping
{
    public static class AuthorMapping
    {
        public static Author ToModel(this CreateAuthorDto dto)
        {
            return new Author
            {
                First_name = dto.First_name,
                Last_name = dto.Last_name,
                Biography = dto.Biography,
                Date_of_birth = dto.Date_of_birth,
                Country = dto.Country
            };
        }

        public static AuthorDto ToDto(this Author model)
        {
            return new AuthorDto
            {
                Id = model.Id,
                First_name = model.First_name,
                Last_name = model.Last_name,
                Biography = model.Biography,
                Date_of_birth = model.Date_of_birth,
                Country = model.Country
            };
        }
    }
}