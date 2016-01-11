namespace SFA.Apprenticeship.Api.AvService.Mediators.Version51
{
    using MessageContracts.Version51;

    public interface IVacancyUploadMediator
    {
        VacancyUploadResponse UploadVacancies(VacancyUploadRequest request);
    }
}
