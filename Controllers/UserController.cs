using Closetly.DTO;
using Closetly.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Closetly.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //criar usuario
        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDTO user)
        {
            _userService.CreateUser(user);

            return Ok();
        }

        //updateUser ---- Géssica

        [HttpPatch("{id}", Name = "UpdateUser")]
        public IActionResult UpdateUser([FromRoute] Guid id, [FromBody] UpdateUserRequest request)
        {
            string error = _userService.UpdateUser(id, request.Name, request.Phone, request.Email);
            if (error == "error")
            {
                return BadRequest();
            }
            return Ok();
        }


    }
}
