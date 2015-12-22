namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using ViewModels;
    using ViewModels.Vacancy;

    public interface IVacancyQAProvider
    {
        List<DashboardVacancySummaryViewModel> GetPendingQAVacanciesOverview();

        List<DashboardVacancySummaryViewModel> GetPendingQAVacancies();

        void ApproveVacancy(long vacancyReferenceNumber);

        void RejectVacancy(long vacancyReferenceNumber);

        VacancyViewModel ReserveVacancyForQA(long vacancyReferenceNumber);

        NewVacancyViewModel GetNewVacancyViewModel(long vacancyReferenceNumber);

        VacancySummaryViewModel GetVacancySummaryViewModel(long vacancyReferenceNumber);

        VacancyRequirementsProspectsViewModel GetVacancyRequirementsProspectsViewModel(long vacancyReferenceNumber);

        VacancyQuestionsViewModel GetVacancyQuestionsViewModel(long vacancyReferenceNumber);

        VacancySummaryViewModel UpdateVacancyWithComments(VacancySummaryViewModel viewModel);

        NewVacancyViewModel UpdateVacancyWithComments(NewVacancyViewModel viewModel);

        VacancyRequirementsProspectsViewModel UpdateVacancyWithComments(VacancyRequirementsProspectsViewModel viewModel);

        VacancyQuestionsViewModel UpdateVacancyWithComments(VacancyQuestionsViewModel viewModel);

        List<SelectListItem> GetSectorsAndFrameworks();

        List<StandardViewModel> GetStandards();

        void RemoveLocationAddresses(Guid vacancyGuid);

        NewVacancyViewModel UpdateEmployerInformationWithComments(NewVacancyViewModel existingVacancy);
    }
}
