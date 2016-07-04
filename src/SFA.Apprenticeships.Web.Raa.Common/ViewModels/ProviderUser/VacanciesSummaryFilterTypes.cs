namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.ProviderUser
{
    public enum VacanciesSummaryFilterTypes
    {
        All = 0,
        Live = 1,
        Submitted,

        /// <summary>AKA Referred</summary>
        Rejected,

        ClosingSoon,
        Closed,
        Draft,
        NewApplications,
        Withdrawn,
        Completed
    }
}