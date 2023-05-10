using TechBlogCore.RestApi.Entities;

namespace TechBlogCore.RestApi.Repositories;

public interface ITagRepo
{
    IEnumerable<Blog_Tag> GetTags(int size);
}
