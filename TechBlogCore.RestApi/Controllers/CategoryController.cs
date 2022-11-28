using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TechBlogCore.RestApi.Dtos;
using TechBlogCore.RestApi.Services;

namespace TechBlogCore.RestApi.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepo categoryRepo;
    private readonly IMapper mapper;

    public CategoryController(ICategoryRepo categoryRepo, IMapper mapper)
    {
        this.categoryRepo = categoryRepo;
        this.mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetCategories(int size)
    {
        var categories = categoryRepo.GetCategories(size);
        var categoryDtos = mapper.Map<IEnumerable<CategoryDto>>(categories);
        return Ok(categoryDtos);
    }
}