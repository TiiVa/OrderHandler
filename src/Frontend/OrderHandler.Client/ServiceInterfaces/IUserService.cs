using OrderHandler.CommonInterfaces;
using OrderHandler.DataTransferContracts.DTOs.User;

namespace OrderHandler.Client.ServiceInterfaces;

public interface IUserService : IService<UserDto, int>
{
    
}