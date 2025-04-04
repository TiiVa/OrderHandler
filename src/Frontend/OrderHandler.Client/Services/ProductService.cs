using OrderHandler.Client.ServiceInterfaces;
using OrderHandler.DataTransferContracts.DTOs.Product;

namespace OrderHandler.Client.Services;

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;


    public ProductService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("OrderHandlerApi");
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var response = await _httpClient.GetAsync("/products");

        if (!response.IsSuccessStatusCode)
        {
            return Enumerable.Empty<ProductDto>();
        }

        var result = await response.Content.ReadFromJsonAsync<ProductDtoList>();
        return result.Products ?? Enumerable.Empty<ProductDto>();
    }

    public async Task<IEnumerable<ProductDto>> GetAllInactiveProducts()
    {
        var response = await _httpClient.GetAsync($"/products/status");

        if (!response.IsSuccessStatusCode)
        {
            return Enumerable.Empty<ProductDto>();
        }

        var result = await response.Content.ReadFromJsonAsync<InactiveProductDtoList>();
        return result.InactiveProducts ?? Enumerable.Empty<ProductDto>();
    }

    public async Task<ProductDto> GetByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"/products/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return new ProductDto();
        }

        var result = await response.Content.ReadFromJsonAsync<ProductResponseDto>();

        return result.Product ?? new ProductDto();

        
    }

    public async Task AddAsync(ProductDto entity)
    {
        var response = await _httpClient.PostAsJsonAsync("/products", entity);

        if (!response.IsSuccessStatusCode)
        {
            return;
        }
    }

    public async Task UpdateAsync(ProductDto entity, int id)
    {
        var response = await _httpClient.PutAsJsonAsync($"/products/{id}", entity);

        if (!response.IsSuccessStatusCode)
        {
            return;
        }
    }

    public async Task DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"/products/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return;
        }
    }
}