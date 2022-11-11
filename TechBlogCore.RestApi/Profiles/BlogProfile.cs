using AutoMapper;
using TechBlogCore.RestApi.Dtos;
using TechBlogCore.RestApi.Entities;

namespace TechBlogCore.RestApi.Profiles;

public class BlogProfile : Profile
{
	public BlogProfile()
	{
		CreateMap<Blog_Article, ArticleDto>()
			.ForMember(dest => dest.Category,
					   opt => opt.MapFrom(a => a.Category.Name))
			.ForMember(dest => dest.Tags,
					   opt => opt.MapFrom(t => t.Tags == null ? Enumerable.Empty<string>() : t.Tags.Select(a => a.Name)));

		CreateMap<Blog_Comment, CommentDto>()
			.ForMember(dest => dest.UserName,
					   opt => opt.MapFrom(c => c.User.UserName))
			.ForMember(dest => dest.Email,
					   opt => opt.MapFrom(c => c.User.Email));
	}
}
