﻿namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using Domain.Entities.Raa.Vacancies;

    public class ValidationResultViewModel
    {
        public const string PartialView = "_ValidationResultIcons";

        public ValidationResultViewModel(VacancyStatus status, string anchorName, bool hasError, string error, bool hasWarning, string warning, string viewValidationResultUrl, string comment)
        {
            Status = status;
            HasError = hasError;
            Error = error;
            HasWarning = hasWarning;
            Warning = warning;
            ViewValidationResultUrl = viewValidationResultUrl;
            AnchorName = anchorName;
            CommentViewModel = new CommentViewModel(status, anchorName, comment, viewValidationResultUrl, string.Empty);
        }

        public VacancyStatus Status { get; private set; }

        public string AnchorName { get; private set; }

        public bool HasError { get; private set; }

        public string Error { get; private set; }

        public bool HasWarning { get; private set; }

        public string Warning { get; private set; }

        public string ViewValidationResultUrl { get; private set; }

        public CommentViewModel CommentViewModel { get; private set; }
    }
}