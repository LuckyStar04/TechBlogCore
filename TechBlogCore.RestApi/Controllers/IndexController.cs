using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Text.Json;
using TechBlogCore.RestApi.DtoParams;
using TechBlogCore.RestApi.Dtos;
using TechBlogCore.RestApi.Repositories;

namespace TechBlogCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly IArticleRepo articleRepo;
        private readonly ICategoryRepo categoryRepo;
        private readonly ITagRepo tagRepo;
        private readonly IMapper mapper;
        public IndexController(IArticleRepo articleRepo,
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
        public async Task<IActionResult> Index()
        {
            var articles = await articleRepo.GetArticles(new ArticleDtoParam());
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

            var categories = categoryRepo.GetCategories(30);
            var categoryDtos = mapper.Map<IEnumerable<CategoryDto>>(categories);
            var tags = tagRepo.GetTags(30);
            var tagDtos = mapper.Map<IEnumerable<TagDto>>(tags);
            return Ok(new
            {
                Articles = articleDtos,
                Categories = categoryDtos,
                Tags = tagDtos
            });
        }
    }
}
