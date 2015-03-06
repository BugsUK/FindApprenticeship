namespace SFA.Apprenticeships.Web.ContactForms.Controllers
{
    using System.Web.Mvc;
    using ContactForms.Constants;

    public class EmployerControllerBase : Controller
    {
        protected void SetPageMessage(string message, UserMessageLevel level = UserMessageLevel.Success)
        {
            switch (level)
            {
                case UserMessageLevel.Info:
                    ViewBag.UserMessageLevel = UserMessageLevel.Info;
                    ViewBag.ConfirmationMessageType = MessageConstants.InfoMessage;
                    ViewBag.ConfirmationMessage = message;
                    break;
                case UserMessageLevel.Success:
                    ViewBag.UserMessageLevel = UserMessageLevel.Success;
                    ViewBag.ConfirmationMessageType = MessageConstants.SuccessMessage;
                    ViewBag.ConfirmationMessage = message;
                    break;
                case UserMessageLevel.Warning:
                    ViewBag.UserMessageLevel = UserMessageLevel.Warning;
                    ViewBag.ConfirmationMessageType = MessageConstants.WarningMessage;
                    ViewBag.ConfirmationMessage = message;
                    break;
                case UserMessageLevel.Error:
                    ViewBag.UserMessageLevel = UserMessageLevel.Error;
                    ViewBag.ConfirmationMessageType = MessageConstants.ErrorMessage;
                    ViewBag.ConfirmationMessage = message;
                    break;
            }
        }
    }
}