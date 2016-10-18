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
            ContractOwnerId = 20
        };

        private readonly VacancyOwnerRelationship _vacancyOwnerRelationship = new VacancyOwnerRelationship
        {
            EmployerId = 10,
            VacancyOwnerRelationshipId = 101,
            ProviderSiteId = 105,
            EmployerDescription = "Original description",
            EmployerWebsiteUrl = "http://original.com"
        };

        private readonly VacancyOwnerRelationship _vacancyOwnerRelationshipWithRelationship = new VacancyOwnerRelationship
        {
            EmployerId = 10,
            VacancyOwnerRelationshipId = 102,
            ProviderSiteId = 12,
            EmployerDescription = "Existing description",
            EmployerWebsiteUrl = "http://existing.com"
        };

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

            MockProviderService.Setup(ps => ps.GetVacancyOwnerRelationship(_existingVacancy.VacancyOwnerRelationshipId, false)).Returns(_vacancyOwnerRelationship);

            MockProviderService.Setup(ps => ps.GetVacancyOwnerRelationship(_vacancyOwnerRelationship.EmployerId, vacancyTransferViewModel.ProviderSiteId)).Returns(_vacancyOwnerRelationshipWithRelationship);

            var vacancyPostingProvider = GetVacancyPostingProvider();

            //Act
            vacancyPostingProvider.TransferVacancies(vacancyTransferViewModel);

            //Assert
            //Vacancy should have new provider and provider site ids set and use the VOR id from the new provider's VOR
            MockVacancyPostingService.Verify(mvps =>
            mvps.UpdateVacanciesWithNewProvider(It.Is<Vacancy>(v => v.DeliveryOrganisationId == vacancyTransferViewModel.ProviderSiteId &&
            v.VacancyManagerId == vacancyTransferViewModel.ProviderSiteId && v.ContractOwnerId == vacancyTransferViewModel.ProviderId && 
            v.VacancyOwnerRelationshipId == _vacancyOwnerRelationshipWithRelationship.VacancyOwnerRelationshipId)));
            //Neither VOR should have been updated
            MockProviderService.Verify(mps => mps.SaveVacancyOwnerRelationship(It.Is<VacancyOwnerRelationship>(vp => vp.VacancyOwnerRelationshipId == _vacancyOwnerRelationship.VacancyOwnerRelationshipId)), Times.Never);
            MockProviderService.Verify(mps => mps.SaveVacancyOwnerRelationship(It.Is<VacancyOwnerRelationship>(vp => vp.VacancyOwnerRelationshipId == _vacancyOwnerRelationshipWithRelationship.VacancyOwnerRelationshipId)), Times.Never);
        }

        [Test]
        public void TransferVacancy_IfNoRelationshipExists_CreateVacancyOwnerRelationship()
        {
            const int newVorId = 1234;

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

            MockProviderService.Setup(ps => ps.GetVacancyOwnerRelationship(_existingVacancy.VacancyOwnerRelationshipId, false)).Returns(_vacancyOwnerRelationship);
            
            //This method actually returns a new VOR with a zero'd ID instead of null if it doesn't exist
            MockProviderService.Setup(ps => ps.GetVacancyOwnerRelationship(_vacancyOwnerRelationship.EmployerId, vacancyTransferViewModel.ProviderSiteId)).Returns<int, int>((employerId, providerSiteId) => new VacancyOwnerRelationship { EmployerId = employerId, ProviderSiteId = providerSiteId });

            MockProviderService.Setup(ps => ps.SaveVacancyOwnerRelationship(It.Is<VacancyOwnerRelationship>(
                            vor =>
                                vor.VacancyOwnerRelationshipId == 0 &&
                                vor.EmployerId == _vacancyOwnerRelationship.EmployerId &&
                                vor.ProviderSiteId == vacancyTransferViewModel.ProviderSiteId &&
                                vor.EmployerDescription == _vacancyOwnerRelationship.EmployerDescription &&
                                vor.EmployerWebsiteUrl == _vacancyOwnerRelationship.EmployerWebsiteUrl)))
                .Returns<VacancyOwnerRelationship>(
                    vor =>
                    {
                        vor.VacancyOwnerRelationshipId = newVorId;
                        return vor;
                    });

            var vacancyPostingProvider = GetVacancyPostingProvider();

            //Act
            vacancyPostingProvider.TransferVacancies(vacancyTransferViewModel);

            //Assert
            //A new VOR should have been created for the new provider and provider site
            MockProviderService.Verify(
                mps =>
                    mps.SaveVacancyOwnerRelationship(
                        It.Is<VacancyOwnerRelationship>(
                            vor =>
                                vor.VacancyOwnerRelationshipId == newVorId &&
                                vor.EmployerId == _vacancyOwnerRelationship.EmployerId &&
                                vor.ProviderSiteId == vacancyTransferViewModel.ProviderSiteId &&
                                vor.EmployerDescription == _vacancyOwnerRelationship.EmployerDescription &&
                                vor.EmployerWebsiteUrl == _vacancyOwnerRelationship.EmployerWebsiteUrl)));

            //And the vacancy should now use that new VOR as well as the new provider and provider site ids
            MockVacancyPostingService.Verify(mvps =>
            mvps.UpdateVacanciesWithNewProvider(It.Is<Vacancy>(v => v.DeliveryOrganisationId == vacancyTransferViewModel.ProviderSiteId &&
            v.VacancyManagerId == vacancyTransferViewModel.ProviderSiteId && v.ContractOwnerId == vacancyTransferViewModel.ProviderId && v.VacancyOwnerRelationshipId == newVorId)));
        }
    }
}
