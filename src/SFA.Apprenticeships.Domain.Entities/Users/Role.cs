namespace SFA.Apprenticeships.Domain.Entities.Users
{
    public class Role
    {
        public string Id { get; set; }
        public string CodeName { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
    }
}