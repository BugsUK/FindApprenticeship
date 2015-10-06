namespace SFA.Apprenticeships.Infrastructure.TacticalDataServices.Models
{
    using System;

    public class Provider
    {
        public int ProviderID { get; set; }
        public int UPIN { get; set; }
        public int UKPRN { get; set; }
        public string FullName { get; set; }
        public string TradingName { get; set; }
        public bool IsContracted { get; set; }
        public DateTime ContractedFrom { get; set; }
        public DateTime ContractedTo { get; set; }
        public int ProviderStatusTypeID { get; set; }
        public bool IsNASProvider { get; set; }
        public int OriginalUPIN { get; set; }
    }
}