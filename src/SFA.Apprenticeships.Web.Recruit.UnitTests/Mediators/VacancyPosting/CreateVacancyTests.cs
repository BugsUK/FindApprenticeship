namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using Common.Constants;
    using Common.Mediators;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using ViewModels.Provider;
    using ViewModels.Vacancy;

    [TestFixture]
    public class CreateVacancyTests : TestsBase
    {
        private const string AWebPage = "http://www.google.com";
        private const string AString = "A string";
        private const long ALong = 1234;

        [Test]
        public void ShouldWarnUserIfSwitchingFromOnlineToOfflineVacancyHavingTextInQuestionOne()
        {
            VacancyPostingProvider.Setup(p => p.GetVacancy(It.IsAny<long>())).Returns(AVacancyWithQuestionOneFilled());
            var mediator = GetMediator();

            var result = mediator.CreateVacancy(new NewVacancyViewModel
            {
                ProviderSiteEmployerLink = new ProviderSiteEmployerLinkViewModel
                {
                    Employer = new EmployerViewModel()
                },
                OfflineVacancy = true,
                OfflineApplicationUrl = AWebPage,
                OfflineApplicationInstructions = AString,
                FrameworkCodeName = AString,
                Title = AString,
                ShortDescription = AString,
                ApprenticeshipLevel = ApprenticeshipLevel.Higher,
                VacancyReferenceNumber = ALong
            });

            result.Should()
                .Match((MediatorResponse<NewVacancyViewModel> p) => p.Message.Level == UserMessageLevel.Warning
                                                                    && !string.IsNullOrWhiteSpace(p.Message.Text));
        }

        [Test]
        public void ShouldWarnUserIfSwitchingFromOnlineToOfflineVacancyHavingTextInQuestionTwo()
        {
            VacancyPostingProvider.Setup(p => p.GetVacancy(It.IsAny<long>())).Returns(AVacancyWithQuestionTwoFilled());
            var mediator = GetMediator();

            var result = mediator.CreateVacancy(new NewVacancyViewModel
            {
                ProviderSiteEmployerLink = new ProviderSiteEmployerLinkViewModel
                {
                    Employer = new EmployerViewModel()
                },
                OfflineVacancy = true,
                OfflineApplicationUrl = AWebPage,
                OfflineApplicationInstructions = AString,
                FrameworkCodeName = AString,
                Title = AString,
                ShortDescription = AString,
                ApprenticeshipLevel = ApprenticeshipLevel.Higher,
                VacancyReferenceNumber = ALong
            });

            result.Should()
                .Match((MediatorResponse<NewVacancyViewModel> p) => p.Message.Level == UserMessageLevel.Warning
                                                                    && !string.IsNullOrWhiteSpace(p.Message.Text));
        }

        [Test]
        public void ShouldntWarnUserIfSwitchingFromOnlineToOfflineVacancyWithoutHavingAnyQuestionFilled()
        {
            VacancyPostingProvider.Setup(p => p.GetVacancy(It.IsAny<long>())).Returns(AVacancyWithNoQuestionsFilled());
            var mediator = GetMediator();

            var result = mediator.CreateVacancy(new NewVacancyViewModel
            {
                ProviderSiteEmployerLink = new ProviderSiteEmployerLinkViewModel
                {
                    Employer = new EmployerViewModel()
                },
                OfflineVacancy = true,
                OfflineApplicationUrl = AWebPage,
                OfflineApplicationInstructions = AString,
                FrameworkCodeName = AString,
                Title = AString,
                ShortDescription = AString,
                ApprenticeshipLevel = ApprenticeshipLevel.Higher,
                VacancyReferenceNumber = ALong
            });

            result.Should()
                .Match((MediatorResponse<NewVacancyViewModel> p) => p.Message == null);
        }

        [Test]
        public void ShouldntWarnUserIfTheVacancyWasAlreadyOffline()
        {
            VacancyPostingProvider.Setup(p => p.GetVacancy(It.IsAny<long>())).Returns(AnOfflineVacancy());
            var mediator = GetMediator();

            var result = mediator.CreateVacancy(new NewVacancyViewModel
            {
                ProviderSiteEmployerLink = new ProviderSiteEmployerLinkViewModel
                {
                    Employer = new EmployerViewModel()
                },
                OfflineVacancy = true,
                OfflineApplicationUrl = AWebPage,
                OfflineApplicationInstructions = AString,
                FrameworkCodeName = AString,
                Title = AString,
                ShortDescription = AString,
                ApprenticeshipLevel = ApprenticeshipLevel.Higher,
                VacancyReferenceNumber = ALong
            });

            result.Should()
                .Match((MediatorResponse<NewVacancyViewModel> p) => p.Message == null);
        }

        [Test]
        public void ShouldntWarnUserIfSwitchingFromOfflineToOnlineVacancyWithoutHavingAnyQuestionFilled()
        {
            VacancyPostingProvider.Setup(p => p.GetVacancy(It.IsAny<long>())).Returns(AnOfflineVacancy);
            var mediator = GetMediator();

            var result = mediator.CreateVacancy(new NewVacancyViewModel
            {
                ProviderSiteEmployerLink = new ProviderSiteEmployerLinkViewModel
                {
                    Employer = new EmployerViewModel()
                },
                OfflineVacancy = false,
                OfflineApplicationUrl = AWebPage,
                OfflineApplicationInstructions = AString,
                FrameworkCodeName = AString,
                Title = AString,
                ShortDescription = AString,
                ApprenticeshipLevel = ApprenticeshipLevel.Higher,
                VacancyReferenceNumber = ALong
            });

            result.Should()
                .Match((MediatorResponse<NewVacancyViewModel> p) => p.Message == null);
        }

        private VacancyViewModel BasicVacancy()
        {
            return new VacancyViewModel
            {
                OfflineVacancy = false,
                ProviderSiteEmployerLink = new ProviderSiteEmployerLinkViewModel
                {
                    Employer = new EmployerViewModel()
                }
            };
        }

        private VacancyViewModel AVacancyWithQuestionOneFilled()
        {
            var vacancy = BasicVacancy();
            vacancy.FirstQuestion = AString;

            return vacancy;
        }

        private VacancyViewModel AVacancyWithQuestionTwoFilled()
        {
            var vacancy = BasicVacancy();
            vacancy.SecondQuestion = AString;

            return vacancy;
        }

        private VacancyViewModel AVacancyWithNoQuestionsFilled()
        {
            return BasicVacancy();
        }

        private VacancyViewModel AnOfflineVacancy()
        {
            var vacancy = BasicVacancy();
            vacancy.OfflineVacancy = true;

            return vacancy;
        }
    }
}