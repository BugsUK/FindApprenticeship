namespace SFA.Apprenticeships.Infrastructure.Presentation
{
    using System;

    public static class ApplicantIdPresenter
    {
        public static string GetApplicantId(this Guid candidateGuid, int legacyCandidateId)
        {
            var applicantId = candidateGuid.ToString().Replace("-", "").Substring(0, 7).ToUpperInvariant();

            if (legacyCandidateId != 0)
            {
                var legacyCandidateReference = legacyCandidateId.ToString("0###-###-###").Substring(1, 11);
                applicantId = $"{applicantId} ({legacyCandidateReference})";
            }

            return applicantId;
        }
    }
}
