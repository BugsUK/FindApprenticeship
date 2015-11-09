namespace SFA.Apprenticeship.Api.AvService.Providers.Version51
{
    using MessageContracts.Version51;

    public interface IVacancyDetailsProvider
    {
        VacancyDetailsResponse Get(VacancyDetailsRequest request);
    }
}
