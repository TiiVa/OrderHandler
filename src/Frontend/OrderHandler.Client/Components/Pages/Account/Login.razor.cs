using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using OrderHandler.Client.ServiceInterfaces;
using OrderHandler.DataTransferContracts.DTOs.User;

namespace OrderHandler.Client.Components.Pages.Account;

public partial class Login : ComponentBase
{
    //[Inject] private IConfiguration Config { get; set; } 

    [Inject] private IUserService UserService { get; set; }
    [CascadingParameter] public HttpContext? HttpContext { get; set; }

    [SupplyParameterFromForm] public UserDto LoginUserDto { get; set; } = new();

    private List<UserDto> Users { get; set; } = new();
    private string? errorMessage;

    private async Task Authenticate()
    {
        var users = await UserService.GetAllAsync();

        Users = users.ToList();

        var userAccount = Users.FirstOrDefault(u => u.Email == LoginUserDto.Email);

        if (userAccount == null || userAccount.Password != LoginUserDto.Password)
        {
            errorMessage = "Invalid email or password";
            return;
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, LoginUserDto.Email),
            new Claim(ClaimTypes.Role, userAccount.Role),
            new Claim("UserId", userAccount.Id.ToString()),
            new Claim("FullName", $"{userAccount.FirstName} {userAccount.LastName}")
        };

        // Generate JWT token
        //var token = GenerateToken(userAccount.Id, LoginUserDto.Email, userAccount.Role);

        //// Set the JWT token in a cookie
        //var cookieOptions = new CookieOptions
        //{
        //    HttpOnly = true,
        //    Secure = true,
        //    SameSite = SameSiteMode.Strict,
        //    Expires = DateTime.Now.AddDays(1)
        //};

        //HttpContext.Response.Cookies.Append("cookie_token", token, cookieOptions);

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(principal);

        navigationManager.NavigateTo("/");
    }

    //private string GenerateToken(int userId, string email, string role)
    //{
    //    var tokenHandler = new JsonWebTokenHandler();
    //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["Jwt:Key"]!));


    //    var tokenDescriptor = new SecurityTokenDescriptor
    //    {
    //        Subject = new ClaimsIdentity(new[]{
    //            new Claim(ClaimTypes.Name, email),
    //            new Claim(ClaimTypes.Role, role),
    //            new("UserId", userId.ToString())
    //        }),
    //        Expires = DateTime.Now.AddMinutes(10),
    //        SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256),
    //        Issuer = Config["Jwt:Issuer"],
    //        Audience = Config["Jwt:Audience"]
    //    };

    //    return tokenHandler.CreateToken(tokenDescriptor);

        
    //}
}