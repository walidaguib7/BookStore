using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models.authors.dtos;
using api.models.authors.mapping;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/authors")]
    [Authorize(Roles = "admin")]
    public class AuthorsController(
        IAuthor _authorsService
    ) : ControllerBase
    {
        private readonly IAuthor authorsService = _authorsService;


        [HttpGet]
        public async Task<IActionResult> GetAuthors([FromQuery] AuthorsQuery query)
        {
            var authors = await authorsService.GetAuthors(query);
            var authorsDto = authors.Select(x => x.ToDto());
            return Ok(authorsDto);
        }

        [HttpGet()]
        [Route("{id:int}")]
        public async Task<IActionResult> GetAuthor([FromRoute] int id)
        {
            var author = await authorsService.GetAuthor(id);
            if (author == null) return NotFound();
            return Ok(author.ToDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorDto dto)
        {
            var author = await authorsService.CreateAuthor(dto);
            return Created();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteAuthor([FromRoute] int id)
        {
            var author = await authorsService.DeleteAuthor(id);
            if (author == null) return NotFound();
            return Ok($"Author - {author.First_name} {author.Last_name} has been deleted!");
        }

        [HttpPatch]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateAuthor([FromRoute] int id, [FromBody] UpdateAuthorDto dto)
        {
            var author = await authorsService.UpdateAuthor(id, dto);
            if (author == null) return NotFound();
            return Ok($"Author - {author.First_name} {author.Last_name} has been updated!");
        }
    }
}