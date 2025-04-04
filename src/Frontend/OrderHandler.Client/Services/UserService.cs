using OrderHandler.Client.ServiceInterfaces;
using OrderHandler.DataTransferContracts.DTOs.User;

namespace OrderHandler.Client.Services;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;

    public UserService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("OrderHandlerApi");
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var response = await _httpClient.GetAsync("/users");

        if (!response.IsSuccessStatusCode)
        {
            return Enumerable.Empty<UserDto>();
        }

        var result = await response.Content.ReadFromJsonAsync<UserDtoList>();
        return result.Users ?? Enumerable.Empty<UserDto>();
    }

    public async Task<UserDto> GetByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"/users/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return new UserDto();
        }

        var result = await response.Content.ReadFromJsonAsync<UserResponseDto>();
        return result.User ?? new UserDto();
    }

    public async Task AddAsync(UserDto entity)
    {
        var response = await _httpClient.PostAsJsonAsync("/users", entity); 

        if (!response.IsSuccessStatusCode)
        {
            return;
        }
    }

    public async Task UpdateAsync(UserDto entity, int id)
    {
        var response = await _httpClient.PutAsJsonAsync($"/users/{id}", entity);

        if (!response.IsSuccessStatusCode)
        {
            return;
        }
    }

    public async Task DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"/users/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return;
        }
    }
}