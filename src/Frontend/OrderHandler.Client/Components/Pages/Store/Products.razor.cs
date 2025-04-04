using System.Security.Claims;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using OrderHandler.Client.ServiceInterfaces;
using OrderHandler.Client.Services;
using OrderHandler.DataTransferContracts.DTOs.OrderProduct;
using OrderHandler.DataTransferContracts.DTOs.Product;

namespace OrderHandler.Client.Components.Pages.Store;

public partial class Products : ComponentBase
{
    private string userName;
    private string fullName;
    [Inject] IProductService ProductService { get; set; }
    [Inject] CartService CartService { get; set; }
    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }
    public ClaimsPrincipal? ClaimsPrincipal { get; set; }
    private List<ProductDto> ProductDtoList { get; set; } = new();
    private ProductDto SearchedProduct { get; set; }
    public bool ShowShop { get; set; }
    private Offcanvas? offcanvasRef;
    private bool _modalVisible;
    private bool ShowSingleProduct { get; set; }
    private List<ProductDto> FilteredProducts = [];
    private string? _searchQuery = string.Empty;


    private async Task SeeAllProducts()
    {
        ShowShop = true;
        ShowSingleProduct = false;
        FilteredProducts = ProductDtoList;
    }
    private Task ShowOffCanvas()
    {
        return offcanvasRef!.Show();
    }

    private Task HideOffCanvas()
    {
        return offcanvasRef!.Hide();
    }

    protected override async Task OnInitializedAsync()
    {
       
        ProductDtoList.AddRange(await ProductService.GetAllAsync());
        FilteredProducts = ProductDtoList;

        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        ClaimsPrincipal = authState.User;

        if (ClaimsPrincipal.Identity.IsAuthenticated)
        {
            userName = ClaimsPrincipal.Identity.Name;
            fullName = ClaimsPrincipal.FindFirstValue("FullName");
        }

        
        ShowShop = true;

    }

    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            _searchQuery = value;
            SearchProducts(value);
        }
    }

    private async Task SearchProducts(string searchQuery)
    {
        FilteredProducts = ProductDtoList.Where(p => p.Name.ToLower().Contains(searchQuery.ToLower())).ToList();

        if (FilteredProducts.Count == 1)
        {
            SearchedProduct = FilteredProducts[0];
            FilteredProducts.Clear();
            ShowSingleProduct = true;
        }
    }


    private void RemoveFromCart(int id)
    {
        var productToRemove = CartService.CartProducts.FirstOrDefault(p => p.ProductId == id);

        CartService.RemoveProductFromCart(productToRemove);

        NavigationManager.NavigateTo("/store/products");

        StateHasChanged();
    }

    private void AddToCart(int id)
    {
        var productToAdd = ProductDtoList.FirstOrDefault(p => p.Id == id);  

        var cartProduct = new OrderProductDto()
        {
            ProductId = productToAdd.Id,
            Name = productToAdd.Name,
            Price = productToAdd.Price,
            Description = productToAdd.Description,

        };

        var productExistsInCart = CartService.CartProducts.FirstOrDefault(p => p.ProductId == id);

        if (productExistsInCart is not null) 
        { 
            var productIsInCart = CartService.CartProducts.FirstOrDefault(p => p.ProductId == productToAdd.Id);
            var newQuantity = CountQuantity(productIsInCart);
            cartProduct.Quantity = newQuantity;
        }
        else
        {
            cartProduct.Quantity++;
        }

        CartService.AddProductToCart(cartProduct);

        NavigationManager.NavigateTo("/store/products");

        StateHasChanged();

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

    private int CountQuantity(OrderProductDto productDto)
    {
       var totalQuantity = CartService.CartProducts.Count(p => p.ProductId == productDto.ProductId);

        return totalQuantity;
    }

    private void ShowShoppingCart()
    {
        ShowOffCanvas();
    }
    public void CloseShoppingCart()
    {
        HideOffCanvas();
    }

    private async Task GoToCheckOut()
    {
        
        NavigationManager.NavigateTo("/store/checkout");
    }

    public void ClearCart()
    {
        CartService.CartProducts.Clear();
        ToggleModal();
        NavigationManager.NavigateTo("/store/products");
    }

    

    Task ToggleModal()
    {
        _modalVisible = !_modalVisible;

        return Task.CompletedTask;
    }

    

}