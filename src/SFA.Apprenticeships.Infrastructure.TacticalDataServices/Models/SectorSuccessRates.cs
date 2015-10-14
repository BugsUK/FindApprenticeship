namespace SFA.Apprenticeships.Infrastructure.TacticalDataServices.Models
{
    public class SectorSuccessRates
    {
        public int ProviderID { get; set; }
        public int SectorID { get; set; }
        public int PassRate { get; set; }
        public bool New { get; set; }
    }
}