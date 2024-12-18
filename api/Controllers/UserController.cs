using api.models.users.dtos;
using api.models.users.Mapping;
using api.Services;
using api.utils;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/Auth")]
    public class UserController(
        IUser _usersService
    ) : ControllerBase
    {
        private readonly IUser usersService = _usersService;



        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetUser([FromRoute] string userId)
        {
            var user = await usersService.getUser(userId);
            if (user == null) return NotFound("user not found!");
            return Ok(user.ToDto());
        }


        [HttpPost("SignUp")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterDto dto)
        {
            try
            {
                var result = await usersService.Register(dto);
                if (result == null) return NotFound();
                return Ok(result);
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



        [HttpPost]
        [Route("GenerateToken/{userId}")]
        public async Task<IActionResult> GenerateToken([FromRoute] string userId)
        {
            try
            {
                var result = await usersService.GenerateResetPasswordToken(userId);
                if (result == null) return NotFound();
                return Ok("Token has been sent, check you inbox!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPatch]
        [Route("ResetPassword/{userId}")]
        public async Task<IActionResult> ResetPassword([FromRoute] string userId, [FromBody] PasswordDto dto)
        {
            try
            {
                var result = await usersService.PasswordReset(userId, dto);
                if (result == null) return NotFound("User Not Found!");
                return Ok("password updated!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPatch("verifyUser")]
        public async Task<IActionResult> verifyUser(string userId, int code)
        {
            var result = await usersService.VerifyUser(userId, code);
            if (result == null) return NotFound("verification code or user are not found!");
            return Ok("User account confirmed!");
        }



        [HttpPatch]
        [Route("update/{userId}")]
        public async Task<IActionResult> UpdateUser([FromRoute] string userId, [FromBody] UpdateUserDto dto)
        {
            try
            {
                var user = await usersService.updateUser(userId, dto);
                if (user == null) return NotFound();
                return Ok("User Updated");
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

        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            await usersService.DeleteAll();
            return Ok("users deleted!");
        }

        [HttpDelete]
        [Route("{userId}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string userId)
        {
            var result = await usersService.DeleteUser(userId);
            if (result == null) return NotFound("user not found!");
            return Ok($"user {result.UserName} is deleted!");
        }

    }
}