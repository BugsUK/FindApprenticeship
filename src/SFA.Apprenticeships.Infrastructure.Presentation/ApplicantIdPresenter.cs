namespace SFA.Apprenticeships.Infrastructure.Presentation
{
    using System;

    public static class ApplicantIdPresenter
    {
        public static string GetApplicantId(this Guid candidateId)
        {
            return candidateId.ToString().Replace("-", "").Substring(0, 7).ToUpperInvariant();
        }
    }
}
