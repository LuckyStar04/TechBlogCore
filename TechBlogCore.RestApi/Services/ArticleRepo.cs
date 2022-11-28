using Microsoft.EntityFrameworkCore;
using TechBlogCore.RestApi.Data;
using TechBlogCore.RestApi.DtoParams;
using TechBlogCore.RestApi.Dtos;
using TechBlogCore.RestApi.Entities;
using TechBlogCore.RestApi.Helpers;

namespace TechBlogCore.RestApi.Services
{
    public class ArticleRepo : IArticleRepo
    {
        private readonly BlogDbContext context;

        public ArticleRepo(BlogDbContext context)
        {
            this.context = context;
        }

        public Task<PagedList<Blog_Article>> GetArticles(ArticleDtoParam param)
        {
            var query = context.Blog_Articles.Include(a => a.Comments).Include(a => a.Category).Include(a => a.Tags).AsQueryable();
            if (!param.IncludeDeleted)
            {
                query = query.Where(a => a.State != State.Deleted);
            }
            if (!string.IsNullOrEmpty(param.Category))
            {
                query = query.Where(c => c.Category.Name == param.Category);
            }
            if (!string.IsNullOrEmpty(param.Tag))
            {
                query = query.Where(c => c.Tags.Any(t => t.Name == param.Tag));
            }
            if (!string.IsNullOrEmpty(param.Keyword))
            {
                query = query.Where(a => a.Title.Contains(param.Keyword));
            }
            query = query.OrderByDescending(a => a.CreateTime);
            return PagedList<Blog_Article>.CreateAsync(query, param.PageNumber, param.PageSize);
        }

        public Task<Blog_Article> GetArticle(int id)
        {
            return context.Blog_Articles.Include(a => a.Comments).Include(a => a.Category).Include(a => a.Tags).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Blog_Article> CreateArticle(ArticleCreateDto article)
        {
            var newTags = new List<Blog_Tag>(article.Tags?.Count() ?? 0);
            if (article.Tags != null)
            {
                article.Tags = article.Tags.DistinctBy(t => t.ToLower());
                foreach (var tag in article.Tags)
                {
                    newTags.Add(await context.Blog_Tags.FirstOrDefaultAsync(t => t.Key == tag.ToLower()) ?? new Blog_Tag { Key = tag.ToLower(), Name = tag });
                }
            }
            var articleCreate = new Blog_Article
            {
                Title = article.Title,
                Content = article.Content,
                Category = await context.Blog_Categories.FirstOrDefaultAsync(c => c.Name == article.Category) ?? new Blog_Category { Name = article.Category, CreateTime = DateTime.Now },
                State = article.State,
                CreateTime = DateTime.Now,
                ModifyTime = DateTime.Now,
                Tags = newTags,
            };
            await context.Blog_Articles.AddAsync(articleCreate);
            await context.SaveChangesAsync();
            return articleCreate;
        }

        public async Task<bool> DeleteArticle(int id)
        {
            var article = context.Blog_Articles.FirstOrDefault(t => t.Id == id);
            if (article == null)
            {
                return false;
            }
            article.State = State.Deleted;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateArticle(Blog_Article entity, ArticleUpdateDto article)
        {
            entity.Title = article.Title;
            entity.State = article.State;
            entity.Content = article.Content;
            entity.ModifyTime = DateTime.Now;
            entity.Category = await context.Blog_Categories.FirstOrDefaultAsync(c => c.Name == article.Category) ?? new Blog_Category { Name = article.Category, CreateTime = DateTime.Now };
            var newTags = new List<Blog_Tag>(article.Tags?.Count() ?? 0);
            if (article.Tags != null)
            {
                article.Tags = article.Tags.DistinctBy(t => t.ToLower());
                foreach (var tag in article.Tags)
                {
                    newTags.Add(await context.Blog_Tags.FirstOrDefaultAsync(t => t.Key == tag.ToLower()) ?? new Blog_Tag { Key = tag.ToLower(), Name = tag });
                }
            }
            entity.Tags = newTags;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SaveChanges()
        {
            return (await context.SaveChangesAsync()) > 0;
        }
    }
}
