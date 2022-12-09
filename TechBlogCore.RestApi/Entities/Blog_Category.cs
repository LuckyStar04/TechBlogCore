using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechBlogCore.RestApi.Entities;
[Index(nameof(Name))]
public class Blog_Category
{
    [Key]
    public int Id { get; set; }
    [Required]
    [Column(TypeName = "VARCHAR(20)")]
    public string Name { get; set; }
    [Required]
    public DateTime CreateTime { get; set; }
    public IEnumerable<Blog_Article> Articles { get; set; }
}