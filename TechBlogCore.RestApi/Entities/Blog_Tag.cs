using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechBlogCore.RestApi.Entities;
[Index(nameof(Key))]
public class Blog_Tag
{
    [Key]
    public int Id { get; set; }
    [Required, StringLength(20)]
    [Column(TypeName = "VARCHAR(20)")]
    public string Key { get; set; }
    [Required, StringLength(20)]
    [Column(TypeName = "VARCHAR(20)")]
    public string Name { get; set; }
    public IEnumerable<Blog_Article> Articles { get; set; }
}