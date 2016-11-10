
namespace SFA.Apprenticeships.Web.Raa.Common.Converters
{
    using Domain.Entities.Raa.Vacancies;
    using ViewModels.Admin;

    public static class AdminSectorViewModelConverter
    {
        public static SectorViewModel Convert(this Sector sector)
        {
            return new SectorViewModel
            {
                SectorId = sector.SectorId,
                Name = sector.Name,
                ApprenticeshipOccupationId = sector.ApprenticeshipOccupationId
            };
        }
    }
}