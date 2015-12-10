namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    public class CommentViewModel
    {
        public const string PartialView = "_CommentIcon";

        public CommentViewModel(string comment, string viewCommentUrl, string commentLabel)
        {
            Comment = comment;
            ViewCommentUrl = viewCommentUrl;
            CommentLabel = commentLabel;
        }

        public string Comment { get; private set; }

        public string CommentLabel { get; private set; }

        public string ViewCommentUrl { get; private set; }

        public bool HasComment => !string.IsNullOrEmpty(Comment);
    }
}