namespace SFA.Apprenticeships.Domain.Entities.Raa.Parties
{
    using System;
    using Locations;

    public class Employer
    {
        public int EmployerId { get; set; }
        //TODO: Remove once mongo repos have been retired
        public Guid EmployerGuid { get; set; }
        public string EdsUrn { get; set; }
        public string FullName { get; set; }
        public string TradingName { get; set; }
        public int PrimaryContact { get; set; }
        public PostalAddress Address { get; set; }
        public bool IsPositiveAboutDisability { get; set; }
    }
}
