namespace SFA.Apprenticeships.Domain.Entities.Applications
{
    public static class ApplicationDetailHelper
    {
        public static bool IsSavedVacancy(this ApplicationDetail applicationDetail)
        {
            return applicationDetail.CandidateInformation == null;
        }
    }
}
