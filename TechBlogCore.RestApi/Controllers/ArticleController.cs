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
	private readonly IArticleRepo articleRepo;
    private readonly ICategoryRepo categoryRepo;
    private readonly ITagRepo tagRepo;
    private readonly IMapper mapper;

	public ArticleController(IArticleRepo articleRepo,
                             ICategoryRepo categoryRepo,
                             ITagRepo tagRepo,
							 IMapper mapper)
	{
		this.articleRepo = articleRepo;
        this.categoryRepo = categoryRepo;
        this.tagRepo = tagRepo;
        this.mapper = mapper;
	}

	[HttpGet]
	public async Task<IActionResult> GetArticles([FromQuery]ArticleDtoParam param)
	{
		var articles = await articleRepo.GetArticles(param);
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
        var article = await articleRepo.GetArticle(id);
        var dto = mapper.Map<ArticleDetailDto>(article);
        return Ok(dto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateArticle(ArticleCreateDto createDto)
    {
        var result = await articleRepo.CreateArticle(createDto);
        var dto = mapper.Map<ArticleDetailDto>(result);
        return CreatedAtRoute(nameof(GetArticle), new { id = result.Id }, dto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> UpdateArticle(int id, [FromBody]ArticleUpdateDto updateDto)
    {
        var entity = await articleRepo.GetArticle(id);
        if (entity == null)
        {
            return NotFound();
        }
        var result = await articleRepo.UpdateArticle(entity, updateDto);
        if (result)
        {
            return Ok();
        }
        return BadRequest();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> DeleteArticle(int id)
    {
        var result = await articleRepo.DeleteArticle(id);
        if (result)
        {
            return Ok();
        }
        return BadRequest();
    }
}