using AutoMapper;
using TechBlogCore.RestApi.Dtos;
using TechBlogCore.RestApi.Repositories;

namespace TechBlogCore.RestApi.Services
{
    public class TagService
    {
        private readonly ITagRepo repo;
        private readonly IMapper mapper;

        public TagService(ITagRepo repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }
        public IEnumerable<TagDto> GetTags(int size)
        {
            var tags = repo.GetTags(size);
            var tagDtos = mapper.Map<IEnumerable<TagDto>>(tags);
            return tagDtos;
        }
    }
}
