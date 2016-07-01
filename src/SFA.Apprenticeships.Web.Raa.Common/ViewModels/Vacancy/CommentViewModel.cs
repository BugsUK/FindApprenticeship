namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using Domain.Entities.Raa.Vacancies;

    public class CommentViewModel
    {
        public const string PartialIconView = "_CommentIcon";

        public CommentViewModel(string anchorName, string comment, string viewCommentUrl, string commentLabel)
        {
            AnchorName = anchorName;
            Comment = comment;
            ViewCommentUrl = viewCommentUrl;
            CommentLabel = commentLabel;
        }

        public CommentViewModel(VacancyStatus status, string anchorName, string comment, string viewCommentUrl, string commentLabel)
            :this(anchorName, comment, viewCommentUrl, commentLabel)
        {
            Status = status;
        }

        public VacancyStatus Status { get; private set; }

        public string AnchorName { get; private set; }

        public string Comment { get; private set; }

        public string CommentLabel { get; private set; }

        public string ViewCommentUrl { get; private set; }

        public bool HasComment => !string.IsNullOrEmpty(Comment);
    }
}