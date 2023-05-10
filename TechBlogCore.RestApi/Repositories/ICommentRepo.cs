using TechBlogCore.RestApi.DtoParams;
using TechBlogCore.RestApi.Dtos;
using TechBlogCore.RestApi.Entities;
using TechBlogCore.RestApi.Helpers;

namespace TechBlogCore.RestApi.Repositories
{
    public interface ICommentRepo
    {
        Task<PagedList<Blog_Comment>> GetArticleComments(int articleId, CommentDtoParam param);
        Task<Blog_Comment> GetComment(int articleId, int commentId);
        Task<Blog_Comment> CreateComment(Blog_User user, Blog_Article article, Blog_Comment parent, string content, string replyTo);
        Task<bool> ModifyComment(Blog_Comment comment, string content);
        Task<bool> DeleteComment(Blog_Comment comment);
    }
}
