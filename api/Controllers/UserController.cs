using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models.users.dtos;
using api.Services;
using api.utils;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController(
        IUser _usersService
    ) : ControllerBase
    {
        private readonly IUser usersService = _usersService;


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                var result = await usersService.Register(dto);
                if (result == null) return BadRequest("User Credentials are invalid!");
                return Created();
            }
            catch (ValidationException e)
            {
                return BadRequest(new ValidationErrorResponse { Errors = e.Errors.Select(e => e.ErrorMessage) });

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpPost("SignIn")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var user = await usersService.Login(dto);
                if (user == null) return NotFound();
                return Ok(user);
            }
            catch (ValidationException e)
            {
                return BadRequest(new ValidationErrorResponse { Errors = e.Errors.Select(e => e.ErrorMessage) });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

    }
}