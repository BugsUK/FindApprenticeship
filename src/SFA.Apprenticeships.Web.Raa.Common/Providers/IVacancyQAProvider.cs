namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;

    public interface IVacancyQAProvider
    {
        DashboardVacancySummariesViewModel GetPendingQAVacanciesOverview(DashboardVacancySummariesSearchViewModel searchViewModel);

        List<DashboardVacancySummaryViewModel> GetPendingQAVacancies();

        void ApproveVacancy(int vacancyReferenceNumber);

        void RejectVacancy(int vacancyReferenceNumber);

        VacancyViewModel ReserveVacancyForQA(int vacancyReferenceNumber);

        NewVacancyViewModel GetNewVacancyViewModel(int vacancyReferenceNumber);

        TrainingDetailsViewModel GetTrainingDetailsViewModel(int vacancyReferenceNumber);

        VacancySummaryViewModel GetVacancySummaryViewModel(int vacancyReferenceNumber);

        VacancyRequirementsProspectsViewModel GetVacancyRequirementsProspectsViewModel(int vacancyReferenceNumber);

        VacancyQuestionsViewModel GetVacancyQuestionsViewModel(int vacancyReferenceNumber);

        VacancySummaryViewModel UpdateVacancyWithComments(VacancySummaryViewModel viewModel);

        NewVacancyViewModel UpdateVacancyWithComments(NewVacancyViewModel viewModel);

        TrainingDetailsViewModel UpdateVacancyWithComments(TrainingDetailsViewModel viewModel);

        VacancyRequirementsProspectsViewModel UpdateVacancyWithComments(VacancyRequirementsProspectsViewModel viewModel);

        VacancyQuestionsViewModel UpdateVacancyWithComments(VacancyQuestionsViewModel viewModel);

        List<SelectListItem> GetSectorsAndFrameworks();

        List<StandardViewModel> GetStandards();

        void RemoveLocationAddresses(Guid vacancyGuid);

        NewVacancyViewModel UpdateEmployerInformationWithComments(NewVacancyViewModel existingVacancy);

        LocationSearchViewModel LocationAddressesViewModel(string ukprn, string providerSiteErn, string ern, Guid vacancyGuid);

        LocationSearchViewModel AddLocations(LocationSearchViewModel viewModel);

        VacancyViewModel GetVacancy(Guid vacancyGuid);
    }
}
