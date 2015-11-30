namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    public class CommentViewModel
    {
        public const string PartialView = "_CommentIcon";

        public CommentViewModel(string comment, string viewCommentUrl)
        {
            Comment = comment;
            ViewCommentUrl = viewCommentUrl;
        }

        public string Comment { get; private set; }

        public string ViewCommentUrl { get; private set; }

        public bool HasComment => !string.IsNullOrEmpty(Comment);
    }
}