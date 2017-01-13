using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider;

namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System.Threading.Tasks;

    public interface IProviderQAProvider
    {
        Task<VacancyOwnerRelationshipViewModel> ConfirmVacancyOwnerRelationship(VacancyOwnerRelationshipViewModel viewModel);
    }
}