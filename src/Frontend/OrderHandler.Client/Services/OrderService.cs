using OrderHandler.Client.ServiceInterfaces;
using OrderHandler.DataTransferContracts.DTOs.Order;

namespace OrderHandler.Client.Services;

public class OrderService : IOrderService
{
    private readonly HttpClient _httpClient;

    public OrderService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("OrderHandlerApi"); // TODO: Fixa
    }

    public async Task<IEnumerable<OrderDto>> GetAllAsync()
    {
        var response = await _httpClient.GetAsync("/orders");

        if (!response.IsSuccessStatusCode)
        {
            return Enumerable.Empty<OrderDto>();
        }

        var result = await response.Content.ReadFromJsonAsync<OrderDtoList>();
        return result.Orders ?? Enumerable.Empty<OrderDto>();
    }

    public async Task<OrderDto> GetByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"/orders/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return new OrderDto();
        }

        var result = await response.Content.ReadFromJsonAsync<OrderDto>();
        return result;
    }

    public async Task<IEnumerable<OrderDto>> GetAllOrdersWithProductAsync(int productId)
    {
        var response = await _httpClient.GetAsync($"/orders/products/{productId}");

        if (!response.IsSuccessStatusCode)
        {
            return Enumerable.Empty<OrderDto>();
        }

        var result = await response.Content.ReadFromJsonAsync<OrdersWithProductDtoList>();
        return result.OrdersWithProduct ?? Enumerable.Empty<OrderDto>();
    }

    public async Task<IEnumerable<OrderDto>> GetAllOrdersForAUserAsync(int userId)
    {
        var response = await _httpClient.GetAsync($"/orders/users/{userId}");

        if (!response.IsSuccessStatusCode)
        {
            return Enumerable.Empty<OrderDto>();
        }

        var result = await response.Content.ReadFromJsonAsync<OrdersForAUserDtoList>();
        return result.OrdersForAUser ?? Enumerable.Empty<OrderDto>();
    }


    public async Task AddAsync(OrderDto entity)
    {
        var response = await _httpClient.PostAsJsonAsync("/orders", entity);

        if (!response.IsSuccessStatusCode)
        {
            return;
        }
    }

    public async Task UpdateAsync(OrderDto entity, int id)
    {
        var response = await _httpClient.PutAsJsonAsync($"/orders/{id}", entity);

        if (!response.IsSuccessStatusCode)
        {
            return;
        }
    }

    public async Task DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"/orders/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return;
        }
    }
}