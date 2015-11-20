namespace SFA.Apprenticeship.Api.AvService.Providers.Version51
{
    using Apprenticeships.Application.Interfaces.VacancyPosting;
    using MessageContracts.Version51;

    public class VacancyUploadProvider : IVacancyUploadProvider
    {
        private readonly IVacancyPostingService _vacancyPostingService;

        public VacancyUploadProvider(IVacancyPostingService vacancyPostingService)
        {
            _vacancyPostingService = vacancyPostingService;
        }

        public VacancyUploadResponse UploadVacancies(VacancyUploadRequest request)
        {
            return null;
        }
    }
}
