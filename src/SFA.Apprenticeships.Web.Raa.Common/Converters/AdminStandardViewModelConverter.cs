
namespace SFA.Apprenticeships.Web.Raa.Common.Converters
{
    using Domain.Entities.Raa.Vacancies;
    using ViewModels.Admin;

    public static class AdminStandardViewModelConverter
    {
        public static StandardViewModel Convert(this Standard standard)
        {
            return new StandardViewModel
            {
                StandardId = standard.StandardId,
                Name = standard.Name,
                ApprenticeshipSectorId = standard.ApprenticeshipSectorId,
                ApprenticeshipLevel = standard.ApprenticeshipLevel
            };
        }
    }
}