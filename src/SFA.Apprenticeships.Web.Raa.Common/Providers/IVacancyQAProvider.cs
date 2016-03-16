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

        QAActionResult ApproveVacancy(int vacancyReferenceNumber);

        QAActionResult RejectVacancy(int vacancyReferenceNumber);

        VacancyViewModel ReserveVacancyForQA(int vacancyReferenceNumber);

        NewVacancyViewModel GetNewVacancyViewModel(int vacancyReferenceNumber);

        TrainingDetailsViewModel GetTrainingDetailsViewModel(int vacancyReferenceNumber);

        FurtherVacancyDetailsViewModel GetVacancySummaryViewModel(int vacancyReferenceNumber);

        VacancyRequirementsProspectsViewModel GetVacancyRequirementsProspectsViewModel(int vacancyReferenceNumber);

        VacancyQuestionsViewModel GetVacancyQuestionsViewModel(int vacancyReferenceNumber);

        FurtherVacancyDetailsViewModel UpdateVacancyWithComments(FurtherVacancyDetailsViewModel viewModel);

        NewVacancyViewModel UpdateVacancyWithComments(NewVacancyViewModel viewModel);

        TrainingDetailsViewModel UpdateVacancyWithComments(TrainingDetailsViewModel viewModel);

        VacancyRequirementsProspectsViewModel UpdateVacancyWithComments(VacancyRequirementsProspectsViewModel viewModel);
        
        VacancyQuestionsViewModel UpdateVacancyWithComments(VacancyQuestionsViewModel viewModel);

        List<SelectListItem> GetSectorsAndFrameworks();

        List<StandardViewModel> GetStandards();

        List<SelectListItem> GetSectors();

        void RemoveLocationAddresses(Guid vacancyGuid);

        NewVacancyViewModel UpdateEmployerInformationWithComments(NewVacancyViewModel existingVacancy);

        LocationSearchViewModel LocationAddressesViewModel(string ukprn, int providerSiteId, int employerId, Guid vacancyGuid);

        LocationSearchViewModel AddLocations(LocationSearchViewModel viewModel);

        VacancyViewModel GetVacancy(Guid vacancyGuid);

        VacancyViewModel GetVacancy(int vacancyReferenceNumber);
    }
}
