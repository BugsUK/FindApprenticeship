namespace SFA.Apprenticeships.Application.Vacancies
{
    using Entities.SiteMap;

    public interface ISiteMapVacancyProcessor
    {
        void Process(CreateVacancySiteMapRequest request);
    }
}