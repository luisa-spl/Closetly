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

        [HttpPatch("{id}", Name = "UpdateUser")]
        public IActionResult UpdateUser([FromRoute] Guid id, [FromBody] UpdateUserRequest request)
        {
            string error = _userService.UpdateUser(id, request.Name, request.Phone, request.Email);

            if(error == "Usuário não encontrado")
            {
                ProblemDetails problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Usuário não existe na base de dados",
                    Detail = "Usuário não foi encontrado para o id informado",
                };
                return BadRequest(problemDetails);
            }

            return Ok();
        }

        [HttpGet("{userId}/orders", Name = "GetUserOrders")]
        public IActionResult GetUserOrders([FromRoute] Guid userId)
        {
            List<OrderDTO>? orders = _userService.GetUserOrders(userId);
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
    }
}