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
    using Domain.Entities.Raa.Parties;
    using FluentAssertions;
    using Ploeh.AutoFixture;
    using Raa.Common.ViewModels.VacancyPosting;

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

        [Test]
        public void ShouldCreateTheVacancyIfItDoesnExist()
        {
            // Arrange
            const string ukprn = "1234";
            var vacancyGuid = Guid.NewGuid();
            const int vacanyPartyId = 1;
            const bool isEmployerLocationMainApprenticeshipLocation = true;
            int? numberOfPositions = 2;


            var viewModel = new VacancyPartyViewModel
            {
                IsEmployerLocationMainApprenticeshipLocation = isEmployerLocationMainApprenticeshipLocation,
                NumberOfPositions = numberOfPositions,
                ProviderSiteId = 42,
                Employer = new EmployerViewModel
                {
                    EmployerId = 7
                },
                EmployerDescription = "Text about Employer Description",
                VacancyPartyId = vacanyPartyId,
                VacancyGuid = vacancyGuid
            };

            ProviderProvider.Setup(p => p.ConfirmVacancyParty(viewModel)).Returns(viewModel);

            // Act.
            var mediator = GetMediator();
            mediator.ConfirmEmployer(viewModel, ukprn);
            

            // Assert.
            VacancyPostingProvider.Verify(p => p.CreateVacancy(new VacancyMinimumData
            {
                Ukprn = ukprn,
                VacancyGuid = vacancyGuid,
                VacancyPartyId = vacanyPartyId,
                IsEmployerLocationMainApprenticeshipLocation = isEmployerLocationMainApprenticeshipLocation,
                NumberOfPositions = numberOfPositions
            }));
        }

        [Test]
        public void CreateVacancyShouldCreateTheVacancy()
        {
            const int vacancyPartyId = 1;
            const int employerId = 2;
            const string ukprn = "1234";
            const string employersPostcode = "cv1 9SX";
            var vacancyGuid = Guid.NewGuid();
            const int vacancyReferenceNumber = 123456;
            const bool isEmployerLocationMainApprenticeshipLocation = true;
            int? numberOfPositions = 2;
            var address = new Fixture().Build<PostalAddress>().With(a => a.Postcode, employersPostcode).Create();
            const int providerId = 4;
            const string localAuthorityCode = "lac";

            // Arrange.
            MockVacancyPostingService.Setup(s => s.GetNextVacancyReferenceNumber()).Returns(vacancyReferenceNumber);
            MockProviderService.Setup(s => s.GetVacancyParty(vacancyPartyId, true))
                .Returns(
                    new Fixture().Build<VacancyParty>()
                        .With(v => v.VacancyPartyId, vacancyPartyId)
                        .With(v => v.EmployerId, employerId)
                        .Create());
            MockEmployerService.Setup(s => s.GetEmployer(employerId))
                .Returns(
                    new Fixture().Build<Employer>()
                        .With(e => e.EmployerId, employerId)
                        .With(e => e.Address, address)
                        .Create());
            MockProviderService.Setup(s => s.GetProvider(ukprn))
                .Returns(new Fixture().Build<Provider>().With(p => p.ProviderId, providerId).Create());
            MockLocalAuthorityService.Setup(s => s.GetLocalAuthorityCode(employersPostcode)).Returns(localAuthorityCode);


            // Act.
            var provider = GetVacancyPostingProvider();
            provider.CreateVacancy(new VacancyMinimumData
            {
                IsEmployerLocationMainApprenticeshipLocation = isEmployerLocationMainApprenticeshipLocation,
                VacancyGuid = vacancyGuid,
                VacancyPartyId = vacancyPartyId,
                Ukprn = ukprn,
                NumberOfPositions = numberOfPositions
            });

            // Assert.
            MockVacancyPostingService.Verify(s => s.GetNextVacancyReferenceNumber());
            MockProviderService.Verify(s => s.GetVacancyParty(vacancyPartyId, true));
            MockEmployerService.Verify(s => s.GetEmployer(employerId));
            MockProviderService.Verify(s => s.GetProvider(ukprn));
            MockLocalAuthorityService.Verify(s => s.GetLocalAuthorityCode(employersPostcode));
            MockVacancyPostingService.Verify(s => s.CreateApprenticeshipVacancy(It.Is<Vacancy>(v => 
                v.VacancyGuid == vacancyGuid 
                && v.VacancyReferenceNumber == vacancyReferenceNumber 
                && v.Title == null 
                && v.ShortDescription == null 
                && v.OwnerPartyId == vacancyPartyId 
                && v.Status == VacancyStatus.Draft 
                && v.OfflineVacancy.HasValue == false
                && v.OfflineApplicationUrl == null 
                && v.OfflineApplicationInstructions == null 
                && v.IsEmployerLocationMainApprenticeshipLocation == isEmployerLocationMainApprenticeshipLocation 
                && v.NumberOfPositions == numberOfPositions 
                && v.VacancyType == VacancyType.Unknown 
                && v.Address == address 
                && v.ProviderId == providerId 
                && v.LocalAuthorityCode == localAuthorityCode
            )));
        }

        [Test]
        public void CreateVacancyShouldNullTheAddressIfItsAMultilocationVacancy()
        {
            const int vacancyPartyId = 1;
            const int employerId = 2;
            const string ukprn = "1234";
            const string employersPostcode = "cv1 9SX";
            var vacancyGuid = Guid.NewGuid();
            const int vacancyReferenceNumber = 123456;
            const bool isEmployerLocationMainApprenticeshipLocation = false;
            int? numberOfPositions = null;
            var address = new Fixture().Build<PostalAddress>().With(a => a.Postcode, employersPostcode).Create();
            const int providerId = 4;
            const string localAuthorityCode = "lac";

            // Arrange.
            MockVacancyPostingService.Setup(s => s.GetNextVacancyReferenceNumber()).Returns(vacancyReferenceNumber);
            MockProviderService.Setup(s => s.GetVacancyParty(vacancyPartyId, true))
                .Returns(
                    new Fixture().Build<VacancyParty>()
                        .With(v => v.VacancyPartyId, vacancyPartyId)
                        .With(v => v.EmployerId, employerId)
                        .Create());
            MockEmployerService.Setup(s => s.GetEmployer(employerId))
                .Returns(
                    new Fixture().Build<Employer>()
                        .With(e => e.EmployerId, employerId)
                        .With(e => e.Address, address)
                        .Create());
            MockProviderService.Setup(s => s.GetProvider(ukprn))
                .Returns(new Fixture().Build<Provider>().With(p => p.ProviderId, providerId).Create());
            MockLocalAuthorityService.Setup(s => s.GetLocalAuthorityCode(employersPostcode)).Returns(localAuthorityCode);


            // Act.
            var provider = GetVacancyPostingProvider();
            provider.CreateVacancy(new VacancyMinimumData
            {
                IsEmployerLocationMainApprenticeshipLocation = isEmployerLocationMainApprenticeshipLocation,
                VacancyGuid = vacancyGuid,
                VacancyPartyId = vacancyPartyId,
                Ukprn = ukprn,
                NumberOfPositions = numberOfPositions
            });

            // Assert.
            MockVacancyPostingService.Verify(s => s.CreateApprenticeshipVacancy(It.Is<Vacancy>(v =>
                v.Address == null
            )));
        }

        [Test]
        public void CreateVacancyShouldGeoCodeTheEmpoyersAddressIfTheGeoPointIsInvalid()
        {
            const int vacancyPartyId = 1;
            const int employerId = 2;
            const string ukprn = "1234";
            const string employersPostcode = "cv1 9SX";
            var vacancyGuid = Guid.NewGuid();
            const int vacancyReferenceNumber = 123456;
            const bool isEmployerLocationMainApprenticeshipLocation = true;
            int? numberOfPositions = 2;
            var address =
                new Fixture().Build<PostalAddress>()
                    .With(a => a.Postcode, employersPostcode)
                    .With(a => a.GeoPoint, null)
                    .Create();
            const int providerId = 4;
            const string localAuthorityCode = "lac";

            // Arrange.
            MockVacancyPostingService.Setup(s => s.GetNextVacancyReferenceNumber()).Returns(vacancyReferenceNumber);
            MockProviderService.Setup(s => s.GetVacancyParty(vacancyPartyId, true))
                .Returns(
                    new Fixture().Build<VacancyParty>()
                        .With(v => v.VacancyPartyId, vacancyPartyId)
                        .With(v => v.EmployerId, employerId)
                        .Create());
            MockEmployerService.Setup(s => s.GetEmployer(employerId))
                .Returns(
                    new Fixture().Build<Employer>()
                        .With(e => e.EmployerId, employerId)
                        .With(e => e.Address, address)
                        .Create());
            MockProviderService.Setup(s => s.GetProvider(ukprn))
                .Returns(new Fixture().Build<Provider>().With(p => p.ProviderId, providerId).Create());
            MockLocalAuthorityService.Setup(s => s.GetLocalAuthorityCode(employersPostcode)).Returns(localAuthorityCode);


            // Act.
            var provider = GetVacancyPostingProvider();
            provider.CreateVacancy(new VacancyMinimumData
            {
                IsEmployerLocationMainApprenticeshipLocation = isEmployerLocationMainApprenticeshipLocation,
                VacancyGuid = vacancyGuid,
                VacancyPartyId = vacancyPartyId,
                Ukprn = ukprn,
                NumberOfPositions = numberOfPositions
            });

            // Assert.
            MockGeoCodingService.Verify(s => s.GetGeoPointFor(address));
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