namespace SFA.Apprenticeships.Domain.Entities.Users
{
    public class Team
    {
        public int Id { get; set; }
        public string CodeName { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
    }
}