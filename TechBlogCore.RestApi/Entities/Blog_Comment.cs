using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechBlogCore.RestApi.Entities;
public class Blog_Comment
{
    [Key]
    public int Id { get; set; }
    [Required, StringLength(95)]
    [Column(TypeName = "VARCHAR(95)")]
    public string Blog_UserId { get; set; }
    public Blog_User User { get; set; }
    [StringLength(191)]
    [Column(TypeName = "VARCHAR(191)")]
    public string ReplyTo { get; set; }
    [Required]
    public int ArticleId { get; set; }
    public Blog_Article Article { get; set; }
    public int? ParentId { get; set; }
    public Blog_Comment Parent { get; set; }
    public IEnumerable<Blog_Comment> Children { get; set; }
    [Required]
    [Column(TypeName = "VARCHAR(1000)")]
    public string Content { get; set; }
    [Required]
    public DateTime CommentTime { get; set; }
    public DateTime? ModifyTime { get; set; }
    [Required]
    public State State { get; set; }
}

public class Blog_RoleComment : Blog_Comment
{
    public string Role { get; set; }
}