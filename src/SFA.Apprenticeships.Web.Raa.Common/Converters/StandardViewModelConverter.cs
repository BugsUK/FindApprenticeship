
namespace SFA.Apprenticeships.Web.Raa.Common.Converters
{
    using Domain.Entities.ReferenceData;
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
                ApprenticeshipLevel = standard.ApprenticeshipLevel
            };
        }
    }
}