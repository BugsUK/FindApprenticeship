namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    public class ValidationResultViewModel
    {
        public const string PartialView = "_ValidationResultIcons";

        public ValidationResultViewModel(string anchorName, bool hasError, string error, bool hasWarning, string warning, string viewValidationResultUrl, string comment)
        {
            HasError = hasError;
            Error = error;
            HasWarning = hasWarning;
            Warning = warning;
            ViewValidationResultUrl = viewValidationResultUrl;
            AnchorName = anchorName;
            CommentViewModel = new CommentViewModel(comment, viewValidationResultUrl);
        }

        public string AnchorName { get; private set; }

        public bool HasError { get; private set; }

        public string Error { get; private set; }

        public bool HasWarning { get; private set; }

        public string Warning { get; private set; }

        public string ViewValidationResultUrl { get; private set; }

        public CommentViewModel CommentViewModel { get; private set; }
    }
}