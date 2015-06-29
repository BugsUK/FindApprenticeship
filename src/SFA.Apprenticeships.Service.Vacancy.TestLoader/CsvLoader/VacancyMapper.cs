namespace SFA.Apprenticeships.Service.Vacancy.TestLoader.CsvLoader
{
    using Application.Vacancies.Entities;
    using CsvHelper.Configuration;

    public sealed class VacancyMapper : CsvClassMap<ApprenticeshipSummaryUpdate>
    {
        public VacancyMapper()
        {
            Map(m => m.Id).Name("Test ID");
            Map(m => m.Title).Name("Title");
            Map(m => m.Description).Name("Description");
            Map(m => m.EmployerName).Name("Employer");
            Map(m => m.ClosingDate).Name("Closing date");
        }
    }
}
