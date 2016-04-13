namespace SFA.Apprenticeships.Web.Recruit.Mediators.Home
{
    using System;

    using Common.Mediators;
    using ViewModels.Home;

    public interface IHomeMediator
    {
        MediatorResponse<ContactMessageViewModel> SendContactMessage(string username, ContactMessageViewModel registerViewModel);

        MediatorResponse<ContactMessageViewModel> GetContactMessageViewModel(string username);       
    }
}