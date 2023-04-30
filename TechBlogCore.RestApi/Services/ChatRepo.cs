using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TechBlogCore.RestApi.Data;
using TechBlogCore.RestApi.Dtos;
using TechBlogCore.RestApi.Entities;

namespace TechBlogCore.RestApi.Services
{
    public class ChatRepo : IChatRepo
    {
        private readonly BlogDbContext context;

        public ChatRepo(BlogDbContext context)
        {
            this.context = context;
        }
        public bool AddMessage(MessageCreateDto dto)
        {
            var message = new Chat_Message
            {
                Blog_UserId = dto.Blog_UserId,
                Group = dto.Group,
                IsMe = dto.IsMe,
                Time = DateTime.Now,
                Role = dto.Role ?? "",
                Message = dto.Message
            };
            context.Messages.Add(message);
            return context.SaveChanges() > 0;
        }

        public IEnumerable<Chat_Message> GetMessagesByUserId(string user_id)
        {
            return context.Messages.Where(m => m.Blog_UserId == user_id);
        }
    }
}
