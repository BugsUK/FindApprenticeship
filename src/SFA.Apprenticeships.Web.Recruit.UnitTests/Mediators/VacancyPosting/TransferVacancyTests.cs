namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using NUnit.Framework;
    using Raa.Common.ViewModels.Admin;
    using Raa.Common.ViewModels.Provider;
    using Raa.Common.ViewModels.Vacancy;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    [Parallelizable]
    class TransferVacancyTests : TestsBase
    {
        private readonly Vacancy _existingVacancy = new Vacancy
        {
            VacancyOwnerRelationshipId = 101,
            VacancyReferenceNumber = 1001,
        };

        private readonly VacancyParty _vacancyParty = new VacancyParty
        {
            EmployerId = 10,
            VacancyPartyId = 101,
            ProviderSiteId = 12
        };

        private readonly VacancyParty _existingvacancyParty = new VacancyParty
        {
            EmployerId = 10,
            ProviderSiteId = 12
        };

        [Test]
        public void TransferVacancy_IfNoRelationshipExists_UpdateVacancyOwnerRelationship()
        {
            var vacancyTransferViewModel = new ManageVacancyTransferViewModel
            {
                ProviderId = 10,
                ProviderSiteId = 12,
                VacancyReferenceNumbers = new List<int> { 1001 }
            };
            NewVacancyViewModel validNewVacancyViewModelWithReferenceNumber = new NewVacancyViewModel
            {
                VacancyReferenceNumber = 1001,
                OfflineVacancy = false,
                OwnerParty = new VacancyPartyViewModel()
            };

            VacancyPostingService.Setup(vps => vps.UpdateVacanciesWithNewProvider(_existingVacancy))
                .Returns(_existingVacancy);

            MockProviderService.Setup(mps => mps.SaveVacancyParty(_vacancyParty)).Returns(_vacancyParty);

            VacancyPostingService.Setup(
                vps =>
                    vps.GetVacancyByReferenceNumber(
                        vacancyTransferViewModel.VacancyReferenceNumbers.FirstOrDefault()))
                .Returns(_existingVacancy);

            MockProviderService.Setup(ps => ps.GetVacancyParty(_existingVacancy.VacancyOwnerRelationshipId, false)).Returns(_vacancyParty);

            MockProviderService.Setup(ps => ps.GetVacancyParty(_vacancyParty.EmployerId, vacancyTransferViewModel.ProviderSiteId)).Returns((VacancyParty)null);

            var vacancyPostingProvider = GetVacancyPostingProvider();
            vacancyPostingProvider.TransferVacancies(vacancyTransferViewModel);
        }

        [Test]
        public void TransferVacancy_IfRelationshipExists_UpdateVacancyOwnerRelationshipIdOfVacancy()
        {

        }
    }
}
