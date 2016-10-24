namespace SFA.Apprenticeships.Web.Raa.Common.CsvClassMaps
{
    using CsvHelper.Configuration;
    using ViewModels.Admin;

    public sealed class FrameworkDataClassMap : CsvClassMap<FrameworkViewModel>
    {
        public FrameworkDataClassMap()
        {
            Map(m => m.FrameworkId).Name("Id");
            Map(m => m.SSAT1Name).Name("SSAT1");
            Map(m => m.FrameworkFullName).Name("Framework");
            Map(m => m.FrameworkStatus).Name("Status");
        }
    }
}