namespace SFA.Apprenticeships.Web.Common.SiteMap
{
    using System.Collections.Generic;

    public interface ISiteMapVacancyProvider
    {
        IEnumerable<SiteMapVacancy> GetVacancies();

        void SetVacancies(IEnumerable<SiteMapVacancy> siteMapVacancies);
    }
}
