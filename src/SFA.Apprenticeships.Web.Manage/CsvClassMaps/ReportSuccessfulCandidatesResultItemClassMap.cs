namespace SFA.Apprenticeships.Web.Manage.CsvClassMaps
{
    using CsvHelper.Configuration;
    using Domain.Raa.Interfaces.Reporting.Models;

    public class ReportSuccessfulCandidatesResultItemClassMap : CsvClassMap<ReportSuccessfulCandidatesResultItem>
    {
        public ReportSuccessfulCandidatesResultItemClassMap()
        {
            Map(m => m.Name).Name("Name");
            Map(m => m.Gender).Name("Gender");
            Map(m => m.Postcode).Name("Postcode");
            Map(m => m.LearningProvider).Name("Learning Provider");
            Map(m => m.VacancyReferenceNumber).Name("Vacancy Reference Number");
            Map(m => m.VacancyTitle).Name("Vacancy Title");
            Map(m => m.VacancyType).Name("Vacancy Type");
            Map(m => m.Sector).Name("Sector");
            Map(m => m.Framework).Name("Framework");
            Map(m => m.Employer).Name("Employer");
            Map(m => m.SuccessfulAppDate).Name("Application Successful Date");
            Map(m => m.ILRStartDate).Name("ILR Start Date");
            Map(m => m.ILRReference).Name("ILR Reference");
        }
    }
}