﻿namespace SFA.Apprenticeships.Web.Common.ViewModels.Candidate
{
    using System;
    using FluentValidation.Attributes;
    using Validators;

    [Serializable]
    [Validator(typeof (WorkExperienceViewModelValidator))]
    public class WorkExperienceViewModel
    {
        public string Description { get; set; }
        public string Employer { get; set; }
        public string JobTitle { get; set; }
        public int FromMonth { get; set; }
        public string FromYear { get; set; }
        public int ToMonth { get; set; }
        public string ToYear { get; set; }
    }
}