namespace SFA.Apprenticeships.Domain.Entities
{
    public class AccessRequest
    {
        public string UserType { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Companyname { get; set; }
        public string Position { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public bool HasApprenticeshipVacancies { get; set; }
        public Address Address { get; set; }
        public string ServiceTypeIds { get; set; }
        public string Contactname { get; set; }
        public string AdditionalPhoneNumber { get; set; }
        public string AdditionalEmail { get; set; }
        public string Systemname { get; set; } 
    }
}