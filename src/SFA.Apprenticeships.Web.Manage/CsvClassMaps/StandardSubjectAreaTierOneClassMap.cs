namespace SFA.Apprenticeships.Web.Manage.CsvClassMaps
{
    using CsvHelper.Configuration;
    using ViewModels.Admin;

    public sealed class StandardSubjectAreaTierOneClassMap : CsvClassMap<StandardSubjectAreaTierOneViewModel>
    {
        public StandardSubjectAreaTierOneClassMap()
        {
            Map(m => m.StandardId);
            Map(m => m.SSAT1Name).Name("SSAT1");
            Map(m => m.StandardSectorName).Name("StandardSectorName");
            Map(m => m.StandardName).Name("StandardName");
        }
    }
}