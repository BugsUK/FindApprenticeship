namespace SFA.Apprenticeships.Web.Manage.CsvClassMaps
{
    using CsvHelper.Configuration;
    using Domain.Raa.Interfaces.Reporting.Models;

    public class ReportVacanciesResultItemClassMap : CsvClassMap<ReportVacanciesResultItem>
    {
        public ReportVacanciesResultItemClassMap()
        {
            Map(m => m.VacancyTitle).Name("Vacancy_Title");
            Map(m => m.VacancyType).Name("Vacancy_Type");
            Map(m => m.Reference).Name("Reference");
            Map(m => m.EmployerName).Name("Employer_Name");
            Map(m => m.EmployerAnonymousName).Name("Employer_Anonymous_Name");
            Map(m => m.IsEmployerAnonymous).Name("Anonymous_Employer");
            Map(m => m.Postcode).Name("Postcode");
            Map(m => m.Sector).Name("Sector");
            Map(m => m.Framework).Name("Framework");
            Map(m => m.LearningProvider).Name("Learning_Provider");
            Map(m => m.NumberOfPositions).Name("Number_of_Vacancies");
            Map(m => m.NoOfPositionsAvailable).Name("Number_of_Vacancies_Available");
            Map(m => m.DatePosted).Name("Date_Posted");
            Map(m => m.ClosingDate).Name("Closing_Date");
            Map(m => m.NoOfApplications).Name("No_Of_Applications");
            Map(m => m.Status).Name("Status");
        }
    }
}