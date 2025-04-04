
using Microsoft.AspNetCore.Components;
using OrderHandler.Client.ServiceInterfaces;
using OrderHandler.Client.Services;
using OrderHandler.DataTransferContracts.DTOs.OrderProduct;

namespace OrderHandler.Client.Components.Pages.Store;

public partial class Cart : ComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private CartService CartService { get; set; }
    [Inject] private IProductService ProductService { get; set; }
   


    private async Task RemoveFromCart(int id)
    {
        var productToRemove = CartService.CartProducts.FirstOrDefault(p => p.ProductId == id);

        CartService.CartProducts.Remove(productToRemove);
        NavigationManager.NavigateTo("/store/cart");
        StateHasChanged();
    }

    private int CountQuantity(OrderProductDto productDto)
    {
        var totalQuantity = CartService.CartProducts.Count(p => p.ProductId == productDto.ProductId);

        return totalQuantity;
    }

    private async Task AddToCart(int id)
    {
      
        var productToAdd = await ProductService.GetByIdAsync(id);

        var cartProduct = new OrderProductDto()
        {
            ProductId = productToAdd.Id,
            Name = productToAdd.Name,
            Price = productToAdd.Price
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

        CartService.CartProducts.Add(cartProduct);
        NavigationManager.NavigateTo("/store/cart");
        StateHasChanged();
    }

    public async Task CloseShoppingCart()
    {
        NavigationManager.NavigateTo("/store/products");
    }

    private async Task GoToCheckOut()
    {
        NavigationManager.NavigateTo("/store/checkout");
    }
}