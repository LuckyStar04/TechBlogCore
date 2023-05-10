using AutoMapper;
using TechBlogCore.RestApi.Dtos;
using TechBlogCore.RestApi.Repositories;

namespace TechBlogCore.RestApi.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepo categoryRepo;
        private readonly IMapper mapper;

        public CategoryService(ICategoryRepo categoryRepo, IMapper mapper)
        {
            this.categoryRepo = categoryRepo;
            this.mapper = mapper;
        }
        public IEnumerable<CategoryDto> GetCategories(int size)
        {
            var categories = categoryRepo.GetCategories(size);
            var categoryDtos = mapper.Map<IEnumerable<CategoryDto>>(categories);
            return categoryDtos;
        }
    }
}
