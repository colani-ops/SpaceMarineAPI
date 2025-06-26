namespace SpaceMarineAPI.Models
{
    public class SpaceMarine
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public int Experience { get; set; }
        public int SquadId { get; set; } // FK to Squad.Id
        public string? PortraitImage { get; set; } // null for now — later file upload
    }
}
