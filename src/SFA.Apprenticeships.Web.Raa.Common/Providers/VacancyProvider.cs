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
    using Domain.Entities.Exceptions;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Reference;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Vacancies;
    using Domain.Raa.Interfaces.Repositories.Models;
    using Infrastructure.Presentation;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
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
    using Order = Domain.Raa.Interfaces.Repositories.Models.Order;
    using TrainingType = Domain.Entities.Raa.Vacancies.TrainingType;
    using VacancySummary = Domain.Entities.Raa.Vacancies.VacancySummary;
    using VacancyType = Domain.Entities.Raa.Vacancies.VacancyType;

    public class VacancyProvider : IVacancyPostingProvider, IVacancyQAProvider
    {
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

        public VacancyProvider(ILogService logService, IConfigurationService configurationService,
            IVacancyPostingService vacancyPostingService, IReferenceDataService referenceDataService,
            IProviderService providerService, IEmployerService employerService, IDateTimeService dateTimeService,
            IMapper mapper, IApprenticeshipApplicationService apprenticeshipApplicationService,
            ITraineeshipApplicationService traineeshipApplicationService, IVacancyLockingService vacancyLockingService,
            ICurrentUserService currentUserService, IUserProfileService userProfileService,
            IGeoCodeLookupService geocodingService, ILocalAuthorityLookupService localAuthLookupService,
            IVacancySummaryService vacancySummaryService)
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
        }

        public NewVacancyViewModel GetNewVacancyViewModel(int vacancyPartyId, Guid vacancyGuid, int? numberOfPositions)
        {
            var existingVacancy = _vacancyPostingService.GetVacancy(vacancyGuid);

            var vacancyParty = _providerService.GetVacancyParty(vacancyPartyId, true);
            var employer = _employerService.GetEmployer(vacancyParty.EmployerId, true);
            var vacancyPartyViewModel = vacancyParty.Convert(employer);

            if (existingVacancy != null)
            {
                var vacancyViewModel = _mapper.Map<Vacancy, NewVacancyViewModel>(existingVacancy);
                vacancyViewModel.OwnerParty = vacancyPartyViewModel;
                vacancyViewModel.AutoSaveTimeoutInSeconds =
                    _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;
                vacancyViewModel.LocationAddresses = GetLocationsAddressViewModel(existingVacancy);
                return vacancyViewModel;
            }

            return new NewVacancyViewModel
            {
                OwnerParty = vacancyPartyViewModel,
                IsEmployerLocationMainApprenticeshipLocation = numberOfPositions.HasValue,
                NumberOfPositions = numberOfPositions,
                AutoSaveTimeoutInSeconds = _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds
            };
        }

        private List<VacancyLocationAddressViewModel> GetLocationsAddressViewModel(Vacancy vacancy)
        {
            var locationAddressesVm = new List<VacancyLocationAddressViewModel>();
            var locationAddresses = _vacancyPostingService.GetVacancyLocations(vacancy.VacancyId);
            if (locationAddresses != null && locationAddresses.Any())
            {
                return
                    _mapper.Map<List<VacancyLocation>, List<VacancyLocationAddressViewModel>>(locationAddresses);
            }
            if (vacancy.IsEmployerLocationMainApprenticeshipLocation.HasValue &&
                vacancy.IsEmployerLocationMainApprenticeshipLocation.Value == false &&
                vacancy.Address != null)
            {

                locationAddressesVm.Add(
                    new VacancyLocationAddressViewModel
                    {
                        Address = _mapper.Map<PostalAddress, AddressViewModel>(vacancy.Address),
                        NumberOfPositions = vacancy.NumberOfPositions
                    });
            }
            return locationAddressesVm;
        }

        public NewVacancyViewModel GetNewVacancyViewModel(int vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);
            var vacancyParty = _providerService.GetVacancyParty(vacancy.OwnerPartyId, false);
            var viewModel = _mapper.Map<Vacancy, NewVacancyViewModel>(vacancy);
            var employer = _employerService.GetEmployer(vacancyParty.EmployerId, false);
            viewModel.LocationAddresses = GetLocationsAddressViewModel(vacancy);
            viewModel.OwnerParty = vacancyParty.Convert(employer);
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

        public LocationSearchViewModel LocationAddressesViewModel(string ukprn, int providerSiteId, int employerId, Guid vacancyGuid)
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
                    IsEmployerLocationMainApprenticeshipLocation = false,
                    Addresses = new List<VacancyLocationAddressViewModel>(),
                    LocationAddressesComment = vacancy.LocationAddressesComment,
                    AdditionalLocationInformationComment = vacancy.AdditionalLocationInformationComment,
                    AutoSaveTimeoutInSeconds = _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds,
                    ProviderSiteId = providerSite.ProviderSiteId
                };

                var locationAddresses = _vacancyPostingService.GetVacancyLocations(vacancy.VacancyId);
                if (locationAddresses.Any())
                {
                    viewModel.Addresses =
                        _mapper.Map<List<VacancyLocation>, List<VacancyLocationAddressViewModel>>(locationAddresses);
                }
                else
                {
                    if (vacancy.IsEmployerLocationMainApprenticeshipLocation.HasValue &&
                    vacancy.IsEmployerLocationMainApprenticeshipLocation.Value == false && vacancy.Address != null)
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
            else
            {
                return new LocationSearchViewModel
                {
                    ProviderSiteId = providerSite.ProviderSiteId,
                    ProviderSiteEdsUrn = providerSite.EdsUrn,
                    EmployerId = employer.EmployerId,
                    EmployerEdsUrn = employer.EdsUrn,
                    VacancyGuid = vacancyGuid,
                    Ukprn = ukprn,
                    Addresses = new List<VacancyLocationAddressViewModel>(),
                    AutoSaveTimeoutInSeconds = _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds
                };
            }
        }

        public VacancyMinimumData UpdateVacancy(VacancyMinimumData vacancyMinimumData)
        {
            var vacancyParty =
                _providerService.GetVacancyParty(vacancyMinimumData.VacancyPartyId, true);
            if (vacancyParty == null)
                throw new Exception($"Vacancy Party {vacancyMinimumData.VacancyPartyId} not found / no longer current");

            var employer = _employerService.GetEmployer(vacancyParty.EmployerId, true);

            if (!employer.Address.GeoPoint.IsValid())
            {
                employer.Address.GeoPoint = _geoCodingService.GetGeoPointFor(employer.Address);
            }

            var vacancy = _vacancyPostingService.GetVacancy(vacancyMinimumData.VacancyGuid);

            vacancy.IsEmployerLocationMainApprenticeshipLocation = vacancy.IsEmployerLocationMainApprenticeshipLocation;
            vacancy.NumberOfPositions = vacancy.NumberOfPositions ?? 0;
            vacancy.Address = (vacancy.IsEmployerLocationMainApprenticeshipLocation.HasValue &&
                               vacancy.IsEmployerLocationMainApprenticeshipLocation.Value)
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
            var vacancyParty = _providerService.GetVacancyParty(vacancyMinimumData.VacancyPartyId, true);
            var employer = _employerService.GetEmployer(vacancyParty.EmployerId, true);
            var provider = _providerService.GetProvider(vacancyMinimumData.Ukprn);

            if (!employer.Address.GeoPoint.IsValid())
            {
                employer.Address.GeoPoint = _geoCodingService.GetGeoPointFor(employer.Address);
            }

            _vacancyPostingService.CreateVacancy(new Vacancy
            {
                VacancyGuid = vacancyMinimumData.VacancyGuid,
                VacancyReferenceNumber = vacancyReferenceNumber,
                OwnerPartyId = vacancyParty.VacancyPartyId,
                Status = VacancyStatus.Draft,
                IsEmployerLocationMainApprenticeshipLocation = vacancyMinimumData.IsEmployerLocationMainApprenticeshipLocation,
                NumberOfPositions = vacancyMinimumData.NumberOfPositions ?? 0,
                Address = vacancyMinimumData.IsEmployerLocationMainApprenticeshipLocation ? employer.Address : null,
                ProviderId = provider.ProviderId, //Confirmed from ReportUnsuccessfulCandidateApplications stored procedure
                LocalAuthorityCode = _localAuthorityLookupService.GetLocalAuthorityCode(employer.Address.Postcode),
                EmployerDescription = vacancyMinimumData.EmployerDescription,
                EmployerWebsiteUrl = vacancyMinimumData.EmployerWebsiteUrl
            });
        }

        public void TransferVacancies(ManageVacancyTransferViewModel vacancyTransferViewModel)
        {
            try
            {
                foreach (var referenceNumber in vacancyTransferViewModel.VacancyReferenceNumbers)
                {
                    var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(referenceNumber);
                    var vacancyOwnerRelationship = _providerService.GetVacancyParty(vacancy.VacancyOwnerRelationshipId, false);
                    var existingVacancyOwnerRelationship = _providerService.GetVacancyParty(vacancyOwnerRelationship.EmployerId, vacancyTransferViewModel.ProviderSiteId);
                    if (existingVacancyOwnerRelationship == null)
                    {
                        vacancyOwnerRelationship.ProviderSiteId = vacancyTransferViewModel.ProviderSiteId;
                        _providerService.SaveVacancyParty(vacancyOwnerRelationship);
                    }
                    else
                    {
                        vacancy.VacancyOwnerRelationshipId = vacancyOwnerRelationship.VacancyPartyId;
                    }

                    vacancy.ProviderId = vacancyTransferViewModel.ProviderId;
                    vacancy.DeliveryOrganisationId = vacancyTransferViewModel.ProviderSiteId;
                    vacancy.VacancyManagerId = vacancyTransferViewModel.ProviderSiteId;

                    _vacancyPostingService.UpdateVacanciesWithNewProvider(vacancy);
                }
            }
            catch (Exception exception)
            {
                _logService.Error($"Exception occurred while transferring the vacancy:{exception.Message}");
                throw exception;
            }
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
            var offlineApplicationUrl = !string.IsNullOrEmpty(newVacancyViewModel.OfflineApplicationUrl) ? new UriBuilder(newVacancyViewModel.OfflineApplicationUrl).Uri.ToString() : newVacancyViewModel.OfflineApplicationUrl;

            var vacancyParty =
                _providerService.GetVacancyParty(newVacancyViewModel.OwnerParty.VacancyPartyId, true);
            if (vacancyParty == null)
                throw new Exception($"Vacancy Party {newVacancyViewModel.OwnerParty.VacancyPartyId} not found / no longer current");

            //var employer = _employerService.GetEmployer(vacancyParty.EmployerId, true);

            //if (!employer.Address.GeoPoint.IsValid())
            //{
            //    employer.Address.GeoPoint = _geoCodingService.GetGeoPointFor(employer.Address);
            //}

            var vacancy = newVacancyViewModel.VacancyReferenceNumber.HasValue
                ? _vacancyPostingService.GetVacancyByReferenceNumber(newVacancyViewModel.VacancyReferenceNumber.Value)
                : _vacancyPostingService.GetVacancy(newVacancyViewModel.VacancyGuid);

            vacancy.Title = newVacancyViewModel.Title;
            vacancy.ShortDescription = newVacancyViewModel.ShortDescription;
            vacancy.OfflineVacancy = newVacancyViewModel.OfflineVacancy;
            vacancy.OfflineApplicationUrl = offlineApplicationUrl;
            vacancy.OfflineApplicationInstructions = newVacancyViewModel.OfflineApplicationInstructions;
            //vacancy.IsEmployerLocationMainApprenticeshipLocation = newVacancyViewModel.IsEmployerLocationMainApprenticeshipLocation;
            //vacancy.NumberOfPositions = newVacancyViewModel.NumberOfPositions ?? 0;
            vacancy.VacancyType = newVacancyViewModel.VacancyType;
            //vacancy.Address = (newVacancyViewModel.IsEmployerLocationMainApprenticeshipLocation.HasValue &&
            //                   newVacancyViewModel.IsEmployerLocationMainApprenticeshipLocation.Value)
            //                    ? employer.Address
            //                    : null;
            //vacancy.LocalAuthorityCode = _localAuthorityLookupService.GetLocalAuthorityCode(employer.Address.Postcode);

            vacancy = _vacancyPostingService.UpdateVacancy(vacancy);

            newVacancyViewModel = _mapper.Map<Vacancy, NewVacancyViewModel>(vacancy);

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

            viewModel.VacancyApplicationsState = GetVacancyApplicationsState(vacancy);

            viewModel.AutoSaveTimeoutInSeconds =
                    _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;

            return viewModel;
        }

        public FurtherVacancyDetailsViewModel UpdateVacancy(FurtherVacancyDetailsViewModel viewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber);

            vacancy.WorkingWeek = viewModel.WorkingWeek;
            vacancy.Wage = new Wage(viewModel.Wage.Type, viewModel.Wage.Amount, viewModel.Wage.Text, viewModel.Wage.Unit, viewModel.Wage.HoursPerWeek);
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

            viewModel = vacancy.ConvertToVacancySummaryViewModel();
            viewModel.AutoSaveTimeoutInSeconds =
                _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;

            return viewModel;
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
            vacancy.Wage = new Wage(viewModel.Wage.Type, viewModel.Wage.Amount, vacancy.Wage.Text, viewModel.Wage.Unit, vacancy.Wage.HoursPerWeek);
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

        public VacancyViewModel GetVacancy(int vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);

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
            var provider = _providerService.GetProvider(vacancy.ProviderId);
            viewModel.Provider = provider.Convert();
            var vacancyParty = _providerService.GetVacancyParty(vacancy.OwnerPartyId, false);  // Some current vacancies have non-current vacancy parties
            if (vacancyParty != null)
            {
                var employer = _employerService.GetEmployer(vacancyParty.EmployerId, false);  //Same with employers
                viewModel.NewVacancyViewModel.OwnerParty = vacancyParty.Convert(employer, vacancy.EmployerAnonymousName);
                var providerSite = _providerService.GetProviderSite(vacancyParty.ProviderSiteId);
                viewModel.ProviderSite = providerSite.Convert();
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
                }
                else if (viewModel.VacancyType == VacancyType.Traineeship)
                {
                    viewModel.ApplicationCount = _traineeshipApplicationService.GetApplicationCount(vacancy.VacancyId);
                }
            }
            var vacancyManager = _userProfileService.GetProviderUser(vacancy.CreatedByProviderUsername);
            viewModel.ContactDetailsAndVacancyHistory = ContactDetailsAndVacancyHistoryViewModelConverter.Convert(provider,
                vacancyManager, vacancy);
            var vacancyLocationAddressViewModels = GetLocationsAddressViewModel(vacancy);

            viewModel.LocationAddresses = vacancyLocationAddressViewModels;
            viewModel.NewVacancyViewModel.LocationAddresses = vacancyLocationAddressViewModels;
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

            foreach (var sector in categories.Where(category => !blacklistedCategoryCodes.Contains(category.CodeName)))
            {
                if (sector.SubCategories != null)
                {
                    var sectorGroup = new SelectListGroup { Name = sector.FullName };
                    foreach (var framework in sector.SubCategories)
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

        /// <summary>
        /// TODO: OO: Refactor/clean up this code.  Not a priority for now, as it works, but this is horrendous.
        /// </summary>
        /// <param name="providerId"></param>
        /// <param name="providerSiteId"></param>
        /// <param name="vacanciesSummarySearch"></param>
        /// <returns></returns>
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

            // reset filter on search
            vacanciesSummarySearch.FilterType = string.IsNullOrEmpty(vacanciesSummarySearch.SearchString)
                ? vacanciesSummarySearch.FilterType
                : VacanciesSummaryFilterTypes.All;

            var query = new VacancySummaryQuery()
            {
                ProviderId = providerId,
                ProviderSiteId = providerSiteId,
                OrderByField = orderByField,
                Filter = vacanciesSummarySearch.FilterType,
                PageSize = vacanciesSummarySearch.PageSize,
                RequestedPage = vacanciesSummarySearch.CurrentPage,
                SearchString = vacanciesSummarySearch.SearchString,
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
                TotalNumberOfPages = (int)Math.Ceiling((double)totalRecords / (double)vacanciesSummarySearch.PageSize)
            };

            var viewModel = new VacanciesSummaryViewModel()
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

        public VacancyPartyViewModel CloneVacancy(int vacancyReferenceNumber)
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
            vacancy.EmployerAnonymousName = null;

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
            vacancy.OfflineApplicationClickThroughCount = 0;
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

            var vacancyParty = _providerService.GetVacancyParty(vacancy.OwnerPartyId, true);
            if (vacancyParty == null)
                throw new Exception($"Vacancy Party {vacancy.OwnerPartyId} not found / no longer current");

            var employer = _employerService.GetEmployer(vacancyParty.EmployerId, true);
            var result = vacancyParty.Convert(employer);
            result.VacancyGuid = vacancy.VacancyGuid;

            return result;
        }

        public DashboardVacancySummariesViewModel GetPendingQAVacanciesOverview(DashboardVacancySummariesSearchViewModel searchViewModel)
        {
            var agencyUser = _userProfileService.GetAgencyUser(_currentUserService.CurrentUserName);
            var regionalTeam = agencyUser.RegionalTeam;

            var vacancies = _vacancyPostingService.GetWithStatus(VacancyStatus.Submitted, VacancyStatus.ReservedForQA).OrderBy(v => v.DateSubmitted).ToList();

            vacancies = SearchVacancySummaries(searchViewModel, vacancies);

            var utcNow = _dateTimeService.UtcNow;

            var submittedToday = vacancies.Where(v => v.DateSubmitted.HasValue && v.DateSubmitted >= utcNow.Date).ToList();
            var resubmitted = vacancies.Where(v => v.SubmissionCount > 1).ToList();

            var submittedYesterday =
                vacancies.Where(v => IsVacancySubmittedYesterday(v, utcNow)).ToList();

            var submittedMoreThan48Hours =
                vacancies.Where(v => IsVacancySubmittedMoreThan48HrsAgo(v, utcNow)).ToList();

            var regionalTeamsMetrics = GetRegionalTeamsMetrics(vacancies, submittedToday, submittedYesterday, submittedMoreThan48Hours, resubmitted);

            if (!IsSearch(searchViewModel))
            {
                //Only filter by regional team if not a search
                vacancies = vacancies.Where(v => v.RegionalTeam == regionalTeam).ToList();
                submittedToday = submittedToday.Where(v => v.RegionalTeam == regionalTeam).ToList();
                submittedYesterday = submittedYesterday.Where(v => v.RegionalTeam == regionalTeam).ToList();
                submittedMoreThan48Hours = submittedMoreThan48Hours.Where(v => v.RegionalTeam == regionalTeam).ToList();
                resubmitted = resubmitted.Where(v => v.RegionalTeam == regionalTeam).ToList();
            }

            if (vacancies.Count == 0)
            {
                //No vacancies for current team selection. Redirect to metrics
                searchViewModel.Mode = DashboardVacancySummariesMode.Metrics;
            }

            if (string.IsNullOrEmpty(searchViewModel.OrderByField))
            {
                switch (searchViewModel.FilterType)
                {
                    case DashboardVacancySummaryFilterTypes.SubmittedToday:
                        vacancies = submittedToday.OrderBy(v => v.DateFirstSubmitted).ToList();
                        break;
                    case DashboardVacancySummaryFilterTypes.SubmittedYesterday:
                        vacancies = submittedYesterday.OrderBy(v => v.DateFirstSubmitted).ToList();
                        break;
                    case DashboardVacancySummaryFilterTypes.SubmittedMoreThan48Hours:
                        vacancies = submittedMoreThan48Hours;
                        break;
                    case DashboardVacancySummaryFilterTypes.Resubmitted:
                        vacancies = resubmitted.OrderBy(v => v.DateFirstSubmitted).ToList();
                        break;
                }
            }

            var providerMap = _providerService.GetProviders(vacancies.Select(v => v.ProviderId)).ToDictionary(p => p.ProviderId, p => p);

            var viewModel = new DashboardVacancySummariesViewModel
            {
                SearchViewModel = searchViewModel,
                SubmittedTodayCount = submittedToday.Count,
                SubmittedYesterdayCount = submittedYesterday.Count,
                SubmittedMoreThan48HoursCount = submittedMoreThan48Hours.Count,
                ResubmittedCount = resubmitted.Count,
                Vacancies = GetOrderedVacancySummaries(searchViewModel, vacancies.Select(v => ConvertToDashboardVacancySummaryViewModel(v, providerMap[v.ProviderId]))).ToList(),
                RegionalTeamsMetrics = regionalTeamsMetrics
            };

            return viewModel;
        }

        private static bool IsVacancySubmittedMoreThan48HrsAgo(VacancySummary v, DateTime utcNow)
        {
            if (!v.DateSubmitted.HasValue || v.DateSubmitted >= utcNow.Date)
            {
                return false;
            }

            if (v.DateSubmitted.Value.DayOfWeek == DayOfWeek.Friday)
            {
                if (utcNow.DayOfWeek == DayOfWeek.Sunday || utcNow.DayOfWeek == DayOfWeek.Monday)
                {
                    return v.DateSubmitted < utcNow.Date.AddDays(-3);
                }
            }

            if (v.DateSubmitted.Value.DayOfWeek == DayOfWeek.Saturday)
            {
                if (utcNow.DayOfWeek == DayOfWeek.Monday || utcNow.DayOfWeek == DayOfWeek.Tuesday)
                {
                    return v.DateSubmitted < utcNow.Date.AddDays(-3);
                }
            }

            if (v.DateSubmitted.Value.DayOfWeek == DayOfWeek.Sunday)
            {
                if (utcNow.DayOfWeek == DayOfWeek.Tuesday)
                {
                    return v.DateSubmitted < utcNow.Date.AddDays(-2);
                }
            }

            return v.DateSubmitted < utcNow.Date.AddDays(-1);
        }

        private static bool IsVacancySubmittedYesterday(VacancySummary v, DateTime utcNow)
        {
            if (!v.DateSubmitted.HasValue || v.DateSubmitted >= utcNow.Date)
            {
                return false;
            }

            if (v.DateSubmitted.Value.DayOfWeek == DayOfWeek.Friday)
            {
                if (utcNow.DayOfWeek == DayOfWeek.Saturday)
                {
                    return v.DateSubmitted >= utcNow.Date.AddDays(-1) && v.DateSubmitted < utcNow.Date;
                }

                if (utcNow.DayOfWeek == DayOfWeek.Sunday)
                {
                    return v.DateSubmitted >= utcNow.Date.AddDays(-2) && v.DateSubmitted < utcNow.Date.AddDays(-1);
                }

                if (utcNow.DayOfWeek == DayOfWeek.Monday)
                {
                    return v.DateSubmitted >= utcNow.Date.AddDays(-3) && v.DateSubmitted < utcNow.Date.AddDays(-2);
                }
            }

            if (v.DateSubmitted.Value.DayOfWeek == DayOfWeek.Saturday)
            {
                if (utcNow.DayOfWeek == DayOfWeek.Monday)
                {
                    return v.DateSubmitted >= utcNow.Date.AddDays(-2) && v.DateSubmitted < utcNow.Date.AddDays(-1);
                }

                if (utcNow.DayOfWeek == DayOfWeek.Tuesday)
                {
                    return v.DateSubmitted >= utcNow.Date.AddDays(-3) && v.DateSubmitted < utcNow.Date.AddDays(-2);
                }
            }

            if (v.DateSubmitted.Value.DayOfWeek == DayOfWeek.Sunday)
            {
                if (utcNow.DayOfWeek == DayOfWeek.Tuesday)
                {
                    return v.DateSubmitted >= utcNow.Date.AddDays(-2) && v.DateSubmitted < utcNow.Date.AddDays(-1);
                }
            }

            return v.DateSubmitted >= utcNow.Date.AddDays(-1);
        }

        private List<VacancySummary> SearchVacancySummaries(DashboardVacancySummariesSearchViewModel searchViewModel, List<VacancySummary> vacancies)
        {
            if (IsSearch(searchViewModel))
            {
                if (!string.IsNullOrEmpty(searchViewModel.Provider) && searchViewModel.Provider.Length >= 3)
                {
                    var providers = _providerService.GetProviders(vacancies.Select(v => v.ProviderId));
                    var providerIds = new List<int>();
                    foreach (var provider in providers)
                    {
                        if (provider.TradingName.IndexOf(searchViewModel.Provider, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            providerIds.Add(provider.ProviderId);
                        }
                    }
                    vacancies = vacancies.Where(v => providerIds.Contains(v.ProviderId)).ToList();
                }
            }
            return vacancies;
        }

        private bool IsSearch(DashboardVacancySummariesSearchViewModel searchViewModel)
        {
            return !string.IsNullOrEmpty(searchViewModel.Provider);
        }

        private IEnumerable<DashboardVacancySummaryViewModel> GetOrderedVacancySummaries(DashboardVacancySummariesSearchViewModel searchViewModel, IEnumerable<DashboardVacancySummaryViewModel> vacancySummaries)
        {
            switch (searchViewModel.OrderByField)
            {
                case DashboardVacancySummariesSearchViewModel.OrderByFieldTitle:
                    vacancySummaries = searchViewModel.Order == Order.Descending
                        ? vacancySummaries.OrderByDescending(a => a.Title).ToList()
                        : vacancySummaries.OrderBy(a => a.Title).ToList();
                    break;
                case DashboardVacancySummariesSearchViewModel.OrderByFieldProvider:
                    vacancySummaries = searchViewModel.Order == Order.Descending
                        ? vacancySummaries.OrderByDescending(a => a.ProviderName).ToList()
                        : vacancySummaries.OrderBy(a => a.ProviderName).ToList();
                    break;
                case DashboardVacancySummariesSearchViewModel.OrderByFieldDateSubmitted:
                    vacancySummaries = searchViewModel.Order == Order.Descending
                        ? vacancySummaries.OrderByDescending(a => a.DateSubmitted).ToList()
                        : vacancySummaries.OrderBy(a => a.DateSubmitted).ToList();
                    break;
                case DashboardVacancySummariesSearchViewModel.OrderByFieldClosingDate:
                    vacancySummaries = searchViewModel.Order == Order.Descending
                        ? vacancySummaries.OrderByDescending(a => a.ClosingDate).ToList()
                        : vacancySummaries.OrderBy(a => a.ClosingDate).ToList();
                    break;
                case DashboardVacancySummariesSearchViewModel.OrderByFieldSubmissionCount:
                    vacancySummaries = searchViewModel.Order == Order.Descending
                        ? vacancySummaries.OrderByDescending(a => a.SubmissionCount).ToList()
                        : vacancySummaries.OrderBy(a => a.SubmissionCount).ToList();
                    break;
            }
            return vacancySummaries;
        }

        public DashboardVacancySummaryViewModel GetNextAvailableVacancy()
        {
            var vacancies = GetTeamVacancySummaries();

            var nextVacancy = _vacancyLockingService.GetNextAvailableVacancy(_currentUserService.CurrentUserName,
                vacancies);

            return nextVacancy != null ? ConvertToDashboardVacancySummaryViewModel(nextVacancy, _providerService.GetProvider(nextVacancy.ProviderId)) : null;
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

        private static List<RegionalTeamMetrics> GetRegionalTeamsMetrics(List<VacancySummary> vacancies, List<VacancySummary> submittedToday, List<VacancySummary> submittedYesterday, List<VacancySummary> submittedMoreThan48Hours, List<VacancySummary> resubmitted)
        {
            return new List<RegionalTeamMetrics>
            {
                GetRegionalTeamMetrics(RegionalTeam.North, vacancies, submittedToday, submittedYesterday, submittedMoreThan48Hours, resubmitted),
                GetRegionalTeamMetrics(RegionalTeam.NorthWest, vacancies, submittedToday, submittedYesterday, submittedMoreThan48Hours, resubmitted),
                GetRegionalTeamMetrics(RegionalTeam.YorkshireAndHumberside, vacancies, submittedToday, submittedYesterday, submittedMoreThan48Hours, resubmitted),
                GetRegionalTeamMetrics(RegionalTeam.EastMidlands, vacancies, submittedToday, submittedYesterday, submittedMoreThan48Hours, resubmitted),
                GetRegionalTeamMetrics(RegionalTeam.WestMidlands, vacancies, submittedToday, submittedYesterday, submittedMoreThan48Hours, resubmitted),
                GetRegionalTeamMetrics(RegionalTeam.EastAnglia, vacancies, submittedToday, submittedYesterday, submittedMoreThan48Hours, resubmitted),
                GetRegionalTeamMetrics(RegionalTeam.SouthEast, vacancies, submittedToday, submittedYesterday, submittedMoreThan48Hours, resubmitted),
                GetRegionalTeamMetrics(RegionalTeam.SouthWest, vacancies, submittedToday, submittedYesterday, submittedMoreThan48Hours, resubmitted)
            };
        }

        private static RegionalTeamMetrics GetRegionalTeamMetrics(RegionalTeam regionalTeam, IEnumerable<VacancySummary> vacancies, IEnumerable<VacancySummary> submittedToday, IEnumerable<VacancySummary> submittedYesterday, IEnumerable<VacancySummary> submittedMoreThan48Hours, IEnumerable<VacancySummary> resubmitted)
        {
            return new RegionalTeamMetrics
            {
                RegionalTeam = regionalTeam,
                TotalCount = vacancies.Count(v => v.RegionalTeam == regionalTeam),
                SubmittedTodayCount = submittedToday.Count(v => v.RegionalTeam == regionalTeam),
                SubmittedYesterdayCount = submittedYesterday.Count(v => v.RegionalTeam == regionalTeam),
                SubmittedMoreThan48HoursCount = submittedMoreThan48Hours.Count(v => v.RegionalTeam == regionalTeam),
                ResubmittedCount = resubmitted.Count(v => v.RegionalTeam == regionalTeam),
            };
        }

        private DashboardVacancySummaryViewModel ConvertToDashboardVacancySummaryViewModel(VacancySummary vacancy, Provider provider)
        {
            var userName = _currentUserService.CurrentUserName;

            return new DashboardVacancySummaryViewModel
            {
                ClosingDate = vacancy.ClosingDate,
                DateSubmitted = vacancy.DateSubmitted,
                DateFirstSubmitted = vacancy.DateFirstSubmitted,
                ProviderName = provider.TradingName,
                Status = vacancy.Status,
                Title = vacancy.Title,
                VacancyReferenceNumber = vacancy.VacancyReferenceNumber,
                DateStartedToQA = vacancy.DateStartedToQA,
                QAUserName = vacancy.QAUserName,
                CanBeReservedForQaByCurrentUser = _vacancyLockingService.IsVacancyAvailableToQABy(userName, vacancy),
                SubmissionCount = vacancy.SubmissionCount,
                VacancyType = vacancy.VacancyType
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
            newVacancy.IsEmployerLocationMainApprenticeshipLocation = true;

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
                if (!submittedVacancy.Address.GeoPoint.IsValid())
                {
                    submittedVacancy.Address.GeoPoint = _geoCodingService.GetGeoPointFor(submittedVacancy.Address);
                }

                if (submittedVacancy.IsEmployerLocationMainApprenticeshipLocation.HasValue &&
                    !submittedVacancy.IsEmployerLocationMainApprenticeshipLocation.Value)
                {
                    var vacancyLocationAddresses = _vacancyPostingService.GetVacancyLocations(submittedVacancy.VacancyId);

                    if (vacancyLocationAddresses != null && vacancyLocationAddresses.Any())
                    {
                        GeoCodeVacancyLocations(vacancyLocationAddresses);

                        var vacancyLocation = vacancyLocationAddresses.First();
                        submittedVacancy.Address = vacancyLocation.Address;
                        submittedVacancy.ParentVacancyId = submittedVacancy.VacancyId;
                        submittedVacancy.NumberOfPositions = vacancyLocation.NumberOfPositions;
                        submittedVacancy.IsEmployerLocationMainApprenticeshipLocation = true;

                        foreach (var locationAddress in vacancyLocationAddresses.Skip(1))
                        {
                            CreateChildVacancy(submittedVacancy, locationAddress, qaApprovalDate);
                        }

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

        private List<VacancySummary> GetTeamVacancySummaries()
        {
            var regionalTeam = GetRegionalTeamForCurrentUser();

            var vacancies =
                _vacancyPostingService.GetWithStatus(VacancyStatus.Submitted, VacancyStatus.ReservedForQA)
                    .Where(v => v.RegionalTeam == regionalTeam)
                    .OrderBy(v => v.DateSubmitted)
                    .ToList();

            return vacancies;
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
            vacancy.Wage = new Wage(viewModel.Wage.Type, viewModel.Wage.Amount, viewModel.Wage.Text, viewModel.Wage.Unit, viewModel.Wage.HoursPerWeek);
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

            var offlineApplicationUrl = !string.IsNullOrEmpty(viewModel.OfflineApplicationUrl) ? new UriBuilder(viewModel.OfflineApplicationUrl).Uri.ToString() : viewModel.OfflineApplicationUrl;

            //update properties
            vacancy.Title = viewModel.Title;
            vacancy.ShortDescription = viewModel.ShortDescription;
            vacancy.OfflineVacancy = viewModel.OfflineVacancy.Value; // At this point we'll always have a value
            vacancy.OfflineApplicationUrl = offlineApplicationUrl;
            vacancy.OfflineApplicationInstructions = viewModel.OfflineApplicationInstructions;
            vacancy.OfflineApplicationInstructionsComment = viewModel.OfflineApplicationInstructionsComment;
            vacancy.OfflineApplicationUrlComment = viewModel.OfflineApplicationUrlComment;
            vacancy.ShortDescriptionComment = viewModel.ShortDescriptionComment;
            vacancy.TitleComment = viewModel.TitleComment;
            vacancy.VacancyType = viewModel.VacancyType;
            // TODO: not sure if do this or call reserveForQA in the service
            AddQAInformation(vacancy);

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

            var vacancyParty = _providerService.GetVacancyParty(viewModel.OwnerParty.VacancyPartyId, false);
            if (vacancyParty == null)
                throw new Exception($"Vacancy Party {viewModel.OwnerParty.VacancyPartyId} not found / no longer current");

            var employer = _employerService.GetEmployer(vacancyParty.EmployerId, false);

            //update properties
            vacancy.EmployerDescriptionComment = viewModel.EmployerDescriptionComment;
            vacancy.EmployerWebsiteUrlComment = viewModel.EmployerWebsiteUrlComment;
            vacancy.IsEmployerLocationMainApprenticeshipLocation =
                viewModel.IsEmployerLocationMainApprenticeshipLocation;

            if (vacancy.IsEmployerLocationMainApprenticeshipLocation.HasValue &&
                vacancy.IsEmployerLocationMainApprenticeshipLocation.Value)
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
            var addresses = viewModel.Addresses.Select(_mapper.Map<VacancyLocationAddressViewModel, VacancyLocation>);

            var vacancyParty =
                _providerService.GetVacancyParty(viewModel.ProviderSiteId, viewModel.EmployerEdsUrn);
            viewModel.VacancyPartyId = vacancyParty.VacancyPartyId;

            var employer = _employerService.GetEmployer(vacancyParty.EmployerId, true);

            employer.Address.GeoPoint = _geoCodingService.GetGeoPointFor(employer.Address);

            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber);

            vacancy.IsEmployerLocationMainApprenticeshipLocation =
                viewModel.IsEmployerLocationMainApprenticeshipLocation;
            vacancy.NumberOfPositions = null;
            vacancy.Address = employer.Address;
            vacancy.LocationAddressesComment = viewModel.LocationAddressesComment;
            vacancy.AdditionalLocationInformation = viewModel.AdditionalLocationInformation;
            vacancy.AdditionalLocationInformationComment = viewModel.AdditionalLocationInformationComment;

            GeoCodeVacancyLocations(viewModel);

            if (addresses.Count() == 1)
            {
                //Set address
                vacancy.Address = addresses.Single().Address;
                vacancy.NumberOfPositions = addresses.Single().NumberOfPositions;
                vacancy.LocalAuthorityCode =
                    _localAuthorityLookupService.GetLocalAuthorityCode(vacancy.Address.Postcode);
                _vacancyPostingService.DeleteVacancyLocationsFor(vacancy.VacancyId);
                _vacancyPostingService.UpdateVacancy(vacancy);

            }
            else
            {
                _vacancyPostingService.UpdateVacancy(vacancy);
                var vacancyLocations =
                    _mapper.Map<List<VacancyLocationAddressViewModel>, List<VacancyLocation>>(
                        viewModel.Addresses);
                foreach (var vacancyLocation in vacancyLocations)
                {
                    vacancyLocation.VacancyId = vacancy.VacancyId;
                    vacancyLocation.LocalAuthorityCode =
                    _localAuthorityLookupService.GetLocalAuthorityCode(vacancyLocation.Address.Postcode);
                }
                _vacancyPostingService.DeleteVacancyLocationsFor(vacancy.VacancyId);
                _vacancyPostingService.SaveVacancyLocations(vacancyLocations);
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
                vacancy.IsEmployerLocationMainApprenticeshipLocation = null;
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
