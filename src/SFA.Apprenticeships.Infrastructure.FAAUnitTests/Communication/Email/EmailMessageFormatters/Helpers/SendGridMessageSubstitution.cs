namespace SFA.Apprenticeships.Infrastructure.FAAUnitTests.Communication.Email.EmailMessageFormatters.Helpers
{
    using System.Collections.Generic;

    public class SendGridMessageSubstitution
    {
        public SendGridMessageSubstitution(string replacementTag, List<string> substitutionValues)
        {
            ReplacementTag = replacementTag;
            SubstitutionValues = substitutionValues;
        }

        public string ReplacementTag { get; private set; }

        public List<string> SubstitutionValues { get; private set; } 
    }
}