using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TechBlogCore.RestApi.Dtos;
using TechBlogCore.RestApi.Entities;

namespace TechBlogCore.RestApi.Profiles;

public class BlogProfile : Profile
{
	public BlogProfile()
	{
        CreateMap<Blog_Article, ArticleDetailDto>()
			.ForMember(dest => dest.Category,
					   opt => opt.MapFrom(a => a.Category.Name))
			.ForMember(dest => dest.Tags,
					   opt => opt.MapFrom(t => t.Tags == null ? Enumerable.Empty<string>() : t.Tags.Select(a => a.Name)))
			.ForMember(dest => dest.ViewCount,
			           opt => opt.MapFrom(t => t.ViewsCount));
		CreateMap<Blog_Article, ArticleListDto>()
			.ForMember(dest => dest.Content,
			           opt => opt.MapFrom(a => a.Content.Length > 260 ? a.Content.Substring(0, 260) + "…" : a.Content))
			.ForMember(dest => dest.Category,
					   opt => opt.MapFrom(a => a.Category.Name))
			.ForMember(dest => dest.CommentCount,
			           opt => opt.MapFrom(a => a.Comments.Where(c => c.State != State.Deleted).Count()));

		CreateMap<Blog_Comment, CommentDto>()
			.ForMember(dest => dest.UserName,
					   opt => opt.MapFrom(c => c.User.UserName))
			.ForMember(dest => dest.Email,
					   opt => opt.MapFrom(c => c.User.Email))
			.ForMember(dest => dest.Role,
					   opt => opt.MapFrom(r => r.User.Role));

		CreateMap<Blog_Category, CategoryDto>()
			.ForMember(dest => dest.Count,
					   opt => opt.MapFrom(c => c.Articles.Where(a => a.State != State.Deleted).Count()));

		CreateMap<Blog_Tag, TagDto>()
			.ForMember(dest => dest.Count,
					   opt => opt.MapFrom(t => t.Articles.Where(a => a.State != State.Deleted).Count()));
	}
}
