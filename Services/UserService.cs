using SpaceMarineAPI.Models;
using SpaceMarineAPI.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace SpaceMarineAPI.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;


        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public List<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public void DeleteUser(int id)
        {
            _userRepository.DeleteUser(id);
        }

        public void UpdateUserRole(int id, string role)
        {
            _userRepository.UpdateUserRole(id, role);
        }



        public User? Authenticate(string username, string password)
        {
            var user = _userRepository.GetUserByUsername(username);
            if (user == null) return null;

            var hashedInput = ComputeSha256Hash(password);

            return user.PasswordHash == hashedInput ? user : null;
        }


        public void RegisterUser(string username, string password, string role)
        {
            var hashed = ComputeSha256Hash(password);

            var user = new User
            {
                Username = username,
                PasswordHash = hashed,
                Role = role
            };

            _userRepository.AddUser(user);
        }



        public static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(rawData);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder builder = new StringBuilder();
                foreach (var b in hash)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }



        public void UpdatePassword(int id, string hash)
        {
            _userRepository.UpdatePassword(id, hash);
        }

        public void UpdateUsername(int id, string username)
        {
            _userRepository.UpdateUsername(id, username);
        }

        public void UpdatePortrait(int id, string filename)
        {
            _userRepository.UpdatePortrait(id, filename);
        }
    }
}
