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
using TechBlogCore.RestApi.Repositories;
using TechBlogCore.RestApi.Services;

namespace TechBlogCore.RestApi.Controllers
{
    [Route("api/articles/{articleId:int:min(1)}/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly CommentService service;
        private readonly IMapper mapper;
        private readonly UserManager<Blog_User> userManager;

        public CommentController(CommentService service, IMapper mapper, UserManager<Blog_User> userManager)
        {
            this.service = service;
            this.mapper = mapper;
            this.userManager = userManager;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments(int articleId, [FromQuery]CommentDtoParam param)
        {
            var comments = await service.GetComments(articleId, param);
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
            var dto = await service.GetComment(articleId, commentId);
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
            var commentDto = await service.CreateComment(user, articleId, dto);

            return CreatedAtRoute(nameof(GetComment), new { articleId, commentId = commentDto.Id }, commentDto);
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
            var result = await service.ModifyComment(User, user, articleId, commentId, dto);
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
            var result = await service.DeleteComment(User, user, articleId, commentId);
            if (result)
            {
                return Ok();
            }
            return BadRequest("删除失败");
        }
    }
}
