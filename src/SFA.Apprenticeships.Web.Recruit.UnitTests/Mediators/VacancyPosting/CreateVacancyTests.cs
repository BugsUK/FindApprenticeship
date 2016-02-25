namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using Common.Constants;
    using Common.Mediators;
    using Common.ViewModels.Locations;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.ViewModels.Provider;
    using Raa.Common.ViewModels.Vacancy;

    [TestFixture]
    public class CreateVacancyTests : TestsBase
    {
        private const string AWebPage = "http://www.google.com";
        private const string AString = "A string";
        private const int AnInt = 1234;
        private const long ALong = 1234;

        [Test]
        public void ShouldWarnUserIfSwitchingFromOnlineToOfflineVacancyHavingTextInQuestionOne()
        {
            VacancyPostingProvider.Setup(p => p.GetVacancy(It.IsAny<int>())).Returns(AVacancyWithQuestionOneFilled());
            var mediator = GetMediator();

            var result = mediator.CreateVacancy(new NewVacancyViewModel
            {
                OwnerParty = new VacancyPartyViewModel
                {
                    Employer = new EmployerViewModel()
                },
                OfflineVacancy = true,
                OfflineApplicationUrl = AWebPage,
                OfflineApplicationInstructions = AString,
                Title = AString,
                ShortDescription = AString,
                VacancyReferenceNumber = AnInt,
                VacancyType = VacancyType.Apprenticeship
            });

            result.Should()
                .Match((MediatorResponse<NewVacancyViewModel> p) => p.Message.Level == UserMessageLevel.Info
                                                                    && !string.IsNullOrWhiteSpace(p.Message.Text));
        }

        [Test]
        public void ShouldWarnUserIfSwitchingFromOnlineToOfflineVacancyHavingTextInQuestionTwo()
        {
            VacancyPostingProvider.Setup(p => p.GetVacancy(It.IsAny<int>())).Returns(AVacancyWithQuestionTwoFilled());
            var mediator = GetMediator();

            var result = mediator.CreateVacancy(new NewVacancyViewModel
            {
                OwnerParty = new VacancyPartyViewModel
                {
                    Employer = new EmployerViewModel()
                },
                OfflineVacancy = true,
                OfflineApplicationUrl = AWebPage,
                OfflineApplicationInstructions = AString,
                Title = AString,
                ShortDescription = AString,
                VacancyReferenceNumber = AnInt,
                VacancyType = VacancyType.Apprenticeship
            });

            result.Should()
                .Match((MediatorResponse<NewVacancyViewModel> p) => p.Message.Level == UserMessageLevel.Info
                                                                    && !string.IsNullOrWhiteSpace(p.Message.Text));
        }

        [Test]
        public void ShouldntWarnUserIfSwitchingFromOnlineToOfflineVacancyWithoutHavingAnyQuestionFilled()
        {
            VacancyPostingProvider.Setup(p => p.GetVacancy(It.IsAny<int>())).Returns(AVacancyWithNoQuestionsFilled());
            var mediator = GetMediator();

            var result = mediator.CreateVacancy(new NewVacancyViewModel
            {
                OwnerParty = new VacancyPartyViewModel
                {
                    Employer = new EmployerViewModel()
                },
                OfflineVacancy = true,
                OfflineApplicationUrl = AWebPage,
                OfflineApplicationInstructions = AString,
                Title = AString,
                ShortDescription = AString,
                VacancyReferenceNumber = AnInt
            });

            result.Should()
                .Match((MediatorResponse<NewVacancyViewModel> p) => p.Message == null);
        }

        [Test]
        public void ShouldntWarnUserIfTheVacancyWasAlreadyOffline()
        {
            VacancyPostingProvider.Setup(p => p.GetVacancy(It.IsAny<int>())).Returns(AnOfflineVacancy());
            var mediator = GetMediator();

            var result = mediator.CreateVacancy(new NewVacancyViewModel
            {
                OwnerParty = new VacancyPartyViewModel
                {
                    Employer = new EmployerViewModel()
                },
                OfflineVacancy = true,
                OfflineApplicationUrl = AWebPage,
                OfflineApplicationInstructions = AString,
                Title = AString,
                ShortDescription = AString,
                VacancyReferenceNumber = AnInt
            });

            result.Should()
                .Match((MediatorResponse<NewVacancyViewModel> p) => p.Message == null);
        }

        [Test]
        public void ShouldntWarnUserIfSwitchingFromOfflineToOnlineVacancyWithoutHavingAnyQuestionFilled()
        {
            VacancyPostingProvider.Setup(p => p.GetVacancy(It.IsAny<int>())).Returns(AnOfflineVacancy);
            var mediator = GetMediator();

            var result = mediator.CreateVacancy(new NewVacancyViewModel
            {
                OwnerParty = new VacancyPartyViewModel
                {
                    Employer = new EmployerViewModel()
                },
                OfflineVacancy = false,
                OfflineApplicationUrl = AWebPage,
                OfflineApplicationInstructions = AString,
                Title = AString,
                ShortDescription = AString,
                VacancyReferenceNumber = AnInt
            });

            result.Should()
                .Match((MediatorResponse<NewVacancyViewModel> p) => p.Message == null);
        }

        [Test]
        public void ShouldIncludeLocationTypeAndNumberOfPositionsInTheViewModelReturnedWhenThereIsAValidationError()
        {
            var isEmployerLocationMainApprenticeshipLocation = true;
            var numberOfPositions = 5;
            var viewModel = new VacancyPartyViewModel
            {
                IsEmployerLocationMainApprenticeshipLocation = isEmployerLocationMainApprenticeshipLocation,
                NumberOfPositions = numberOfPositions,
                ProviderSiteId = 42,
                Employer = new EmployerViewModel
                {
                    EmployerId = 7
                }
            };

            ProviderProvider.Setup(p => p.GetVacancyPartyViewModel(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new VacancyPartyViewModel
                {
                    Employer = new EmployerViewModel
                    {
                        Address = new AddressViewModel()
                    }
                });

            var mediator = GetMediator();

            var result = mediator.ConfirmEmployer(viewModel);
            result.ViewModel.IsEmployerLocationMainApprenticeshipLocation.Should()
                .Be(isEmployerLocationMainApprenticeshipLocation);
            result.ViewModel.NumberOfPositions.Should().Be(numberOfPositions);
        }

        private VacancyViewModel BasicVacancy()
        {
            return new VacancyViewModel
            {
                NewVacancyViewModel = new NewVacancyViewModel { 
                    OfflineVacancy = false,
                    OwnerParty = new VacancyPartyViewModel
                    {
                        Employer = new EmployerViewModel()
                    }
                },
                VacancyQuestionsViewModel = new VacancyQuestionsViewModel(),
                FurtherVacancyDetailsViewModel = new FurtherVacancyDetailsViewModel(),
                VacancyRequirementsProspectsViewModel = new VacancyRequirementsProspectsViewModel()
            };
        }

        private VacancyViewModel AVacancyWithQuestionOneFilled()
        {
            var vacancy = BasicVacancy();
            vacancy.VacancyQuestionsViewModel.FirstQuestion = AString;

            return vacancy;
        }

        private VacancyViewModel AVacancyWithQuestionTwoFilled()
        {
            var vacancy = BasicVacancy();
            vacancy.VacancyQuestionsViewModel.SecondQuestion = AString;

            return vacancy;
        }

        private VacancyViewModel AVacancyWithNoQuestionsFilled()
        {
            return BasicVacancy();
        }

        private VacancyViewModel AnOfflineVacancy()
        {
            var vacancy = BasicVacancy();
            vacancy.NewVacancyViewModel.OfflineVacancy = true;

            return vacancy;
        }
    }
}