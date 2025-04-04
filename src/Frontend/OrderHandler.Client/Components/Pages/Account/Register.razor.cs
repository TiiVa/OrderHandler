using Microsoft.AspNetCore.Components;
using OrderHandler.Client.ServiceInterfaces;
using OrderHandler.DataTransferContracts.DTOs.User;

namespace OrderHandler.Client.Components.Pages.Account;

public partial class Register : ComponentBase
{
    [CascadingParameter] public HttpContext? HttpContext { get; set; }
    [Inject] private IUserService UserService { get; set; }
    [SupplyParameterFromForm(FormName = "registerForm")] 
    public UserDto NewUserDto { get; set; } = new();
    private bool _visible;

    private string? errorMessage;
    public List<UserDto> Users { get; set; } = new();

    private async Task RegisterNewUser()
    {
        _visible = true;
        await Task.Delay(2000);
        _visible = false;
        var allUsers = await UserService.GetAllAsync();

        Users = allUsers.ToList();

        
        if (Users.Any(u => u.Email == NewUserDto.Email))
        {
            errorMessage = $"There is already an account associated with this email or name";
            return;
        }

        await UserService.AddAsync(NewUserDto);
        NavigationManager.NavigateTo("/login");
    }
}