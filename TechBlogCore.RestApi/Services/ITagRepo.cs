using TechBlogCore.RestApi.Entities;

namespace TechBlogCore.RestApi.Services;

public interface ITagRepo
{
    IEnumerable<Blog_Tag> GetTags(int size);
}
