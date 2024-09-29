using App.Interfaces;
using App.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("save")]
        public async Task<IActionResult> SaveUser([FromBody] UserDto userDto)
        {
            if (string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Role))
            {
                return BadRequest("Invalid data.");
            }

            await _userService.SaveUserAsync(userDto.Email, userDto.Role);
            return Ok("User saved successfully.");
        }
    }
}
