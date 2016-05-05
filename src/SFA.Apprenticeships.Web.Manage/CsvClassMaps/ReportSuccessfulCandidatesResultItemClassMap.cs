namespace SFA.Apprenticeships.Web.Manage.CsvClassMaps
{
    using CsvHelper.Configuration;
    using Domain.Raa.Interfaces.Reporting.Models;

    public sealed class ReportSuccessfulCandidatesResultItemClassMap : CsvClassMap<ReportSuccessfulCandidatesResultItem>
    {
        public ReportSuccessfulCandidatesResultItemClassMap()
        {
            Map(m => m.Name).Name("Candidate_Name");
            Map(m => m.Gender).Name("Gender");
            Map(m => m.Postcode).Name("Postcode");
            Map(m => m.LearningProvider).Name("Learning_Provider");
            Map(m => m.VacancyReferenceNumber).Name("Vacancy_Reference_Number");
            Map(m => m.VacancyTitle).Name("Vacancy_Title");
            Map(m => m.VacancyType).Name("Vacancy_Type");
            Map(m => m.Sector).Name("Sector");
            Map(m => m.Framework).Name("Framework");
            Map(m => m.Employer).Name("Employer");
            Map(m => m.SuccessfulAppDate).Name("Successful_App_Date");
            Map(m => m.ILRStartDate).Name("ILR_Start_Date");
        }
    }
}