using AiTiman_API.Services.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AiTiman_API.Services.Interfaces;
using System.Threading.Tasks;

namespace AiTiman_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUsers _Users;

        // Constructor injection of the IUsers service
        public UsersController(IUsers Users)
        {
            _Users = Users;
        }

        [HttpGet("All-Userss")]
        public async Task<IActionResult> AllUsers()
        {
            var Users = await _Users.fetchUsers();
            return Ok(Users);
        }

        [HttpGet("Fetch-Users")]
        public async Task<IActionResult> FetchUsers(string id)
        {
            var Users = await _Users.fetchUsers(id);
            return Ok(Users);
        }

        [HttpPost("Create-New-Users")]
        public async Task<IActionResult> CreateNewUsers(CreateUsersDTO createUsers)
        {
            var (isSuccess, message) = await _Users.AddNewUsers(createUsers);

            if (!isSuccess)
                return BadRequest(message);

            return Ok(message);
        }

        [HttpPut("Update-Users")]
        public async Task<IActionResult> UpdateUsers(string id, UpdateUsersDTO updateUsers)
        {
            var (isSuccess, message) = await _Users.UpdateUsers(id, updateUsers);

            if (!isSuccess)
                return BadRequest(message);

            return Ok(message);
        }

        [HttpDelete("Delete-Users")]
        public async Task<IActionResult> DeleteUsers(string id)
        {
            var (isSuccess, message) = await _Users.DeleteUsers(id);

            if (!isSuccess)
                return BadRequest(message);

            return Ok(message);
        }
    }
}

