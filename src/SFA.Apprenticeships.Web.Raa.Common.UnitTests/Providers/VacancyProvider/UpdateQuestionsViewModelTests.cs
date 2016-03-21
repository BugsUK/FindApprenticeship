namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyProvider
{
    using System;
    using Application.Interfaces.Vacancies;
    using Application.Interfaces.VacancyPosting;
    using Common.Providers;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using SFA.Infrastructure.Interfaces;
    using ViewModels.Vacancy;

    [TestFixture]
    public class UpdateQuestionsViewModelTests
    {
        [Test]
        public void ShouldReturnOKIfTheUserCanLockTheVacancy()
        {
            //Arrange
            const string userName = "userName";
            var utcNow = DateTime.UtcNow;
            const int vacancyReferenceNumber = 1;
            const string aString = "aString";

            var viewModel = new VacancyQuestionsViewModel
            {
                FirstQuestion = aString,
                SecondQuestion = aString,
                FirstQuestionComment = aString,
                SecondQuestionComment = aString,
                VacancyReferenceNumber = vacancyReferenceNumber
            };

            var vacancy = new Fixture().Build<Vacancy>()
                            .With(av => av.VacancyReferenceNumber, vacancyReferenceNumber)
                            .With(av => av.FirstQuestion, aString)
                            .With(av => av.SecondQuestion, aString)
                            .With(av => av.FirstQuestionComment, aString)
                            .With(av => av.SecondQuestionComment, aString)
                            .Create();

            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var currentUserService = new Mock<ICurrentUserService>();
            currentUserService.Setup(cus => cus.CurrentUserName).Returns(userName);
            var dateTimeService = new Mock<IDateTimeService>();
            dateTimeService.Setup(dts => dts.UtcNow).Returns(utcNow);
            var vacancylockingService = new Mock<IVacancyLockingService>();
            vacancylockingService.Setup(vls => vls.IsVacancyAvailableToQABy(userName, vacancy)).Returns(true);

            //Arrange: get AV, update retrieved AV with NVVM, save modified AV returning same modified AV, map AV to new NVVM with same properties as input
            vacancyPostingService.Setup(
                vps => vps.GetVacancyByReferenceNumber(vacancyReferenceNumber)).Returns(vacancy);

            vacancyPostingService.Setup(vps => vps.UpdateVacancy(It.IsAny<Vacancy>())).Returns((Vacancy av) => av);

            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<Vacancy, VacancyQuestionsViewModel>(It.IsAny<Vacancy>()))
                .Returns((Vacancy av) => viewModel);

            var vacancyProvider =
                new VacancyProviderBuilder().With(vacancyPostingService)
                    .With(mapper)
                    .With(currentUserService)
                    .With(dateTimeService)
                    .With(vacancylockingService)
                    .Build();

            var expectedResult = new QAActionResult<VacancyQuestionsViewModel>(QAActionResultCode.Ok, viewModel);

            //Act
            var result = vacancyProvider.UpdateVacancyWithComments(viewModel);
            
            //Assert
            vacancyPostingService.Verify(
                vps => vps.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber), Times.Once);
            vacancyPostingService.Verify(
                vps =>
                    vps.UpdateVacancy(
                        It.Is<Vacancy>(av => av.VacancyReferenceNumber == viewModel.VacancyReferenceNumber &&
                            av.QAUserName == userName && av.DateStartedToQA == utcNow)));
            result.ShouldBeEquivalentTo(expectedResult);
        }

        [Test]
        public void ShouldReturnInvalidVacancyIfTheUserCantQATheVacancy()
        {
            const int vacanyReferenceNumber = 1;
            const string userName = "userName";

            var vacancyQuestionsViewModel = new Fixture().Build<VacancyQuestionsViewModel>().Create();

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

            var result = vacancyProvider.UpdateVacancyWithComments(vacancyQuestionsViewModel);

            result.Code.Should().Be(QAActionResultCode.InvalidVacancy);
            result.ViewModel.Should().BeNull();
        }
    }
}
