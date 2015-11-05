namespace SFA.Apprenticeship.Api.AvService.ServiceImplementation.Version51
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using Apprenticeships.Application.Interfaces.Logging;
    using Common;
    using DataContracts.Version51;
    using MessageContracts.Version51;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
    using Namespaces.Version51;
    using ServiceContracts.Version51;

    [ExceptionShielding("Default Exception Policy")]
    [ServiceBehavior(Namespace = Namespace.Uri)]
    public class VacancyDetailsService : IVacancyDetails
    {
        public VacancyDetailsService(ILogService logService)
        {
        }

        public VacancyDetailsResponse Get(VacancyDetailsRequest request)
        {
            return new VacancyDetailsResponse
            {
                MessageId = request.MessageId,
                SearchResults = new VacancyDetailResponseData
                {
                    AVMSHeader = new WebInterfaceGenericDetailsData
                    {
                        ApprenticeshipVacanciesDescription = "TODO",
                        ApprenticeshipVacanciesURL = "TODO"
                    },
                    SearchResults = new List<VacancyFullData>
                    {
                        new VacancyFullData
                        {
                            VacancyLocationType = "TODO",
                            VacancyAddress = new AddressData
                            {
                                AddressLine1 = "TODO", // ProviderSiteEmployerLink.Employer.AddressLine1
                                AddressLine2 = "TODO", // ProviderSiteEmployerLink.Employer.AddressLine2
                                AddressLine3 = "TODO", // ProviderSiteEmployerLink.Employer.AddressLine3
                                AddressLine4 = "TODO", // ProviderSiteEmployerLink.Employer.AddressLine4
                                AddressLine5 = "TODO", // TODO: no mapping
                                County = "TODO", // TODO: no mapping
                                GridEastM = -1, // via ProviderSiteEmployerLink.Employer.GeoPoint
                                GridNorthM = -1, // via ProviderSiteEmployerLink.Employer.GeoPoint
                                Latitude = -1m, // via ProviderSiteEmployerLink.Employer.GeoPoint
                                Longitude = -1m, // via ProviderSiteEmployerLink.Employer.GeoPoint
                                PostCode = "TODO", // via ProviderSiteEmployerLink.Employer.Postcode
                                Town = "TODO", // TODO: no mapping
                                LocalAuthority = "TODO" // TODO: no mapping
                            },
                            ApprenticeshipFramework = "TODO", // via FrameworkCodeName
                            ClosingDate = DateTime.MinValue, // ClosingDate
                            ShortDescription = "TODO", // ShortDescription
                            EmployerName = "TODO", // Employer.Name
                            LearningProviderName = "TODO", // TODO: via UKPRN?
                            NumberOfPositions = -1, // TODO: no mapping
                            VacancyTitle = "TODO", // Title
                            CreatedDateTime = DateTime.MinValue, // DateCreated
                            VacancyReference = -1, // VacancyReferenceNumber
                            VacancyType = "TODO", // TODO: no mapping 
                            VacancyUrl = "TODO", // TODO: OfflineApplicationUrl? ProviderSiteEmployerLink.WebsiteUrl? 
                            FullDescription = "TODO", // LongDescription
                            SupplementaryQuestion1 = "TODO", // FirstQuestion
                            SupplementaryQuestion2 = "TODO", // SecondQuestion
                            ContactPerson = "TODO", // TODO: no mapping
                            EmployerDescription = "TODO", // ProviderSiteEmployerLink.Description
                            ExpectedDuration = "TODO", // TODO: Duration + DurationType?
                            FutureProspects = "TODO", // FutureProspects
                            InterviewFromDate = DateTime.MinValue, // TODO: no mapping
                            LearningProviderDesc = "TODO", // TODO: no mapping
                            LearningProviderSectorPassRate = -1, // TODO: no mapping
                            PersonalQualities = "TODO", // PersonalQualities
                            PossibleStartDate = DateTime.MinValue, // PossibleStartDate
                            QualificationRequired = "TODO", // DesiredQualifications
                            SkillsRequired = "TODO", // DesiredSkills
                            TrainingToBeProvided = "TODO", // TODO: no mapping
                            WageType = WageType.Weekly, // WageType
                            Wage = -1m, // Wage
                            WageText = "TODO", // TODO: Wage + WageUnit + ?
                            WorkingWeek = "TODO", // TODO: WorkingWeek + HoursPerWeek?
                            OtherImportantInformation = "TODO", // TODO: no mapping
                            EmployerWebsite = "TODO", // ProviderSiteEmployerLink.WebsiteUrl
                            VacancyOwner = "TODO", // TODO: no mapping
                            ContractOwner = "TODO", // TODO: no mapping
                            DeliveryOrganisation = "TODO", // TODO: no mapping
                            VacancyManager = "TODO", // TODO: no mapping
                            IsDisplayRecruitmentAgency = null, // TODO: no mapping
                            IsSmallEmployerWageIncentive = false // TODO: no mapping
                        }
                    },
                    TotalPages = 1
                }
            };
        }
    }
}
