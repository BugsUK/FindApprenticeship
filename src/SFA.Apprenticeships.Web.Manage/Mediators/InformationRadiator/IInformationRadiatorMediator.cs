namespace SFA.Apprenticeships.Web.Manage.Mediators.InformationRadiator
{
    using Common.Mediators;
    using ViewModels;

    public interface IInformationRadiatorMediator
    {
        MediatorResponse<InformationRadiatorViewModel> GetInformationRadiatorViewModel();
    }
}