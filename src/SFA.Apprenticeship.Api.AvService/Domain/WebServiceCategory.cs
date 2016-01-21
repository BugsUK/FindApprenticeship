namespace SFA.Apprenticeship.Api.AvService.Domain
{
    using System;

    [Flags]
    public enum WebServiceCategory
    {
        Unknown,
        Reference = 1,
        VacancyUpload = 2,
        VacancySummary = 4,
        VacancyDetail = 8
    }
}
