using System.ComponentModel.DataAnnotations;

namespace TechBlogCore.RestApi.Dtos
{
    public class CommentCreateDto
    {
        [Required]
        public int ArticleId { get; set; }
        public int? ParentId { get; set; }
        [Required, StringLength(1000)]
        public string Content { get; set; }
    }

    public class CommentModifyDto
    {
        public string Content { get; set; }
    }

    public class CommentDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
        public IEnumerable<CommentDto> Children { get; set; }
        public DateTime CommentTime { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}
