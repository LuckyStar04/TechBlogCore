using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TechBlogCore.RestApi.Dtos;
using TechBlogCore.RestApi.Repositories;
using TechBlogCore.RestApi.Services;

namespace TechBlogCore.RestApi.Controllers;

[ApiController]
[Route("api/tags")]
public class TagController : ControllerBase
{
    private readonly TagService service;

    public TagController(TagService service)
    {
        this.service = service;
    }

    [HttpGet]
    public IActionResult GetCategories(int size)
    {
        var tags = service.GetTags(size);
        return Ok(tags);
    }
}