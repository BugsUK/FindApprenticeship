#pragma warning disable 612
namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using Application.Interfaces;
    using Application.Interfaces.Applications;
    using Application.Interfaces.Employers;
    using Application.Interfaces.Locations;
    using Application.Interfaces.Providers;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.Users;
    using Application.Interfaces.Vacancies;
    using Application.Interfaces.VacancyPosting;
    using Application.Vacancy;
    using Configuration;
    using Converters;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Reference;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Vacancies;
    using Domain.Raa.Interfaces.Repositories.Models;
    using Infrastructure.Presentation;
    using Infrastructure.Raa.Extensions;
    using Mappers;
    using Microsoft.Rest;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels;
    using ViewModels.Admin;
    using ViewModels.Provider;
    using ViewModels.ProviderUser;
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;
    using Web.Common.Configuration;
    using Web.Common.ViewModels;
    using Web.Common.ViewModels.Locations;
    using ApiVacancy = DAS.RAA.Api.Client.V1.Models.Vacancy;
    using ApprenticeshipLevel = Domain.Entities.Raa.Vacancies.ApprenticeshipLevel;
    using TrainingType = Domain.Entities.Raa.Vacancies.TrainingType;
    using VacancyLocationType = Domain.Entities.Raa.Vacancies.VacancyLocationType;
    using VacancySummary = Domain.Entities.Raa.Vacancies.VacancySummary;
    using VacancyType = Domain.Entities.Raa.Vacancies.VacancyType;

    public class VacancyProvider : IVacancyPostingProvider, IVacancyQAProvider
    {
        private static readonly IMapper ApiClientMappers = new ApiClientMappers();

        private readonly ILogService _logService;
        private readonly IVacancyPostingService _vacancyPostingService;
        private readonly IReferenceDataService _referenceDataService;
        private readonly IProviderService _providerService;
        private readonly IEmployerService _employerService;
        private readonly IDateTimeService _dateTimeService;
        private readonly IApprenticeshipApplicationService _apprenticeshipApplicationService;
        private readonly ITraineeshipApplicationService _traineeshipApplicationService;
        private readonly IVacancyLockingService _vacancyLockingService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserProfileService _userProfileService;
        private readonly IConfigurationService _configurationService;
        private readonly IMapper _mapper;
        private readonly IGeoCodeLookupService _geoCodingService;
        private readonly ILocalAuthorityLookupService _localAuthorityLookupService;
        private readonly IVacancySummaryService _vacancySummaryService;
        private readonly IApiClientProvider _apiClientProvider;

        public VacancyProvider(ILogService logService, IConfigurationService configurationService,
            IVacancyPostingService vacancyPostingService, IReferenceDataService referenceDataService,
            IProviderService providerService, IEmployerService employerService, IDateTimeService dateTimeService,
            IMapper mapper, IApprenticeshipApplicationService apprenticeshipApplicationService,
            ITraineeshipApplicationService traineeshipApplicationService, IVacancyLockingService vacancyLockingService,
            ICurrentUserService currentUserService, IUserProfileService userProfileService,
            IGeoCodeLookupService geocodingService, ILocalAuthorityLookupService localAuthLookupService,
            IVacancySummaryService vacancySummaryService, IApiClientProvider apiClientProvider)
        {
            _logService = logService;
            _vacancyPostingService = vacancyPostingService;
            _referenceDataService = referenceDataService;
            _providerService = providerService;
            _employerService = employerService;
            _dateTimeService = dateTimeService;
            _configurationService = configurationService;
            _mapper = mapper;
            _apprenticeshipApplicationService = apprenticeshipApplicationService;
            _traineeshipApplicationService = traineeshipApplicationService;
            _vacancyLockingService = vacancyLockingService;
            _currentUserService = currentUserService;
            _userProfileService = userProfileService;
            _geoCodingService = geocodingService;
            _localAuthorityLookupService = localAuthLookupService;
            _vacancySummaryService = vacancySummaryService;
            _apiClientProvider = apiClientProvider;
        }

        public NewVacancyViewModel GetNewVacancyViewModel(int vacancyOwnerRelationshipId, Guid vacancyGuid, int? numberOfPositions)
        {
            var existingVacancy = _vacancyPostingService.GetVacancy(vacancyGuid);

            var vacancyOwnerRelationship = _providerService.GetVacancyOwnerRelationship(vacancyOwnerRelationshipId, true);
            var employer = _employerService.GetEmployer(vacancyOwnerRelationship.EmployerId, true);
            var vacancyOwnerRelationshipViewModel = vacancyOwnerRelationship.Convert(employer);

            if (existingVacancy != null)
            {
                var vacancyViewModel = _mapper.Map<Vacancy, NewVacancyViewModel>(existingVacancy);
                vacancyViewModel.VacancyOwnerRelationship = vacancyOwnerRelationshipViewModel;
                vacancyViewModel.AutoSaveTimeoutInSeconds =
                    _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;
                vacancyViewModel.LocationAddresses = GetLocationsAddressViewModel(existingVacancy);
                return vacancyViewModel;
            }

            var newVacancyModel = new NewVacancyViewModel
            {
                VacancyOwnerRelationship = vacancyOwnerRelationshipViewModel,
                NumberOfPositions = numberOfPositions,
                AutoSaveTimeoutInSeconds = _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds
            };
            if (numberOfPositions.HasValue)
                newVacancyModel.VacancyLocationType = VacancyLocationType.SpecificLocation;
            return newVacancyModel;
        }

        public List<VacancyLocationAddressViewModel> GetLocationsAddressViewModelsByReferenceNumber(
            int vacancyReferenceNumber)
        {
            var locationAddresses = _vacancyPostingService.GetVacancyLocationsByReferenceNumber(vacancyReferenceNumber);
            return GetVacancyLocationAddressViewModels(locationAddresses);
        }

        public List<VacancyLocationAddressViewModel> GetLocationsAddressViewModels(int vacancyId)
        {
            var locationAddresses = _vacancyPostingService.GetVacancyLocations(vacancyId);
            return GetVacancyLocationAddressViewModels(locationAddresses);
        }

        private List<VacancyLocationAddressViewModel> GetVacancyLocationAddressViewModels(List<VacancyLocation> locationAddresses)
        {
            if (locationAddresses != null && locationAddresses.Any())
            {
                return _mapper.Map<List<VacancyLocation>, List<VacancyLocationAddressViewModel>>(locationAddresses);
            }
            return new List<VacancyLocationAddressViewModel>();
        }

        private List<VacancyLocationAddressViewModel> GetLocationsAddressViewModel(Vacancy vacancy)
        {
            var locationAddresses = GetLocationsAddressViewModels(vacancy.VacancyId);
            if (locationAddresses.Any())
            {
                foreach (VacancyLocationAddressViewModel vacancyLocationAddressViewModel in locationAddresses)
                {
                    var vacancyAddress =
                        _mapper.Map<AddressViewModel, PostalAddress>(vacancyLocationAddressViewModel.Address);
                    if (vacancyAddress.GeoPoint == null || vacancyAddress.GeoPoint.IsSet())
                    {
                        var geoPoint =
                    _geoCodingService.GetGeoPointFor(vacancyAddress);
                        vacancyLocationAddressViewModel.Address.GeoPoint =
                            _mapper.Map<GeoPoint, GeoPointViewModel>(geoPoint);
                    }
                }
                return locationAddresses;
            }

            if (vacancy.VacancyLocationType == VacancyLocationType.MultipleLocations &&
                vacancy.Address != null)
            {

                locationAddresses.Add(
                    new VacancyLocationAddressViewModel
                    {
                        Address = _mapper.Map<PostalAddress, AddressViewModel>(vacancy.Address),
                        NumberOfPositions = vacancy.NumberOfPositions
                    });

            }
            return locationAddresses;
        }

        private AddressViewModel GeoCodeAddressForVacancy(Vacancy vacancy, AddressViewModel addressViewModel)
        {
            if (vacancy.Address != null && (vacancy.Address.GeoPoint == null || vacancy.Address.GeoPoint.IsSet()))
            {
                vacancy.Address.GeoPoint = _geoCodingService.GetGeoPointFor(vacancy.Address);
                addressViewModel = _mapper.Map<PostalAddress, AddressViewModel>(vacancy.Address);
            }
            return addressViewModel;
        }

        public NewVacancyViewModel GetNewVacancyViewModel(int vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);
            var vacancyOwnerRelationship = _providerService.GetVacancyOwnerRelationship(vacancy.VacancyOwnerRelationshipId, false);
            var viewModel = _mapper.Map<Vacancy, NewVacancyViewModel>(vacancy);
            var employer = _employerService.GetEmployer(vacancyOwnerRelationship.EmployerId, false);
            viewModel.LocationAddresses = GetLocationsAddressViewModel(vacancy);
            viewModel.VacancyOwnerRelationship = vacancyOwnerRelationship.Convert(employer, vacancy);
            viewModel.AutoSaveTimeoutInSeconds =
                _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;
            viewModel.VacancyGuid = vacancy.VacancyGuid;
            return viewModel;
        }

        private void GeoCodeVacancyLocations(LocationSearchViewModel viewModel)
        {
            foreach (var vacancyLocationAddressViewModel in viewModel.Addresses)
            {
                var geoPoint =
                    _geoCodingService.GetGeoPointFor(
                        _mapper.Map<AddressViewModel, PostalAddress>(vacancyLocationAddressViewModel.Address));

                vacancyLocationAddressViewModel.Address.GeoPoint =
                    _mapper.Map<GeoPoint, GeoPointViewModel>(geoPoint);
            }
        }

        private void GeoCodeVacancyLocations(List<VacancyLocation> vacancyLocations)
        {
            foreach (var vacancyLocation in vacancyLocations)
            {
                vacancyLocation.Address.GeoPoint =
                    _geoCodingService.GetGeoPointFor(vacancyLocation.Address);
            }
        }

        public LocationSearchViewModel LocationAddressesViewModel(string ukprn, int providerSiteId, int employerId, Guid vacancyGuid, bool isEmployerAnonymous = false)
        {
            var vacancy = _vacancyPostingService.GetVacancy(vacancyGuid);
            var providerSite = _providerService.GetProviderSite(providerSiteId);
            var employer = _employerService.GetEmployer(employerId, true);

            if (vacancy != null)
            {
                var viewModel = new LocationSearchViewModel
                {
                    ProviderSiteEdsUrn = providerSite.EdsUrn,
                    EmployerEdsUrn = employer.EdsUrn,
                    VacancyGuid = vacancyGuid,
                    Ukprn = ukprn,
                    AdditionalLocationInformation = vacancy.AdditionalLocationInformation,
                    Status = vacancy.Status,
                    VacancyReferenceNumber = vacancy.VacancyReferenceNumber,
                    EmployerApprenticeshipLocation = VacancyLocationType.MultipleLocations,
                    Addresses = new List<VacancyLocationAddressViewModel>(),
                    LocationAddressesComment = vacancy.LocationAddressesComment,
                    AdditionalLocationInformationComment = vacancy.AdditionalLocationInformationComment,
                    AutoSaveTimeoutInSeconds = _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds,
                    ProviderSiteId = providerSite.ProviderSiteId,
                    IsAnonymousEmployer = isEmployerAnonymous
                };

                var locationAddresses = _vacancyPostingService.GetVacancyLocations(vacancy.VacancyId);
                if (locationAddresses.Any())
                {
                    viewModel.Addresses =
                        _mapper.Map<List<VacancyLocation>, List<VacancyLocationAddressViewModel>>(locationAddresses);
                }
                else
                {
                    if (vacancy.VacancyLocationType == VacancyLocationType.MultipleLocations && vacancy.Address != null)
                    {
                        viewModel.Addresses = new List<VacancyLocationAddressViewModel>();
                        viewModel.Addresses.Add(
                            new VacancyLocationAddressViewModel
                            {
                                Address = _mapper.Map<PostalAddress, AddressViewModel>(vacancy.Address),
                                NumberOfPositions = vacancy.NumberOfPositions
                            });
                    }
                }

                return viewModel;
            }
            return new LocationSearchViewModel
            {
                ProviderSiteId = providerSite.ProviderSiteId,
                ProviderSiteEdsUrn = providerSite.EdsUrn,
                EmployerId = employer.EmployerId,
                EmployerEdsUrn = employer.EdsUrn,
                VacancyGuid = vacancyGuid,
                Ukprn = ukprn,
                Addresses = new List<VacancyLocationAddressViewModel>(),
                AutoSaveTimeoutInSeconds = _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds,
                IsAnonymousEmployer = isEmployerAnonymous
            };
        }

        public VacancyMinimumData UpdateVacancy(VacancyMinimumData vacancyMinimumData)
        {
            var vacancyOwnerRelationship =
                _providerService.GetVacancyOwnerRelationship(vacancyMinimumData.VacancyOwnerRelationshipId, true);
            if (vacancyOwnerRelationship == null)
                throw new Exception($"Vacancy Party {vacancyMinimumData.VacancyOwnerRelationshipId} not found / no longer current");

            var employer = _employerService.GetEmployer(vacancyOwnerRelationship.EmployerId, true);

            if (!employer.Address.GeoPoint.IsValid())
            {
                employer.Address.GeoPoint = _geoCodingService.GetGeoPointFor(employer.Address);
            }

            var vacancy = _vacancyPostingService.GetVacancy(vacancyMinimumData.VacancyGuid);

            vacancy.VacancyLocationType = vacancy.VacancyLocationType;
            vacancy.NumberOfPositions = vacancy.NumberOfPositions ?? 0;
            vacancy.Address = vacancy.VacancyLocationType == VacancyLocationType.SpecificLocation
                                || vacancy.VacancyLocationType == VacancyLocationType.Nationwide
                                ? employer.Address
                                : null;
            vacancy.LocalAuthorityCode = _localAuthorityLookupService.GetLocalAuthorityCode(employer.Address.Postcode);
            vacancy.EmployerDescription = vacancyMinimumData.EmployerDescription;
            vacancy.EmployerWebsiteUrl = vacancyMinimumData.EmployerWebsiteUrl;

            _vacancyPostingService.UpdateVacancy(vacancy);

            return vacancyMinimumData;
        }

        /// <summary>
        /// This method will create a new Vacancy record if the model provided does not have a vacancy reference number.
        /// Otherwise, it updates the existing one.
        /// </summary>
        /// <param name="newVacancyViewModel"></param>
        /// <returns></returns>
        public NewVacancyViewModel UpdateVacancy(NewVacancyViewModel newVacancyViewModel)
        {
            var resultViewModel = UpdateExistingVacancy(newVacancyViewModel);

            resultViewModel.AutoSaveTimeoutInSeconds =
                _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;

            return resultViewModel;
        }

        public void CreateVacancy(VacancyMinimumData vacancyMinimumData)
        {
            var vacancyReferenceNumber = _vacancyPostingService.GetNextVacancyReferenceNumber();
            var vacancyOwnerRelationship = _providerService.GetVacancyOwnerRelationship(vacancyMinimumData.VacancyOwnerRelationshipId, true);
            var employer = _employerService.GetEmployer(vacancyOwnerRelationship.EmployerId, true);
            var provider = _providerService.GetProvider(vacancyMinimumData.Ukprn);

            if (!employer.Address.GeoPoint.IsValid())
            {
                employer.Address.GeoPoint = _geoCodingService.GetGeoPointFor(employer.Address);
            }

            _vacancyPostingService.CreateVacancy(new Vacancy
            {
                VacancyGuid = vacancyMinimumData.VacancyGuid,
                VacancyReferenceNumber = vacancyReferenceNumber,
                VacancyOwnerRelationshipId = vacancyOwnerRelationship.VacancyOwnerRelationshipId,
                Status = VacancyStatus.Draft,
                VacancyLocationType = vacancyMinimumData.VacancyLocationType,
                NumberOfPositions = vacancyMinimumData.NumberOfPositions ?? 0,
                Address = vacancyMinimumData.VacancyLocationType == VacancyLocationType.SpecificLocation
                || vacancyMinimumData.VacancyLocationType == VacancyLocationType.Nationwide
                ? employer.Address : null,
                ContractOwnerId = provider.ProviderId, //Confirmed from ReportUnsuccessfulCandidateApplications stored procedure
                OriginalContractOwnerId = provider.ProviderId, //Confirmed from ReportUnsuccessfulCandidateApplications stored procedure
                LocalAuthorityCode = _localAuthorityLookupService.GetLocalAuthorityCode(employer.Address.Postcode),
                EmployerDescription = vacancyMinimumData.EmployerDescription,
                EmployerWebsiteUrl = vacancyMinimumData.EmployerWebsiteUrl,
                EmployerAnonymousName = vacancyMinimumData.AnonymousEmployerDescription,
                EmployerAnonymousReason = vacancyMinimumData.AnonymousEmployerReason,
                AnonymousAboutTheEmployer = vacancyMinimumData.AnonymousAboutTheEmployer
            });
        }

        public void TransferVacancies(ManageVacancyTransferViewModel vacancyTransferViewModel)
        {
            try
            {
                foreach (var referenceNumber in vacancyTransferViewModel.VacancyReferenceNumbers)
                {
                    var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(referenceNumber);
                    if (vacancy != null)
                    {
                        var vacancyOwnerRelationship =
                            _providerService.GetVacancyOwnerRelationship(vacancy.VacancyOwnerRelationshipId, false);
                        //This method actually returns a new VOR but with an Id of 0 if none exists however that couples this code to that implementation so we should still perform the null check 
                        var existingVacancyOwnerRelationship =
                            _providerService.GetVacancyOwnerRelationship(vacancyOwnerRelationship.EmployerId,
                                vacancyTransferViewModel.ProviderSiteId, true);
                        if (existingVacancyOwnerRelationship == null || existingVacancyOwnerRelationship.VacancyOwnerRelationshipId == 0)
                        {
                            //No matching VOR exists for the new provider and provider site so create it.
                            //We do this because changing the provider site id for a VOR could make any non transfered vacancies associated with it unavailable to any provider
                            existingVacancyOwnerRelationship = existingVacancyOwnerRelationship ?? new VacancyOwnerRelationship { ProviderSiteId = vacancyTransferViewModel.ProviderSiteId, EmployerId = vacancyOwnerRelationship.EmployerId };
                            existingVacancyOwnerRelationship.EmployerWebsiteUrl = vacancyOwnerRelationship.EmployerWebsiteUrl;
                            existingVacancyOwnerRelationship.EmployerDescription = vacancyOwnerRelationship.EmployerDescription;
                            existingVacancyOwnerRelationship.StatusType = VacancyOwnerRelationshipStatusTypes.Live;
                            existingVacancyOwnerRelationship = _providerService.SaveVacancyOwnerRelationship(existingVacancyOwnerRelationship);
                        }

                        vacancy.VacancyOwnerRelationshipId = existingVacancyOwnerRelationship.VacancyOwnerRelationshipId;
                        vacancy.ContractOwnerId = vacancyTransferViewModel.ProviderId;
                        vacancy.DeliveryOrganisationId = vacancyTransferViewModel.ProviderSiteId;
                        vacancy.VacancyManagerId = vacancyTransferViewModel.ProviderSiteId;
                        _vacancyPostingService.UpdateVacanciesWithNewProvider(vacancy);
                    }
                }
            }
            catch (Exception exception)
            {
                _logService.Error($"Exception occurred while transferring the vacancy:{exception.Message}");
                throw exception;
            }
        }

        public FurtherVacancyDetailsViewModel CloseVacancy(FurtherVacancyDetailsViewModel viewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber);
            vacancy.ClosingDate = DateTime.Now;
            vacancy.Status = VacancyStatus.Closed;
            FurtherVacancyDetailsViewModel result;
            try
            {
                vacancy = _vacancyPostingService.UpdateVacancy(vacancy);
            }
            catch (CustomException)
            {
                result = _mapper.Map<Vacancy, FurtherVacancyDetailsViewModel>(vacancy);
                result.VacancyApplicationsState = VacancyApplicationsState.Invalid;
                result.AutoSaveTimeoutInSeconds =
                    _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;
                return result;
            }

            result = _mapper.Map<Vacancy, FurtherVacancyDetailsViewModel>(vacancy);
            result.VacancyApplicationsState = GetVacancyApplicationsState(vacancy);
            result.AutoSaveTimeoutInSeconds =
                _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;
            return result;
        }

        private string GetFrameworkCodeName(TrainingDetailsViewModel trainingDetailsViewModel)
        {
            return trainingDetailsViewModel.TrainingType == TrainingType.Standards ? null : CategoryPrefixes.GetOriginalFrameworkCode(trainingDetailsViewModel.FrameworkCodeName);
        }

        private int? GetStandardId(TrainingDetailsViewModel trainingDetailsViewModel)
        {
            return trainingDetailsViewModel.TrainingType == TrainingType.Frameworks ? null : trainingDetailsViewModel.StandardId;
        }

        private ApprenticeshipLevel GetApprenticeshipLevel(TrainingDetailsViewModel trainingDetailsViewModel)
        {
            var apprenticeshipLevel = trainingDetailsViewModel.ApprenticeshipLevel;
            if (trainingDetailsViewModel.TrainingType == TrainingType.Standards)
            {
                var standard = GetStandard(trainingDetailsViewModel.StandardId);
                apprenticeshipLevel = standard?.ApprenticeshipLevel ?? ApprenticeshipLevel.Unknown;
            }
            return apprenticeshipLevel;
        }

        private string GetSectorCodeName(TrainingDetailsViewModel trainingDetailsViewModel)
        {
            return trainingDetailsViewModel.VacancyType == VacancyType.Traineeship ? CategoryPrefixes.GetOriginalSectorSubjectAreaTier1Code(trainingDetailsViewModel.SectorCodeName) : null;
        }

        private NewVacancyViewModel UpdateExistingVacancy(NewVacancyViewModel newVacancyViewModel)
        {
            var comesFromPreview = newVacancyViewModel.ComeFromPreview;

            var vacancyOwnerRelationship =
                _providerService.GetVacancyOwnerRelationship(newVacancyViewModel.VacancyOwnerRelationship.VacancyOwnerRelationshipId, true);
            if (vacancyOwnerRelationship == null)
                throw new Exception($"Vacancy Party {newVacancyViewModel.VacancyOwnerRelationship.VacancyOwnerRelationshipId} not found / no longer current");

            var vacancy = newVacancyViewModel.VacancyReferenceNumber.HasValue
                ? _vacancyPostingService.GetVacancyByReferenceNumber(newVacancyViewModel.VacancyReferenceNumber.Value)
                : _vacancyPostingService.GetVacancy(newVacancyViewModel.VacancyGuid);
            var currentOfflineVacancyType = vacancy.OfflineVacancyType;

            vacancy.Title = newVacancyViewModel.Title;
            vacancy.ShortDescription = newVacancyViewModel.ShortDescription;
            vacancy.OfflineVacancy = newVacancyViewModel.OfflineVacancy;
            vacancy.OfflineVacancyType = newVacancyViewModel.OfflineVacancyType;

            if (currentOfflineVacancyType != OfflineVacancyType.MultiUrl)
            {
                var offlineApplicationUrl = !string.IsNullOrEmpty(newVacancyViewModel.OfflineApplicationUrl)
                    ? new UriBuilder(newVacancyViewModel.OfflineApplicationUrl).Uri.ToString()
                    : newVacancyViewModel.OfflineApplicationUrl;
                vacancy.OfflineApplicationUrl = offlineApplicationUrl;
            }

            vacancy.OfflineApplicationInstructions = newVacancyViewModel.OfflineApplicationInstructions;
            vacancy.VacancyType = newVacancyViewModel.VacancyType;

            if (newVacancyViewModel.LocationAddresses != null)
            {
                var vacancyLocations = _vacancyPostingService.GetVacancyLocations(vacancy.VacancyId);
                if (vacancyLocations != null)
                {
                    foreach (var locationAddress in newVacancyViewModel.LocationAddresses)
                    {
                        var vacancyLocation = vacancyLocations.SingleOrDefault(vl => vl.VacancyLocationId == locationAddress.VacancyLocationId);
                        if (vacancyLocation != null)
                        {
                            var offlineApplicationUrl = !string.IsNullOrEmpty(locationAddress.OfflineApplicationUrl) ? new UriBuilder(locationAddress.OfflineApplicationUrl).Uri.ToString() : locationAddress.OfflineApplicationUrl;
                            vacancyLocation.EmployersWebsite = offlineApplicationUrl;
                        }
                    }
                }
                _vacancyPostingService.UpdateVacancyLocations(vacancyLocations);
            }

            vacancy = _vacancyPostingService.UpdateVacancy(vacancy);

            newVacancyViewModel = _mapper.Map<Vacancy, NewVacancyViewModel>(vacancy);
            newVacancyViewModel.ComeFromPreview = comesFromPreview;

            return newVacancyViewModel;
        }

        public TrainingDetailsViewModel GetTrainingDetailsViewModel(int vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);
            var viewModel = _mapper.Map<Vacancy, TrainingDetailsViewModel>(vacancy);
            if (viewModel.VacancyType == VacancyType.Traineeship)
            {
                viewModel.TrainingType = TrainingType.Sectors;
            }
            var sectorsAndFrameworks = GetSectorsAndFrameworks();
            var standards = GetStandards();
            var sectors = GetSectors();
            viewModel.SectorsAndFrameworks = sectorsAndFrameworks;
            viewModel.Standards = standards;
            viewModel.Sectors = sectors;
            viewModel.AutoSaveTimeoutInSeconds =
                    _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;

            return viewModel;
        }

        public TrainingDetailsViewModel UpdateVacancy(TrainingDetailsViewModel viewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber.Value);

            vacancy.TrainingType = viewModel.TrainingType;
            vacancy.FrameworkCodeName = GetFrameworkCodeName(viewModel);
            vacancy.StandardId = GetStandardId(viewModel);
            vacancy.ApprenticeshipLevel = GetApprenticeshipLevel(viewModel);
            vacancy.SectorCodeName = GetSectorCodeName(viewModel);
            vacancy.TrainingProvided = viewModel.TrainingProvided;
            vacancy.ContactName = viewModel.ContactName;
            vacancy.ContactNumber = viewModel.ContactNumber;
            vacancy.ContactEmail = viewModel.ContactEmail;

            vacancy = _vacancyPostingService.UpdateVacancy(vacancy);

            viewModel = _mapper.Map<Vacancy, TrainingDetailsViewModel>(vacancy);

            viewModel.AutoSaveTimeoutInSeconds =
                _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;

            return viewModel;
        }

        public FurtherVacancyDetailsViewModel GetVacancySummaryViewModel(int vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);
            var viewModel = vacancy.ConvertToVacancySummaryViewModel();
            viewModel.Wage = _mapper.Map<Wage, WageViewModel>(vacancy.Wage);

            viewModel.VacancyApplicationsState = GetVacancyApplicationsState(vacancy);

            viewModel.AutoSaveTimeoutInSeconds =
                    _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;

            return viewModel;
        }

        public FurtherVacancyDetailsViewModel UpdateVacancy(FurtherVacancyDetailsViewModel viewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber);

            vacancy.WorkingWeek = viewModel.WorkingWeek;
            vacancy.Wage = _mapper.Map<WageViewModel, Wage>(viewModel.Wage);
            vacancy.DurationType = viewModel.DurationType;
            vacancy.Duration = viewModel.Duration.HasValue ? (int?)Math.Round(viewModel.Duration.Value) : null;

            if (viewModel.VacancyDatesViewModel.ClosingDate.HasValue)
            {
                vacancy.ClosingDate = viewModel.VacancyDatesViewModel.ClosingDate?.Date;
            }
            if (viewModel.VacancyDatesViewModel.PossibleStartDate.HasValue)
            {
                vacancy.PossibleStartDate = viewModel.VacancyDatesViewModel.PossibleStartDate?.Date;
            }

            vacancy.LongDescription = viewModel.LongDescription;

            vacancy = _vacancyPostingService.UpdateVacancy(vacancy);

            var updatedViewModel = vacancy.ConvertToVacancySummaryViewModel();
            updatedViewModel.Wage = _mapper.Map<Wage, WageViewModel>(vacancy.Wage);

            updatedViewModel.AutoSaveTimeoutInSeconds =
                _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;

            return updatedViewModel;
        }

        public VacancyRequirementsProspectsViewModel GetVacancyRequirementsProspectsViewModel(int vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);
            var viewModel = vacancy.ConvertToVacancyRequirementsProspectsViewModel();
            viewModel.AutoSaveTimeoutInSeconds =
                    _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;

            return viewModel;
        }

        public VacancyRequirementsProspectsViewModel UpdateVacancy(VacancyRequirementsProspectsViewModel viewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber);

            vacancy.DesiredSkills = viewModel.DesiredSkills;
            vacancy.FutureProspects = viewModel.FutureProspects;
            vacancy.PersonalQualities = viewModel.PersonalQualities;
            vacancy.ThingsToConsider = viewModel.ThingsToConsider;
            vacancy.DesiredQualifications = viewModel.DesiredQualifications;

            vacancy = _vacancyPostingService.UpdateVacancy(vacancy);

            viewModel = vacancy.ConvertToVacancyRequirementsProspectsViewModel();
            viewModel.AutoSaveTimeoutInSeconds =
                _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;

            return viewModel;
        }

        public VacancyQuestionsViewModel GetVacancyQuestionsViewModel(int vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);
            var viewModel = vacancy.ConvertToVacancyQuestionsViewModel();
            viewModel.AutoSaveTimeoutInSeconds =
                    _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;

            return viewModel;
        }

        public VacancyQuestionsViewModel UpdateVacancy(VacancyQuestionsViewModel viewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber);

            vacancy.FirstQuestion = viewModel.FirstQuestion;
            vacancy.SecondQuestion = viewModel.SecondQuestion;

            vacancy = _vacancyPostingService.UpdateVacancy(vacancy);

            viewModel = vacancy.ConvertToVacancyQuestionsViewModel();
            viewModel.AutoSaveTimeoutInSeconds =
                _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;

            return viewModel;
        }

        public FurtherVacancyDetailsViewModel UpdateVacancyDates(FurtherVacancyDetailsViewModel viewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber);

            vacancy.ClosingDate = viewModel.VacancyDatesViewModel.ClosingDate.Date;
            vacancy.PossibleStartDate = viewModel.VacancyDatesViewModel.PossibleStartDate.Date;
            vacancy.Wage = _mapper.Map<WageViewModel, Wage>(viewModel.Wage);
            vacancy.Status = VacancyStatus.Live;

            FurtherVacancyDetailsViewModel result;

            try
            {
                vacancy = _vacancyPostingService.UpdateVacancy(vacancy);
            }
            catch (CustomException)
            {
                result = _mapper.Map<Vacancy, FurtherVacancyDetailsViewModel>(vacancy);
                result.VacancyApplicationsState = VacancyApplicationsState.Invalid;
                result.AutoSaveTimeoutInSeconds =
                    _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;

                return result;
            }

            result = _mapper.Map<Vacancy, FurtherVacancyDetailsViewModel>(vacancy);

            result.VacancyApplicationsState = GetVacancyApplicationsState(vacancy);

            result.AutoSaveTimeoutInSeconds =
                _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;

            return result;
        }

        public void EmptyVacancyLocation(int vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);

            vacancy.Address = null;
            vacancy.NumberOfPositions = null;
            vacancy.NumberOfPositionsComment = null;

            _vacancyPostingService.UpdateVacancy(vacancy);
        }

        public async Task<VacancyViewModel> GetVacancy(int vacancyReferenceNumber)
        {
            Vacancy vacancy;

            if (_configurationService.Get<CommonWebConfiguration>().Features.RaaApiEnabled)
            {
                var apiClient = _apiClientProvider.GetApiClient();

                try
                {
                    var apiVacancyResult = await apiClient.GetVacancyWithHttpMessagesAsync(vacancyReferenceNumber: vacancyReferenceNumber);
                    var apiVacancy = apiVacancyResult.Body;
                    vacancy = ApiClientMappers.Map<ApiVacancy, Vacancy>(apiVacancy);
                }
                catch (HttpOperationException ex)
                {
                    _logService.Info(ex.ToString());
                    return null;
                }
            }
            else
            {
                vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);
            }

            if (vacancy == null)
                return null;

            var viewModel = GetVacancyViewModelFrom(vacancy);
            return viewModel;
        }

        public VacancyViewModel GetVacancyById(int vacancyId)
        {
            var vacancy = _vacancyPostingService.GetVacancy(vacancyId);

            if (vacancy == null)
                return null;

            var viewModel = GetVacancyViewModelFrom(vacancy);


            return viewModel;
        }

        public VacancyViewModel GetVacancy(Guid vacancyGuid)
        {
            var vacancy = _vacancyPostingService.GetVacancy(vacancyGuid);
            if (vacancy != null)
            {
                var viewModel = GetVacancyViewModelFrom(vacancy);
                return viewModel;
            }

            return null;
        }

        private VacancyViewModel GetVacancyViewModelFrom(Vacancy vacancy)
        {
            var viewModel = _mapper.Map<Vacancy, VacancyViewModel>(vacancy);
            var provider = _providerService.GetProvider(vacancy.ContractOwnerId);
            viewModel.Provider = provider.Convert();
            var vacancyOwnerRelationship = _providerService.GetVacancyOwnerRelationship(vacancy.VacancyOwnerRelationshipId, false);  // Some current vacancies have non-current vacancy parties
            if (vacancyOwnerRelationship != null)
            {
                var employer = _employerService.GetEmployer(vacancyOwnerRelationship.EmployerId, false);  //Same with employers
                viewModel.NewVacancyViewModel.VacancyOwnerRelationship = vacancyOwnerRelationship.Convert(employer, vacancy);
                var providerSite = _providerService.GetProviderSite(vacancyOwnerRelationship.ProviderSiteId);
                viewModel.ProviderSite = providerSite.Convert();

                viewModel.Contact = vacancy.GetContactInformation(providerSite);
            }

            viewModel.FrameworkName = string.IsNullOrEmpty(viewModel.TrainingDetailsViewModel.FrameworkCodeName)
                ? viewModel.TrainingDetailsViewModel.FrameworkCodeName
                : _referenceDataService.GetSubCategoryByCode(viewModel.TrainingDetailsViewModel.FrameworkCodeName).FullName;

            viewModel.SectorName = string.IsNullOrEmpty(viewModel.TrainingDetailsViewModel.SectorCodeName)
                ? viewModel.TrainingDetailsViewModel.SectorCodeName
                : _referenceDataService.GetCategoryByCode(viewModel.TrainingDetailsViewModel.SectorCodeName).FullName;

            var standard = GetStandard(vacancy.StandardId);

            viewModel.StandardName = standard == null ? "" : standard.Name;
            if (viewModel.Status.CanHaveApplicationsOrClickThroughs() && viewModel.NewVacancyViewModel.OfflineVacancy == false)
            {
                //TODO: This information will be returned from _apprenticeshipVacancyReadRepository.GetForProvider or similar once FAA has been migrated
                if (viewModel.VacancyType == VacancyType.Apprenticeship)
                {
                    viewModel.ApplicationCount = _apprenticeshipApplicationService.GetApplicationCount(vacancy.VacancyId);
                    viewModel.ApplicationPendingDecisionCount =
                        _apprenticeshipApplicationService
                            .GetSubmittedApplicationSummaries(
                                vacancy.VacancyId).Count(v => v.Status == ApplicationStatuses.InProgress ||
                                    v.Status == ApplicationStatuses.Submitted);
                }
                else if (viewModel.VacancyType == VacancyType.Traineeship)
                {
                    viewModel.ApplicationCount = _traineeshipApplicationService.GetApplicationCount(vacancy.VacancyId);
                    viewModel.ApplicationPendingDecisionCount =
                        _traineeshipApplicationService
                            .GetSubmittedApplicationSummaries(
                                vacancy.VacancyId).Count(v => v.Status == ApplicationStatuses.InProgress ||
                                    v.Status == ApplicationStatuses.Submitted);
                }
            }
            var vacancyManager = _userProfileService.GetProviderUser(vacancy.CreatedByProviderUsername);
            viewModel.ContactDetailsAndVacancyHistory = ContactDetailsAndVacancyHistoryViewModelConverter.Convert(provider,
                vacancyManager, vacancy);
            var vacancyLocationAddressViewModels = GetLocationsAddressViewModel(vacancy);
            viewModel.LocationAddresses = vacancyLocationAddressViewModels;
            viewModel.NewVacancyViewModel.LocationAddresses = vacancyLocationAddressViewModels;
            if (vacancy.Address != null && (vacancy.Address.GeoPoint == null || vacancy.Address.GeoPoint.IsSet()))
            {
                viewModel.Address = GeoCodeAddressForVacancy(vacancy, viewModel.Address);
            }
            return viewModel;
        }

        public VacancyViewModel SubmitVacancy(int vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);

            vacancy.Status = VacancyStatus.Submitted;
            vacancy.DateSubmitted = _dateTimeService.UtcNow;
            if (!vacancy.DateFirstSubmitted.HasValue)
            {
                vacancy.DateFirstSubmitted = vacancy.DateSubmitted;
            }
            vacancy.SubmissionCount++;

            vacancy = _vacancyPostingService.UpdateVacancy(vacancy);

            //TODO: should we return this VM or the one returned by GetVacancyByReferenceNumber?
            var viewModel = _mapper.Map<Vacancy, VacancyViewModel>(vacancy);

            return viewModel;
        }

        public List<SelectListItem> GetSectorsAndFrameworks()
        {
            var categories = _referenceDataService.GetFrameworks();

            var sectorsAndFrameworkItems = new List<SelectListItem>
            {
                new SelectListItem { Value = string.Empty, Text = "Choose from the list of frameworks"}
            };

            var blacklistedCategoryCodes = GetBlacklistedCategoryCodeNames(_configurationService);

            foreach (var sector in categories.Where(category => !blacklistedCategoryCodes.Contains(category.CodeName) && category.Status == CategoryStatus.Active))
            {
                if (sector.SubCategories != null)
                {
                    var sectorGroup = new SelectListGroup { Name = sector.FullName };
                    foreach (var framework in sector.SubCategories.Where(subCategory => subCategory.Status == CategoryStatus.Active))
                    {
                        sectorsAndFrameworkItems.Add(new SelectListItem
                        {
                            Group = sectorGroup,
                            Value = framework.CodeName,
                            Text = framework.FullName
                        });
                    }
                }
            }

            return sectorsAndFrameworkItems;
        }

        public List<StandardViewModel> GetStandards()
        {
            var sectors = _referenceDataService.GetSectors();

            return (from sector in sectors
                    from standard in sector.Standards
                    select standard.Convert(sector)).ToList();
        }

        public List<SelectListItem> GetSectors()
        {
            var categories = _referenceDataService.GetCategories();

            var sectorItems = new List<SelectListItem>
            {
                new SelectListItem { Value = string.Empty, Text = "Choose from the list of sectors"}
            };

            var blacklistedCategoryCodes = GetBlacklistedCategoryCodeNames(_configurationService);

            foreach (var sector in categories.Where(category => !blacklistedCategoryCodes.Contains(category.CodeName)))
            {
                sectorItems.Add(new SelectListItem
                {
                    Value = sector.CodeName,
                    Text = sector.FullName
                });
            }

            return sectorItems;
        }

        public StandardViewModel GetStandard(int? standardId)
        {
            if (!standardId.HasValue) return null;

            var sectors = _referenceDataService.GetSectors().ToList();
            var standard = sectors.SelectMany(s => s.Standards).First(s => s.Id == standardId.Value);
            var sector = sectors.First(s => s.Id == standard.ApprenticeshipSectorId);
            return standard.Convert(sector);
        }

        public VacanciesSummaryViewModel GetVacanciesSummaryForProvider(int providerId, int providerSiteId,
            VacanciesSummarySearchViewModel vacanciesSummarySearch)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            _logService.Debug("Starting GetVacanciesSummaryForProvider");

            var orderByField = string.IsNullOrEmpty(vacanciesSummarySearch.OrderByField)
                ? VacancySummaryOrderByColumn.OrderByFilter
                : (VacancySummaryOrderByColumn)
                Enum.Parse(typeof(VacancySummaryOrderByColumn), vacanciesSummarySearch.OrderByField);

            // Support searching by full vacancy reference number
            string searchString;
            if (!VacancyHelper.TryGetVacancyReference(vacanciesSummarySearch.SearchString, out searchString))
            {
                searchString = vacanciesSummarySearch.SearchString;
            }

            var query = new VacancySummaryQuery
            {
                ProviderId = providerId,
                ProviderSiteId = providerSiteId,
                OrderByField = orderByField,
                Filter = vacanciesSummarySearch.FilterType,
                PageSize = vacanciesSummarySearch.PageSize,
                RequestedPage = vacanciesSummarySearch.CurrentPage,
                SearchMode = vacanciesSummarySearch.SearchMode,
                SearchString = searchString,
                Order = vacanciesSummarySearch.Order,
                VacancyType = vacanciesSummarySearch.VacancyType
            };

            _logService.Debug("Calling Vacancy Summary Service: " + stopwatch.Elapsed);

            int totalRecords;
            var summaries = _vacancySummaryService.GetSummariesForProvider(query, out totalRecords);

            _logService.Debug("Mapping vacancy summaries: " + stopwatch.Elapsed);

            var mapped = _mapper.Map<IList<VacancySummary>, IList<VacancySummaryViewModel>>(summaries);

            _logService.Debug("Constructing view models: " + stopwatch.Elapsed);

            var vacancyPage = new PageableViewModel<VacancySummaryViewModel>
            {
                Page = mapped,
                ResultsCount = totalRecords,
                CurrentPage = vacanciesSummarySearch.CurrentPage,
                TotalNumberOfPages = totalRecords == 0 ? 1 : (int)Math.Ceiling((double)totalRecords / (double)vacanciesSummarySearch.PageSize)
            };

            var viewModel = new VacanciesSummaryViewModel
            {
                Vacancies = vacancyPage,
                VacanciesSummarySearch = vacanciesSummarySearch,
            };

            _logService.Debug("Fetching vacancy counts: " + stopwatch.Elapsed);

            var counts = _vacancySummaryService.GetLotteryCounts(query);

            viewModel.LiveCount = counts.LiveCount;
            viewModel.ClosedCount = counts.ClosedCount;
            viewModel.ClosingSoonCount = counts.ClosingSoonCount;
            viewModel.CompletedCount = counts.CompletedCount;
            viewModel.DraftCount = counts.DraftCount;
            viewModel.RejectedCount = counts.RejectedCount;
            viewModel.SubmittedCount = counts.SubmittedCount;
            viewModel.NewApplicationsAcrossAllVacanciesCount = counts.NewApplicationsCount;

            viewModel.HasVacancies = (viewModel.LiveCount +
                                      viewModel.ClosedCount +
                                      viewModel.ClosingSoonCount +
                                      viewModel.CompletedCount +
                                      viewModel.DraftCount +
                                      viewModel.RejectedCount +
                                      viewModel.SubmittedCount +
                                      viewModel.NewApplicationsAcrossAllVacanciesCount) > 0;

            _logService.Debug("Finished getting vacancy summaries: " + stopwatch.Elapsed);
            stopwatch.Stop();

            return viewModel;
        }

        public VacancyOwnerRelationshipViewModel CloneVacancy(int vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);

            //TODO: control vacancy doesn't exist

            vacancy.VacancyReferenceNumber = _vacancyPostingService.GetNextVacancyReferenceNumber();
            vacancy.Title = $"(Copy of) {vacancy.Title}";
            vacancy.Status = VacancyStatus.Draft;
            vacancy.CreatedDateTime = _dateTimeService.UtcNow;
            vacancy.UpdatedDateTime = null;
            vacancy.DateSubmitted = null;
            vacancy.DateFirstSubmitted = null;
            vacancy.DateStartedToQA = null;
            vacancy.DateQAApproved = null;
            vacancy.ClosingDate = null;
            vacancy.PossibleStartDate = null;
            vacancy.SubmissionCount = 0;
            if (vacancy.IsAnonymousEmployer != null && vacancy.IsAnonymousEmployer.Value)
            {
                vacancy.EmployerAnonymousName = vacancy.EmployerAnonymousName;
                vacancy.EmployerAnonymousReason = vacancy.EmployerAnonymousReason;
            }
            else
            {
                vacancy.EmployerAnonymousName = vacancy.EmployerAnonymousReason = null;
            }
            vacancy.OfflineVacancyType = null;

            //Comments
            vacancy.TitleComment = null;
            vacancy.ShortDescriptionComment = null;
            vacancy.DesiredSkillsComment = null;
            vacancy.FutureProspectsComment = null;
            vacancy.PersonalQualitiesComment = null;
            vacancy.ThingsToConsiderComment = null;
            vacancy.DesiredQualificationsComment = null;
            vacancy.OfflineApplicationUrlComment = null;
            vacancy.OfflineApplicationInstructionsComment = null;
            vacancy.NoOfOfflineApplicants = 0;
            vacancy.ApprenticeshipLevelComment = null;
            vacancy.FrameworkCodeNameComment = null;
            vacancy.StandardIdComment = null;
            vacancy.SectorCodeNameComment = null;
            vacancy.WageComment = null;
            vacancy.ClosingDateComment = null;
            vacancy.DurationComment = null;
            vacancy.LongDescriptionComment = null;
            vacancy.PossibleStartDateComment = null;
            vacancy.WorkingWeekComment = null;
            vacancy.FirstQuestionComment = null;
            vacancy.SecondQuestionComment = null;
            vacancy.TrainingProvidedComment = null;
            vacancy.ContactDetailsComment = null;
            vacancy.NumberOfPositionsComment = null;

            vacancy.VacancyId = 0;
            vacancy.VacancyGuid = Guid.NewGuid();

            _vacancyPostingService.CreateVacancy(vacancy);

            var vacancyOwnerRelationship = _providerService.GetVacancyOwnerRelationship(vacancy.VacancyOwnerRelationshipId, true);
            if (vacancyOwnerRelationship == null)
                throw new Exception($"Vacancy Party {vacancy.VacancyOwnerRelationshipId} not found / no longer current");

            var employer = _employerService.GetEmployer(vacancyOwnerRelationship.EmployerId, true);
            var result = vacancyOwnerRelationship.Convert(employer, vacancy);
            result.VacancyGuid = vacancy.VacancyGuid;

            return result;
        }

        public DashboardVacancySummariesViewModel GetPendingQAVacanciesOverview(DashboardVacancySummariesSearchViewModel searchViewModel)
        {
            var agencyUser = _userProfileService.GetAgencyUser(_currentUserService.CurrentUserName);
            var regionalTeam = agencyUser.RegionalTeam;

            var orderByField = string.IsNullOrEmpty(searchViewModel.OrderByField)
                ? VacancySummaryOrderByColumn.OrderByFilter
                : (VacancySummaryOrderByColumn)
                Enum.Parse(typeof(VacancySummaryOrderByColumn), searchViewModel.OrderByField);

            var query = new VacancySummaryByStatusQuery()
            {
                PageSize = 0,
                RequestedPage = 1,
                Filter = searchViewModel.FilterType,
                OrderByField = orderByField,
                SearchString = searchViewModel.SearchString,
                SearchMode = searchViewModel.SearchMode,
                DesiredStatuses = new[] { VacancyStatus.Submitted, VacancyStatus.ReservedForQA },
                Order = searchViewModel.Order,
                RegionalTeamName = regionalTeam
            };

            int totalRecords;
            var vacancies = _vacancyPostingService.GetWithStatus(query, out totalRecords);

            var regionalTeamsMetrics = _vacancyPostingService.GetRegionalTeamsMetrics(query);

            if (string.IsNullOrEmpty(searchViewModel.SearchString) && regionalTeamsMetrics.Sum(s => s.TotalCount) == 0)
            {
                //No vacancies for current team selection. Redirect to metrics
                searchViewModel.Mode = DashboardVacancySummariesMode.Metrics;
            }

            RegionalTeamMetrics currentTeamMetrics = null;

            if (string.IsNullOrEmpty(searchViewModel.SearchString))
            {
                currentTeamMetrics = regionalTeamsMetrics.SingleOrDefault(s => s.RegionalTeam == regionalTeam);
            }
            else
            {
                currentTeamMetrics = new RegionalTeamMetrics()
                {
                    RegionalTeam = RegionalTeam.Other,
                    TotalCount = regionalTeamsMetrics.Sum(s => s.TotalCount),
                    ResubmittedCount = regionalTeamsMetrics.Sum(s => s.ResubmittedCount),
                    SubmittedYesterdayCount = regionalTeamsMetrics.Sum(s => s.SubmittedYesterdayCount),
                    SubmittedMoreThan48HoursCount = regionalTeamsMetrics.Sum(s => s.SubmittedMoreThan48HoursCount),
                    SubmittedTodayCount = regionalTeamsMetrics.Sum(s => s.SubmittedTodayCount)
                };
            }

            var viewModel = new DashboardVacancySummariesViewModel
            {
                SearchViewModel = searchViewModel,
                SubmittedTodayCount = currentTeamMetrics?.SubmittedTodayCount ?? 0,
                SubmittedYesterdayCount = currentTeamMetrics?.SubmittedYesterdayCount ?? 0,
                SubmittedMoreThan48HoursCount = currentTeamMetrics?.SubmittedMoreThan48HoursCount ?? 0,
                ResubmittedCount = currentTeamMetrics?.ResubmittedCount ?? 0,
                Vacancies = vacancies.Select(vacancy => ConvertToDashboardVacancySummaryViewModel(vacancy)).ToList(),
                RegionalTeamsMetrics = regionalTeamsMetrics
            };

            return viewModel;
        }

        private List<VacancySummary> GetTeamVacancySummaries()
        {
            var regionalTeam = GetRegionalTeamForCurrentUser();

            var query = new VacancySummaryByStatusQuery()
            {
                DesiredStatuses = new[] { VacancyStatus.Submitted, VacancyStatus.ReservedForQA },
                PageSize = 0,
                RequestedPage = 0,
                RegionalTeamName = regionalTeam,
                OrderByField = VacancySummaryOrderByColumn.DateSubmitted
            };

            int totalRecords;

            var vacancies = _vacancyPostingService.GetWithStatus(query, out totalRecords);

            return vacancies.ToList();
        }

        public DashboardVacancySummaryViewModel GetNextAvailableVacancy()
        {
            var vacancies = GetTeamVacancySummaries();

            var nextVacancy = _vacancyLockingService.GetNextAvailableVacancy(_currentUserService.CurrentUserName,
                vacancies);

            return nextVacancy != null ? ConvertToDashboardVacancySummaryViewModel(nextVacancy, _providerService.GetProvider(nextVacancy.ContractOwnerId)) : null;
        }

        public void UnReserveVacancyForQA(int vacancyReferenceNumber)
        {
            var vacancyToUnReserve = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);

            if (_vacancyLockingService.IsVacancyAvailableToQABy(_currentUserService.CurrentUserName, vacancyToUnReserve))
            {

                _vacancyPostingService.UnReserveVacancyForQa(vacancyReferenceNumber);
            }
        }

        public VacancyViewModel ReserveVacancyForQA(int vacancyReferenceNumber)
        {
            var vacancyToReserve = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);

            if (_vacancyLockingService.IsVacancyAvailableToQABy(_currentUserService.CurrentUserName, vacancyToReserve))
            {
                var vacancy = _vacancyPostingService.ReserveVacancyForQA(vacancyReferenceNumber);

                return GetVacancyViewModelFrom(vacancy);
            }

            var vacancies = GetTeamVacancySummaries();

            var nextAvailableVacancySummary =
                _vacancyLockingService.GetNextAvailableVacancy(_currentUserService.CurrentUserName, vacancies);

            if (nextAvailableVacancySummary == null) return default(VacancyViewModel);

            var nextAvailableVacancy =
                _vacancyPostingService.ReserveVacancyForQA(nextAvailableVacancySummary.VacancyReferenceNumber);

            return GetVacancyViewModelFrom(nextAvailableVacancy);
        }

        private static string[] GetBlacklistedCategoryCodeNames(IConfigurationService configurationService)
        {
            var blacklistedCategoryCodeNames = configurationService.Get<CommonWebConfiguration>().BlacklistedCategoryCodes;

            if (string.IsNullOrWhiteSpace(blacklistedCategoryCodeNames))
            {
                return new string[] { };
            }

            return blacklistedCategoryCodeNames
                .Split(',')
                .Select(each => each.Trim())
                .ToArray();
        }

        private DashboardVacancySummaryViewModel ConvertToDashboardVacancySummaryViewModel(VacancySummary vacancy, Provider provider = null)
        {
            var userName = _currentUserService.CurrentUserName;

            return new DashboardVacancySummaryViewModel
            {
                ClosingDate = vacancy.ClosingDate,
                DateSubmitted = vacancy.DateSubmitted,
                DateFirstSubmitted = vacancy.DateFirstSubmitted,
                ProviderName = provider?.TradingName ?? vacancy.ProviderTradingName,
                Status = vacancy.Status,
                Title = vacancy.Title,
                VacancyReferenceNumber = vacancy.VacancyReferenceNumber,
                DateStartedToQA = vacancy.DateStartedToQA,
                QAUserName = vacancy.QAUserName,
                CanBeReservedForQaByCurrentUser = _vacancyLockingService.IsVacancyAvailableToQABy(userName, vacancy),
                SubmissionCount = vacancy.SubmissionCount,
                VacancyType = vacancy.VacancyType,
                Location = _mapper.Map<PostalAddress, AddressViewModel>(vacancy.Address),
                IsAnonymousEmployer = vacancy.IsAnonymousEmployer,
                IsMultiLocation = vacancy.IsMultiLocation
            };
        }

        public List<DashboardVacancySummaryViewModel> GetPendingQAVacancies()
        {
            return GetPendingQAVacanciesOverview(new DashboardVacancySummariesSearchViewModel()).Vacancies.Where(vm => vm.CanBeReservedForQaByCurrentUser).ToList();
        }

        private Vacancy CreateChildVacancy(Vacancy vacancy, VacancyLocation address, DateTime approvalTime)
        {
            var newVacancy = (Vacancy)vacancy.Clone();
            newVacancy.VacancyReferenceNumber = _vacancyPostingService.GetNextVacancyReferenceNumber();
            newVacancy.Status = VacancyStatus.Live;
            newVacancy.VacancyGuid = Guid.NewGuid();
            newVacancy.Address = address.Address;
            newVacancy.DateQAApproved = approvalTime;
            newVacancy.ParentVacancyId = vacancy.VacancyId;
            newVacancy.NumberOfPositions = address.NumberOfPositions;
            newVacancy.VacancyLocationType = VacancyLocationType.SpecificLocation;
            if (!string.IsNullOrWhiteSpace(vacancy.EmployerAnonymousName))
            {
                newVacancy.EmployerAnonymousReason = vacancy.EmployerAnonymousReason;
                newVacancy.EmployerAnonymousName = vacancy.EmployerAnonymousName;
            }
            if (newVacancy.OfflineVacancyType == OfflineVacancyType.MultiUrl)
            {
                newVacancy.OfflineApplicationUrl = address.EmployersWebsite;
            }

            return _vacancyPostingService.CreateVacancy(newVacancy);
        }

        public QAActionResultCode ApproveVacancy(int vacancyReferenceNumber)
        {
            var qaApprovalDate = _dateTimeService.UtcNow;
            var submittedVacancy = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);

            if (!_vacancyLockingService.IsVacancyAvailableToQABy(_currentUserService.CurrentUserName, submittedVacancy))
            {
                return QAActionResultCode.InvalidVacancy;
            }

            try
            {
                if (submittedVacancy.Address != null && !submittedVacancy.Address.GeoPoint.IsValid())
                {
                    submittedVacancy.Address.GeoPoint = _geoCodingService.GetGeoPointFor(submittedVacancy.Address);
                }

                if (submittedVacancy.VacancyLocationType == VacancyLocationType.MultipleLocations)
                {
                    var vacancyLocationAddresses = _vacancyPostingService.GetVacancyLocations(submittedVacancy.VacancyId);

                    if (vacancyLocationAddresses != null && vacancyLocationAddresses.Any())
                    {
                        GeoCodeVacancyLocations(vacancyLocationAddresses);

                        var vacancyLocation = vacancyLocationAddresses.First();
                        submittedVacancy.Address = vacancyLocation.Address;
                        submittedVacancy.ParentVacancyId = submittedVacancy.VacancyId;
                        submittedVacancy.NumberOfPositions = vacancyLocation.NumberOfPositions;
                        submittedVacancy.VacancyLocationType = VacancyLocationType.SpecificLocation;

                        if (submittedVacancy.OfflineVacancyType == OfflineVacancyType.MultiUrl)
                        {
                            submittedVacancy.OfflineApplicationUrl = vacancyLocation.EmployersWebsite;
                        }

                        foreach (var locationAddress in vacancyLocationAddresses.Skip(1))
                        {
                            CreateChildVacancy(submittedVacancy, locationAddress, qaApprovalDate);
                        }

                        submittedVacancy.OfflineVacancyType = null;

                        _vacancyPostingService.DeleteVacancyLocationsFor(submittedVacancy.VacancyId);
                    }
                }
            }
            catch (CustomException ex)
                when (ex.Code == Application.Interfaces.Locations.ErrorCodes.GeoCodeLookupProviderFailed)
            {
                // Catch and return before any new vacancy is created
                return QAActionResultCode.GeocodingFailure;
            }

            submittedVacancy.Status = VacancyStatus.Live;
            submittedVacancy.DateQAApproved = qaApprovalDate;

            _vacancyPostingService.UpdateVacancy(submittedVacancy);

            return QAActionResultCode.Ok;
        }

        public QAActionResultCode RejectVacancy(int vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);

            if (!_vacancyLockingService.IsVacancyAvailableToQABy(_currentUserService.CurrentUserName, vacancy))
            {
                return QAActionResultCode.InvalidVacancy;
            }

            vacancy.Status = VacancyStatus.Referred;
            vacancy.QAUserName = null;

            _vacancyPostingService.UpdateVacancy(vacancy);

            return QAActionResultCode.Ok;
        }

        private RegionalTeam GetRegionalTeamForCurrentUser()
        {
            var agencyUser = _userProfileService.GetAgencyUser(_currentUserService.CurrentUserName);
            var regionalTeam = agencyUser.RegionalTeam;
            return regionalTeam;
        }

        public VacancyViewModel ReviewVacancy(int vacancyReferenceNumber)
        {
            var vacancyToReserve = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);

            if (!_vacancyLockingService.IsVacancyAvailableToQABy(_currentUserService.CurrentUserName, vacancyToReserve))
                return null;

            var vacancy = _vacancyPostingService.ReserveVacancyForQA(vacancyReferenceNumber);

            return GetVacancyViewModelFrom(vacancy);
        }

        public QAActionResult<FurtherVacancyDetailsViewModel> UpdateVacancyWithComments(FurtherVacancyDetailsViewModel viewModel)
        {
            // TODO: merge with vacancypostingprovider? -> how we deal with comments. Add them as hidden fields in vacancy posting journey?
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber);

            if (!_vacancyLockingService.IsVacancyAvailableToQABy(_currentUserService.CurrentUserName, vacancy))
            {
                return new QAActionResult<FurtherVacancyDetailsViewModel>(QAActionResultCode.InvalidVacancy);
            }

            vacancy.WorkingWeek = viewModel.WorkingWeek;
            vacancy.Wage = _mapper.Map<WageViewModel, Wage>(viewModel.Wage);
            vacancy.DurationType = viewModel.DurationType;
            vacancy.Duration = viewModel.Duration.HasValue ? (int?)Math.Round(viewModel.Duration.Value) : null;

            if (viewModel.VacancyDatesViewModel.ClosingDate.HasValue)
            {
                vacancy.ClosingDate = viewModel.VacancyDatesViewModel.ClosingDate?.Date;
            }
            if (viewModel.VacancyDatesViewModel.PossibleStartDate.HasValue)
            {
                vacancy.PossibleStartDate = viewModel.VacancyDatesViewModel.PossibleStartDate?.Date;
            }

            vacancy.LongDescription = viewModel.LongDescription;

            vacancy.WageComment = viewModel.WageComment;
            vacancy.ClosingDateComment = viewModel.VacancyDatesViewModel.ClosingDateComment;
            vacancy.DurationComment = viewModel.DurationComment;
            vacancy.LongDescriptionComment = viewModel.LongDescriptionComment;
            vacancy.PossibleStartDateComment = viewModel.VacancyDatesViewModel.PossibleStartDateComment;
            vacancy.WorkingWeekComment = viewModel.WorkingWeekComment;
            vacancy.ExpectedDuration = viewModel.ExpectedDuration;

            AddQAInformation(vacancy);

            vacancy = _vacancyPostingService.UpdateVacancy(vacancy);

            viewModel = vacancy.ConvertToVacancySummaryViewModel();
            viewModel.Wage = _mapper.Map<Wage, WageViewModel>(vacancy.Wage);

            viewModel.AutoSaveTimeoutInSeconds =
                _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;

            return new QAActionResult<FurtherVacancyDetailsViewModel>(QAActionResultCode.Ok, viewModel);
        }

        public QAActionResult<NewVacancyViewModel> UpdateVacancyWithComments(NewVacancyViewModel viewModel)
        {
            if (!viewModel.VacancyReferenceNumber.HasValue)
                throw new ArgumentNullException("viewModel.VacancyReferenceNumber", "VacancyReferenceNumber required for update");

            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber.Value);

            if (!_vacancyLockingService.IsVacancyAvailableToQABy(_currentUserService.CurrentUserName, vacancy))
            {
                return new QAActionResult<NewVacancyViewModel>(QAActionResultCode.InvalidVacancy);
            }

            var currentOfflineVacancyType = vacancy.OfflineVacancyType;

            //update properties
            vacancy.Title = viewModel.Title;
            vacancy.ShortDescription = viewModel.ShortDescription;
            vacancy.OfflineVacancy = viewModel.OfflineVacancy.Value; // At this point we'll always have a value
            vacancy.OfflineVacancyType = viewModel.OfflineVacancyType;
            if (currentOfflineVacancyType != OfflineVacancyType.MultiUrl)
            {
                var offlineApplicationUrl = !string.IsNullOrEmpty(viewModel.OfflineApplicationUrl)
                    ? new UriBuilder(viewModel.OfflineApplicationUrl).Uri.ToString()
                    : viewModel.OfflineApplicationUrl;
                vacancy.OfflineApplicationUrl = offlineApplicationUrl;
            }
            vacancy.OfflineApplicationInstructions = viewModel.OfflineApplicationInstructions;
            vacancy.OfflineApplicationInstructionsComment = viewModel.OfflineApplicationInstructionsComment;
            vacancy.OfflineApplicationUrlComment = viewModel.OfflineApplicationUrlComment;
            vacancy.ShortDescriptionComment = viewModel.ShortDescriptionComment;
            vacancy.TitleComment = viewModel.TitleComment;
            vacancy.VacancyType = viewModel.VacancyType;
            // TODO: not sure if do this or call reserveForQA in the service
            AddQAInformation(vacancy);

            if (viewModel.LocationAddresses != null)
            {
                var vacancyLocations = _vacancyPostingService.GetVacancyLocations(vacancy.VacancyId);
                if (vacancyLocations != null)
                {
                    foreach (var locationAddress in viewModel.LocationAddresses)
                    {
                        var vacancyLocation = vacancyLocations.SingleOrDefault(vl => vl.VacancyLocationId == locationAddress.VacancyLocationId);
                        if (vacancyLocation != null)
                        {
                            var offlineApplicationUrl = !string.IsNullOrEmpty(locationAddress.OfflineApplicationUrl) ? new UriBuilder(locationAddress.OfflineApplicationUrl).Uri.ToString() : locationAddress.OfflineApplicationUrl;
                            vacancyLocation.EmployersWebsite = offlineApplicationUrl;
                        }
                    }
                }
                _vacancyPostingService.UpdateVacancyLocations(vacancyLocations);
            }

            vacancy = _vacancyPostingService.UpdateVacancy(vacancy);

            viewModel = _mapper.Map<Vacancy, NewVacancyViewModel>(vacancy);
            viewModel.AutoSaveTimeoutInSeconds =
                _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;

            return new QAActionResult<NewVacancyViewModel>(QAActionResultCode.Ok, viewModel);
        }

        private void AddQAInformation(Vacancy vacancy)
        {
            vacancy.QAUserName = _currentUserService.CurrentUserName;
            vacancy.DateStartedToQA = _dateTimeService.UtcNow;
        }

        public QAActionResult<TrainingDetailsViewModel> UpdateVacancyWithComments(TrainingDetailsViewModel viewModel)
        {
            if (!viewModel.VacancyReferenceNumber.HasValue)
                throw new ArgumentNullException("viewModel.VacancyReferenceNumber", "VacancyReferenceNumber required for update");

            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber.Value);

            if (!_vacancyLockingService.IsVacancyAvailableToQABy(_currentUserService.CurrentUserName, vacancy))
            {
                return new QAActionResult<TrainingDetailsViewModel>(QAActionResultCode.InvalidVacancy);
            }

            //update properties
            vacancy.TrainingType = viewModel.TrainingType;
            vacancy.FrameworkCodeName = GetFrameworkCodeName(viewModel);
            vacancy.StandardId = GetStandardId(viewModel);
            vacancy.StandardIdComment = viewModel.StandardIdComment;
            vacancy.ApprenticeshipLevel = GetApprenticeshipLevel(viewModel);
            vacancy.ApprenticeshipLevelComment = viewModel.ApprenticeshipLevelComment;
            vacancy.SectorCodeName = GetSectorCodeName(viewModel);
            vacancy.SectorCodeNameComment = viewModel.SectorCodeNameComment;
            vacancy.FrameworkCodeNameComment = viewModel.FrameworkCodeNameComment;
            vacancy.TrainingProvided = viewModel.TrainingProvided;
            vacancy.TrainingProvidedComment = viewModel.TrainingProvidedComment;
            vacancy.ContactName = viewModel.ContactName;
            vacancy.ContactNumber = viewModel.ContactNumber;
            vacancy.ContactEmail = viewModel.ContactEmail;
            vacancy.ContactDetailsComment = viewModel.ContactDetailsComment;

            vacancy = _vacancyPostingService.UpdateVacancy(vacancy);

            viewModel = _mapper.Map<Vacancy, TrainingDetailsViewModel>(vacancy);
            var sectorsAndFrameworks = GetSectorsAndFrameworks();
            var standards = GetStandards();
            var sectors = GetSectors();
            viewModel.SectorsAndFrameworks = sectorsAndFrameworks;
            viewModel.Standards = standards;
            viewModel.Sectors = sectors;

            // TODO: not sure if do this or call reserveForQA in the service
            AddQAInformation(vacancy);

            viewModel.AutoSaveTimeoutInSeconds =
                _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;

            return new QAActionResult<TrainingDetailsViewModel>(QAActionResultCode.Ok, viewModel);
        }

        public NewVacancyViewModel UpdateEmployerInformationWithComments(NewVacancyViewModel viewModel)
        {
            if (!viewModel.VacancyReferenceNumber.HasValue)
                throw new ArgumentNullException("viewModel.VacancyReferenceNumber", "VacancyReferenceNumber required for update");

            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber.Value);

            var vacancyOwnerRelationship = _providerService.GetVacancyOwnerRelationship(viewModel.VacancyOwnerRelationship.VacancyOwnerRelationshipId, false);
            if (vacancyOwnerRelationship == null)
                throw new Exception($"Vacancy Party {viewModel.VacancyOwnerRelationship.VacancyOwnerRelationshipId} not found / no longer current");

            var employer = _employerService.GetEmployer(vacancyOwnerRelationship.EmployerId, false);

            //update properties
            vacancy.EmployerDescriptionComment = viewModel.EmployerDescriptionComment;
            vacancy.EmployerWebsiteUrlComment = viewModel.EmployerWebsiteUrlComment;
            vacancy.VacancyLocationType =
                viewModel.VacancyLocationType;

            if (viewModel.VacancyOwnerRelationship.IsAnonymousEmployer.HasValue && viewModel.VacancyOwnerRelationship.IsAnonymousEmployer.Value)
            {
                vacancy.EmployerAnonymousReason = viewModel.AnonymousEmployerReason;
                vacancy.AnonymousEmployerReasonComment =
                    viewModel.AnonymousEmployerReasonComment;
                vacancy.EmployerAnonymousName = viewModel.AnonymousEmployerDescription;
                vacancy.AnonymousEmployerDescriptionComment =
                    viewModel.AnonymousEmployerDescriptionComment;
                vacancy.AnonymousAboutTheEmployer = viewModel.AnonymousAboutTheEmployer;
                vacancy.AnonymousAboutTheEmployerComment =
                    viewModel.AnonymousAboutTheEmployerComment;
            }

            if (vacancy.VacancyLocationType == VacancyLocationType.SpecificLocation
                || vacancy.VacancyLocationType == VacancyLocationType.Nationwide)
            {
                vacancy.NumberOfPositions = viewModel.NumberOfPositions;
                vacancy.NumberOfPositionsComment = viewModel.NumberOfPositionsComment;

                if (!employer.Address.GeoPoint.IsValid())
                {
                    employer.Address.GeoPoint = _geoCodingService.GetGeoPointFor(employer.Address);
                }

                vacancy.Address = employer.Address;

                vacancy.LocationAddressesComment = null;
            }
            else
            {
                vacancy.NumberOfPositions = null;
                vacancy.NumberOfPositionsComment = null;
                vacancy.Address = null;
            }

            vacancy = _vacancyPostingService.UpdateVacancy(vacancy);

            viewModel = _mapper.Map<Vacancy, NewVacancyViewModel>(vacancy);
            return viewModel;
        }

        public QAActionResult<VacancyRequirementsProspectsViewModel> UpdateVacancyWithComments(VacancyRequirementsProspectsViewModel viewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber);

            if (!_vacancyLockingService.IsVacancyAvailableToQABy(_currentUserService.CurrentUserName, vacancy))
            {
                return new QAActionResult<VacancyRequirementsProspectsViewModel>(QAActionResultCode.InvalidVacancy);
            }

            vacancy.DesiredSkills = viewModel.DesiredSkills;
            vacancy.DesiredSkillsComment = viewModel.DesiredSkillsComment;
            vacancy.FutureProspects = viewModel.FutureProspects;
            vacancy.FutureProspectsComment = viewModel.FutureProspectsComment;
            vacancy.PersonalQualities = viewModel.PersonalQualities;
            vacancy.PersonalQualitiesComment = viewModel.PersonalQualitiesComment;
            vacancy.ThingsToConsider = viewModel.ThingsToConsider;
            vacancy.ThingsToConsiderComment = viewModel.ThingsToConsiderComment;
            vacancy.DesiredQualifications = viewModel.DesiredQualifications;
            vacancy.DesiredQualificationsComment = viewModel.DesiredQualificationsComment;
            vacancy.OtherInformation = viewModel.OtherInformation;
            vacancy.OtherInformationComment = viewModel.OtherInformationComment;

            AddQAInformation(vacancy);

            vacancy = _vacancyPostingService.UpdateVacancy(vacancy);

            viewModel = vacancy.ConvertToVacancyRequirementsProspectsViewModel();
            viewModel.AutoSaveTimeoutInSeconds =
                _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;

            return new QAActionResult<VacancyRequirementsProspectsViewModel>(QAActionResultCode.Ok, viewModel);
        }

        public QAActionResult<VacancyQuestionsViewModel> UpdateVacancyWithComments(VacancyQuestionsViewModel viewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber);

            if (!_vacancyLockingService.IsVacancyAvailableToQABy(_currentUserService.CurrentUserName, vacancy))
            {
                return new QAActionResult<VacancyQuestionsViewModel>(QAActionResultCode.InvalidVacancy);
            }

            vacancy.FirstQuestion = viewModel.FirstQuestion;
            vacancy.SecondQuestion = viewModel.SecondQuestion;
            vacancy.FirstQuestionComment = viewModel.FirstQuestionComment;
            vacancy.SecondQuestionComment = viewModel.SecondQuestionComment;

            // TODO: not sure if do this or call reserveForQA in the service
            AddQAInformation(vacancy);

            vacancy = _vacancyPostingService.UpdateVacancy(vacancy);

            viewModel = vacancy.ConvertToVacancyQuestionsViewModel();
            viewModel.AutoSaveTimeoutInSeconds =
                _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;

            return new QAActionResult<VacancyQuestionsViewModel>(QAActionResultCode.Ok, viewModel);
        }

        public LocationSearchViewModel AddLocations(LocationSearchViewModel viewModel)
        {
            var vacancyLocations = viewModel.Addresses.Select(_mapper.Map<VacancyLocationAddressViewModel, VacancyLocation>).ToList();

            var vacancyOwnerRelationship =
                _providerService.GetVacancyOwnerRelationship(viewModel.ProviderSiteId, viewModel.EmployerEdsUrn, true);
            viewModel.VacancyOwnerRelationshipId = vacancyOwnerRelationship.VacancyOwnerRelationshipId;

            var employer = _employerService.GetEmployer(vacancyOwnerRelationship.EmployerId, true);

            employer.Address.GeoPoint = _geoCodingService.GetGeoPointFor(employer.Address);

            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber);

            vacancy.VacancyLocationType =
                viewModel.EmployerApprenticeshipLocation;
            vacancy.NumberOfPositions = null;
            vacancy.Address = employer.Address;
            vacancy.LocationAddressesComment = viewModel.LocationAddressesComment;
            vacancy.AdditionalLocationInformation = viewModel.AdditionalLocationInformation;
            vacancy.AdditionalLocationInformationComment = viewModel.AdditionalLocationInformationComment;

            GeoCodeVacancyLocations(viewModel);

            var existingVacancyLocations = _vacancyPostingService.GetVacancyLocations(vacancy.VacancyId);
            if (existingVacancyLocations != null && existingVacancyLocations.Count > 0)
            {
                foreach (var vacancyLocation in vacancyLocations.Where(a => a.VacancyLocationId != 0))
                {
                    var existingVacancyLocation = existingVacancyLocations.SingleOrDefault(l => l.VacancyLocationId == vacancyLocation.VacancyLocationId);
                    if (existingVacancyLocation != null)
                    {
                        vacancyLocation.EmployersWebsite = existingVacancyLocation.EmployersWebsite;
                    }
                }
            }

            if (vacancyLocations.Count == 1)
            {
                //Set address
                vacancy.Address = vacancyLocations.Single().Address;
                vacancy.NumberOfPositions = vacancyLocations.Single().NumberOfPositions;
                vacancy.LocalAuthorityCode =
                    _localAuthorityLookupService.GetLocalAuthorityCode(vacancy.Address.Postcode);
                _vacancyPostingService.DeleteVacancyLocationsFor(vacancy.VacancyId);
                _vacancyPostingService.UpdateVacancy(vacancy);

            }
            else
            {
                _vacancyPostingService.UpdateVacancy(vacancy);
                foreach (var vacancyLocation in vacancyLocations)
                {
                    vacancyLocation.VacancyId = vacancy.VacancyId;
                    vacancyLocation.LocalAuthorityCode =
                    _localAuthorityLookupService.GetLocalAuthorityCode(vacancyLocation.Address.Postcode);
                }
                _vacancyPostingService.DeleteVacancyLocationsFor(vacancy.VacancyId);
                _vacancyPostingService.CreateVacancyLocations(vacancyLocations);
            }

            viewModel.AutoSaveTimeoutInSeconds =
                _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;

            return viewModel;
        }

        public void RemoveVacancyLocationInformation(Guid vacancyGuid)
        {
            var vacancy = _vacancyPostingService.GetVacancy(vacancyGuid);
            if (vacancy != null)
            {
                vacancy.VacancyLocationType = VacancyLocationType.Unknown;
                vacancy.NumberOfPositions = null;
                vacancy.LocationAddressesComment = null;
                vacancy.AdditionalLocationInformation = null;
                vacancy.AdditionalLocationInformationComment = null;
                vacancy.VacancyLocationType = VacancyLocationType.Unknown;
                _vacancyPostingService.UpdateVacancy(vacancy);

                _vacancyPostingService.DeleteVacancyLocationsFor(vacancy.VacancyId);
            }
        }

        public void RemoveLocationAddresses(Guid vacancyGuid)
        {
            var vacancy = _vacancyPostingService.GetVacancy(vacancyGuid);
            if (vacancy != null)
            {
                vacancy.AdditionalLocationInformation = null;
                _vacancyPostingService.UpdateVacancy(vacancy);

                _vacancyPostingService.DeleteVacancyLocationsFor(vacancy.VacancyId);
            }
        }

        private VacancyApplicationsState GetVacancyApplicationsState(Vacancy vacancy)
        {
            return _apprenticeshipApplicationService.GetApplicationCount(vacancy.VacancyId) > 0 ? VacancyApplicationsState.HasApplications : VacancyApplicationsState.NoApplications;
        }
    }

    public static class Extensions
    {
        public static V GetValueOrDefault<K, V>(this IReadOnlyDictionary<K, V> dict, K key)
        {
            return GetValueOrDefault(dict, key, _ => default(V));
        }

        public static V GetValueOrDefault<K, V>(this IReadOnlyDictionary<K, V> dict, K key, Func<K, V> getDefault)
        {
            V result;
            if (dict.TryGetValue(key, out result))
                return result;
            else
                return getDefault(key);
        }

        public static IEnumerable<V> GetValueOrEmpty<K, V>(this IReadOnlyDictionary<K, IEnumerable<V>> dict, K key)
        {
            IEnumerable<V> result;
            if (dict.TryGetValue(key, out result))
                return result;
            else
                return Enumerable.Empty<V>();
        }

        public static V GetValue<K, V>(this IReadOnlyDictionary<K, V> dict, K key)
        {
            V result;
            if (dict.TryGetValue(key, out result))
                return result;
            else
                throw new KeyNotFoundException($"{key} in ({string.Join(",", dict.Keys.Take(10))})");
        }

        public static bool EqualsAnyOf<T>(this T value, params T[] values)
        {
            foreach (var v in values)
            {
                if ((v == null && value == null) || value.Equals(v))
                    return true;
            }

            return false;
        }

        public static IEnumerable<T> GetCurrentPage<T>(this IEnumerable<T> enumerable, PageableSearchViewModel pagedSearchCriteria)
        {
            return enumerable.Skip((pagedSearchCriteria.CurrentPage - 1) * pagedSearchCriteria.PageSize).Take(pagedSearchCriteria.PageSize);
        }

        public static int TotalPages<T>(this IEnumerable<T> enumerable, PageableSearchViewModel pagedSearchCriteria)
        {
            // TODO: This looks overly complicated
            return enumerable.Any() ? (int)Math.Ceiling((double)enumerable.Count() / pagedSearchCriteria.PageSize) : 1;
        }

    }
}
