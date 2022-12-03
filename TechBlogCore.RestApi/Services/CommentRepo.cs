using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using TechBlogCore.RestApi.Data;
using TechBlogCore.RestApi.DtoParams;
using TechBlogCore.RestApi.Dtos;
using TechBlogCore.RestApi.Entities;
using TechBlogCore.RestApi.Helpers;

namespace TechBlogCore.RestApi.Services
{
    public class CommentRepo : ICommentRepo
    {
        private readonly BlogDbContext context;

        public CommentRepo(BlogDbContext context)
        {
            this.context = context;
        }

        public Task<PagedList<Blog_Comment>> GetArticleComments(int articleId, CommentDtoParam param)
        {
            var query = context.Blog_Comments.Include(c => c.User).Include(c => c.Children).Where(c => c.ArticleId == articleId && c.State != State.Deleted);
            return PagedList<Blog_Comment>.CreateAsync(query, param.PageNumber, param.PageSize);
        }

        public Task<Blog_Comment> GetComment(int articleId, int commentId)
        {
            return context.Blog_Comments.Include(c => c.User).Include(c => c.Children).FirstOrDefaultAsync(c => c.Id == commentId);
                    
        }
        public async Task<Blog_Comment> CreateComment(Blog_User user, Blog_Article article, Blog_Comment parent, string content)
        {
            var commentCreate = new Blog_Comment
            {
                User = user,
                Article = article,
                Parent = parent,
                Content = content,
                CommentTime = DateTime.Now,
                State = State.Active
            };
            await context.Blog_Comments.AddAsync(commentCreate);
            await context.SaveChangesAsync();
            return commentCreate;
        }

        public async Task<bool> ModifyComment(Blog_Comment comment, string content)
        {
            comment.Content = content;
            comment.ModifyTime = DateTime.Now;
            comment.State = State.Modified;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteComment(Blog_Comment comment)
        {
            comment.State = State.Deleted;
            await context.SaveChangesAsync();
            return true;
        }
    }
}
