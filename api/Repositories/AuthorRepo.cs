using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.models;
using api.models.authors;
using api.models.authors.dtos;
using api.models.authors.mapping;
using api.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace api.Repositories
{
    public class AuthorRepo(
        AppDBContext _context,
        [FromKeyedServices("createAuthor")] IValidator<CreateAuthorDto> _createAuthorValidator,
        [FromKeyedServices("updateAuthor")] IValidator<UpdateAuthorDto> _updateAuthorValidator,
        ICache _cacheService
    ) : IAuthor
    {
        private readonly AppDBContext context = _context;
        private readonly IValidator<CreateAuthorDto> createAuthorValidator = _createAuthorValidator;
        private readonly IValidator<UpdateAuthorDto> updateAuthorValidator = _updateAuthorValidator;
        private readonly ICache cacheService = _cacheService;
        public async Task<Author?> CreateAuthor(CreateAuthorDto dto)
        {
            var result = createAuthorValidator.Validate(dto);
            if (result.IsValid)
            {
                var author = dto.ToModel();
                await context.authors.AddAsync(author);
                await context.SaveChangesAsync();
                await cacheService.RemoveByPattern("authors");
                await cacheService.RemoveByPattern("author");
                return author;
            }
            else
            {
                throw new ValidationException(result.Errors);
            }
        }

        public async Task<Author?> DeleteAuthor(int id)
        {
            var author = await context.authors.Where(x => x.Id == id).FirstAsync();
            if (author == null) return null;
            context.authors.Remove(author);
            await context.SaveChangesAsync();
            await cacheService.RemoveByPattern("authors");
            await cacheService.RemoveByPattern("author");
            return author;
        }

        public async Task<Author?> GetAuthor(int id)
        {
            string key = $"author_{id}";
            var cachedAuthor = await cacheService.GetFromCacheAsync<Author>(key);
            if (cachedAuthor != null) return cachedAuthor;
            var author = await context.authors.FirstOrDefaultAsync(u => u.Id == id);
            if (author == null) return null;
            await cacheService.SetAsync(key, author);
            return author;
        }

        public async Task<List<Author>> GetAuthors(AuthorsQuery query)
        {

            string key = $"authors_${query.First_name}_${query.SortBy}_${query.IsDescending}_${query.PageNumber}_${query.Limit}";
            var cachedAuthors = await cacheService.GetFromCacheAsync<List<Author>>(key);
            if (!cachedAuthors.IsNullOrEmpty()) return cachedAuthors;
            // 
            var authors = context.authors.AsQueryable();
            if (authors.Count() == 0) return await authors.ToListAsync();
            if (!string.IsNullOrEmpty(query.First_name) || !string.IsNullOrWhiteSpace(query.First_name))
            {
                await cacheService.RemoveCaching("authors");
                authors = authors.Where(f => f.First_name.Contains(query.First_name));
            }
            if (!string.IsNullOrEmpty(query.SortBy) || !string.IsNullOrWhiteSpace(query.SortBy))
            {
                await cacheService.RemoveCaching("authors");
                if (query.SortBy.Equals("Id", StringComparison.OrdinalIgnoreCase))
                {
                    authors = query.IsDescending ?
                                 authors.OrderByDescending(f => f.Id)
                                 : authors.OrderBy(f => f.Id);
                }

                if (query.SortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    authors = query.IsDescending ?
                                 authors.OrderByDescending(f => f.First_name)
                                 : authors.OrderBy(f => f.First_name);
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.Limit;
            var pagedauthors = await authors.Skip(skipNumber).Take(query.Limit).ToListAsync();
            await cacheService.SetAsync(key, pagedauthors);
            return pagedauthors;
        }

        public async Task<Author?> UpdateAuthor(int id, UpdateAuthorDto dto)
        {
            var author = await context.authors.FirstOrDefaultAsync(x => x.Id == id);
            if (author == null) return null;
            var result = updateAuthorValidator.Validate(dto);
            if (result.IsValid)
            {
                author.First_name = dto.First_name;
                author.Last_name = dto.Last_name;
                author.Date_of_birth = dto.Date_of_birth;
                author.Country = dto.Country;
                author.Biography = dto.Biography;
                await context.SaveChangesAsync();
                await cacheService.RemoveByPattern("authors");
                await cacheService.RemoveByPattern("author");
                return author;
            }
            else
            {
                throw new ValidationException(result.Errors);
            }

        }
    }
}