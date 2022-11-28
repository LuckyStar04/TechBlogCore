using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TechBlogCore.RestApi.Dtos;
using TechBlogCore.RestApi.Services;

namespace TechBlogCore.RestApi.Controllers;

[ApiController]
[Route("api/tags")]
public class TagController : ControllerBase
{
    private readonly ITagRepo tagRepo;
    private readonly IMapper mapper;

    public TagController(ITagRepo tagRepo, IMapper mapper)
    {
        this.tagRepo = tagRepo;
        this.mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetCategories(int size)
    {
        var tags = tagRepo.GetTags(size);
        var tagDtos = mapper.Map<IEnumerable<TagDto>>(tags);
        return Ok(tagDtos);
    }
}