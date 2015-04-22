namespace SFA.Apprenticeships.Application.Vacancy.SiteMap
{
    using System.Collections.Generic;

    public interface ISiteMapVacancyProvider
    {
        IEnumerable<SiteMapVacancy> GetVacancies();

        void SetVacancies(IEnumerable<SiteMapVacancy> siteMapVacancies);
    }
}
