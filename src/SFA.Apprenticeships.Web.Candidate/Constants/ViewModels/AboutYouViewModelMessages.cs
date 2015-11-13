namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    using Common.Constants;

    public static class AboutYouViewModelMessages
    {
        public static class WhatAreYourStrengthsMessages
        {
            public const string LabelText = "What are your main strengths?";
            public const string HintText = "Please provide examples of when you've demonstrated your strengths";
            public const string RequiredErrorText = "Please provide examples of your strengths";
            public const string TooLongErrorText = "Examples of your strengths must not be more than 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Examples of your strengths " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class WhatDoYouFeelYouCouldImproveMessages
        {
            public const string LabelText = "What skills would you like to improve during this apprenticeship?";
            public const string HintText = "Think of what your main duties would be and whether there are skills you'd like to develop";
            public const string RequiredErrorText = "Please provide examples of where you could improve";
            public const string TooLongErrorText = "Examples of where you could improve must not be more than 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Examples of where you could improve " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class WhatAreYourHobbiesInterestsMessages
        {
            public const string LabelText = "What are your hobbies and interests?";
            public const string HintText = "Remember to include any personal achievements";
            public const string RequiredErrorText = "Please provide examples of your hobbies/interests";
            public const string TooLongErrorText = "Examples of your hobbies/interests must not be more than 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Examples of your hobbies/interests " + Whitelists.FreetextWhitelist.ErrorText;
        }
    }
}