using Microsoft.AspNetCore.Mvc;
using SpaceMarineAPI.Services;
using SpaceMarineAPI.Models;

namespace SpaceMarineAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginRequest request)
        {
            var user = _userService.Authenticate(request.Username, request.Password);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            return Ok(new { message = "Login successful", user.Username, user.Role });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterRequest request)
        {
            _userService.RegisterUser(request.Username, request.Password, "User");
            return Ok(new { message = "Registration successful!" });
        }
    }

    public class UserLoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserRegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
