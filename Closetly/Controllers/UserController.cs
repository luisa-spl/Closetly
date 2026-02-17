using Closetly.DTO;
using Closetly.Models;
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

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDTO user)
        {
            var createdUser = _userService.CreateUser(user);
            if(createdUser == null)
            {
                ProblemDetails problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Erro ao criar usuário",
                    Detail = "Não foi possível criar o usuário.",
                };
                return BadRequest(problemDetails);
            }
            return Ok(createdUser);
        }

        [HttpPatch("{id}/update", Name = "UpdateUser")]
        public IActionResult UpdateUser([FromRoute] Guid id, [FromBody] UpdateUserRequest request)
        {
            string error = _userService.UpdateUser(id, request);

            if (error != "")
            {
                ProblemDetails problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Usuário não existe na base de dados",
                    Detail = error,
                };
                return BadRequest(problemDetails);                
            }     
            return NoContent();
        }

        [HttpGet("{userId}/orders", Name = "GetUserOrders")]
        public IActionResult GetUserOrders([FromRoute] Guid userId)
        {
            List<UserOrders>? orders = _userService.GetUserOrders(userId);
            if(orders == null)
            {
                ProblemDetails problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Usuário não existe na base de dados",
                    Detail = "Usuário não foi encontrado para o id informado",
                };
                return BadRequest(problemDetails);
            }
            return Ok(orders);
        }

        [HttpGet("all", Name = "GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetUsers();
            return Ok(users);
        }
        
    }

}