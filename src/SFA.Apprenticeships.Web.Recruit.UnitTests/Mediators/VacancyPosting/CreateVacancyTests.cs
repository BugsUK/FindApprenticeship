namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using System;
    using Common.Constants;
    using Common.Mediators;
    using Common.ViewModels.Locations;
    using Domain.Entities.Raa.Vacancies;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.ViewModels.Provider;
    using Raa.Common.ViewModels.Vacancy;
    using Domain.Entities.Raa.Locations;

    [TestFixture]
    public class CreateVacancyTests : TestsBase
    {
        private const string Ukprn = "12345";
        private const string AWebPage = "http://www.google.com";
        private const string AString = "A string";
        private const int AnInt = 1234;

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
            }, Ukprn);

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
            }, Ukprn);

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
            }, Ukprn);

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
            }, Ukprn);

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
            }, Ukprn);

            result.Should()
                .Match((MediatorResponse<NewVacancyViewModel> p) => p.Message == null);
        }

        [Test]
        public void ShouldIncludeLocationTypeAndNumberOfPositionsInTheViewModelReturnedWhenThereIsAValidationError()
        {
            var numberOfPositions = 5;
            var viewModel = new VacancyPartyViewModel
            {
                IsEmployerLocationMainApprenticeshipLocation = true,
                NumberOfPositions = numberOfPositions,
                ProviderSiteId = 42,
                Employer = new EmployerViewModel
                {
                    EmployerId = 7,
                }                
            };

            ProviderProvider.Setup(p => p.GetVacancyPartyViewModel(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new VacancyPartyViewModel
                {
                    Employer = new EmployerViewModel
                    {
                        Address = new AddressViewModel()
                    }
                });

            var mediator = GetMediator();

            var result = mediator.ConfirmEmployer(viewModel, Ukprn);
            result.ViewModel.IsEmployerLocationMainApprenticeshipLocation.Should()
                .Be(true);
            result.ViewModel.NumberOfPositions.Should().Be(numberOfPositions);
        }

        [Test]
        public void EmptyVacancyIfPreviousStateEmployerLocationIsDifferentFromCurrentStateEmployerLocation()
        {
            var numberOfPositions = 5;                        
            const string initialVacancyTitle = "title";
            var viewModel = new VacancyPartyViewModel
            {
                IsEmployerLocationMainApprenticeshipLocation = false,
                NumberOfPositions = numberOfPositions,
                ProviderSiteId = 42,
                Employer = new EmployerViewModel
                {
                    EmployerId = 7
                },                
                EmployerDescription = "Text about Employer Description",
                VacancyReferenceNumber = AnInt
            };        

            MockVacancyPostingService.Setup(s => s.GetVacancyByReferenceNumber(AnInt))
               .Returns(GetLiveVacancyWithComments(AnInt, initialVacancyTitle));

            var provider = GetVacancyPostingProvider();

            provider.EmptyVacancyLocation(viewModel.VacancyReferenceNumber);

            MockVacancyPostingService.Verify(
                 s =>
                     s.UpdateVacancy(It.Is<Vacancy>(v => v.Address == null &&
                         v.NumberOfPositions == null &&
                         v.NumberOfPositionsComment == null)));            
        }

        private Vacancy GetLiveVacancyWithComments(int initialVacancyReferenceNumber, string initialVacancyTitle)
        {
            return new Vacancy
            {
                VacancyReferenceNumber = initialVacancyReferenceNumber,
                Title = initialVacancyTitle,
                WorkingWeekComment = "some comment",
                ApprenticeshipLevelComment = "some comment",
                ClosingDateComment = "some comment",
                DesiredQualificationsComment = "some comment",
                DesiredSkillsComment = "some comment",
                DurationComment = "some comment",
                FirstQuestionComment = "some comment",
                FrameworkCodeNameComment = "some comment",
                FutureProspectsComment = "some comment",
                LongDescriptionComment = "some comment",
                CreatedDateTime = DateTime.UtcNow.AddDays(-4),
                UpdatedDateTime = DateTime.UtcNow.AddDays(-1),
                DateSubmitted = DateTime.UtcNow.AddHours(-12),
                DateStartedToQA = DateTime.UtcNow.AddHours(-8),
                QAUserName = "some user name",
                DateQAApproved = DateTime.UtcNow.AddHours(-4),
                Status = VacancyStatus.Live,
                ClosingDate = DateTime.UtcNow.AddDays(10),
                OwnerPartyId = 42,
                EmployerAnonymousName = "Anon Corp",
                Address = new PostalAddress(),
                NumberOfPositions = 2,
                NumberOfPositionsComment = string.Empty
            };
        }

        private VacancyViewModel BasicVacancy()
        {
            return new VacancyViewModel
            {
                NewVacancyViewModel = new NewVacancyViewModel
                {
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