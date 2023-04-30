using Microsoft.AspNetCore.Identity;

namespace TechBlogCore.RestApi.Entities;

public class Blog_User : IdentityUser
{
    public string Role { get; set; }
    public IEnumerable<Blog_Comment> Comments { get; set; }
    public IEnumerable<Chat_Message> Messages { get; set; }
}
