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
                var command = new SqlCommand(
                    "INSERT INTO Squads (Name, Type, PortraitImage) VALUES (@name, @type, @portraitImage)",
                connection);

                command.Parameters.AddWithValue("@name", squad.Name);
                command.Parameters.AddWithValue("@type", squad.Type);
                command.Parameters.AddWithValue("@portraitImage", squad.PortraitImage ?? "");
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
                            Type = reader["Type"].ToString(),
                            PortraitImage = reader["PortraitImage"] == DBNull.Value ? null : reader["PortraitImage"].ToString()
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
                var command = new SqlCommand("SELECT Id, Name, Type, PortraitImage FROM Squads", connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    squads.Add(new Squad
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        Type = reader["Type"].ToString(),
                        PortraitImage = reader["PortraitImage"] == DBNull.Value ? null : reader["PortraitImage"].ToString()
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
                    "UPDATE Squads SET Name = @name, Type = @type, PortraitImage = @portraitImage WHERE Id = @id",
                   connection);

                command.Parameters.AddWithValue("@name", squad.Name);
                command.Parameters.AddWithValue("@type", squad.Type);
                command.Parameters.AddWithValue("@portraitImage", squad.PortraitImage ?? "");
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
