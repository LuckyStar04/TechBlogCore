using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using TechBlogCore.RestApi.DtoParams;
using TechBlogCore.RestApi.Dtos;
using TechBlogCore.RestApi.Entities;
using TechBlogCore.RestApi.Services;

namespace TechBlogCore.RestApi.Controllers
{
    [Route("api/articles/{articleId:int:min(1)}/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IArticleRepo articleRepo;
        private readonly ICommentRepo commentRepo;
        private readonly UserManager<Blog_User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;

        public CommentController(IArticleRepo articleRepo,
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments(int articleId, [FromQuery]CommentDtoParam param)
        {
            var exists = await articleRepo.ArticleExists(articleId);
            if (!exists)
            {
                return NotFound("文章未找到！");
            }
            var comments = await commentRepo.GetArticleComments(articleId, param);
            var dtos = mapper.Map<IEnumerable<CommentDto>>(comments);
            var paginationMetadata = new
            {
                totalCount = comments.TotalCount,
                pageSize = comments.PageSize,
                currentPage = comments.CurrentPage,
                totalPages = comments.TotalPages
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata,
                new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                }));

            return Ok(dtos);
        }

        [HttpGet("{commentId:int:min(1)}", Name = nameof(GetComment))]
        public async Task<ActionResult<CommentDto>> GetComment(int articleId, int commentId)
        {
            var exists = await articleRepo.ArticleExists(articleId);
            if (!exists)
            {
                return NotFound("文章未找到！");
            }
            var commentEntity = await commentRepo.GetComment(articleId, commentId);
            if (commentEntity == null)
            {
                return NotFound("评论未找到！");
            }
            var dto = mapper.Map<CommentDto>(commentEntity);
            return Ok(dto);
        }

        [Authorize(Roles = "Admin,CommonUser")]
        [HttpPost]
        public async Task<IActionResult> CreateComment(int articleId, CommentCreateDto dto)
        {
            var user = await userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email)?.Value);
            if (user == null)
            {
                return Unauthorized("请登录");
            }
            var articleEntity = await articleRepo.GetArticle(articleId);
            if (articleEntity == null)
            {
                return NotFound("文章未找到！");
            }
            Blog_Comment parent = null;
            if (dto.ParentId != null && dto.ParentId > 0)
            {
                parent = await commentRepo.GetComment(articleId, dto.ParentId.Value);
                if (parent == null)
                {
                    return NotFound("父评论未找到！");
                }
            }
            var commentCreate = await commentRepo.CreateComment(user, articleEntity, parent, dto.Content, dto.ReplyTo);
            var commentDto = mapper.Map<CommentDto>(commentCreate);
            return CreatedAtRoute(nameof(GetComment), new { articleId, commentId = commentCreate.Id }, commentDto);
        }

        [Authorize(Roles = "Admin,CommonUser")]
        [HttpPut("{commentId:int:min(1)}")]
        public async Task<IActionResult> ModifyComment(int articleId, int commentId, [FromBody] CommentModifyDto dto)
        {
            var user = await userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email)?.Value);
            if (user == null)
            {
                return Unauthorized("请登录");
            }
            var articleEntity = await articleRepo.GetArticle(articleId);
            if (articleEntity == null)
            {
                return NotFound("文章未找到！");
            }
            var comment = await commentRepo.GetComment(articleId, commentId);
            if (comment == null)
            {
                return NotFound("评论未找到！");
            }
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role != "Admin" && comment.Blog_UserId != user.Id)
            {
                return Unauthorized("不能修改他人评论");
            }

            var result = await commentRepo.ModifyComment(comment, dto.Content);
            if (result)
            {
                return Ok("修改成功");
            }
            return BadRequest("修改失败");
        }

        [Authorize(Roles = "Admin,CommonUser")]
        [HttpDelete("{commentId:int:min(1)}")]
        public async Task<IActionResult> DeleteComment(int articleId, int commentId)
        {
            var user = await userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email)?.Value);
            if (user == null)
            {
                return Unauthorized("用户未找到");
            }
            var articleEntity = await articleRepo.GetArticle(articleId);
            if (articleEntity == null)
            {
                return NotFound("文章未找到！");
            }
            var comment = await commentRepo.GetComment(articleId, commentId);
            if (comment == null)
            {
                return NotFound("评论未找到！");
            }
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role != "Admin" && comment.Blog_UserId != user.Id)
            {
                return Unauthorized("不能删除他人评论！");
            }

            var result = await commentRepo.DeleteComment(comment);
            if (result)
            {
                return Ok();
            }
            return BadRequest("删除失败");
        }
    }
}
