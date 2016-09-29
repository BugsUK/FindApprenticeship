namespace SFA.Apprenticeships.Web.Recruit.Mediators.Admin
{
    using Common.Mediators;
    using System.Collections.Generic;
    using ViewModels.Admin;

    public interface IAdminMediator
    {
        MediatorResponse<VacancyDetailsViewModel> GetVacancyDetails(IList<string> vacancyReferences);
    }
}