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

            _userService.RegisterUser(request.Username, request.Password, request.Role ?? "Marine", request.SquadId);

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
                role = u.Role,
                squadId = u.SquadId
            });
            return Ok(result);
        }



        //GET USER BY ID
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            var user = _userService.GetByUserId(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }



        //GET USER BY SQUAD ID
        [HttpGet("bysquad/{squadId}")]
        public IActionResult GetUsersBySquad(int squadId)
        {
            var users = _userService.GetUsersBySquad(squadId);
            return Ok(users);
        }



        //UPDATE USER
        public class UpdateUserProfileRequest
        {
            public string DisplayName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int? Age { get; set; }
            public int? Experience { get; set; }
            public string PortraitImage { get; set; }
        }

        [HttpPut("{id}/profile")]
        public IActionResult UpdateProfile(int id, [FromBody] UpdateUserProfileRequest request)
        {
            var user = _userService.GetByUserId(id);
            if (user == null) return NotFound();

            user.DisplayName = request.DisplayName;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Age = request.Age;
            user.Experience = request.Experience;
            user.PortraitImage = request.PortraitImage;

            _userService.UpdateProfile(id, user);
            return NoContent();
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
        public class UpdatePasswordRequest
        {
            public string NewPassword { get; set; }
        }

        [HttpPut("{id}/password")]
        public IActionResult UpdatePassword(int id, [FromBody] UpdatePasswordRequest request)
        {
            var hash = UserService.ComputeSha256Hash(request.NewPassword);
            _userService.UpdatePassword(id, hash);
            return Ok(new { message = "Password updated." });
        }




        //UPDATE USERNAME
        public class UpdateUsernameRequest
        {
            public string NewUsername { get; set; }
        }
        [HttpPut("{id}/username")]
        public IActionResult UpdateUsername(int id, [FromBody] UpdateUsernameRequest request)
        {
            _userService.UpdateUsername(id, request.NewUsername);
            return Ok(new { message = "Username updated." });
        }



        //UPDATE SQUAD
        public class UpdateSquadRequest
        {
            public int? SquadId { get; set; }
        }

        [HttpPut("{id}/squad")]
        public IActionResult UpdateSquad(int id, [FromBody] UpdateSquadRequest request)
        {
            _userService.UpdateUserSquad(id, request.SquadId);
            return Ok(new { message = "Squad updated." });
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
        public string Role { get; set; }
        public int? SquadId { get; set; }
    }
}
