namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate
{
    using System;
    using FluentValidation.Attributes;
    using Validators;

    [Serializable]
    [Validator(typeof(TrainingHistoryViewModelValidator))]
    public class TrainingHistoryViewModel
    {
        public string Provider { get; set; }
        public string CourseTitle { get; set; }
        public int FromMonth { get; set; }
        public string FromYear { get; set; }
        public int ToMonth { get; set; }
        public string ToYear { get; set; }
    }
}