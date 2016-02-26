namespace SFA.Apprenticeships.Web.Common.ViewModels.Candidate
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators;

    [Serializable]
    [Validator(typeof(AboutYouViewModelClientValidator))]
    public class AboutYouViewModel
    {
        [Display(Name = AboutYouViewModelMessages.WhatAreYourStrengthsMessages.LabelText, Description = AboutYouViewModelMessages.WhatAreYourStrengthsMessages.HintText)]
        public string WhatAreYourStrengths { get; set; }

        [Display(Name = AboutYouViewModelMessages.WhatDoYouFeelYouCouldImproveMessages.LabelText, Description = AboutYouViewModelMessages.WhatDoYouFeelYouCouldImproveMessages.HintText)]
        public string WhatDoYouFeelYouCouldImprove { get; set; }

        [Display(Name = AboutYouViewModelMessages.WhatAreYourHobbiesInterestsMessages.LabelText, Description = AboutYouViewModelMessages.WhatAreYourHobbiesInterestsMessages.HintText)]
        public string WhatAreYourHobbiesInterests { get; set; }
    }
}