using Microsoft.EntityFrameworkCore;
using TechBlogCore.RestApi.Data;
using TechBlogCore.RestApi.Entities;

namespace TechBlogCore.RestApi.Services
{
    public class TagRepo : ITagRepo
    {
        private readonly BlogDbContext context;

        public TagRepo(BlogDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<Blog_Tag> GetTags(int size)
        {
            return context.Blog_Tags.Include(t => t.Articles).Where(t => t.Articles.Count() > 0).OrderByDescending(t => t.Articles.Count()).Take(size);
        }
    }
}
