using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TechBlogCore.RestApi.Dtos;
using TechBlogCore.RestApi.Repositories;
using TechBlogCore.RestApi.Services;

namespace TechBlogCore.RestApi.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly CategoryService service;

    public CategoryController(CategoryService service)
    {
        this.service = service;
    }

    [HttpGet]
    public IActionResult GetCategories(int size)
    {
        var categoryDtos = service.GetCategories(size);
        return Ok(categoryDtos);
    }
}