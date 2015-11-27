namespace SFA.Apprenticeships.Web.Recruit.ViewModels
{
    public class ValidationResultViewModel
    {
        public const string PartialView = "_ValidationResultIcons";

        public ValidationResultViewModel(bool hasError, string error, bool hasWarning, string warning, string viewValidationResultUrl, string comment)
        {
            HasError = hasError;
            Error = error;
            HasWarning = hasWarning;
            Warning = warning;
            ViewValidationResultUrl = viewValidationResultUrl;
            CommentViewModel = new CommentViewModel(comment, viewValidationResultUrl);
        }

        public bool HasError { get; private set; }

        public string Error { get; private set; }

        public bool HasWarning { get; private set; }

        public string Warning { get; private set; }

        public string ViewValidationResultUrl { get; private set; }

        public CommentViewModel CommentViewModel { get; private set; }
    }
}