using TechBlogCore.RestApi.Dtos;
using TechBlogCore.RestApi.Entities;

namespace TechBlogCore.RestApi.Services
{
    public interface IChatRepo
    {
        IEnumerable<Chat_Message> GetMessagesByUserId(string user_id);
        bool AddMessage(MessageCreateDto dto);
    }
}
