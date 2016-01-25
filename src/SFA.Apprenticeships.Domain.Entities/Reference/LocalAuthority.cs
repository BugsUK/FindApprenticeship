namespace SFA.Apprenticeships.Domain.Entities.Reference
{
    public class LocalAuthority
    {
        public int LocalAuthorityId { get; set; }

        public string CodeName { get; set; }

        public string ShortName { get; set; }

        public string FullName { get; set; }

        public County County { get; set; }
    }
}