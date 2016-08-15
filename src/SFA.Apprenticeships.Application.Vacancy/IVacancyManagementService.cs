namespace SFA.Apprenticeships.Application.Vacancy
{
    using Domain.Entities.Raa.Vacancies;
    using Interfaces.Service;

    public interface IVacancyManagementService
    {
        IServiceResult Delete(int vacancyId);
        IServiceResult<VacancySummary> FindSummary(int vacancyId);
    }

    public class VacancyManagementServiceCodes
    {
        public class Delete
        {
            public const string Ok = "VacancyManagement.Delete.Ok";
            public const string VacancyInIncorrectState = "Vacancy.Delete.VacancyInIncorrectState";
        }

        public class FindSummary
        {
            public const string Ok = "VacancyManagement.FindSummary.Ok";
            public const string NotFound = "VacancyManagement.FindSummary.NotFound";
        }
    }
}