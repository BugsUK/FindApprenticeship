namespace SFA.Apprenticeships.Domain.Entities
{
    public class EmployerBroadsytemRecord
    {
        public PrimaryContactName PrimaryContactName { get; set; }
        public string PrimaryContactEmailAddress { get; set; }
        public string PrimaryContactLandlineTelephoneNumber { get; set; }
        public string PrimaryContactFaxnumber { get; set; }
        public string PrimaryContactMobileTelephoneNumber { get; set; }
        public string Question { get; set; }
        public string EmployerName { get; set; }
        public AddressDetails AddressDetails { get; set; }

        public string NumberOfEmployees { get; set; }
        public string Sector { get; set; }
        public string PreviousExperience { get; set; }
        public string HowDidYOuhearaboutWebsite { get; set; }
    }

    public class LscRegionDetails
    {
        public string FullName { get; set; }
    }

    public class AddressDetails
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string PostCode { get; set; }
        public string County { get; set; }
        public string Town { get; set; }
        public LscRegionDetails LscRegionDetails { get; set; }
    }

    public class PrimaryContactName
    {
        public string Title { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string Position { get; set; }
    }
}