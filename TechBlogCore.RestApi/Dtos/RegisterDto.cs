using System.ComponentModel.DataAnnotations;

namespace TechBlogCore.RestApi.Dtos;

public class RegisterDto
{
    [Required(ErrorMessage = "用户名是必填项")]
    public string Username { get; set; }
    [EmailAddress]
    [Required(ErrorMessage = "邮箱是必填项")]
    public string Email { get; set; }
    [Required(ErrorMessage = "密码是必填项")]
    public string Password { get; set; }
}
