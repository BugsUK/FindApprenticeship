namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate
{
    using System;

    [Flags]
    public enum MonitoringInformationViewOptions
    {
        RenderNone = 0,
        RenderGender = 1,
        RenderDisabilityStatus = 2,
        RenderEthnicity = 4,
        RenderWhyAreWeAsking = 8,
        RenderAll = RenderGender | RenderDisabilityStatus | RenderEthnicity | RenderWhyAreWeAsking,
    }
}