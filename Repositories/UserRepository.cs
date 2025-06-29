using Microsoft.Data.SqlClient;
using SpaceMarineAPI.Models;

namespace SpaceMarineAPI.Repositories
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public User? GetUserByUsername(string username)
        {
            User? user = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand("SELECT * FROM Users WHERE Username = @username", connection);
                command.Parameters.AddWithValue("@username", username);

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    user = new User
                    {
                        Id = (int)reader["Id"],
                        Username = reader["Username"].ToString(),
                        PasswordHash = reader["PasswordHash"].ToString(),
                        Role = reader["Role"].ToString()
                    };
                }
            }

            return user;
        }

        public void AddUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand(
                    "INSERT INTO Users (Username, PasswordHash, Role) VALUES (@username, @passwordHash, @role)",
                    connection);

                command.Parameters.AddWithValue("@username", user.Username);
                command.Parameters.AddWithValue("@passwordHash", user.PasswordHash);
                command.Parameters.AddWithValue("@role", user.Role);

                command.ExecuteNonQuery();
            }
        }

        public List<User> GetAllUsers()
        {
            var users = new List<User>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Users", connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(new User
                    {
                        Id = (int)reader["Id"],
                        Username = reader["Username"].ToString(),
                        PasswordHash = reader["PasswordHash"].ToString(),
                        Role = reader["Role"].ToString()
                    });
                }
            }

            return users;
        }

        public void DeleteUser(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Users WHERE Id = @id", connection);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        public void UpdateUserRole(int id, string role)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UPDATE Users SET Role = @role WHERE Id = @id", connection);
                command.Parameters.AddWithValue("@role", role);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }


        public void UpdatePassword(int id, string hash)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UPDATE Users SET PasswordHash = @hash WHERE Id = @id", connection);
                command.Parameters.AddWithValue("@hash", hash);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }


        public void UpdateUsername(int id, string username)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UPDATE Users SET Username = @username WHERE Id = @id", connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }


        public void UpdatePortrait(int id, string filename)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UPDATE Users SET PortraitImage = @portrait WHERE Id = @id", connection);
                command.Parameters.AddWithValue("@portrait", filename);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}
