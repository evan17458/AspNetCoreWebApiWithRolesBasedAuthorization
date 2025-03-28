using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiWithRoleAuthentication.Models;
using WebApiWithRoleAuthentication.Services;

namespace WebApiWithRoleAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly ILogger<AccountController> _logger;
        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ITouristRouteRepository touristRouteRepository, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _touristRouteRepository = touristRouteRepository;
            _logger = logger;
        }

        [HttpPost("register")] //驗證使用者的帳號密碼，並在驗證成功後生成並返回一個 JWT
        public async Task<IActionResult> Register([FromBody] Register model)
        {


            var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {


                // 3 初始化購物車
                var shoppingCart = new ShoppingCart()
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id
                };
                await _touristRouteRepository.CreateShoppingCart(shoppingCart);
                await _touristRouteRepository.SaveAsync();
                return Ok(new { message = "你註冊成功" });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))//檢查提供的密碼是否與資料庫中該使用者的密碼匹配。
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                //JwtRegisteredClaimNames.Sub 是 JWT 的標準聲明，表示「主體」（Subject），
                //通常用來儲存使用者的唯一標識，這裡使用的是 user.UserName（使用者名稱）。
                //JwtRegisteredClaimNames.Jti 是 JWT 的標準聲明，表示「JWT ID」，用來確保每個 Token 都有唯一的標識符。
                //Guid.NewGuid().ToString() 生成一個全球唯一的識別碼（GUID）並轉為字串，作為 Token 的唯一 ID。
                authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

                //ClaimTypes.Role 表示聲明的類型為「角色」。
                //role 是角色的名稱（例如 "Admin" 或 "User"）。
                //結果是一個 Claim 集合，例如[Claim("Role", "Admin"), Claim("Role", "User")]。

                //假設某個使用者的資料如下：
                //使用者名稱：john_doe
                //角色：["User", "Editor"]
                //執行這段程式碼後，authClaims 的內容可能是：

                //  [
                //     Claim("sub", "john_doe"),
                //   Claim("jti", "550e8400-e29b-41d4-a716-446655440000"),
                //   Claim("role", "User"),
                //   Claim("role", "Editor")
                //  ]
                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"]!)),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                    SecurityAlgorithms.HmacSha256));

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return Unauthorized();
        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody] string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(role));
                if (result.Succeeded)
                {
                    return Ok(new { message = "角色新增增成功" });
                }

                return BadRequest(result.Errors);
            }

            return BadRequest("Role already exists");
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] UserRole model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            var result = await _userManager.AddToRoleAsync(user, model.Role);
            if (result.Succeeded)
            {
                return Ok(new { message = "角色分配成功" });
            }

            return BadRequest(result.Errors);
        }
    }
}
