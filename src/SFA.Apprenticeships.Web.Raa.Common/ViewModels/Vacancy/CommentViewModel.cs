namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using Domain.Entities.Vacancies.ProviderVacancies;

    public class CommentViewModel
    {
        public const string PartialView = "_CommentIcon";

        public CommentViewModel(string comment, string viewCommentUrl, string commentLabel)
        {
            Comment = comment;
            ViewCommentUrl = viewCommentUrl;
            CommentLabel = commentLabel;
        }

        public CommentViewModel(ProviderVacancyStatuses status, string comment, string viewCommentUrl, string commentLabel)
            :this(comment, viewCommentUrl, commentLabel)
        {
            Status = status;
        }

        public ProviderVacancyStatuses Status { get; private set; }

        public string Comment { get; private set; }

        public string CommentLabel { get; private set; }

        public string ViewCommentUrl { get; private set; }

        public bool HasComment => !string.IsNullOrEmpty(Comment);
    }
}