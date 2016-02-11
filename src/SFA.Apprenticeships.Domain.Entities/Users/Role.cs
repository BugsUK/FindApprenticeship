namespace SFA.Apprenticeships.Domain.Entities.Users
{
    using System.Net.NetworkInformation;

    public class Role
    {
        public int Id { get; set; }
        public string CodeName { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }

        public const string CodeNameTechnicalAdviser = "TCA";
        public const string CodeNameHelpdeskAdviser = "HDA";
        public const string CodeNameQAAdviser = "QAA";
    }
}