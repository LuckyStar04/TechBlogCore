using System.ComponentModel.DataAnnotations;

namespace TechBlogCore.RestApi.Dtos
{
    public class CommentCreateDto
    {
        public int? ParentId { get; set; }
        [Required(ErrorMessage = "{0} 字段是必填的")]
        [MaxLength(1000, ErrorMessage = "{0} 的最大长度为 {1}。")]
        public string Content { get; set; }
        public string ReplyTo { get; set; }
    }

    public class CommentModifyDto
    {
        public string Content { get; set; }
    }

    public class CommentDto
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int ArticleId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Content { get; set; }
        public string ReplyTo { get; set; }
        public IEnumerable<CommentDto> Children { get; set; }
        public DateTime CommentTime { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}
