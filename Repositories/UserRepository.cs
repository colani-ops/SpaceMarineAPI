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
                        Role = reader["Role"].ToString(),
                        SquadId = reader["SquadId"] == DBNull.Value ? null : (int?)reader["SquadId"],
                        
                        DisplayName = reader["DisplayName"]?.ToString(),
                        FirstName = reader["FirstName"]?.ToString(),
                        LastName = reader["LastName"]?.ToString(),
                        Age = reader["Age"] == DBNull.Value ? null : (int?)reader["Age"],
                        Experience = reader["Experience"] == DBNull.Value ? null : (int?)reader["Experience"],
                        PortraitImage = reader["PortraitImage"]?.ToString()
                    };
                }
            }

            return user;
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
                        Role = reader["Role"].ToString(),
                        SquadId = reader["SquadId"] == DBNull.Value ? null : (int?)reader["SquadId"],

                        DisplayName = reader["DisplayName"]?.ToString(),
                        FirstName = reader["FirstName"]?.ToString(),
                        LastName = reader["LastName"]?.ToString(),
                        Age = reader["Age"] == DBNull.Value ? null : (int?)reader["Age"],
                        Experience = reader["Experience"] == DBNull.Value ? null : (int?)reader["Experience"],
                        PortraitImage = reader["PortraitImage"]?.ToString()
                    });
                }
            }

            return users;
        }



        public User GetByUserId(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Users WHERE Id = @id", connection);
                command.Parameters.AddWithValue("@id", id);

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new User
                    {
                        Id = (int)reader["Id"],
                        Username = reader["Username"].ToString(),
                        Role = reader["Role"].ToString(),
                        SquadId = reader["SquadId"] == DBNull.Value ? null : (int?)reader["SquadId"],
                        DisplayName = reader["DisplayName"]?.ToString(),
                        FirstName = reader["FirstName"]?.ToString(),
                        LastName = reader["LastName"]?.ToString(),
                        Age = reader["Age"] as int?,
                        Experience = reader["Experience"] as int?,
                        PortraitImage = reader["PortraitImage"]?.ToString()
                    };
                }

                return null;
            }
        }



        public List<User> GetUsersBySquad(int squadId)
        {
            var users = new List<User>();
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var command = new SqlCommand(
                "SELECT * FROM Users WHERE SquadId = @squadId",
                connection
            );
            command.Parameters.AddWithValue("@squadId", squadId);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                users.Add(new User
                {
                    Id = (int)reader["Id"],
                    Username = reader["Username"].ToString(),
                    DisplayName = reader["DisplayName"]?.ToString(),
                    PortraitImage = reader["PortraitImage"]?.ToString(),
                    Role = reader["Role"].ToString()
                });
            }
            return users;
        }



        public void AddUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand(
                    "INSERT INTO Users (Username, PasswordHash, Role, SquadId) VALUES (@username, @passwordHash, @role, @squadId)",
                    connection);

                command.Parameters.AddWithValue("@username", user.Username);
                command.Parameters.AddWithValue("@passwordHash", user.PasswordHash);
                command.Parameters.AddWithValue("@role", user.Role);
                command.Parameters.AddWithValue("@squadId", (object?)user.SquadId ?? DBNull.Value);

                command.ExecuteNonQuery();
            }
        }



        public void UpdateProfile(int id, User updated)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
            UPDATE Users SET
                DisplayName = @displayName,
                FirstName = @firstName,
                LastName = @lastName,
                Age = @age,
                Experience = @experience,
                PortraitImage = @portraitImage
            WHERE Id = @id
        ", connection);

                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@displayName", (object?)updated.DisplayName ?? DBNull.Value);
                command.Parameters.AddWithValue("@firstName", (object?)updated.FirstName ?? DBNull.Value);
                command.Parameters.AddWithValue("@lastName", (object?)updated.LastName ?? DBNull.Value);
                command.Parameters.AddWithValue("@age", (object?)updated.Age ?? DBNull.Value);
                command.Parameters.AddWithValue("@experience", (object?)updated.Experience ?? DBNull.Value);
                command.Parameters.AddWithValue("@portraitImage", (object?)updated.PortraitImage ?? DBNull.Value);

                command.ExecuteNonQuery();
            }
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



        public void UpdateSquad(int id, int? squadId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UPDATE Users SET SquadId = @squadId WHERE Id = @id", connection);
                command.Parameters.AddWithValue("@squadId", (object?)squadId ?? DBNull.Value);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}