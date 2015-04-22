namespace SFA.Apprenticeships.Application.Vacancies
{
    using Web.Common.SiteMap;

    public interface IVacancySiteMapProcessor
    {
        void Process(CreateVacancySiteMapRequest request);
    }
}