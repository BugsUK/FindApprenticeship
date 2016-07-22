namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using System.Collections.Generic;
    using Common.UnitTests.Mediators;
    using NUnit.Framework;
    using Raa.Common.ViewModels.Vacancy;
    using Raa.Common.ViewModels.VacancyPosting;
    using Recruit.Mediators.VacancyPosting;

    [TestFixture]
    public class ReviewVacancyTests : TestsBase
    {
        [Test]
        public void ShouldReturnNoLocationSelectedIfItsAMultilocationVacancyWithoutAnyLocationSet()
        {
            const int vacancyReferenceNumber = 1234;

            VacancyPostingProvider.Setup(p => p.GetNewVacancyViewModel(vacancyReferenceNumber)).Returns(new NewVacancyViewModel
            {
                IsEmployerLocationMainApprenticeshipLocation = false,
                LocationAddresses = new List<VacancyLocationAddressViewModel>()
            });


            var mediator = GetMediator();
            var result = mediator.GetNewVacancyViewModel(vacancyReferenceNumber, true, null);
            
            result.AssertCode(VacancyPostingMediatorCodes.GetNewVacancyViewModel.LocationNotSet);
        }
    }
}