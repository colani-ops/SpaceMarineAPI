using Microsoft.Data.SqlClient;
using SpaceMarineAPI.Models;

namespace SpaceMarineAPI.Repositories
{
    public class SquadRepository
    {
        private readonly string _connectionString;

        public SquadRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }


        public void AddSquad(Squad squad)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO Squads (Name, Type) VALUES (@name, @type)", connection);
                command.Parameters.AddWithValue("@name", squad.Name);
                command.Parameters.AddWithValue("@type", squad.Type);
                command.ExecuteNonQuery();
            }
        }



        public Squad GetSquadById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Squads WHERE Id = @id", connection);
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Squad
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Type = reader["Type"].ToString()
                        };
                    }
                }
            }
            return null;
        }



        public List<Squad> GetAllSquads()
        {
            var squads = new List<Squad>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Squads", connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    squads.Add(new Squad
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        Type = reader["Type"].ToString()
                    });
                }
            }

            return squads;
        }


        public void UpdateSquad(int id, Squad squad)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "UPDATE Squads SET Name = @name, Type = @type WHERE Id = @id", connection);

                command.Parameters.AddWithValue("@name", squad.Name);
                command.Parameters.AddWithValue("@type", squad.Type);
                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();
            }
        }


        public void DeleteSquad(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Squads WHERE Id = @id", connection);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}
