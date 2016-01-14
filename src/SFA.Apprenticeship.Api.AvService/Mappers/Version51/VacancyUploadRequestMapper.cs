namespace SFA.Apprenticeship.Api.AvService.Mappers.Version51
{
    using System;
    using System.Linq;
    using Apprenticeships.Domain.Entities.Locations;
    using Apprenticeships.Domain.Entities.Organisations;
    using Apprenticeships.Domain.Entities.Providers;
    using Apprenticeships.Domain.Entities.Vacancies;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Common;
    using DataContracts.Version51;
    using WageType = Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.WageType;

    public class VacancyUploadRequestMapper : IVacancyUploadRequestMapper
    {
        public ApprenticeshipVacancy ToVacancy(long vacancyReferenceNumber, VacancyUploadData vacancyUploadData, ProviderSiteEmployerLink providerSiteEmployerLink)
        {
            var ukprn = Convert.ToString(vacancyUploadData.ContractedProviderUkprn);
            var providerSiteErn = Convert.ToString(vacancyUploadData.VacancyOwnerEdsUrn);
            var ern = Convert.ToString(vacancyUploadData.Employer.EdsUrn);

            // TODO: US872: AG: handle multi-location vacancies in future iteration.
            var firstLocationDetails = vacancyUploadData.Vacancy.LocationDetails.First();

            return new ApprenticeshipVacancy
            {
                EntityId = Guid.NewGuid(),
                VacancyReferenceNumber = vacancyReferenceNumber,

                Ukprn = ukprn,

                Title = vacancyUploadData.Title,
                TitleComment = null,

                ShortDescription = vacancyUploadData.ShortDescription,
                ShortDescriptionComment = null,

                LongDescription = vacancyUploadData.LongDescription,
                LongDescriptionComment = null,

                // TODO: US872: AG: move to separate mapper and unit test.
                ProviderSiteEmployerLink = new ProviderSiteEmployerLink
                {
                    EntityId = Guid.NewGuid(),
                    DateCreated = DateTime.UtcNow,
                    ProviderSiteErn = providerSiteErn,
                    Description = vacancyUploadData.Employer.Description,
                    // TODO: US872: AG: vacancy.Employer.AnonymousName
                    // TODO: US872: AG: vacancy.Employer.ContactName
                    WebsiteUrl = vacancyUploadData.Employer.Website,
                    // TODO: US872: AG: vacancy.Employer.Image
                    Employer = new Employer
                    {
                        EntityId = Guid.NewGuid(),
                        DateCreated = DateTime.UtcNow,
                        Ern = ern,
                        Name = providerSiteEmployerLink?.Employer?.Name,
                        Address = new Address
                        {
                            AddressLine1  = firstLocationDetails.AddressDetails.AddressLine1,
                            AddressLine2 = firstLocationDetails.AddressDetails.AddressLine2,
                            AddressLine3 = firstLocationDetails.AddressDetails.AddressLine3,
                            AddressLine4 = firstLocationDetails.AddressDetails.AddressLine4,
                            // AddressLine5 is not mapped.
                            GeoPoint = new GeoPoint
                            {
                                Latitude = Convert.ToDouble(firstLocationDetails.AddressDetails.Latitude ?? 0.0m),
                                Longitude = Convert.ToDouble(firstLocationDetails.AddressDetails.Longitude ?? 0.0m)
                            },
                            Postcode = firstLocationDetails.AddressDetails.PostCode
                            // Uprn is not mapped.
                        }
                    }
                },

                NumberOfPositions = firstLocationDetails.NumberOfVacancies,

                WorkingWeek = vacancyUploadData.Vacancy.WorkingWeek,
                HoursPerWeek = null,
                WorkingWeekComment = "Enter the hours per week", // TOOD: content

                WageType = WageType.Custom,
                WageUnit = WageUnit.Weekly,
                Wage = vacancyUploadData.Vacancy.Wage,
                WageComment = null,

                DurationType = DurationType.Weeks, // TODO: US872: AG: confirm default
                Duration = default(int?),
                DurationComment = vacancyUploadData.Apprenticeship.ExpectedDuration,

                ClosingDate = vacancyUploadData.Application.ClosingDate,
                ClosingDateComment = null,

                InterviewStartDate = vacancyUploadData.Application.InterviewStartDate,

                PossibleStartDate = vacancyUploadData.Application.PossibleStartDate,
                PossibleStartDateComment = null,

                DesiredSkills = vacancyUploadData.Vacancy.SkillsRequired,
                DesiredSkillsComment = null,

                FutureProspects = vacancyUploadData.Vacancy.FutureProspects,
                FutureProspectsComment = null,

                PersonalQualities = vacancyUploadData.Vacancy.PersonalQualities,
                PersonalQualitiesComment = null,

                ThingsToConsider = vacancyUploadData.Vacancy.RealityCheck,
                ThingsToConsiderComment = null,

                DesiredQualifications = vacancyUploadData.Vacancy.QualificationRequired,
                DesiredQualificationsComment = null,

                FirstQuestion = vacancyUploadData.Vacancy.SupplementaryQuestion1,
                FirstQuestionComment = null,

                SecondQuestion = vacancyUploadData.Vacancy.SupplementaryQuestion2,
                SecondQuestionComment = null,

                OfflineVacancy = vacancyUploadData.Application.Type == ApplicationType.Offline,
                OfflineApplicationUrl = firstLocationDetails.EmployerWebsite,
                OfflineApplicationUrlComment = null,

                OfflineApplicationInstructions = vacancyUploadData.Application.Instructions,
                OfflineApplicationInstructionsComment = null,

                ApprenticeshipLevel = MapVacancyApprenticeshipType(vacancyUploadData.Apprenticeship.Type),
                ApprenticeshipLevelComment = null,

                FrameworkCodeName = vacancyUploadData.Apprenticeship.Framework,
                FrameworkCodeNameComment = null,

                TrainingType = TrainingType.Frameworks,
                StandardId = default(int?),
                Status = ProviderVacancyStatuses.Draft,
                DateCreated = DateTime.UtcNow,

                IsEmployerLocationMainApprenticeshipLocation = true

                // TODO: US872: LocationAddresses
                // TODO: US872: AdditionalLocationInformation
            };
        }

        #region Helpers

        private static ApprenticeshipLevel MapVacancyApprenticeshipType(VacancyApprenticeshipType vacancyApprenticeshipType)
        {
            switch (vacancyApprenticeshipType)
            {
                case VacancyApprenticeshipType.IntermediateLevelApprenticeship:
                    return ApprenticeshipLevel.Intermediate;
                case VacancyApprenticeshipType.AdvancedLevelApprenticeship:
                    return ApprenticeshipLevel.Advanced;
                case VacancyApprenticeshipType.HigherApprenticeship:
                    return ApprenticeshipLevel.Higher;
            }

            throw new ArgumentOutOfRangeException(
                nameof(vacancyApprenticeshipType), vacancyApprenticeshipType, "Invalid vacancy apprenticeship type.");
        }

        #endregion
    }
}
