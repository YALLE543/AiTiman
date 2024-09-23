using AiTiman_API.Models;
using AiTiman_API.Services.DTO;

namespace AiTiman_API.Services.Interfaces
{
    public interface IUsers
    {
        Task<(bool, string)> AddNewUsers(CreateUsersDTO createUsers);
        Task<List<Users>> fetchUsers();
        Task<Users?> fetchUsers(string? id);
        Task<(bool, string)> UpdateUsers(string id, UpdateUsersDTO updateUsers);
        Task<(bool, string)> DeleteUsers(string? id);
    }
}
