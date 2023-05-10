using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Text.Json;
using TechBlogCore.RestApi.DtoParams;
using TechBlogCore.RestApi.Dtos;
using TechBlogCore.RestApi.Services;

namespace TechBlogCore.RestApi.Controllers;

[ApiController]
[Route("api/articles")]
public class ArticleController : ControllerBase
{
    private readonly ArticleService service;
    private readonly IMapper mapper;

    public ArticleController(ArticleService service,
                             IMapper mapper)
	{
        this.service = service;
        this.mapper = mapper;
    }

	[HttpGet]
	public async Task<IActionResult> GetArticles([FromQuery]ArticleDtoParam param)
	{
		var articles = await service.GetArticles(param);
        var articleDtos = mapper.Map<IEnumerable<ArticleListDto>>(articles);

        var paginationMetadata = new
        {
            totalCount = articles.TotalCount,
            pageSize = articles.PageSize,
            currentPage = articles.CurrentPage,
            totalPages = articles.TotalPages
        };

        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata,
            new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }));

        return Ok(articleDtos);
    }

    [HttpGet("{id:int:min(1)}", Name = nameof(GetArticle))]
    public async Task<ActionResult<ArticleDetailDto>> GetArticle(int id)
    {
        var dto = await service.GetArticle(id);
        return Ok(dto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateArticle(ArticleCreateDto createDto)
    {
        var dto = await service.CreateArticle(createDto);
        return CreatedAtRoute(nameof(GetArticle), new { id = dto.Id }, dto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> UpdateArticle(int id, [FromBody]ArticleUpdateDto updateDto)
    {
        var result = await service.UpdateArticle(id, updateDto);
        if (result) return Ok();
        return NotFound();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> DeleteArticle(int id)
    {
        var result = await service.DeleteArticle(id);
        if (result) return Ok();
        return NotFound();
    }
}