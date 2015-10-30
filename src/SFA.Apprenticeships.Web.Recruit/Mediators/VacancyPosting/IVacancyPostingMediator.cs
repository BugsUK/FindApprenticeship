﻿using System;

namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyPosting
{
    using Common.Mediators;
    using ViewModels.Provider;
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;

    public interface IVacancyPostingMediator
    {
        MediatorResponse<EmployerSearchViewModel> GetProviderEmployers(string providerSiteErn);

        MediatorResponse<EmployerSearchViewModel> GetProviderEmployers(EmployerSearchViewModel employerFilterViewModel);

        MediatorResponse<EmployerSearchViewModel> GetEmployers(EmployerSearchViewModel employerFilterViewModel);

        MediatorResponse<ProviderSiteEmployerLinkViewModel> GetEmployer(string providerSiteErn, string ern, Guid vacancyGuid);

        MediatorResponse<ProviderSiteEmployerLinkViewModel> ConfirmEmployer(ProviderSiteEmployerLinkViewModel viewModel);

        MediatorResponse<NewVacancyViewModel> GetNewVacancyViewModel(string ukprn, string providerSiteErn, string ern, Guid vacancyGuid);

        MediatorResponse<NewVacancyViewModel> GetNewVacancyViewModel(long vacancyReferenceNumber);

        MediatorResponse<NewVacancyViewModel> CreateVacancyAndExit(NewVacancyViewModel newVacancyViewModel);

        MediatorResponse<NewVacancyViewModel> CreateVacancy(NewVacancyViewModel newVacancyViewModel);

        MediatorResponse<VacancySummaryViewModel> GetVacancySummaryViewModel(long vacancyReferenceNumber);

        MediatorResponse<VacancySummaryViewModel> UpdateVacancy(VacancySummaryViewModel viewModel, bool acceptWarnings);

        MediatorResponse<VacancySummaryViewModel> UpdateVacancyAndExit(VacancySummaryViewModel viewModel);

        MediatorResponse<VacancyRequirementsProspectsViewModel> GetVacancyRequirementsProspectsViewModel(long vacancyReferenceNumber);

        MediatorResponse<VacancyRequirementsProspectsViewModel> UpdateVacancy(VacancyRequirementsProspectsViewModel viewModel);

        MediatorResponse<VacancyRequirementsProspectsViewModel> UpdateVacancyAndExit(VacancyRequirementsProspectsViewModel viewModel);

        MediatorResponse<VacancyQuestionsViewModel> GetVacancyQuestionsViewModel(long vacancyReferenceNumber);

        MediatorResponse<VacancyQuestionsViewModel> UpdateVacancy(VacancyQuestionsViewModel viewModel);

        MediatorResponse<VacancyQuestionsViewModel> UpdateVacancyAndExit(VacancyQuestionsViewModel viewModel);

        MediatorResponse<VacancyViewModel> GetVacancyViewModel(long vacancyReferenceNumber);

        MediatorResponse<VacancyViewModel> SubmitVacancy(VacancyViewModel viewModel);

        MediatorResponse<SubmittedVacancyViewModel> GetSubmittedVacancyViewModel(long vacancyReferenceNumber);

        MediatorResponse<EmployerSearchViewModel> SelectNewEmployer(EmployerSearchViewModel viewModel);
    }
}