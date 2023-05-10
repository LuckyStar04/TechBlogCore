using Microsoft.EntityFrameworkCore;
using TechBlogCore.RestApi.Data;
using TechBlogCore.RestApi.Entities;

namespace TechBlogCore.RestApi.Repositories;

public class CategoryRepo : ICategoryRepo
{
    private readonly BlogDbContext context;

    public CategoryRepo(BlogDbContext context)
    {
        this.context = context;
    }
    public IEnumerable<Blog_Category> GetCategories(int size)
    {
        return context.Blog_Categories.Include(c => c.Articles).Where(t => t.Articles.Count() > 0).OrderByDescending(c => c.Articles.Count()).Take(size);
    }
}
