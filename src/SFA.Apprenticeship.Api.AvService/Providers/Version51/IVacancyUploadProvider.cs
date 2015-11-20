namespace SFA.Apprenticeship.Api.AvService.Providers.Version51
{
    using MessageContracts.Version51;

    public interface IVacancyUploadProvider
    {
        VacancyUploadResponse UploadVacancies(VacancyUploadRequest request);
    }
}
