using SFA.Apprenticeships.Web.Common.Constants;

namespace SFA.Apprenticeships.Web.Common.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Providers;
    using Services;
    using StructureMap.Attributes;

    [AuthenticateUser]
    [AuthorizationData]
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

        protected void SetUserMessage(string message, UserMessageLevel level = UserMessageLevel.Success)
        {
            switch (level)
            {
                case UserMessageLevel.Info:
                    UserData.Push(UserMessageConstants.InfoMessage, message);
                    break;
                case UserMessageLevel.Success:
                    UserData.Push(UserMessageConstants.SuccessMessage, message);
                    break;
                case UserMessageLevel.Warning:
                    UserData.Push(UserMessageConstants.WarningMessage, message);
                    break;
                case UserMessageLevel.Error:
                    UserData.Push(UserMessageConstants.ErrorMessage, message);
                    break;
            }
        }
    }
}
