namespace SFA.Apprenticeships.Web.Recruit.ViewModels
{
    public class EditLinkViewModel
    {
        public const string PartialView = "_EditLink";

        public EditLinkViewModel(string editUrl, string comment)
        {
            EditUrl = editUrl;
            CommentViewModel = new CommentViewModel(comment, editUrl);
        }

        public string EditUrl { get; private set; }

        public CommentViewModel CommentViewModel { get; private set; }
    }
}