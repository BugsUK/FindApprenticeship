namespace SFA.Apprenticeships.Application.Vacancies
{
    using Web.Common.SiteMap;

    public interface IVacancySiteMapProcessor
    {
        void CreateVacancySiteMap(CreateVacancySiteMapRequest request);
    }
}