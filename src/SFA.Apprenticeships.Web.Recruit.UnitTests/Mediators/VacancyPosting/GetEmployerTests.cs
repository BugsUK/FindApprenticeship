namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using System;
    using Apprenticeships.Application.Interfaces.Locations;
    using Common.Constants;
    using Common.UnitTests.Mediators;
    using Constants.Messages;
    using Domain.Entities.Exceptions;
    using NUnit.Framework;
    using Raa.Common.Constants.ViewModels;
    using Raa.Common.Providers;
    using Raa.Common.ViewModels.Provider;
    using Raa.Common.ViewModels.Vacancy;
    using Recruit.Mediators.VacancyPosting;

    [TestFixture]
    [Parallelizable]
    public class GetEmployerTests : TestsBase
    {
        [Test]
        public void GetEmployerShouldCallGeoCodingProvider()
        {
            const int employerId = 4;
            const int providerSiteId = 2;
            const string edsurn = "edsurn";

            ProviderProvider.Setup(pp => pp.GetVacancyOwnerRelationshipViewModel(providerSiteId, edsurn))
                .Returns(new VacancyOwnerRelationshipViewModel
                {
                    Employer = new EmployerViewModel
                    {
                        EdsUrn = edsurn,
                        EmployerId = employerId
                    }
                });

            var mediator = GetMediator();
            var mediatorResponse = mediator.GetEmployer(providerSiteId, edsurn, Guid.NewGuid(), null, null);

            GeoCodingProvider.Verify(gp => gp.EmployerHasAValidAddress(employerId));
        }

        [Test]
        public void ReturnsErrorIfGeoCodingProviderThrowsException()
        {
            const int employerId = 4;
            const int providerSiteId = 2;
            const string edsurn = "edsurn";

            ProviderProvider.Setup(pp => pp.GetVacancyOwnerRelationshipViewModel(providerSiteId, edsurn))
                .Returns(new VacancyOwnerRelationshipViewModel
                {
                    Employer = new EmployerViewModel
                    {
                        EdsUrn = edsurn,
                        EmployerId = employerId
                    }
                });

            GeoCodingProvider
                .Setup(p => p.EmployerHasAValidAddress(employerId))
                .Throws(new CustomException(ErrorCodes.GeoCodeLookupProviderFailed));

            var mediator = GetMediator();
            var result = mediator.GetEmployer(providerSiteId, edsurn, Guid.NewGuid(), null, null);

            result.AssertMessage(VacancyPostingMediatorCodes.GetEmployer.FailedGeoCodeLookup, ApplicationPageMessages.PostcodeLookupFailed, UserMessageLevel.Error);
        }

        [Test]
        public void GetEmployerWithInvalidPostCodeOrAddressShouldReturnAnError()
        {
            const int employerId = 4;
            const int providerSiteId = 2;
            const string edsurn = "edsurn";

            ProviderProvider.Setup(pp => pp.GetVacancyOwnerRelationshipViewModel(providerSiteId, edsurn))
                .Returns(new VacancyOwnerRelationshipViewModel
                {
                    Employer = new EmployerViewModel
                    {
                        EdsUrn = edsurn,
                        EmployerId = employerId
                    }
                });

            GeoCodingProvider.Setup(gp => gp.EmployerHasAValidAddress(employerId))
                .Returns(GeoCodeAddressResult.InvalidAddress);

            var mediator = GetMediator();
            var mediatorResponse = mediator.GetEmployer(providerSiteId, edsurn, Guid.NewGuid(), null, null);

            mediatorResponse.AssertMessage(VacancyPostingMediatorCodes.GetEmployer.InvalidEmployerAddress,
                VacancyOwnerRelationshipViewModelMessages.InvalidEmployerAddress.ErrorText, UserMessageLevel.Info);
        }

        [Test]
        public void GetEmployerWithValidPostCodeOrAddressShouldReturnTheViewModel()
        {
            const int employerId = 4;
            const int providerSiteId = 2;
            const string edsurn = "edsurn";

            ProviderProvider.Setup(pp => pp.GetVacancyOwnerRelationshipViewModel(providerSiteId, edsurn))
                .Returns(new VacancyOwnerRelationshipViewModel
                {
                    Employer = new EmployerViewModel
                    {
                        EdsUrn = edsurn,
                        EmployerId = employerId
                    }
                });

            GeoCodingProvider.Setup(gp => gp.EmployerHasAValidAddress(employerId)).Returns(GeoCodeAddressResult.Ok);

            var mediator = GetMediator();
            var mediatorResponse = mediator.GetEmployer(providerSiteId, edsurn, Guid.NewGuid(), null, null);

            mediatorResponse.AssertCodeAndMessage(VacancyPostingMediatorCodes.GetEmployer.Ok);
        }
    }
}