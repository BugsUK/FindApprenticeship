namespace SFA.Apprenticeships.Infrastructure.Raa.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Domain.Entities.Locations;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using SFA.Infrastructure.Interfaces;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Traineeships;
    using Extensions;
    using Presentation;
    using GeoPoint = Domain.Entities.Locations.GeoPoint;

    public class TraineeshipVacancyDetailMapper
    {
        public static TraineeshipVacancyDetail GetTraineeshipVacancyDetail(Vacancy vacancy, VacancyParty vacancyParty, Employer employer, Provider provider, IEnumerable<Category> categories, ILogService logService)
        {
            //Manually mapping rather than using automapper as the two enties are significantly different
            var wage = new Wage(vacancy.WageType, vacancy.Wage, vacancy.WageText, vacancy.WageUnit);

            var detail = new TraineeshipVacancyDetail
            {
                Id = vacancy.VacancyId,
                VacancyReference = vacancy.VacancyReferenceNumber.GetVacancyReference(),
                Title = vacancy.Title,
                Description = vacancy.ShortDescription,
                FullDescription = vacancy.LongDescription,
                //SubCategory = vacancy.,
                StartDate = vacancy.PossibleStartDate ?? DateTime.MinValue,
                ClosingDate = vacancy.ClosingDate ?? DateTime.MinValue,
                PostedDate = vacancy.DateQAApproved ?? DateTime.MinValue,
                //TODO: Where should this come from?
                InterviewFromDate = DateTime.MinValue,
                Wage = vacancy.Wage ?? 0,
                WageUnit = wage.GetWageUnit(),
                WageDescription = wage.GetDisplayText(vacancy.HoursPerWeek),
                WageType = (LegacyWageType)vacancy.WageType,
                WorkingWeek = vacancy.WorkingWeek,
                OtherInformation = vacancy.OtherInformation,
                FutureProspects = vacancy.FutureProspects,
                //TODO: Where from?
                //VacancyOwner = vacancy.,
                //VacancyManager = vacancy.,
                //LocalAuthority = vacancy.,
                NumberOfPositions = vacancy.NumberOfPositions ?? 0,
                RealityCheck = vacancy.ThingsToConsider,
                Created = vacancy.CreatedDateTime,
                VacancyStatus = vacancy.Status.GetVacancyStatuses(),
                EmployerName = employer.Name,
                AnonymousEmployerName = vacancy.EmployerAnonymousName,
                IsEmployerAnonymous = !string.IsNullOrWhiteSpace(vacancy.EmployerAnonymousName),
                EmployerDescription = vacancyParty.EmployerDescription,
                EmployerWebsite = vacancyParty.EmployerWebsiteUrl,
                ApplyViaEmployerWebsite = vacancy.OfflineVacancy ?? false,
                VacancyUrl = vacancy.OfflineApplicationUrl,
                ApplicationInstructions = vacancy.OfflineApplicationInstructions,
                IsPositiveAboutDisability = employer.IsPositiveAboutDisability,
                ExpectedDuration = vacancy.ExpectedDuration,
                VacancyAddress = GetVacancyAddress(vacancy.Address),
                //TODO: How is this captured in RAA?
                //IsRecruitmentAgencyAnonymous = vacancy.,
                //TODO: How is this captured in RAA?
                //IsSmallEmployerWageIncentive = vacancy.,
                SupplementaryQuestion1 = vacancy.FirstQuestion,
                SupplementaryQuestion2 = vacancy.SecondQuestion,
                //TODO: How is this captured in RAA?
                //RecruitmentAgency = vacancy.,
                ProviderName = provider.Name,
                TradingName = employer.TradingName,
                //ProviderDescription = vacancy.,
                Contact = GetContactInformation(vacancy),
                TrainingToBeProvided = vacancy.TrainingProvided,
                //TODO: How is this captured in RAA?
                //ContractOwner = vacancy.,
                //DeliveryOrganisation = vacancy.,
                //IsNasProvider = vacancy.,
                PersonalQualities = vacancy.PersonalQualities,
                QualificationRequired = vacancy.DesiredQualifications,
                SkillsRequired = vacancy.DesiredSkills,
                TrainingType = vacancy.TrainingType.GetTrainingType()
            };

            return detail;
        }

        private static Address GetVacancyAddress(PostalAddress address)
        {
            return new Address
            {
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                AddressLine3 = address.AddressLine3,
                AddressLine4 = address.AddressLine4,
                Postcode = address.Postcode,
                GeoPoint = GetGeoPoint(address.GeoPoint)
            };
        }

        private static GeoPoint GetGeoPoint(Domain.Entities.Raa.Locations.GeoPoint geoPoint)
        {
            return new GeoPoint
            {
                Longitude = geoPoint.Longitude,
                Latitude = geoPoint.Latitude
            };
        }

        private static string GetContactInformation(Vacancy vacancy)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(vacancy.ContactName))
            {
                sb.Append(vacancy.ContactName);
            }
            if (!string.IsNullOrEmpty(vacancy.ContactNumber))
            {
                if (sb.Length > 0)
                {
                    sb.Append(" ");
                }
                sb.Append(vacancy.ContactNumber);
            }
            if (!string.IsNullOrEmpty(vacancy.ContactEmail))
            {
                if (sb.Length > 0)
                {
                    sb.Append(" ");
                }
                sb.Append(vacancy.ContactEmail);
            }

            return sb.ToString();
        }
    }
}