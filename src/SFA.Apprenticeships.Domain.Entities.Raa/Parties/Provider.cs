namespace SFA.Apprenticeships.Domain.Entities.Raa.Parties
{
    public class Provider
    {
        public int ProviderId { get; set; }
        public string Ukprn { get; set; }
        public string FullName { get; set; }
        public string TradingName { get; set; }
        public bool IsMigrated { get; set; }
    }
}
