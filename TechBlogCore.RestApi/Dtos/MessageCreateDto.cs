using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TechBlogCore.RestApi.Entities;

namespace TechBlogCore.RestApi.Dtos
{
    public class MessageCreateDto
    {
        public string Blog_UserId { get; set; }
        public int Group { get; set; }
        public bool IsMe { get; set; }
        public DateTime Time { get; set; }
        public string Role { get; set; }
        public string Message { get; set; }
    }
}
