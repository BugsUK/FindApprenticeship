namespace SFA.Apprenticeship.Api.AvService.ServiceImplementation.Version51
{
    using System;
    using System.Collections.Generic;
    using System.Security;
    using System.ServiceModel;
    using Common;
    using DataContracts.Version51;
    using MessageContracts.Version51;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
    using Namespaces.Version51;
    using Providers.Version51;
    using ServiceContracts.Version51;

    [ExceptionShielding("Default Exception Policy")]
    [ServiceBehavior(Namespace = Namespace.Uri)]
    public class VacancyManagementService : IVacancyManagement
    {
        private readonly IVacancyUploadProvider _vacancyUploadProvider;

        public VacancyManagementService(IVacancyUploadProvider vacancyUploadProvider)
        {
            _vacancyUploadProvider = vacancyUploadProvider;
        }

        public VacancyUploadResponse UploadVacancies(VacancyUploadRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // TODO: API: AG: remove test code.
            if (request.MessageId == Guid.Empty)
            {
                throw new SecurityException();
            }

            return _vacancyUploadProvider.UploadVacancies(request);
        }

        // ReSharper disable once UnusedMember.Local
        private static VacancyUploadResponse GetDummyVacancyUploadResponse()
        {
            return new VacancyUploadResponse
            {
                // MessageId = request.MessageId,
                Vacancies = new List<VacancyUploadResultData>
                {
                    new VacancyUploadResultData
                    {
                        VacancyId = Guid.Empty,
                        Status = VacancyUploadResult.Success,
                        ErrorCodes = new List<ElementErrorData>
                        {
                            new ElementErrorData
                            {
                                ErrorCode = 1
                            }
                        },
                        ReferenceNumber = 1
                    }
                }
            };
        }

        // ReSharper disable once UnusedMember.Local
        private static VacancyUploadRequest GetDummyVacancyUploadRequest()
        {
            return new VacancyUploadRequest
            {
                MessageId = Guid.Empty,
                ExternalSystemId = Guid.Empty,
                PublicKey = string.Empty,
                Vacancies = new List<VacancyUploadData>
                {
                    new VacancyUploadData
                    {
                        VacancyId = Guid.Empty,
                        Title = string.Empty,
                        ShortDescription = string.Empty,
                        LongDescription = string.Empty,
                        Employer = new EmployerData
                        {
                            EdsUrn = 0,
                            Description = string.Empty,
                            AnonymousName = string.Empty,
                            ContactName = string.Empty,
                            Website = string.Empty,
                            Image = new byte[] {}
                        },
                        Vacancy = new VacancyData
                        {
                            Wage = 0.0m,
                            WageType = WageType.Text,
                            WorkingWeek = string.Empty,
                            SkillsRequired = string.Empty,
                            QualificationRequired = string.Empty,
                            PersonalQualities = string.Empty,
                            FutureProspects = string.Empty,
                            OtherImportantInformation = string.Empty,
                            LocationType = VacancyLocationType.Standard,
                            LocationDetails = new List<SiteVacancyData>
                            {
                                new SiteVacancyData
                                {
                                    AddressDetails = new AddressData
                                    {
                                        AddressLine1 = string.Empty,
                                        AddressLine2 = string.Empty,
                                        AddressLine3 = string.Empty,
                                        AddressLine4 = string.Empty,
                                        AddressLine5 = string.Empty,
                                        County = string.Empty,
                                        GridEastM = 0,
                                        GridNorthM = 0,
                                        Latitude = 0m,
                                        Longitude = 0m,
                                        PostCode = string.Empty,
                                        Town = string.Empty,
                                        LocalAuthority = string.Empty
                                    },
                                    NumberOfVacancies = -1,
                                    EmployerWebsite = string.Empty
                                }
                            },
                            RealityCheck = string.Empty,
                            SupplementaryQuestion1 = string.Empty,
                            SupplementaryQuestion2 = string.Empty
                        },
                        Application = new ApplicationData
                        {
                            ClosingDate = DateTime.Today,
                            InterviewStartDate = DateTime.Today,
                            PossibleStartDate = DateTime.Today,
                            Type = ApplicationType.Online,
                            Instructions = string.Empty
                        },
                        Apprenticeship = new ApprenticeshipData
                        {
                            Framework = string.Empty,
                            Type = VacancyApprenticeshipType.IntermediateLevelApprenticeship,
                            TrainingToBeProvided = string.Empty,
                            ExpectedDuration = string.Empty
                        },
                        ContractedProviderUkprn = default(int?),
                        VacancyOwnerEdsUrn = -1,
                        VacancyManagerEdsUrn = default(int?),
                        DeliveryProviderEdsUrn = default(int?),
                        IsDisplayRecruitmentAgency = default(bool?),
                        IsSmallEmployerWageIncentive = false
                    }
                }
            };
        }
    }
}
