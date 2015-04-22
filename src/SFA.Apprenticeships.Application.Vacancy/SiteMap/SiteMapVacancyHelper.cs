namespace SFA.Apprenticeships.Application.Vacancy.SiteMap
{
    using System;
    using Domain.Entities.Vacancies;

    public static class SiteMapVacancyHelper
    {
        public static string ToUrl(this SiteMapVacancy siteMapItem)
        {
            switch (siteMapItem.VacancyType)
            {
                case VacancyType.Apprenticeship:
                    return string.Format("https://www.findapprenticeship.service.gov.uk/apprenticeship/{0}", siteMapItem.VacancyId);

                case VacancyType.Traineeship:
                    return string.Format("https://www.findapprenticeship.service.gov.uk/traineeship/{0}", siteMapItem.VacancyId);
            }

            throw new ArgumentException(string.Format("Invalid vacancy type: '{0}'", siteMapItem.VacancyType));
        }
    }
}