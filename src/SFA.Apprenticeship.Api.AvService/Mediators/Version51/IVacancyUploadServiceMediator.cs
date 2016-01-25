namespace SFA.Apprenticeship.Api.AvService.Mediators.Version51
{
    using MessageContracts.Version51;

    public interface IVacancyUploadServiceMediator
    {
        VacancyUploadResponse UploadVacancies(VacancyUploadRequest request);
    }
}
