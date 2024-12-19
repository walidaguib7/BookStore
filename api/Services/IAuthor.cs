using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models.authors;
using api.models.authors.dtos;

namespace api.Services
{
    public interface IAuthor
    {
        public Task<List<Author>> GetAuthors(AuthorsQuery query);
        public Task<Author?> GetAuthor(int id);
        public Task<Author?> CreateAuthor(CreateAuthorDto dto);
        public Task<Author?> UpdateAuthor(int id, UpdateAuthorDto dto);
        public Task<Author?> DeleteAuthor(int id);
    }
}