namespace SpaceMarineAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } //Marine, Sergeant, Lieutenant, Captain, SuperAdmin
        public int? SquadId { get; set; }

        public string? DisplayName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Age { get; set; }
        public int? Experience { get; set; }
        public string? PortraitImage { get; set; }
    }
}
