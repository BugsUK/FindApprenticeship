using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider;

namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    public interface IProviderQAProvider
    {
        VacancyOwnerRelationshipViewModel ConfirmVacancyOwnerRelationship(VacancyOwnerRelationshipViewModel viewModel);
    }
}