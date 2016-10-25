namespace SFA.Apprenticeships.Web.Raa.Common.CsvClassMaps
{
    using CsvHelper.Configuration;
    using ViewModels.Admin;

    public sealed class StandardSubjectAreaTierOneClassMap : CsvClassMap<StandardSubjectAreaTierOneViewModel>
    {
        public StandardSubjectAreaTierOneClassMap()
        {
            Map(m => m.StandardId).Name("ID");
            Map(m => m.SSAT1Name).Name("SSAT1");
            Map(m => m.StandardSectorName).Name("Standard Sector");
            Map(m => m.StandardName).Name("Standard");
        }
    }
}