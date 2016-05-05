namespace SFA.Apprenticeships.Web.Manage.CsvClassMaps
{
    using CsvHelper.Configuration;
    using Domain.Raa.Interfaces.Reporting.Models;

    public sealed class ReportVacancyExtensionsResultItemClassMap : CsvClassMap<ReportVacancyExtensionsResultItem>
    { 
        public ReportVacancyExtensionsResultItemClassMap()
        {
            Map(m => m.VacancyReferenceNumber).Name("Vacancy Reference Number");
            Map(m => m.VacancyTitle).Name("Vacancy Title");
            Map(m => m.VacancyStatus).Name("Vacancy Status");
            Map(m => m.ProviderName).Name("Provider Name");
            Map(m => m.EmployerName).Name("Employer Name");
            Map(m => m.NumberOfVacancyExtensions).Name("Number Of Vacancy Extensions");
            Map(m => m.OriginalPostingDate).Name("Original Posting Date");
            Map(m => m.OriginalClosingDate).Name("Original Closing Date");
            Map(m => m.CurrentClosingDate).Name("Current Closing Date");
            Map(m => m.NumberOfSubmittedApplications).Name("Number Of Submitted Applications");
        }
    }
}