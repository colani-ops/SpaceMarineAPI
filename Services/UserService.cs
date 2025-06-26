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

        public User? Authenticate(string username, string password)
        {
            var user = _userRepository.GetUserByUsername(username);
            if (user == null) return null;

            //  Skip hashing for now, compare plain password
            return user.PasswordHash == password ? user : null;
        }


        public void RegisterUser(string username, string password, string role)
        {
            var user = new User
            {
                Username = username,
                PasswordHash = password, // Save plain password
                Role = role
            };

            _userRepository.AddUser(user);
        }

        /*private static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Use UTF8 — standard in most web apps and online tools
                byte[] bytes = Encoding.UTF8.GetBytes(rawData);

                // Compute the hash
                byte[] hash = sha256.ComputeHash(bytes);

                // Convert to hex string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    builder.Append(hash[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }*/
    }
}
