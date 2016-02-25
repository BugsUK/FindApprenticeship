namespace SFA.Apprenticeships.Infrastructure.Raa.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Domain.Entities.Locations;
    using Domain.Entities.Raa.Vacancies;
    using SFA.Infrastructure.Interfaces;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Extensions;
    using Presentation;

    public class ApprenticeshipVacancyDetailMapper
    {
        public static ApprenticeshipVacancyDetail GetApprenticeshipVacancyDetail(Vacancy vacancy, IEnumerable<Category> categories, ILogService logService)
        {
            //Manually mapping rather than using automapper as the two enties are significantly different
            var wage = new Wage(vacancy.WageType, vacancy.Wage, vacancy.WageUnit);
            var detail = new ApprenticeshipVacancyDetail
            {
                Id = (int)vacancy.VacancyReferenceNumber,
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
                WageType = vacancy.WageType.GetLegacyWageType(),
                WorkingWeek = vacancy.WorkingWeek,
                OtherInformation = vacancy.ThingsToConsider,
                FutureProspects = vacancy.FutureProspects,
                //TODO: Where from?
                //VacancyOwner = vacancy.,
                //VacancyManager = vacancy.,
                //LocalAuthority = vacancy.,
                //TODO: Map once Vicenc has finished with multi location work
                NumberOfPositions = 1,
                RealityCheck = vacancy.ThingsToConsider,
                Created = vacancy.CreatedDateTime,
                VacancyStatus = vacancy.Status.GetVacancyStatuses(),
                //EmployerName = vacancy.ProviderSiteEmployerLink.Employer.Name,
                //TODO: How is this captured in RAA?
                //AnonymousEmployerName = vacancy.,
                //EmployerDescription = vacancy.ProviderSiteEmployerLink.Description,
                //EmployerWebsite = vacancy.ProviderSiteEmployerLink.WebsiteUrl,
                ApplyViaEmployerWebsite = vacancy.OfflineVacancy ?? false,
                VacancyUrl = vacancy.OfflineApplicationUrl,
                ApplicationInstructions = vacancy.OfflineApplicationInstructions,
                //TODO: How is this captured in RAA?
                //IsEmployerAnonymous = vacancy.,
                //TODO: Are we going to add this to RAA?
                //IsPositiveAboutDisability = vacancy.,
                ExpectedDuration = new Duration(vacancy.DurationType, vacancy.Duration).GetDisplayText(),
                //VacancyAddress = vacancy.ProviderSiteEmployerLink.Employer.Address,
                //TODO: How is this captured in RAA?
                //IsRecruitmentAgencyAnonymous = vacancy.,
                //TODO: How is this captured in RAA?
                //IsSmallEmployerWageIncentive = vacancy.,
                SupplementaryQuestion1 = vacancy.FirstQuestion,
                SupplementaryQuestion2 = vacancy.SecondQuestion,
                //TODO: How is this captured in RAA?
                //RecruitmentAgency = vacancy.,
                //TODO: Get provider
                //ProviderName = vacancy.VacancyParty.,
                //TradingName = vacancy.,
                //ProviderDescription = vacancy.,
                Contact = GetContactInformation(vacancy),
                //ProviderSectorPassRate = vacancy.,
                TrainingToBeProvided = vacancy.TrainingProvided,
                //TODO: How is this captured in RAA?
                //ContractOwner = vacancy.,
                //DeliveryOrganisation = vacancy.,
                //IsNasProvider = vacancy.,
                PersonalQualities = vacancy.PersonalQualities,
                QualificationRequired = vacancy.DesiredQualifications,
                SkillsRequired = vacancy.DesiredSkills,
                //TODO: How do we determine this in RAA?
                VacancyLocationType = ApprenticeshipLocationType.NonNational,
                ApprenticeshipLevel = vacancy.ApprenticeshipLevel.GetApprenticeshipLevel(),


                //TODO: Get provider
                //ProviderName = vacancy.VacancyParty.,
                //TODO: Are we going to add this to RAA?
                //IsPositiveAboutDisability = vacancy.,
                //TODO: Store geopoints for employers
                //Location = vacancy.VacancyParty.Employer.Address.GeoPoint,
                //Location = new GeoPoint { Latitude = 52.4009991288043, Longitude = -1.50812239495425 }, //Coventry
                //SubCategoryCode = vacancy.FrameworkCodeName
            };

            // TODO: need to remove this hack.
            if (detail.VacancyAddress == null)
            {
                detail.VacancyAddress = new Address
                {
                    AddressLine1 = "Skills Funding Agency",
                    AddressLine2 = "Quinton Road",
                    AddressLine3 = "Coventry",
                    Postcode = "CV1 2WT"                    
                };
            }

            if (detail.VacancyAddress.GeoPoint == null)
            {
                //TODO: Store geopoints for employers
                detail.VacancyAddress.GeoPoint = new GeoPoint { Latitude = 52.4009991288043, Longitude = -1.50812239495425 }; //Coventry
            }

            var frameworkCodeName = vacancy.FrameworkCodeName;
            if (!string.IsNullOrEmpty(frameworkCodeName))
            {
                var category = categories.SingleOrDefault(c => c.SubCategories.Any(sc => sc.CodeName == frameworkCodeName));
                if (category == null)
                {
                    logService.Warn("Cannot find category containing a subcategory matching code {1} for the vacancy with Id {0}", detail.Id, frameworkCodeName);
                }
                else
                {
                    var subCategory = category.SubCategories.SingleOrDefault(sc => sc.CodeName == frameworkCodeName);
                    if (subCategory == null)
                    {
                        detail.SubCategory = "Unknown";
                        logService.Warn("Cannot find subcatagory matching code {1} in category {3} ({2}) for the vacancy with Id {0}", detail.Id, frameworkCodeName, category.CodeName, category.FullName);
                    }
                    else
                    {
                        detail.SubCategory = subCategory.FullName;
                    }
                }
            }

            return detail;
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