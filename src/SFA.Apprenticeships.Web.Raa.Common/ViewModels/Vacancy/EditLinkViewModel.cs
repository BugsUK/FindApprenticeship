namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using Domain.Entities.Raa.Vacancies;

    public class EditLinkViewModel
    {
        public const string PartialView = "_EditLink";

        public EditLinkViewModel(VacancyStatus status, string editUrl, string comment)
        {
            Status = status;
            EditUrl = editUrl;
            CommentViewModel = new CommentViewModel(status, comment, editUrl, string.Empty);
        }

        public VacancyStatus Status { get; private set; }

        public string EditUrl { get; private set; }

        public CommentViewModel CommentViewModel { get; private set; }
    }
}