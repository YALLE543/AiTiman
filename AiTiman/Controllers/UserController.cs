using AiTiman_API.Services.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AiTiman_API.Services.Interfaces;
using System.Threading.Tasks;

namespace AiTiman_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUser _User;

        // Constructor injection of the IUser service
        public UserController(IUser User)
        {
            _User = User;
        }

        [HttpGet("All-Users")]
        public async Task<IActionResult> AllUser()
        {
            var User = await _User.fetchUser();
            return Ok(User);
        }

        [HttpGet("Fetch-User")]
        public async Task<IActionResult> FetchUser(string id)
        {
            var User = await _User.fetchUser(id);
            return Ok(User);
        }

        [HttpPost("Create-New-User")]
        public async Task<IActionResult> CreateNewUser(CreateUserDTO createUser)
        {
            var (isSuccess, message) = await _User.AddNewUser(createUser);

            if (!isSuccess)
                return BadRequest(message);

            return Ok(message);
        }

        [HttpPut("Update-User")]
        public async Task<IActionResult> UpdateUser(string id, UpdateUserDTO updateUser)
        {
            var (isSuccess, message) = await _User.UpdateUser(id, updateUser);

            if (!isSuccess)
                return BadRequest(message);

            return Ok(message);
        }


        [HttpGet("GetUserProfile/{userName}")]
        public async Task<IActionResult> GetUserProfile(string userName)
        {
            var user = await _User.GetUserProfileByUsername(userName);
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

        [HttpDelete("Delete-User")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var (isSuccess, message) = await _User.DeleteUser(id);

            if (!isSuccess)
                return BadRequest(message);

            return Ok(message);
        }

        [HttpPost("ValidateLogin")]
        public async Task<IActionResult> ValidateLogin(LoginDTO login)
        {
            // Assuming _User has a method to validate credentials
            var user = await _User.ValidateUser(login.UserName, login.Password);

            if (user == null)
            {
                // Return Unauthorized status if the credentials are wrong
                return Unauthorized("Invalid username or password");
            }

            // Return user data including role (you might want to return more details)
            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Role,
                user.Email
            });
        }
    }
}