using Microsoft.AspNetCore.Components;
using OrderHandler.Client.ServiceInterfaces;
using OrderHandler.DataTransferContracts.DTOs.Product;

namespace OrderHandler.Client.Components.Pages;

public partial class Products : ComponentBase
{
    [Inject] private IProductService ProductService { get; set; }
    [Inject] private NavigationManager? NavigationManager {get; set; }
    private List<ProductDto> ProductDtoList { get; set; } = [];
    private bool _visible;

    protected override async Task OnInitializedAsync()
    {
       
        ProductDtoList.AddRange(await ProductService.GetAllAsync());
       
    }

    private async Task GoToStore()
    {
        NavigationManager!.NavigateTo("/store/products");
    }
}