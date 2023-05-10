using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBlogCore.RestApi.Dtos;
using TechBlogCore.RestApi.Services;

namespace TechBlogCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ChatService service;

        public ChatController(ChatService service)
        {
            this.service = service;
        }

        [Authorize(Roles = "Admin,CommonUser")]
        [HttpGet]
        public Task<IEnumerable<ChatDto>> GetChatList()
        {
            return service.GetChatList(User);
        }

        [Authorize(Roles = "Admin,CommonUser")]
        [HttpPost]
        public IActionResult ChatComplete([FromBody] ChatCompleteInputDto dto)
        {
            var result = service.ChatComplete(User, dto);
            return Content(result, "application/json");
        }
    }
}
