namespace SFA.Apprenticeships.Web.Common.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Providers;
    using Services;
    using StructureMap.Attributes;

    [AuthenticateUser]
    public abstract class ControllerBase<TContextType> : ControllerBase, IUserController<TContextType> where TContextType : UserContext
    {
        public TContextType UserContext { get; protected set; }
    }

    public abstract class ControllerBase : Controller
    {
        [SetterProperty]
        public IUserDataProvider UserData { get; set; }

        [SetterProperty]
        public IAuthenticationTicketService AuthenticationTicketService { get; set; }
    }
}
