namespace SFA.Apprenticeships.Web.Manage.CsvClassMaps
{
    using CsvHelper.Configuration;
    using Domain.Raa.Interfaces.Reporting.Models;

    public sealed class ReportVacancyTrackerResultItemClassMap : CsvClassMap<ReportVacancyTrackerResultItem>
    {
        public ReportVacancyTrackerResultItemClassMap()
        {
            Map(m => m.OutComeDate).Name("Outcome_Date");
            Map(m => m.Outcome).Name("Outcome");
            Map(m => m.Reference).Name("Reference");
            Map(m => m.ProviderName).Name("Provider_Name");
            Map(m => m.QAUserName).Name("Adviser_Username");
            Map(m => m.DateSubmitted).Name("Date_Submitted_or_Resubmitted");
        }
    }
}