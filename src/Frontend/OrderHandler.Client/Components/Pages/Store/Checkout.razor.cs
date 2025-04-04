using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using OrderHandler.Client.ServiceInterfaces;
using OrderHandler.Client.Services;
using OrderHandler.DataTransferContracts.DTOs.Order;

namespace OrderHandler.Client.Components.Pages.Store;

public partial class Checkout : ComponentBase
{
    [Inject] private IOrderService OrderService { get; set; }
    [Inject] private CartService CartService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    private string _userName = string.Empty;
    private string _fullName = string.Empty;
    public ClaimsPrincipal? ClaimsPrincipal { get; set; }
    public bool ModalVisible { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        ClaimsPrincipal = authState.User;

        if (ClaimsPrincipal.Identity!.IsAuthenticated)
        {
            _userName = ClaimsPrincipal.Identity.Name!;
            _fullName = ClaimsPrincipal.FindFirstValue("FullName")!;
        }
    }

    private void GoBackToStore()
    {
        NavigationManager.NavigateTo("/store/products");
    }

    private double TotalForCart()
    {
        double total = 0;

        foreach (var cartProduct in CartService.CartProducts)
        {
            total += cartProduct.Price;
        }

        return total;
    }

    private async Task AddOrder()
    {
        var order = new OrderDto
        {
            OrderSum = TotalForCart(),
            OrderProducts = CartService.CartProducts,
            IsActive = true,
            UserId = Convert.ToInt32(ClaimsPrincipal.Claims.FirstOrDefault(x => x.Type == "UserId").Value)
        };

        await OrderService.AddAsync(order);

        await ClearCart();

        ModalVisible = true;
    }

    private async Task ClearCart()
    {
        CartService.CartProducts.Clear();
    }

    private Task ToggleModal()
    {
        ModalVisible = !ModalVisible;

        NavigationManager.NavigateTo("/thankYouForYourPurchase");

        return Task.CompletedTask;
    }
}