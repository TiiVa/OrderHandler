using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using OrderHandler.Client.ServiceInterfaces;
using OrderHandler.DataTransferContracts.DTOs.User;

namespace OrderHandler.Client.Components.Pages.User;

public partial class Profile : ComponentBase
{
    [Inject] private IUserService UserService { get; set; }
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    private UserDto UpdatedUserDto { get; set; } = new();
    private UserDto UserToDelete { get; set; } = new();
    private int _currentUser;
    private ClaimsPrincipal? ClaimsPrincipal { get; set; }
    private bool _visible;
    public bool DetailsUpdatedModalVisible { get; set; }
    public bool DeleteUserModalVisible { get; set; }
    private string InitialRole { get; set; } = string.Empty;


    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        ClaimsPrincipal = authState.User;

        if (ClaimsPrincipal.Identity!.IsAuthenticated)
        {
            _currentUser = Convert.ToInt32(ClaimsPrincipal.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            UpdatedUserDto.Id = _currentUser;
            UserToDelete.Id = _currentUser;
        }
    }

    private void OnSelectedChanged(ChangeEventArgs e)
    {
       
        if (e.Value is not null)
        {
            InitialRole = (string)e.Value;
            UpdatedUserDto.Role = (string)e.Value;
           
        }

        
    }

    private Task ToggleModal()
    {
        DetailsUpdatedModalVisible = !DetailsUpdatedModalVisible;
        return Task.CompletedTask;
    }

    private Task ToggleDeleteAccountModal()
    {
        DeleteUserModalVisible = !DeleteUserModalVisible;

        return Task.CompletedTask;
    }

    private async Task ShowCurrentDetails()
    {
        _visible = true;

        await Task.Delay(3000);

        var showDetailsForAuthenticatedUser = await UserService.GetByIdAsync(UpdatedUserDto.Id);
        UpdatedUserDto = showDetailsForAuthenticatedUser;

        _visible = false;
    }

    private async Task UpdateUserDetails()
    {
        _visible = true;

        await Task.Delay(2000);

        await UserService.UpdateAsync(UpdatedUserDto, UpdatedUserDto.Id);
        

        _visible = false;
        DetailsUpdatedModalVisible = true;
    }

    private async Task DeleteUserProfile()
    {
        _visible = true;

        await Task.Delay(2000);

        await UserService.DeleteAsync(UserToDelete.Id);

        _visible = false;
        NavigationManager.NavigateTo("/logout");
    }

    private void GoBackToStartPage()
    {
        NavigationManager.NavigateTo("/user/MyProfile");
    }
}