using OrderHandler.DataTransferContracts.DTOs.User;

namespace OrderHandler.Api.Endpoints.User.GetAll;

public class Response
{
    public IEnumerable<UserDto> Users { get; set; } 
}