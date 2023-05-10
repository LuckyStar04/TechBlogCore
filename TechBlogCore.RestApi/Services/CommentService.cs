using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TechBlogCore.RestApi.DtoParams;
using TechBlogCore.RestApi.Dtos;
using TechBlogCore.RestApi.Entities;
using TechBlogCore.RestApi.Helpers;
using TechBlogCore.RestApi.Repositories;

namespace TechBlogCore.RestApi.Services
{
    public class CommentService
    {
        private readonly IArticleRepo articleRepo;
        private readonly ICommentRepo commentRepo;
        private readonly UserManager<Blog_User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;
        public CommentService(IArticleRepo articleRepo,
                                 ICommentRepo commentRepo,
                                 UserManager<Blog_User> userManager,
                                 RoleManager<IdentityRole> roleManager,
                                 IMapper mapper)
        {
            this.articleRepo = articleRepo;
            this.commentRepo = commentRepo;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mapper = mapper;
        }
        public async Task<PagedList<Blog_Comment>> GetComments(int articleId, CommentDtoParam param)
        {
            var exists = await articleRepo.ArticleExists(articleId);
            if (!exists)
            {
                throw new MessageException("文章未找到！");
            }
            var comments = await commentRepo.GetArticleComments(articleId, param);
            return comments;
        }

        public async Task<CommentDto> GetComment(int articleId, int commentId)
        {
            var exists = await articleRepo.ArticleExists(articleId);
            if (!exists)
            {
                throw new MessageException("文章未找到！");
            }
            var commentEntity = await commentRepo.GetComment(articleId, commentId);
            if (commentEntity == null)
            {
                throw new MessageException("评论未找到！");
            }
            var dto = mapper.Map<CommentDto>(commentEntity);
            return dto;
        }

        public async Task<CommentDto> CreateComment(Blog_User user, int articleId, CommentCreateDto dto)
        {
            var articleEntity = await articleRepo.GetArticle(articleId);
            if (articleEntity == null)
            {
                throw new MessageException("文章未找到！");
            }
            Blog_Comment parent = null;
            if (dto.ParentId != null && dto.ParentId > 0)
            {
                parent = await commentRepo.GetComment(articleId, dto.ParentId.Value);
                if (parent == null)
                {
                    throw new MessageException("父评论未找到！");
                }
            }
            var commentCreate = await commentRepo.CreateComment(user, articleEntity, parent, dto.Content, dto.ReplyTo);
            var commentDto = mapper.Map<CommentDto>(commentCreate);
            return commentDto;
        }

        public async Task<bool> ModifyComment(ClaimsPrincipal User, Blog_User user, int articleId, int commentId, CommentModifyDto dto)
        {
            var articleEntity = await articleRepo.GetArticle(articleId);
            if (articleEntity == null)
            {
                throw new MessageException("文章未找到！");
            }
            var comment = await commentRepo.GetComment(articleId, commentId);
            if (comment == null)
            {
                throw new MessageException("评论未找到！");
            }
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role != "Admin" && comment.Blog_UserId != user.Id)
            {
                throw new MessageException("不能修改他人评论");
            }

            return await commentRepo.ModifyComment(comment, dto.Content);
        }

        public async Task<bool> DeleteComment(ClaimsPrincipal User, Blog_User user, int articleId, int commentId)
        {
            var articleEntity = await articleRepo.GetArticle(articleId);
            if (articleEntity == null)
            {
                throw new MessageException("文章未找到！");
            }
            var comment = await commentRepo.GetComment(articleId, commentId);
            if (comment == null)
            {
                throw new MessageException("评论未找到！");
            }
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role != "Admin" && comment.Blog_UserId != user.Id)
            {
                throw new MessageException("不能删除他人评论！");
            }

            return await commentRepo.DeleteComment(comment);
        }
    }
}
