namespace TechBlogCore.RestApi.DtoParams;

public class ArticleDtoParam
{
    public int PageSize { get; set; } = 30;
    public int PageNumber { get; set; } = 1;
    public string Category { get; set; }
    public string Tag { get; set; }
    public string Keyword { get; set; }
    public bool IncludeDeleted { get; set; } = false;
}
