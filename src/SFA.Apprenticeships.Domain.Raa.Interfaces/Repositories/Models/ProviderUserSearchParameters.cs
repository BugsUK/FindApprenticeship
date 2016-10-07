namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories.Models
{
    public class ProviderUserSearchParameters
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool AllUnverifiedEmails { get; set; }
    }
}