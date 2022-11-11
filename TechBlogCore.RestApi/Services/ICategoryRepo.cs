using TechBlogCore.RestApi.Entities;

namespace TechBlogCore.RestApi.Services;

public interface ICategoryRepo
{
    IEnumerable<Blog_Category> GetCategories(int size);
}
