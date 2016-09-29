namespace SFA.Apprenticeships.Web.Recruit.Mediators.Admin
{
    using Common.Mediators;
    using ViewModels.Admin;

    public interface IAdminMediator
    {
        MediatorResponse<TransferVacanciesViewModel> GetVacancyDetails(TransferVacanciesViewModel viewModel);
    }
}