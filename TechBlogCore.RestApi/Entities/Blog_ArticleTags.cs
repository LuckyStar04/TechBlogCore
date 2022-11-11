using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechBlogCore.RestApi.Entities;
public class Blog_ArticleTags
{
    public int ArticleId { get; set; }
    public int TagId { get; set; }
    public Blog_Article Article { get; set; }
    public Blog_Tag Tag { get; set; }
}