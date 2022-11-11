using Microsoft.EntityFrameworkCore;
using TechBlogCore.RestApi.Data;
using TechBlogCore.RestApi.Entities;

namespace TechBlogCore.RestApi.Services;

public class CategoryRepo : ICategoryRepo
{
    private readonly BlogDbContext context;

    public CategoryRepo(BlogDbContext context)
    {
        this.context = context;
    }
    public IEnumerable<Blog_Category> GetCategories(int size)
    {
        return context.Blog_Categories.Include(c => c.Articles).OrderByDescending(c => c.Articles.Count()).Take(size);
    }
}
