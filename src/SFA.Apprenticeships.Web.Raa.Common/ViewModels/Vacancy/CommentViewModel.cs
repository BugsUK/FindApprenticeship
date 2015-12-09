namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using Domain.Entities.Vacancies.ProviderVacancies;

    public class CommentViewModel
    {
        public const string PartialView = "_CommentIcon";

        public CommentViewModel(ProviderVacancyStatuses status, string comment, string viewCommentUrl)
        {
            Status = status;
            Comment = comment;
            ViewCommentUrl = viewCommentUrl;
        }

        public ProviderVacancyStatuses Status { get; private set; }

        public string Comment { get; private set; }

        public string ViewCommentUrl { get; private set; }

        public bool HasComment => !string.IsNullOrEmpty(Comment);
    }
}