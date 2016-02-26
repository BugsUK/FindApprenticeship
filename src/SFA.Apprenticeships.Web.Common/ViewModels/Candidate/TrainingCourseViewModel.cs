namespace SFA.Apprenticeships.Web.Common.ViewModels.Candidate
{
    using System;
    using FluentValidation.Attributes;
    using Validators;

    [Serializable]
    [Validator(typeof(TrainingCourseViewModelValidator))]
    public class TrainingCourseViewModel
    {
        public string Provider { get; set; }
        public string Title { get; set; }
        public int FromMonth { get; set; }
        public string FromYear { get; set; }
        public int ToMonth { get; set; }
        public string ToYear { get; set; }
    }
}