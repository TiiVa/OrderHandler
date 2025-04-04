using OrderHandler.CommonInterfaces;
using OrderHandler.DataTransferContracts.DTOs.User;

namespace OrderHandler.DataAccess.RepositoryInterfaces;

public interface IUserRepository : IRepository<UserDto, int>
{
    
}