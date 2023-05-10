using TechBlogCore.RestApi.Entities;

namespace TechBlogCore.RestApi.Repositories;

public interface ICategoryRepo
{
    IEnumerable<Blog_Category> GetCategories(int size);
}
