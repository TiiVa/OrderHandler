using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using OrderHandler.Client.ServiceInterfaces;
using OrderHandler.DataTransferContracts.DTOs.User;

namespace OrderHandler.Client.Components.Pages;

public partial class Users : ComponentBase
{
    
    [Inject] private IUserService UserService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    private List<UserDto> UserDtoList { get; set; } = new();


    private UserDto UserToUpdate { get; set; } = new();
    private UserDto NewUserDto { get; set; } = new();

    
    private PaginationState paginationState = new PaginationState { ItemsPerPage = 5 };
    private string? lastNameFilter = string.Empty;
    private string? errorMessage = string.Empty;
    private bool ShowUserForm { get; set; }
    private bool _visible { get; set; }
    private bool ShowAddNewUserFormIsDisplayed { get; set; }
    private bool ShowAllUsers { get; set; }
    private string InitialRole { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await LoadData();
       ShowUsers();
    }

    public async Task LoadData()
    {
        if (string.IsNullOrWhiteSpace(lastNameFilter))
        {
            UserDtoList.Clear();
            UserDtoList.AddRange(await UserService.GetAllAsync());
        }
        else
        {
            UserDtoList = UserDtoList.Where(u => u.LastName.ToLower().Contains(lastNameFilter.ToLower())).ToList();

        }
        
    }


    private void ShowUsers()
    {
        ShowAllUsers = true;
        ShowAddNewUserFormIsDisplayed = false;
        ShowUserForm = false;
    }

    private void ShowAddNewUserView()
    {
        ShowAddNewUserFormIsDisplayed = true;
        ShowUserForm = false;
        ShowAllUsers = false;
    }

   

    private async Task ShowUpdateDetailsView(UserDto user)
    {


        var userToUpdate = UserDtoList.FirstOrDefault(u => u.Id == user.Id);

        if (userToUpdate is null)
        {
            return;
        }

        UserToUpdate = userToUpdate;

        ShowAddNewUserFormIsDisplayed = false;
        ShowAllUsers = false;
        ShowUserForm = true;

        
    }
    private void OnSelectedChanged(ChangeEventArgs e)
    {
        if (e.Value is not null)
        {
            InitialRole = (string)e.Value;
            UserToUpdate.Role = (string)e.Value;

        }
    }


    private void OnPageSizeChanged(ChangeEventArgs e)
    {
        if (e.Value is not null)
        {
            paginationState.ItemsPerPage = int.Parse((string)e.Value);
        }
    }

    private async Task OnDelete(UserDto user)
    {
        await UserService.DeleteAsync(user.Id);
        UserDtoList.Clear();
        UserDtoList.AddRange(await UserService.GetAllAsync());

        

    }


    private async Task UpdateUser(int userId)
    {
        UserToUpdate.Id = userId;
        await UserService.UpdateAsync(UserToUpdate, UserToUpdate.Id);

        UserDtoList.Clear();
        UserDtoList.AddRange(await UserService.GetAllAsync());

        ShowUsers();

    }

    private async Task AddNewUser(UserDto user)
    {
        _visible = true;
        await Task.Delay(2000);

        NewUserDto = user;

        if (UserDtoList.Any(u => u.Email == NewUserDto.Email))
        {
            errorMessage = "User already exists";
        }

        await UserService.AddAsync(NewUserDto);

        UserDtoList.Clear();
        UserDtoList.AddRange(await UserService.GetAllAsync());

        _visible = false;

        ShowUsers();
    }

    private void GoBackToOverView()
    {
        ShowUsers();
    }
}