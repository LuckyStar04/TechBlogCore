using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TechBlogCore.RestApi.DtoParams;
using TechBlogCore.RestApi.Dtos;
using TechBlogCore.RestApi.Entities;
using TechBlogCore.RestApi.Helpers;
using TechBlogCore.RestApi.Repositories;

namespace TechBlogCore.RestApi.Services
{
    public class ArticleService
    {
        private readonly IArticleRepo articleRepo;
        private readonly ICategoryRepo categoryRepo;
        private readonly ITagRepo tagRepo;
        private readonly IMapper mapper;
        public ArticleService(IArticleRepo articleRepo,
                             ICategoryRepo categoryRepo,
                             ITagRepo tagRepo,
                             IMapper mapper)
        {
            this.articleRepo = articleRepo;
            this.categoryRepo = categoryRepo;
            this.tagRepo = tagRepo;
            this.mapper = mapper;
        }

        public async Task<PagedList<Blog_Article>> GetArticles([FromQuery] ArticleDtoParam param)
        {
            var articles = await articleRepo.GetArticles(param);
            return articles;
        }

        public async Task<ArticleDetailDto> GetArticle(int id)
        {
            var article = await articleRepo.GetArticle(id);
            if (article != null)
            {
                article.ViewsCount++;
                await articleRepo.SaveChanges();
                article.Comments = article.Comments.Where(c => c.ParentId == null);
            }
            var dto = mapper.Map<ArticleDetailDto>(article);
            return dto;
        }

        public async Task<ArticleDetailDto> CreateArticle(ArticleCreateDto createDto)
        {
            var result = await articleRepo.CreateArticle(createDto);
            var dto = mapper.Map<ArticleDetailDto>(result);
            return dto;
        }

        public async Task<bool> UpdateArticle(int id, ArticleUpdateDto updateDto)
        {
            var entity = await articleRepo.GetArticle(id);
            if (entity == null)
            {
                return false;
            }
            var result = await articleRepo.UpdateArticle(entity, updateDto);
            if (result)
            {
                return true;
            }
            return false;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int:min(1)}")]
        public async Task<bool> DeleteArticle(int id)
        {
            var result = await articleRepo.DeleteArticle(id);
            return result;
        }
    }
}
