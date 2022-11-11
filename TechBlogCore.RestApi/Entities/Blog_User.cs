using Microsoft.AspNetCore.Identity;

namespace TechBlogCore.RestApi.Entities;

public class Blog_User : IdentityUser
{
    public IEnumerable<Blog_Comment> Comments { get; set; }
}
