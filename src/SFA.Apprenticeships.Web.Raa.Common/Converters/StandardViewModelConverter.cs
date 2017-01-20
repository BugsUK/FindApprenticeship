
namespace SFA.Apprenticeships.Web.Raa.Common.Converters
{
    using Domain.Entities.Raa.Vacancies;
    using ViewModels.Vacancy;

    public static class StandardViewModelConverter
    {
        public static StandardViewModel Convert(this Standard standard, Sector sector)
        {
            return new StandardViewModel
            {
                Id = standard.Id,
                Sector = sector.Name,
                Name = standard.Name,
                ApprenticeshipLevel = standard.ApprenticeshipLevel,
                Status = standard.Status
            };
        }
    }
}