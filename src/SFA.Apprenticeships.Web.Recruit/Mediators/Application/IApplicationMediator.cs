﻿namespace SFA.Apprenticeships.Web.Recruit.Mediators.Application
{
    using Common.Mediators;
    using Raa.Common.ViewModels.Application;

    public interface IApplicationMediator
    {
        MediatorResponse<VacancyApplicationsViewModel> GetVacancyApplicationsViewModel(VacancyApplicationsSearchViewModel vacancyApplicationsSearch);
        MediatorResponse<ShareApplicationsViewModel> ShareApplications(int vacancyReferenceNumber);
        MediatorResponse<ShareApplicationsViewModel> ShareApplications(ShareApplicationsViewModel vacancyReferenceNumber);
    }
}