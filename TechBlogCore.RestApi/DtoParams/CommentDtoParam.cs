using System.ComponentModel.DataAnnotations;

namespace TechBlogCore.RestApi.DtoParams;

public class CommentDtoParam
{
    public int? ParentId { get; set; }
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;
}
