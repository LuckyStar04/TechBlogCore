using TechBlogCore.RestApi.DtoParams;
using TechBlogCore.RestApi.Dtos;
using TechBlogCore.RestApi.Entities;
using TechBlogCore.RestApi.Helpers;

namespace TechBlogCore.RestApi.Repositories
{
    public interface IArticleRepo
    {
        Task<PagedList<Blog_Article>> GetArticles(ArticleDtoParam param);
        Task<bool> ArticleExists(int id);
        Task<Blog_Article> GetArticle(int id);
        Task<Blog_Article> CreateArticle(ArticleCreateDto article);
        Task<bool> DeleteArticle(int id);
        Task<bool> UpdateArticle(Blog_Article entity, ArticleUpdateDto article);
        Task<bool> SaveChanges();
    }
}
