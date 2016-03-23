namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyProvider
{
    using System;
    using System.Collections.Generic;
    using Application.Interfaces.Providers;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.Vacancies;
    using Application.Interfaces.VacancyPosting;
    using Common.Providers;
    using Configuration;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using SFA.Infrastructure.Interfaces;
    using ViewModels.Vacancy;
    using Web.Common.Configuration;

    [TestFixture]
    public class UpdateNewVacancyViewModelTests
    {
        private const int QAVacancyTimeout = 10;

        [Test]
        public void UpdateVacancyBasicDetailsShouldExpectVacancyReferenceNumber()
        {
            //Arrange
            var newVacancyVM = new Fixture().Build<NewVacancyViewModel>()
                .With(vm => vm.VacancyReferenceNumber, null)
                .Create();

            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var providerService = new Mock<IProviderService>();
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration { BlacklistedCategoryCodes = "" });

            var vacancyProvider =
                new VacancyProviderBuilder().With(vacancyPostingService)
                    .With(providerService)
                    .With(configurationService)
                    .Build();

            //Act
            Action action = () => vacancyProvider.UpdateVacancyWithComments(newVacancyVM);

            //Assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void ShouldReturnOKIfTheUserCanLockTheVacancy()
        {
            //Arrange
            const string ukprn = "ukprn";
            const string userName = "userName";
            var utcNow = DateTime.UtcNow;

            var newVacancyVM = new Fixture().Build<NewVacancyViewModel>().Create();
            var vacancy = new Fixture().Build<Vacancy>()
                            .With(av => av.VacancyReferenceNumber, newVacancyVM.VacancyReferenceNumber.Value)
                            .With(av => av.OfflineApplicationInstructionsComment, Guid.NewGuid().ToString())
                            .With(av => av.OfflineApplicationUrlComment, Guid.NewGuid().ToString())
                            .With(av => av.ShortDescriptionComment, Guid.NewGuid().ToString())
                            .With(av => av.TitleComment, Guid.NewGuid().ToString())
                            .With(av => av.OfflineApplicationUrl, $"http://www.google.com/{Guid.NewGuid()}")
                            .With(av => av.OfflineApplicationInstructions, Guid.NewGuid().ToString())
                            .With(av => av.ShortDescription, Guid.NewGuid().ToString())
                            .With(av => av.Title, Guid.NewGuid().ToString())
                            .Create();

            var sectorList = new List<Sector>
            {
                new Fixture().Build<Sector>().Create()
            };

            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var providerService = new Mock<IProviderService>();
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration { BlacklistedCategoryCodes = "" });
            var referenceDataService = new Mock<IReferenceDataService>();
            referenceDataService.Setup(m => m.GetSectors()).Returns(sectorList);
            providerService.Setup(ps => ps.GetProvider(ukprn)).Returns(new Provider());
            var currentUserService = new Mock<ICurrentUserService>();
            currentUserService.Setup(cus => cus.CurrentUserName).Returns(userName);
            var dateTimeService = new Mock<IDateTimeService>();
            dateTimeService.Setup(dts => dts.UtcNow).Returns(utcNow);
            var vacancylockingService = new Mock<IVacancyLockingService>();
            vacancylockingService.Setup(vls => vls.IsVacancyAvailableToQABy(userName, vacancy)).Returns(true);

            //Arrange: get AV, update retrieved AV with NVVM, save modified AV returning same modified AV, map AV to new NVVM with same properties as input
            vacancyPostingService.Setup(
                vps => vps.GetVacancyByReferenceNumber(newVacancyVM.VacancyReferenceNumber.Value)).Returns(vacancy);

            vacancyPostingService.Setup(vps => vps.UpdateVacancy(It.IsAny<Vacancy>())).Returns((Vacancy av) => av);

            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<Vacancy, NewVacancyViewModel>(It.IsAny<Vacancy>()))
                .Returns((Vacancy av) => newVacancyVM);

            var vacancyProvider =
                new VacancyProviderBuilder().With(vacancyPostingService)
                    .With(providerService)
                    .With(configurationService)
                    .With(referenceDataService)
                    .With(mapper)
                    .With(currentUserService)
                    .With(dateTimeService)
                    .With(vacancylockingService)
                    .Build();

            var expectedResult = new QAActionResult<NewVacancyViewModel>(QAActionResultCode.Ok, newVacancyVM);

            //Act
            var result = vacancyProvider.UpdateVacancyWithComments(newVacancyVM);
            
            //Assert
            vacancyPostingService.Verify(
                vps => vps.GetVacancyByReferenceNumber(newVacancyVM.VacancyReferenceNumber.Value), Times.Once);
            vacancyPostingService.Verify(
                vps =>
                    vps.UpdateVacancy(
                        It.Is<Vacancy>(av => av.VacancyReferenceNumber == newVacancyVM.VacancyReferenceNumber.Value &&
                            av.QAUserName == userName && av.DateStartedToQA == utcNow)));
            result.ShouldBeEquivalentTo(expectedResult);
        }

        [Test]
        public void ShouldReturnInvalidVacancyIfTheUserCantQATheVacancy()
        {
            const int vacanyReferenceNumber = 1;
            const string userName = "userName";

            var newVacancyVM = new Fixture().Build<NewVacancyViewModel>().Create();

            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var vacanyLockingService = new Mock<IVacancyLockingService>();
            var currentUserService = new Mock<ICurrentUserService>();

            currentUserService.Setup(cus => cus.CurrentUserName).Returns(userName);
            vacancyPostingService.Setup(vps => vps.GetVacancyByReferenceNumber(vacanyReferenceNumber))
                .Returns(new Vacancy {VacancyReferenceNumber = vacanyReferenceNumber});
            vacanyLockingService.Setup(vls => vls.IsVacancyAvailableToQABy(userName, It.IsAny<Vacancy>()))
                .Returns(false);
            
            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(vacancyPostingService)
                    .With(vacanyLockingService)
                    .With(currentUserService)
                    .Build();

            var result = vacancyProvider.UpdateVacancyWithComments(newVacancyVM);

            result.Code.Should().Be(QAActionResultCode.InvalidVacancy);
            result.ViewModel.Should().BeNull();
        }
    }
}