namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.ViewModels.Admin;
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
            DeliveryOrganisationId = 200,
            VacancyManagerId = 200,
            ProviderId = 20
        };

        private readonly VacancyParty _vacancyParty = new VacancyParty
        {
            EmployerId = 10,
            VacancyPartyId = 101,
            ProviderSiteId = 105
        };

        private readonly VacancyParty _vacancyPartyWithRelationship = new VacancyParty
        {
            EmployerId = 10,
            VacancyPartyId = 102,
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

            MockVacancyPostingService.Setup(
                vps =>
                    vps.GetVacancyByReferenceNumber(
                        vacancyTransferViewModel.VacancyReferenceNumbers.FirstOrDefault()))
                .Returns(_existingVacancy);

            MockProviderService.Setup(ps => ps.GetVacancyParty(_existingVacancy.VacancyOwnerRelationshipId, false)).Returns(_vacancyParty);

            var vacancyPostingProvider = GetVacancyPostingProvider();

            //Act
            vacancyPostingProvider.TransferVacancies(vacancyTransferViewModel);

            //Assert
            MockVacancyPostingService.Verify(mvps =>
            mvps.UpdateVacanciesWithNewProvider(It.Is<Vacancy>(v => v.DeliveryOrganisationId == vacancyTransferViewModel.ProviderSiteId &&
            v.VacancyManagerId == vacancyTransferViewModel.ProviderSiteId && v.ProviderId == vacancyTransferViewModel.ProviderId && v.VacancyOwnerRelationshipId == _vacancyParty.VacancyPartyId)));
        }

        [Test]
        public void TransferVacancy_IfRelationshipExists_UpdateVacancyOwnerRelationshipIdOfVacancy()
        {
            var vacancyTransferViewModel = new ManageVacancyTransferViewModel
            {
                ProviderId = 10,
                ProviderSiteId = 12,
                VacancyReferenceNumbers = new List<int> { 1001 }
            };

            MockVacancyPostingService.Setup(
                vps =>
                    vps.GetVacancyByReferenceNumber(
                        vacancyTransferViewModel.VacancyReferenceNumbers.FirstOrDefault()))
                .Returns(_existingVacancy);

            MockProviderService.Setup(ps => ps.GetVacancyParty(_existingVacancy.VacancyOwnerRelationshipId, false)).Returns(_vacancyParty);

            MockProviderService.Setup(ps => ps.GetVacancyParty(_vacancyParty.EmployerId, vacancyTransferViewModel.ProviderSiteId)).Returns((VacancyParty)null);

            var vacancyPostingProvider = GetVacancyPostingProvider();

            //Act
            vacancyPostingProvider.TransferVacancies(vacancyTransferViewModel);

            //Assert
            MockVacancyPostingService.Verify(mvps =>
            mvps.UpdateVacanciesWithNewProvider(It.Is<Vacancy>(v => v.DeliveryOrganisationId == vacancyTransferViewModel.ProviderSiteId &&
            v.VacancyManagerId == vacancyTransferViewModel.ProviderSiteId && v.ProviderId == vacancyTransferViewModel.ProviderId)));

            MockProviderService.Verify(mps => mps.SaveVacancyParty(It.Is<VacancyParty>(vp => vp.ProviderSiteId == _vacancyPartyWithRelationship.ProviderSiteId)));
        }
    }
}
