using Microsoft.Data.SqlClient;
using SpaceMarineAPI.Models;

namespace SpaceMarineAPI.Repositories
{
    public class SpaceMarineRepository
    {
        private readonly string _connectionString;

        public SpaceMarineRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void AddMarine(SpaceMarine marine)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "INSERT INTO SpaceMarines (FirstName, LastName, Age, Experience, SquadId, PortraitImage) " +
                    "VALUES (@firstName, @lastName, @age, @experience, @squadId, @portraitImage)",
                    connection
                );

                command.Parameters.AddWithValue("@firstName", marine.FirstName);
                command.Parameters.AddWithValue("@lastName", marine.LastName);
                command.Parameters.AddWithValue("@age", marine.Age);
                command.Parameters.AddWithValue("@experience", marine.Experience);
                command.Parameters.AddWithValue("@squadId", marine.SquadId);
                command.Parameters.AddWithValue("@portraitImage", marine.PortraitImage ?? "");

                command.ExecuteNonQuery();
            }
        }

        public List<SpaceMarine> GetMarinesBySquad(int squadId)
        {
            var marines = new List<SpaceMarine>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM SpaceMarines WHERE SquadId = @squadId", connection);
                command.Parameters.AddWithValue("@squadId", squadId);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    marines.Add(new SpaceMarine
                    {
                        Id = (int)reader["Id"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Age = (int)reader["Age"],
                        Experience = (int)reader["Experience"],
                        SquadId = (int)reader["SquadId"],
                        PortraitImage = reader["PortraitImage"].ToString()
                    });
                }
            }

            return marines;
        }

        public void UpdateMarine(int id, SpaceMarine marine)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "UPDATE SpaceMarines SET FirstName = @firstName, LastName = @lastName, Age = @age, " +
                    "Experience = @experience, SquadId = @squadId, PortraitImage = @portraitImage WHERE Id = @id",
                    connection
                );

                command.Parameters.AddWithValue("@firstName", marine.FirstName);
                command.Parameters.AddWithValue("@lastName", marine.LastName);
                command.Parameters.AddWithValue("@age", marine.Age);
                command.Parameters.AddWithValue("@experience", marine.Experience);
                command.Parameters.AddWithValue("@squadId", marine.SquadId);
                command.Parameters.AddWithValue("@portraitImage", marine.PortraitImage ?? "");
                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteMarine(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM SpaceMarines WHERE Id = @id", connection);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

    }
}
