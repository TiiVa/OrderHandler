using Microsoft.AspNetCore.Components;
using OrderHandler.Client.ServiceInterfaces;
using OrderHandler.DataTransferContracts.DTOs.Order;
using OrderHandler.DataTransferContracts.DTOs.Product;
using OrderHandler.DataTransferContracts.DTOs.User;

namespace OrderHandler.Client.Components.Pages.Admin;

public partial class ProductByIdSearch : ComponentBase
{
    [Inject] private IProductService ProductService { get; set; }
    [Inject] private IOrderService OrderService { get; set; }
    [Inject] private IUserService UserService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    private List<UserDto> UserDtoList { get; set; } = [];
    private List<OrderDto> OrdersWithProductList { get; set; } = [];
    private ProductDto ProductById { get; set; } = new();
    private int ProductId { get; set; }
    private int ColorChoice { get; set; }
    private int NumberOfResults { get; set; }
    private bool NoOrdersToDisplay { get; set; }
    public bool DisplayOrders { get; set; }

    protected override async Task OnInitializedAsync()
    {
        UserDtoList.AddRange(await UserService.GetAllAsync());
        DisplayOrders = true;
    }

    private async Task GetOrdersWithItem(int productId)
    {
        OrdersWithProductList.Clear();

        var product = await ProductService.GetByIdAsync(productId);

        if (product.Id == 0)
        {
            NoOrdersToDisplay = true;
            DisplayOrders = false;
            NumberOfResults = 0;
            return;
        }

        ProductById = product;

        var ordersWithItem = await OrderService.GetAllOrdersWithProductAsync(ProductById.Id);

        OrdersWithProductList = ordersWithItem.ToList();
        NumberOfResults = OrdersWithProductList.Count;

        DisplayOrders = true;
        NoOrdersToDisplay = false;


    }

    private void ShowAllInactiveProducts()
    {
        NavigationManager.NavigateTo("/products/status=inactive");
    }
}