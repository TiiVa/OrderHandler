using Microsoft.AspNetCore.Components;
using OrderHandler.Client.ServiceInterfaces;
using OrderHandler.DataTransferContracts.DTOs.Product;

namespace OrderHandler.Client.Components.Pages.Admin;

public partial class ProductManagement_productOverView : ComponentBase
{
    private List<ProductDto> ProductDtoList { get; set; } = [];
    private ProductDto UpdateProductDto { get; set; } = new();
    [Inject] private IProductService ProductService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    private bool ProductsAreDisplayed { get; set; }
    private bool AddProductFormIsDisplayed { get; set; }
    private bool UpdateProductFormIsDisplayed { get; set; }
    private bool ShowProductById { get; set; }
    private bool ModalVisible;
    private bool DeleteModalVisible { get; set; }

    protected override async Task OnInitializedAsync()
    {
        ProductDtoList.AddRange(await ProductService.GetAllAsync());
        ShowAllProducts();
    }

    private Task ToggleModal()
    {
        ModalVisible = !ModalVisible;

        return Task.CompletedTask;
    }

    private Task ToggleDeleteProductModal()
    {
        DeleteModalVisible = !DeleteModalVisible;

        return Task.CompletedTask;
    }

    private void ShowAllProducts()
    {

        ProductsAreDisplayed = true;
        UpdateProductFormIsDisplayed = false;

    }

    private void ShowUpdateProductForm()
    {
        UpdateProductFormIsDisplayed = true;
        AddProductFormIsDisplayed = false;
        ProductsAreDisplayed = false;
        ShowProductById = false;
    }

    private async Task UpdateProductForm(ProductDto product)
    {
        ShowUpdateProductForm();

        UpdateProductDto.Id = product.Id;
        UpdateProductDto.Name = product.Name;
        UpdateProductDto.Description = product.Description;
        UpdateProductDto.Price = product.Price;
        UpdateProductDto.IsActive = product.IsActive;
        UpdateProductDto.Color = product.Color;
    }

    private async Task UpdateProduct(ProductDto product)
    {
        await ToggleModal();

        await ProductService.UpdateAsync(product, UpdateProductDto.Id);

        ProductDtoList.Clear();
        await OnInitializedAsync();


    }

    private async Task DeleteProduct()
    {
        DeleteModalVisible = false;

        await ProductService.DeleteAsync(UpdateProductDto.Id);
        ProductDtoList.Clear();
        await OnInitializedAsync();
    }

    private async Task GoBackToAdminStartPage()
    {
        ShowAllProducts();
    }
}