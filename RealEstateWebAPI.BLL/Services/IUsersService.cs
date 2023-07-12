using RealEstateWebAPI.BLL.DTO;

namespace RealEstateWebAPI.BLL.Services
{
    public interface IUsersService
    {
        
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(int userId);
        Task<int> AddUserAsync(UserDTO userDTO);
        Task UpdateUserAsync(int userId, UserDTO userDTO);
        Task DeleteUserAsync(int userId);
        Task<UserDTO> GetUserByUserNameAsync(string username);
    }
}
