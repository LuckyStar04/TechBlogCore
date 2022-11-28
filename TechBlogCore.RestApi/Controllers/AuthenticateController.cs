using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using TechBlogCore.RestApi.Dtos;
using TechBlogCore.RestApi.Entities;

namespace TechBlogCore.RestApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<Blog_User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<Blog_User> signInManager;
        private readonly IConfiguration configuration;

        public AuthenticateController(UserManager<Blog_User> userManager,
                                    RoleManager<IdentityRole> roleManager,
                                    SignInManager<Blog_User> signInManager,
                                    IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseDto<string>>> Login([FromBody]LoginDto loginDto)
        {
            var user = await userManager.FindByNameAsync(loginDto.Username);
            if (user == null)
            {
                return Unauthorized(new ResponseDto<string>
                {
                    Code = 1,
                    Msg = "用户不存在"
                });
            }
            if (userManager.SupportsUserLockout && await userManager.IsLockedOutAsync(user))
            {
                var maxAttempts = userManager.Options.Lockout.MaxFailedAccessAttempts;
                var lockTimes = userManager.Options.Lockout.DefaultLockoutTimeSpan.TotalMinutes;
                return Unauthorized(new ResponseDto<string>
                {
                    Code = 1,
                    Msg = $"密码错误超过{maxAttempts}次，用户已被锁定，请{lockTimes}分钟后再次尝试。"
                });
            }
            var result = await signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);
            if (result.Succeeded)
            {
                await userManager.ResetAccessFailedCountAsync(user);
                var role = (await userManager.GetRolesAsync(user)).FirstOrDefault();
                return Ok(new ResponseDto<string>
                {
                    Data = GetToken(user.UserName, user.Email, role)
                });
            }
            else if (userManager.SupportsUserLockout && await userManager.GetLockoutEnabledAsync(user))
            {
                await userManager.AccessFailedAsync(user);
            }
            return Unauthorized(new ResponseDto<string>
            {
                Code = 1,
                Msg = $"密码错误，请重试"
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult<ResponseDto<string>>> Register([FromBody]RegisterDto dto)
        {
            var userExists = await userManager.FindByNameAsync(dto.Username);
            if (userExists != null)
            {
                return BadRequest(new ResponseDto<string>
                {
                    Code = 1,
                    Msg = "用户已存在"
                });
            }
            var user = new Blog_User
            {
                Email = dto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = dto.Username
            };
            var result = await userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new ResponseDto<string>
                {
                    Code = 1,
                    Msg = result.Errors?.FirstOrDefault()?.Description
                });
            }
            await userManager.AddToRoleAsync(user, "CommonUser");
            return Ok(new ResponseDto<string> { Msg = "用户创建成功。" });
        }

        [Authorize(Roles = "Admin,CommonUser")]
        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            string role = User.FindFirst(ClaimTypes.Role)?.Value;
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            return Ok(new { role, user, email });
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            await roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            await roleManager.CreateAsync(new IdentityRole { Name = "CommonUser" });

            var user = new Blog_User
            {
                Email = "cx2529507@163.com",
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "LuckyStar",
            };
            var result = await userManager.CreateAsync(user, "2368lst");
            await userManager.AddToRoleAsync(user, "Admin");
            return Ok();
        }

        private string GetToken(string sub, string email, string role)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, sub),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.Email, email)
            };

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(3),
                notBefore: DateTime.Now,
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
