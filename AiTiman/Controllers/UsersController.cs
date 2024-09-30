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

       
        [HttpGet("GetUserProfile/{userName}")]
        public async Task<IActionResult> GetUserProfile(string userName)
        {
            var user = await _Users.GetUserProfileByUsername(userName);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                user.UserName,
                user.ProfilePic
            });
        }

        [HttpDelete("Delete-Users")]
        public async Task<IActionResult> DeleteUsers(string id)
        {
            var (isSuccess, message) = await _Users.DeleteUsers(id);

            if (!isSuccess)
                return BadRequest(message);

            return Ok(message);
        }

        [HttpPost("ValidateLogin")]
        public async Task<IActionResult> ValidateLogin(LoginDTO login)
        {
            // Assuming _Users has a method to validate credentials
            var user = await _Users.ValidateUser(login.UserName, login.Password);

            if (user == null)
            {
                // Return Unauthorized status if the credentials are wrong
                return Unauthorized("Invalid username or password");
            }

            // Return user data including role (you might want to return more details)
            return Ok(new
            {
                user.UserName,
                user.Role,
                user.Email
            });
        }
    }
}

