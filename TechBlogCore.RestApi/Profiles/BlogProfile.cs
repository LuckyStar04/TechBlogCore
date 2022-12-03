using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TechBlogCore.RestApi.Dtos;
using TechBlogCore.RestApi.Entities;

namespace TechBlogCore.RestApi.Profiles;

public class BlogProfile : Profile
{
	private readonly UserManager<Blog_User> userManager;

	public BlogProfile(UserManager<Blog_User> userManager)
	{
        this.userManager = userManager;
        CreateMap<Blog_Article, ArticleDetailDto>()
			.ForMember(dest => dest.Category,
					   opt => opt.MapFrom(a => a.Category.Name))
			.ForMember(dest => dest.Tags,
					   opt => opt.MapFrom(t => t.Tags == null ? Enumerable.Empty<string>() : t.Tags.Select(a => a.Name)));
		CreateMap<Blog_Article, ArticleListDto>()
			.ForMember(dest => dest.Content,
			           opt => opt.MapFrom(a => a.Content.Length > 180 ? a.Content.Substring(0, 180) + "…" : a.Content))
			.ForMember(dest => dest.Category,
					   opt => opt.MapFrom(a => a.Category.Name))
			.ForMember(dest => dest.CommentCount,
			           opt => opt.MapFrom(a => a.Comments.Count()));

        CreateMap<Blog_Comment, CommentDto>()
			.ForMember(dest => dest.UserName,
					   opt => opt.MapFrom(c => c.User.UserName))
			.ForMember(dest => dest.Email,
					   opt => opt.MapFrom(c => c.User.Email))
			.ForMember(dest => dest.Role,
					   opt => opt.MapFrom(r => userManager.GetRolesAsync(r.User).GetAwaiter().GetResult()[0]));

		CreateMap<Blog_Category, CategoryDto>()
			.ForMember(dest => dest.Count,
					   opt => opt.MapFrom(c => c.Articles.Count()));

		CreateMap<Blog_Tag, TagDto>()
			.ForMember(dest => dest.Count,
					   opt => opt.MapFrom(t => t.Articles.Count()));
	}
}
