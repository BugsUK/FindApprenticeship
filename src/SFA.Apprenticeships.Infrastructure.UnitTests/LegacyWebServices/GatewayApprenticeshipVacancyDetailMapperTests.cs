﻿namespace SFA.Apprenticeships.Infrastructure.UnitTests.LegacyWebServices
{
    using System;
    using AutoMapper;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentAssertions;
    using Infrastructure.LegacyWebServices.GatewayServiceProxy;
    using Infrastructure.LegacyWebServices.Mappers.Apprenticeships;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class GatewayApprenticeshipVacancyDetailMapperTests
    {
        [Ignore("These are going to be deleted and are not to be used")]
        [Test]
        public void ShouldCreateAMap()
        {
            // Act.
            new LegacyApprenticeshipVacancyDetailMapper().Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void ShouldThrowIfNotApprenticeship()
        {
            // Arrange.
            var src = new Vacancy
            {
                VacancyType = "Traineeship"
            };

            Action action = () => new LegacyApprenticeshipVacancyDetailMapper().Map<Vacancy, ApprenticeshipVacancyDetail>(src);
            action.ShouldThrow<AutoMapperMappingException>();
        }

        [Test]
        public void ShouldMapAllOneToOneFields()
        {
            // Arrange.
            var src = new Vacancy
            {
                VacancyId =  67,
                VacancyReference = 42,
                Status = "Live",
                ApplicationInstructions = "ApplicationInstructions",
                ClosingDate = DateTime.Today.AddDays(1),
                ContactPerson = "ContactPerson",
                ContractedProviderName = "ContractedProviderName",
                ContractOwner = "ContractOwner",
                DeliveryOrganisation = "DeliveryOrganisation",
                EmployerAnonymousDescription = "EmployerAnonymousDescription",
                EmployerDescription = "EmployerDescription",
                EmployerName = "EmployerName",
                EmployerWebsite = "EmployerWebsite",
                ExpectedDuration = "ExpectedDuration",
                FullDescription = "FullDescription",
                FutureProspects = "FutureProspects",
                ImportantOtherInfo = "ImportantOtherInfo",
                InterviewFromDate = DateTime.Today.AddDays(2),
                NumberOfPositions = 101,
                PersonalQualities = "PersonalQualities",
                PossibleStartDate = DateTime.Today.AddDays(3),
                QualificationRequired = "QualificationRequired",
                RealityCheck = "RealityCheck",
                ShortDescription = "ShortDescription",
                SkillsRequired = "SkillsRequired",
                SupplementaryQuestion1 = "SupplementaryQuestion1",
                SupplementaryQuestion2 = "SupplementaryQuestion2",
                TradingName = "TradingName",
                TrainingProviderDesc = "TrainingProviderDesc",
                TrainingRequired = "TrainingRequired",
                VacancyLocationType = "Standard",
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyManager = "VacancyManager",
                VacancyOwner = "VacancyOwner",
                VacancyTitle = "VacancyTitle",
                VacancyUrl = "VacancyUrl",
                WageType = "Weekly",
                WeeklyWage = 42.42m,
                WorkingWeek = "WorkingWeek"
            };

            // Act.
            var dest = new LegacyApprenticeshipVacancyDetailMapper().Map<Vacancy, ApprenticeshipVacancyDetail>(src);

            // Assert.
            dest.Should().NotBeNull();

            dest.Id.Should().Be(src.VacancyId);
            dest.VacancyStatus.Should().Be(VacancyStatuses.Live);
            dest.VacancyReference.Should().Be("VAC" + src.VacancyReference.ToString("D9"));
            dest.ApplicationInstructions.Should().Be(src.ApplicationInstructions);
            dest.ClosingDate.Should().Be(src.ClosingDate);
            dest.Contact.Should().Be(src.ContactForCandidate);
            dest.ProviderName.Should().Be(src.ContractedProviderName);
            dest.ContractOwner.Should().Be(src.ContractOwner);
            dest.DeliveryOrganisation.Should().Be(src.DeliveryOrganisation);
            dest.AnonymousEmployerName.Should().Be(src.EmployerAnonymousDescription);
            dest.EmployerDescription.Should().Be(src.EmployerDescription);
            dest.EmployerName.Should().Be(src.EmployerName);
            dest.EmployerWebsite.Should().Be(src.EmployerWebsite);
            dest.ExpectedDuration.Should().Be(src.ExpectedDuration);
            dest.FullDescription.Should().Be(src.FullDescription);
            dest.FutureProspects.Should().Be(src.FutureProspects);
            dest.OtherInformation.Should().Be(src.ImportantOtherInfo);
            dest.InterviewFromDate.Should().Be(src.InterviewFromDate);
            dest.NumberOfPositions.Should().Be(src.NumberOfPositions);
            dest.PersonalQualities.Should().Be(src.PersonalQualities);
            dest.StartDate.Should().Be(src.PossibleStartDate);
            dest.QualificationRequired.Should().Be(src.QualificationRequired);
            dest.RealityCheck.Should().Be(src.RealityCheck);
            dest.Description.Should().Be(src.ShortDescription);
            dest.SkillsRequired.Should().Be(src.SkillsRequired);
            dest.SupplementaryQuestion1.Should().Be(src.SupplementaryQuestion1);
            dest.SupplementaryQuestion2.Should().Be(src.SupplementaryQuestion2);
            dest.TradingName.Should().Be(src.TradingName);
            dest.ProviderDescription.Should().Be(src.TrainingProviderDesc);
            dest.TrainingToBeProvided.Should().Be(src.TrainingRequired);
            dest.VacancyManager.Should().Be(src.VacancyManager);
            dest.VacancyOwner.Should().Be(src.VacancyOwner);
            dest.Title.Should().Be(src.VacancyTitle);
            dest.VacancyUrl.Should().Be(src.VacancyUrl);
            dest.Wage.Should().Be(src.WeeklyWage);
            dest.WageDescription.Should().Be(src.WageText);
            dest.WorkingWeek.Should().Be(src.WorkingWeek);
        }

        [Test]
        public void ShouldMapApplyViaEmployerWebsiteWhenNotSpecified()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Live",
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "Standard",
                ApplyViaEmployerWebsite = true,
                ApplyViaEmployerWebsiteSpecified = false,
                WageType = "Weekly"
            };

            // Act.
            var dest = new LegacyApprenticeshipVacancyDetailMapper().Map<Vacancy, ApprenticeshipVacancyDetail>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.ApplyViaEmployerWebsite.Should().Be(false);
        }

        [Test]
        public void ShouldMapApplyViaEmployerWebsiteWhenSpecified()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Live",
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "Standard",
                ApplyViaEmployerWebsite = true,
                ApplyViaEmployerWebsiteSpecified = true,
                WageType = "Weekly"
            };

            // Act.
            var dest = new LegacyApprenticeshipVacancyDetailMapper().Map<Vacancy, ApprenticeshipVacancyDetail>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.ApplyViaEmployerWebsite.Should().Be(true);
        }

        [Test]
        public void ShouldMapEmployerAnonymousWhenNotSpecified()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Live",
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "Standard",
                EmployerAnonymous = true,
                EmployerAnonymousSpecified = false,
                WageType = "Weekly"
            };

            // Act.
            var dest = new LegacyApprenticeshipVacancyDetailMapper().Map<Vacancy, ApprenticeshipVacancyDetail>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.IsEmployerAnonymous.Should().Be(false);
        }

        [Test]
        public void ShouldMapEmployerAnonymousWhenSpecified()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Live",
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "Standard",
                EmployerAnonymous = true,
                EmployerAnonymousSpecified = true,
                WageType = "Weekly"
            };

            // Act.
            var dest = new LegacyApprenticeshipVacancyDetailMapper().Map<Vacancy, ApprenticeshipVacancyDetail>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.IsEmployerAnonymous.Should().Be(true);
        }

        [Test]
        public void ShouldMapApprFrameworkSuccessRateWhenNotSpecified()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Live",
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "Standard",
                ApprFrameworkSuccessRate = 42,
                ApprFrameworkSuccessRateSpecified = false,
                WageType = "Weekly"
            };

            // Act.
            var dest = new LegacyApprenticeshipVacancyDetailMapper().Map<Vacancy, ApprenticeshipVacancyDetail>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.ProviderSectorPassRate.Should().Be(null);
        }

        [Test]
        public void ShouldMapApprFrameworkSuccessRateWhenSpecified()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Live",
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "Standard",
                ApprFrameworkSuccessRate = 42,
                ApprFrameworkSuccessRateSpecified = true,
                WageType = "Weekly"
            };

            // Act.
            var dest = new LegacyApprenticeshipVacancyDetailMapper().Map<Vacancy, ApprenticeshipVacancyDetail>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.ProviderSectorPassRate.Should().Be(42);
        }

        [Test]
        public void ShouldMapVacancyAddressWhenNotSpecified()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Live",
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "Standard",
                VacancyAddress = null,
                WageType = "Weekly"
            };

            // Act.
            var dest = new LegacyApprenticeshipVacancyDetailMapper().Map<Vacancy, ApprenticeshipVacancyDetail>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.VacancyAddress.Should().NotBeNull();

            dest.VacancyAddress.AddressLine1.Should().BeNull();
            dest.VacancyAddress.AddressLine1.Should().BeNull();
            dest.VacancyAddress.AddressLine1.Should().BeNull();
            dest.VacancyAddress.AddressLine1.Should().BeNull();

            dest.VacancyAddress.Postcode.Should().BeNull();
            dest.VacancyAddress.Uprn.Should().BeNull();

            dest.VacancyAddress.GeoPoint.Should().NotBeNull();
            dest.VacancyAddress.GeoPoint.Latitude.Should().Be(0.0);
            dest.VacancyAddress.GeoPoint.Longitude.Should().Be(0.0);
        }


        [Test]
        public void ShouldMapVacancyAddressWhenSpecified()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Live",
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "Standard",
                VacancyAddress = new AddressDetails
                {
                    AddressLine1 = "AddressLine1",
                    AddressLine2 = "AddressLine2",
                    AddressLine3 = "AddressLine3",
                    AddressLine4 = "AddressLine4",
                    AddressLine5 = "AddressLine5",
                    Town = "Town",
                    County = "County",
                    PostCode = "Postcode",
                    LatitudeSpecified = true,
                    Latitude = 1.0m,
                    LongitudeSpecified = true,
                    Longitude = 2.0m
                },
                WageType = "Weekly"
            };

            // Act.
            var dest = new LegacyApprenticeshipVacancyDetailMapper().Map<Vacancy, ApprenticeshipVacancyDetail>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.VacancyAddress.Should().NotBeNull();

            dest.VacancyAddress.AddressLine1.Should().Be("AddressLine1");
            dest.VacancyAddress.AddressLine2.Should().Be("AddressLine2, AddressLine3, AddressLine4, AddressLine5");
            dest.VacancyAddress.AddressLine3.Should().Be("Town");
            dest.VacancyAddress.AddressLine4.Should().Be("County");

            dest.VacancyAddress.Postcode.Should().Be("Postcode");
            dest.VacancyAddress.Uprn.Should().BeNull();

            dest.VacancyAddress.GeoPoint.Should().NotBeNull();
            dest.VacancyAddress.GeoPoint.Latitude.Should().Be(1.0);
            dest.VacancyAddress.GeoPoint.Longitude.Should().Be(2.0);
        }

        [Test]
        public void ShouldMapVacancyLocationTypeStandard()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Live",
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "Standard",
                WageType = "Weekly"
            };

            // Act.
            var dest = new LegacyApprenticeshipVacancyDetailMapper().Map<Vacancy, ApprenticeshipVacancyDetail>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.VacancyLocationType.Should().Be(ApprenticeshipLocationType.NonNational);
        }

        [Test]
        public void ShouldMapVacancyLocationTypeMultipleLocation()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Live",
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "MultipleLocation",
                WageType = "Weekly"
            };

            // Act.
            var dest = new LegacyApprenticeshipVacancyDetailMapper().Map<Vacancy, ApprenticeshipVacancyDetail>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.VacancyLocationType.Should().Be(ApprenticeshipLocationType.NonNational);
        }

        [Test]
        public void ShouldMapVacancyLocationTypeNational()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Live",
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "National",
                WageType = "Weekly"
            };

            // Act.
            var dest = new LegacyApprenticeshipVacancyDetailMapper().Map<Vacancy, ApprenticeshipVacancyDetail>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.VacancyLocationType.Should().Be(ApprenticeshipLocationType.National);
        }

        [Test]
        public void ShouldThrowIfUnknownVacancyLocationType()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Live",
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "Wrong",
                WageType = "Weekly"
            };

            // Act.
            Action action = () => new LegacyApprenticeshipVacancyDetailMapper().Map<Vacancy, ApprenticeshipVacancyDetail>(src);

            // Assert: exception expected.
            action.ShouldThrow<AutoMapperMappingException>();
        }

        [Test]
        public void ShouldMapWageTypeWeekly()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Live",
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "Standard",
                WageType = "Weekly"
            };

            // Act.
            var dest = new LegacyApprenticeshipVacancyDetailMapper().Map<Vacancy, ApprenticeshipVacancyDetail>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.WageType.Should().Be(LegacyWageType.LegacyWeekly);
        }

        [Test]
        public void ShouldMapWageTypeText()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Live",
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "Standard",
                WageType = "Text"
            };

            // Act.
            var dest = new LegacyApprenticeshipVacancyDetailMapper().Map<Vacancy, ApprenticeshipVacancyDetail>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.WageType.Should().Be(LegacyWageType.LegacyText);
        }

        [Test]
        public void ShouldThrowIfUnknownWageType()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Live",
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "Standard",
                WageType = "Wrong"
            };

            // Act.
            Action action = () => new LegacyApprenticeshipVacancyDetailMapper().Map<Vacancy, ApprenticeshipVacancyDetail>(src);

            // Assert: exception expected.
            action.ShouldThrow<AutoMapperMappingException>();
        }

        [Test]
        public void ShouldMapVacancyTypeIntermediateLevelApprenticeship()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Live",
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "Standard",
                WageType = "Weekly"
            };

            // Act.
            var dest = new LegacyApprenticeshipVacancyDetailMapper().Map<Vacancy, ApprenticeshipVacancyDetail>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.ApprenticeshipLevel.Should().Be(ApprenticeshipLevel.Intermediate);
        }

        [Test]
        public void ShouldThrowIfUnknownVacancyType()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Live",
                VacancyType = "Wrong",
                VacancyLocationType = "Standard",
                WageType = "Weekly"
            };

            // Act.
            Action action = () =>new LegacyApprenticeshipVacancyDetailMapper().Map<Vacancy, ApprenticeshipVacancyDetail>(src);

            // Assert: exception expected.
            action.ShouldThrow<AutoMapperMappingException>();
        }

        [Test]
        public void ShouldMapVacancyTypeAdvancedLevelApprenticeship()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Live",
                VacancyType = "AdvancedLevelApprenticeship",
                VacancyLocationType = "Standard",
                WageType = "Weekly"
            };

            // Act.
            var dest = new LegacyApprenticeshipVacancyDetailMapper().Map<Vacancy, ApprenticeshipVacancyDetail>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.ApprenticeshipLevel.Should().Be(ApprenticeshipLevel.Advanced);
        }

        [TestCase("Live", VacancyStatuses.Live)]
        [TestCase("Posted In Error", VacancyStatuses.Unavailable)]
        [TestCase("PostedInError", VacancyStatuses.Unavailable)]
        [TestCase("Withdrawn", VacancyStatuses.Unavailable)]
        [TestCase("Deleted", VacancyStatuses.Unavailable)]
        [TestCase("Closed", VacancyStatuses.Expired)]
        [TestCase("Completed", VacancyStatuses.Expired)]
        public void ShouldMapVacancyStatus(string vacancyStatusString, VacancyStatuses vacancyStatus)
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = vacancyStatusString,
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "Standard",
                WageType = "Weekly"
            };

            // Act.
            var dest = new LegacyApprenticeshipVacancyDetailMapper().Map<Vacancy, ApprenticeshipVacancyDetail>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.VacancyStatus.Should().Be(vacancyStatus);
        }

        [Test]
        public void ShouldThrowIfUnknownVacancyStatus()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Wrong",
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "Standard",
                WageType = "Weekly"
            };

            // Act.
            Action action = () => new LegacyApprenticeshipVacancyDetailMapper().Map<Vacancy, ApprenticeshipVacancyDetail>(src);

            // Assert: exception expected.
            action.ShouldThrow<AutoMapperMappingException>();
        }
   }
}