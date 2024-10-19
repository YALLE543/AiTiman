using AiTiman_API.Models;
using AiTiman_API.Services.DTO;

namespace AiTiman_API.Services.Interfaces
{
    public interface IUser
    {
        Task<(bool, string)> AddNewUser(CreateUserDTO createUser);
        Task<List<User>> fetchUser();
        Task<User?> fetchUser(string? id);
        Task<(bool, string)> UpdateUser(string id, UpdateUserDTO updateUser);
        Task<(bool, string)> DeleteUser(string? id);
        Task<User> ValidateUser(string username, string password);
        Task<User?> GetUserProfileByUsername(string username);
        Task<User?> GetUserByUserNameAsync(string username);
    }

}