using Microsoft.AspNetCore.Mvc;
using SignalR_Chat.Dtos;
using SignalR_Chat.Services;

namespace SignalR_Chat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ChatService _chatService;
        public ChatController(ChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("register-user")]
        public IActionResult RegisterUser(UserDto model)
        {
            if(_chatService.AddUser(model.Name)) return NoContent();
            return BadRequest("User already exists");
        }
    }
}
