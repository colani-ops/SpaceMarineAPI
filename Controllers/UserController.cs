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

            return Ok(new
            {
                id = user.Id,
                username = user.Username,
                role = user.Role
            });
        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterRequest request)
        {
            var existing = _userService.Authenticate(request.Username, request.Password);
            if (existing != null)
            {
                return Conflict(new { message = "User already exists." });
            }

            // Always create as a normal User by default
            _userService.RegisterUser(request.Username, request.Password, "User");

            return Ok(new { message = "User registered successfully." });
        }



        //GET ALL USERS
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            // Don't send password hashes to frontend
            var result = users.Select(u => new {
                id = u.Id,
                username = u.Username,
                role = u.Role
            });
            return Ok(result);
        }

        //DELETE USER
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _userService.GetAllUsers().FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            if (user.Role == "SuperAdmin")
                return BadRequest(new { message = "Super Admin account cannot be deleted." });

            _userService.DeleteUser(id);
            return Ok(new { message = "User deleted." });
        }

        //UPDATE ROLE
        public class UpdateRoleRequest
        {
            public string Role { get; set; }
        }

        [HttpPut("{id}/role")]
        public IActionResult UpdateRole(int id, [FromBody] UpdateRoleRequest request)
        {
            var user = _userService.GetAllUsers().FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            if (user.Role == "SuperAdmin")
                return BadRequest(new { message = "Cannot change role of SuperAdmin." });

            _userService.UpdateUserRole(id, request.Role);
            return Ok(new { message = "Role updated." });
        }



        //UPDATE PASSWORD
        [HttpPut("{id}/password")]
        public IActionResult UpdatePassword(int id, [FromBody] UpdatePasswordRequest request)
        {
            var hash = UserService.ComputeSha256Hash(request.NewPassword);
            _userService.UpdatePassword(id, hash);
            return Ok(new { message = "Password updated." });
        }

        public class UpdatePasswordRequest
        {
            public string NewPassword { get; set; }
        }



        //UPDATE USERNAME
        [HttpPut("{id}/username")]
        public IActionResult UpdateUsername(int id, [FromBody] UpdateUsernameRequest request)
        {
            _userService.UpdateUsername(id, request.NewUsername);
            return Ok(new { message = "Username updated." });
        }

        public class UpdateUsernameRequest
        {
            public string NewUsername { get; set; }
        }



        //UPDATE PORTRAIT
        [HttpPut("{id}/portrait")]
        public IActionResult UpdatePortrait(int id, [FromBody] UpdatePortraitRequest request)
        {
            _userService.UpdatePortrait(id, request.PortraitImage);
            return Ok(new { message = "Portrait updated." });
        }

        public class UpdatePortraitRequest
        {
            public string PortraitImage { get; set; }
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
