namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using System;
    using Common.Constants;
    using Common.UnitTests.Mediators;
    using Common.ViewModels.Locations;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.Constants.ViewModels;
    using Raa.Common.Providers;
    using Raa.Common.ViewModels.Provider;
    using Raa.Common.ViewModels.Vacancy;
    using Recruit.Mediators.VacancyPosting;

    [TestFixture]
    public class GetEmployerTests : TestsBase
    {
        [Test]
        public void GetEmployerShouldCallGeoCodingProvider()
        {
            const int employerId = 4;
            const int providerSiteId = 2;
            const string edsurn = "edsurn";

            ProviderProvider.Setup(pp => pp.GetVacancyPartyViewModel(providerSiteId, edsurn))
                .Returns(new VacancyPartyViewModel
                {
                    Employer = new EmployerViewModel
                    {
                        EdsUrn = edsurn,
                        EmployerId = employerId
                    }
                });

            var mediator = GetMediator();
            var mediatorResponse = mediator.GetEmployer(providerSiteId, edsurn, Guid.NewGuid(), null, null);

            GeoCodingProvider.Verify(gp => gp.GeoCodeAddress(employerId));
        }

        [Test]
        public void GetEmployerWithInvalidPostCodeOrAddressShouldReturnAnError()
        {
            const int employerId = 4;
            const int providerSiteId = 2;
            const string edsurn = "edsurn";

            ProviderProvider.Setup(pp => pp.GetVacancyPartyViewModel(providerSiteId, edsurn))
                .Returns(new VacancyPartyViewModel
                {
                    Employer = new EmployerViewModel
                    {
                        EdsUrn = edsurn,
                        EmployerId = employerId
                    }
                });

            GeoCodingProvider.Setup(gp => gp.GeoCodeAddress(employerId)).Returns(GeoCodeAddressResult.InvalidAddress);

            var mediator = GetMediator();
            var mediatorResponse = mediator.GetEmployer(providerSiteId, edsurn, Guid.NewGuid(), null, null);

            mediatorResponse.AssertMessage(VacancyPostingMediatorCodes.GetEmployer.InvalidEmployerAddress,
                VacancyPartyViewModelMessages.InvalidEmployerAddress.ErrorText, UserMessageLevel.Info);
        }

        [Test]
        public void GetEmployerWithValidPostCodeOrAddressShouldReturnTheViewModel()
        {
            const int employerId = 4;
            const int providerSiteId = 2;
            const string edsurn = "edsurn";

            ProviderProvider.Setup(pp => pp.GetVacancyPartyViewModel(providerSiteId, edsurn))
                .Returns(new VacancyPartyViewModel
                {
                    Employer = new EmployerViewModel
                    {
                        EdsUrn = edsurn,
                        EmployerId = employerId
                    }
                });

            GeoCodingProvider.Setup(gp => gp.GeoCodeAddress(employerId)).Returns(GeoCodeAddressResult.Ok);

            var mediator = GetMediator();
            var mediatorResponse = mediator.GetEmployer(providerSiteId, edsurn, Guid.NewGuid(), null, null);

            mediatorResponse.AssertCode(VacancyPostingMediatorCodes.GetEmployer.Ok);
        }
    }
}