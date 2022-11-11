using System.ComponentModel.DataAnnotations;
using TechBlogCore.RestApi.Entities;

namespace TechBlogCore.RestApi.Dtos
{
    public class ArticleCreateDto
    {
        [Required(ErrorMessage = "{0} 字段是必填的")]
        public string Title { get; set; }
        [Required(ErrorMessage = "{0} 字段是必填的")]
        public string Content { get; set; }
        [Required(ErrorMessage = "{0} 字段是必填的")]
        public string Category { get; set; }
        [Required(ErrorMessage = "{0} 字段是必填的")]
        public State State { get; set; }
        [Required(ErrorMessage = "{0} 字段是必填的")]
        public IEnumerable<string> Tags { get; set; }
    }

    public class ArticleUpdateDto
    {
        [Required(ErrorMessage = "{0} 字段是必填的")]
        public string Title { get; set; }
        [Required(ErrorMessage = "{0} 字段是必填的")]
        public string Content { get; set; }
        [Required(ErrorMessage = "{0} 字段是必填的")]
        public string Category { get; set; }
        [Required(ErrorMessage = "{0} 字段是必填的")]
        public State State { get; set; }
        [Required(ErrorMessage = "{0} 字段是必填的")]
        public IEnumerable<string> Tags { get; set; }
    }

    public class ArticleDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
        public State State { get; set; }
        public int ViewCount { get; set; }
        public DateTime CreateTime { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<CommentDto> Comments { get; set; }
    }
}
