using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechBlogCore.RestApi.Entities;

[Index(nameof(CategoryId))]
public class Blog_Article
{
    [Key]
    public int Id { get; set; }
    [Required, StringLength(100)]
    [Column(TypeName = "VARCHAR(100)")]
    public string Title { get; set; }
    [Required]
    [Column(TypeName = "MEDIUMTEXT")]
    public string Content { get; set; }
    [Required]
    public int CategoryId { get; set; }
    public Blog_Category Category { get; set; }
    [Required]
    public State State { get; set; }
    [Required]
    public int ViewsCount { get; set; }
    [Required]
    public DateTime CreateTime { get; set; }
    [Required]
    public DateTime ModifyTime { get; set; }
    public IEnumerable<Blog_Tag> Tags { get; set; }
    public IEnumerable<Blog_Comment> Comments { get; set; }
}

public enum State
{
    Deleted = 0,
    Active = 1,
    Modified = 2
}