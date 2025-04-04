using Microsoft.AspNetCore.Components;
using OrderHandler.Client.ServiceInterfaces;
using OrderHandler.DataTransferContracts.DTOs.User;

namespace OrderHandler.Client.Components.Pages.Admin;

public partial class UserManagement : ComponentBase
{
    [Inject] private IUserService UserService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    public List<UserDto> UserDtoList { get; set; } = [];
    private UserDto UserToUpdateOrDelete { get; set; } = new();
    private int UserId { get; set; }
    private string? errorMessage;
    private UserDto NewUserDto = new();
    private bool _visible;
    public bool DetailsUpdatedModalVisible { get; set; }
    public bool DeleteUserModalVisible { get; set; }
    private bool UpdateOrDeleteFormIsDisplayed { get; set; }
    private bool DisplayAllUsersForms { get; set; }
    private bool ShowAddNewUserFormIsDisplayed { get; set; }

    protected override async Task OnInitializedAsync()
    {
        UserDtoList.AddRange(await UserService.GetAllAsync());
    }

    private void ShowAllUsers()
    {
        DisplayAllUsersForms = true;
        UpdateOrDeleteFormIsDisplayed = false;
        ShowAddNewUserFormIsDisplayed = false;
    }

    private void ShowUpdateOrDeleteForm()
    {
        DisplayAllUsersForms = false;
        UpdateOrDeleteFormIsDisplayed = true;
        ShowAddNewUserFormIsDisplayed = false;
    }

    private void AddNewUserFormIsDisplayed()
    {
        DisplayAllUsersForms = false;
        UpdateOrDeleteFormIsDisplayed = false;
        ShowAddNewUserFormIsDisplayed = true;
    }

    private Task ToggleModal()
    {
        DetailsUpdatedModalVisible = !DetailsUpdatedModalVisible;
        NavigationManager.NavigateTo("/admin/user_management");
        return Task.CompletedTask;
    }

    private Task ToggleDeleteAccountModal()
    {
        DeleteUserModalVisible = !DeleteUserModalVisible;

        return Task.CompletedTask;
    }

    private async Task ShowCurrentDetails(int id)
    {
        ShowUpdateOrDeleteForm();
        _visible = true;

        await Task.Delay(3000);

        var showDetailsForUser = await UserService.GetByIdAsync(id);
        UserToUpdateOrDelete = showDetailsForUser;

        _visible = false;
    }

    private async Task UpdateUserDetails()
    {
        _visible = true;
        await Task.Delay(2000);

        await UserService.UpdateAsync(UserToUpdateOrDelete, UserToUpdateOrDelete.Id);

        _visible = false;

        UserDtoList.Clear();
        UserDtoList.AddRange(await UserService.GetAllAsync());
        ShowAllUsers();
    }

    private async Task DeleteUserProfile()
    {
        DeleteUserModalVisible = false;
        _visible = true;
        await Task.Delay(2000);
        await UserService.DeleteAsync(UserToUpdateOrDelete.Id);
        _visible = false;

        UserDtoList.Clear();
        UserDtoList.AddRange(await UserService.GetAllAsync());
    }

    private async Task AddNewUser()
    {
        _visible = true;

        await Task.Delay(3000);

        await UserService.AddAsync(NewUserDto);

        _visible = false;

        UserDtoList.Clear();
        UserDtoList.AddRange(await UserService.GetAllAsync());

        DisplayAllUsersForms = true;
        UpdateOrDeleteFormIsDisplayed = false;

    }
}