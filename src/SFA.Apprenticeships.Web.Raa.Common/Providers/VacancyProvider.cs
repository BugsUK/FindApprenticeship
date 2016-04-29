namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Application.Interfaces.Applications;
    using Application.Interfaces.Employers;
    using SFA.Infrastructure.Interfaces;
    using Application.Interfaces.Providers;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.Users;
    using Application.Interfaces.Vacancies;
    using Application.Interfaces.VacancyPosting;
    using Configuration;
    using ViewModels.Vacancy;
    using Web.Common.Configuration;
    using Converters;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Reference;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.ReferenceData;
    using Factories;
    using Infrastructure.Presentation;
    using ViewModels.Provider;
    using ViewModels.ProviderUser;
    using ViewModels.VacancyPosting;
    using Web.Common.ViewModels;
    using Web.Common.ViewModels.Locations;

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

        public VacancyProvider(ILogService logService, IConfigurationService configurationService,
            IVacancyPostingService vacancyPostingService, IReferenceDataService referenceDataService,
            IProviderService providerService, IEmployerService employerService, IDateTimeService dateTimeService,
            IMapper mapper, IApprenticeshipApplicationService apprenticeshipApplicationService,
            ITraineeshipApplicationService traineeshipApplicationService, IVacancyLockingService vacancyLockingService,
            ICurrentUserService currentUserService, IUserProfileService userProfileService)
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
        }

        public NewVacancyViewModel GetNewVacancyViewModel(int vacancyPartyId, Guid vacancyGuid, int? numberOfPositions)
        {
            var existingVacancy = _vacancyPostingService.GetVacancy(vacancyGuid);

            var vacancyParty = _providerService.GetVacancyParty(vacancyPartyId);
            var employer = _employerService.GetEmployer(vacancyParty.EmployerId);
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
            var vacancyParty = _providerService.GetVacancyParty(vacancy.OwnerPartyId);
            var viewModel = _mapper.Map<Vacancy, NewVacancyViewModel>(vacancy);
            var employer = _employerService.GetEmployer(vacancyParty.EmployerId);            
            viewModel.LocationAddresses = GetLocationsAddressViewModel(vacancy);            
            viewModel.OwnerParty = vacancyParty.Convert(employer);
            viewModel.AutoSaveTimeoutInSeconds =
                _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;
            viewModel.VacancyGuid = vacancy.VacancyGuid;
            return viewModel;
        }

        public LocationSearchViewModel CreateVacancy(LocationSearchViewModel locationSearchViewModel, string ukprn)
        {
            var vacancyReferenceNumber = _vacancyPostingService.GetNextVacancyReferenceNumber();
            var vacancyParty =
                _providerService.GetVacancyParty(locationSearchViewModel.ProviderSiteId, locationSearchViewModel.EmployerEdsUrn);
            var provider = _providerService.GetProvider(ukprn);

            locationSearchViewModel.VacancyPartyId = vacancyParty.VacancyPartyId;

            var vacancy = new Vacancy
            {
                VacancyGuid = locationSearchViewModel.VacancyGuid,
                VacancyReferenceNumber = vacancyReferenceNumber,
                OwnerPartyId = vacancyParty.VacancyPartyId,
                Status = VacancyStatus.Draft,
                AdditionalLocationInformation = locationSearchViewModel.AdditionalLocationInformation,
                IsEmployerLocationMainApprenticeshipLocation = locationSearchViewModel.IsEmployerLocationMainApprenticeshipLocation,
                ProviderId = provider.ProviderId
            };

            if (locationSearchViewModel.Addresses.Count == 1)
            {
                //Set address
                var addressViewModel = locationSearchViewModel.Addresses.Single();
                vacancy.Address = _mapper.Map<AddressViewModel, PostalAddress>(addressViewModel.Address);
                vacancy.NumberOfPositions = addressViewModel.NumberOfPositions;
                _vacancyPostingService.CreateApprenticeshipVacancy(vacancy);
            }
            else
            {
                vacancy = _vacancyPostingService.CreateApprenticeshipVacancy(vacancy);
                var vacancyLocations =
                    _mapper.Map<List<VacancyLocationAddressViewModel>, List<VacancyLocation>>(
                        locationSearchViewModel.Addresses);
                foreach (var vacancyLocation in vacancyLocations)
                {
                    vacancyLocation.VacancyId = vacancy.VacancyId;
                }
                _vacancyPostingService.SaveVacancyLocations(vacancyLocations);
            }

            locationSearchViewModel.AutoSaveTimeoutInSeconds =
                _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;

            return locationSearchViewModel;
        }

        public LocationSearchViewModel LocationAddressesViewModel(string ukprn, int providerSiteId, int employerId, Guid vacancyGuid)
        {
            var vacancy = _vacancyPostingService.GetVacancy(vacancyGuid);
            var providerSite = _providerService.GetProviderSite(providerSiteId);
            var employer = _employerService.GetEmployer(employerId);

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

        /// <summary>
        /// This method will create a new Vacancy record if the model provided does not have a vacancy reference number.
        /// Otherwise, it updates the existing one.
        /// </summary>
        /// <param name="newVacancyViewModel"></param>
        /// <returns></returns>
        public NewVacancyViewModel CreateVacancy(NewVacancyViewModel newVacancyViewModel, string ukprn)
        {
            NewVacancyViewModel resultViewModel;

            if (VacancyExists(newVacancyViewModel))
            {
                resultViewModel = UpdateExistingVacancy(newVacancyViewModel);
            }
            else
            {
                _logService.Debug("Creating vacancy reference number");

                try
                {
                    var vacancy = CreateNewVacancy(newVacancyViewModel, ukprn);

                    _logService.Debug("Created vacancy with reference number={0}", vacancy.VacancyReferenceNumber);

                    resultViewModel = _mapper.Map<Vacancy, NewVacancyViewModel>(vacancy);
                }
                catch (Exception e)
                {
                    _logService.Error("Failed to create vacancy", e);
                    throw;
                }
            }

            resultViewModel.AutoSaveTimeoutInSeconds =
                _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;

            return resultViewModel;
        }

        private Vacancy CreateNewVacancy(NewVacancyViewModel newVacancyViewModel, string ukprn)
        {
            var offlineApplicationUrl = !string.IsNullOrEmpty(newVacancyViewModel.OfflineApplicationUrl) ? new UriBuilder(newVacancyViewModel.OfflineApplicationUrl).Uri.ToString() : newVacancyViewModel.OfflineApplicationUrl;
            var vacancyReferenceNumber = _vacancyPostingService.GetNextVacancyReferenceNumber();
            var vacancyParty =
                _providerService.GetVacancyParty(newVacancyViewModel.OwnerParty.VacancyPartyId);
            var employer = _employerService.GetEmployer(vacancyParty.EmployerId);
            var provider = _providerService.GetProvider(ukprn);

            var vacancy = _vacancyPostingService.CreateApprenticeshipVacancy(new Vacancy
            {
                VacancyGuid = newVacancyViewModel.VacancyGuid,
                VacancyReferenceNumber = vacancyReferenceNumber,
                Title = newVacancyViewModel.Title,
                ShortDescription = newVacancyViewModel.ShortDescription,
                OwnerPartyId = vacancyParty.VacancyPartyId,
                Status = VacancyStatus.Draft,
                OfflineVacancy = newVacancyViewModel.OfflineVacancy,
                OfflineApplicationUrl = offlineApplicationUrl,
                OfflineApplicationInstructions = newVacancyViewModel.OfflineApplicationInstructions,
                IsEmployerLocationMainApprenticeshipLocation = newVacancyViewModel.IsEmployerLocationMainApprenticeshipLocation,
                NumberOfPositions = newVacancyViewModel.NumberOfPositions ?? 0,
                VacancyType = newVacancyViewModel.VacancyType,
                Address = employer.Address,
                ProviderId = provider.ProviderId //Confirmed from ReportUnsuccessfulCandidateApplications stored procedure
                // VacancyManagerId = vacancyParty.ProviderSiteId //TODO VGA: is this correct?
            });

            return vacancy;
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
                _providerService.GetVacancyParty(newVacancyViewModel.OwnerParty.VacancyPartyId);
            var employer = _employerService.GetEmployer(vacancyParty.EmployerId);

            var vacancy = newVacancyViewModel.VacancyReferenceNumber.HasValue
                ? _vacancyPostingService.GetVacancyByReferenceNumber(newVacancyViewModel.VacancyReferenceNumber.Value)
                : _vacancyPostingService.GetVacancy(newVacancyViewModel.VacancyGuid);

            vacancy.Title = newVacancyViewModel.Title;
            vacancy.ShortDescription = newVacancyViewModel.ShortDescription;
            vacancy.OfflineVacancy = newVacancyViewModel.OfflineVacancy;
            vacancy.OfflineApplicationUrl = offlineApplicationUrl;
            vacancy.OfflineApplicationInstructions = newVacancyViewModel.OfflineApplicationInstructions;
            vacancy.IsEmployerLocationMainApprenticeshipLocation = newVacancyViewModel.IsEmployerLocationMainApprenticeshipLocation;
            vacancy.NumberOfPositions = newVacancyViewModel.NumberOfPositions ?? 0;
            vacancy.VacancyType = newVacancyViewModel.VacancyType;

            vacancy.Address = newVacancyViewModel.IsEmployerLocationMainApprenticeshipLocation.HasValue
                              && newVacancyViewModel.IsEmployerLocationMainApprenticeshipLocation.Value == false
                              && newVacancyViewModel.LocationAddresses != null
                              && newVacancyViewModel.LocationAddresses.Count == 1
                                  ? _mapper.Map<AddressViewModel, PostalAddress>(newVacancyViewModel.LocationAddresses.First().Address)
                                  : employer.Address;

            vacancy = _vacancyPostingService.UpdateVacancy(vacancy);

            newVacancyViewModel = _mapper.Map<Vacancy, NewVacancyViewModel>(vacancy);

            return newVacancyViewModel;
        }

        private bool VacancyExists(NewVacancyViewModel newVacancyViewModel)
        {
            var hasReferenceNumber = newVacancyViewModel.VacancyReferenceNumber.HasValue && newVacancyViewModel.VacancyReferenceNumber > 0;

            if (hasReferenceNumber) return true;

            var vacancy = _vacancyPostingService.GetVacancy(newVacancyViewModel.VacancyGuid);

            return vacancy != null;
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

            viewModel.AutoSaveTimeoutInSeconds =
                    _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;

            return viewModel;
        }

        public FurtherVacancyDetailsViewModel UpdateVacancy(FurtherVacancyDetailsViewModel viewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber);

            vacancy.WorkingWeek = viewModel.WorkingWeek;
            vacancy.HoursPerWeek = viewModel.HoursPerWeek;
            vacancy.WageType = viewModel.WageType;
            vacancy.Wage = viewModel.Wage;
            vacancy.WageUnit = viewModel.WageUnit;
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

        public VacancyDatesViewModel UpdateVacancy(VacancyDatesViewModel viewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber);

            vacancy.ClosingDate = viewModel.ClosingDate.Date;
            vacancy.PossibleStartDate = viewModel.PossibleStartDate.Date;
            vacancy.Status = VacancyStatus.Live;

            VacancyDatesViewModel result;

            try
            {
                vacancy = _vacancyPostingService.UpdateVacancy(vacancy);
            }
            catch (CustomException)
            {
                result = _mapper.Map<Vacancy, VacancyDatesViewModel>(vacancy);
                result.State = UpdateVacancyDatesState.InvalidState;
                result.AutoSaveTimeoutInSeconds =
                    _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;

                return result;
            }
            
            result = _mapper.Map<Vacancy, VacancyDatesViewModel>(vacancy);
            result.State = _apprenticeshipApplicationService.GetApplicationCount(vacancy.VacancyId) > 0 ? UpdateVacancyDatesState.UpdatedHasApplications : UpdateVacancyDatesState.UpdatedNoApplications;
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
            var provider = _providerService.GetProviderViaOwnerParty(vacancy.OwnerPartyId);
            var vacancyParty = _providerService.GetVacancyParty(vacancy.OwnerPartyId);
            var employer = _employerService.GetEmployer(vacancyParty.EmployerId);
            viewModel.NewVacancyViewModel.OwnerParty = vacancyParty.Convert(employer, vacancy.EmployerAnonymousName);
            var providerSite = _providerService.GetProviderSite(vacancyParty.ProviderSiteId);
            viewModel.ProviderSite = providerSite.Convert();
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
                else if(viewModel.VacancyType == VacancyType.Traineeship)
                {
                    viewModel.ApplicationCount = _traineeshipApplicationService.GetApplicationCount(vacancy.VacancyId);
                }
            }
            var vacancyManager = _userProfileService.GetProviderUser(vacancy.CreatedByProviderUsername);
            viewModel.ContactDetailsAndVacancyHistory = ContactDetailsAndVacancyHistoryViewModelConverter.Convert(provider,
                vacancyManager, vacancy);
            var vacancyLocationAddressViewModels = GetLocationsAddressViewModel(vacancy);

            viewModel.LocationAddresses =  vacancyLocationAddressViewModels;   
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
                    var sectorGroup = new SelectListGroup {Name = sector.FullName};
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

        
        public VacanciesSummaryViewModel GetVacanciesSummaryForProvider(int providerId, int providerSiteId,
            VacanciesSummarySearchViewModel vacanciesSummarySearch)
        {
            var isVacancySearch = !string.IsNullOrEmpty(vacanciesSummarySearch.SearchString);
            if (isVacancySearch)
            {
                //When searching the ﬁlters (lottery numbers) are ignored and the search applies to all vacancies
                vacanciesSummarySearch.FilterType = VacanciesSummaryFilterTypes.All;
            }

            //TODO: Unit tests
            var vacancyParties = _providerService.GetVacancyParties(providerSiteId).ToList();
            var employers = _employerService.GetEmployers(vacancyParties.Select(vp => vp.EmployerId));
            var vacancyPartyToEmployerMap = vacancyParties.ToDictionary(vp => vp.VacancyPartyId, vp => employers.Single(e => e.EmployerId == vp.EmployerId));
            var vacancies = _vacancyPostingService.GetByOwnerPartyIds(vacancyParties.Select(vp => vp.VacancyPartyId));
            var hasVacancies = vacancies.Count > 0;
            vacancies = vacancies.Where(v => v.VacancyType == vacanciesSummarySearch.VacancyType || v.VacancyType == VacancyType.Unknown).ToList();

            var live = vacancies.Where(v => v.Status == VacancyStatus.Live).ToList();
            var liveVacancyIds = live.Select(vacancySummary => vacancySummary.VacancyId).ToList();

            var submitted = vacancies.Where(v => v.Status == VacancyStatus.Submitted || v.Status == VacancyStatus.ReservedForQA).ToList();
            var rejected = vacancies.Where(v => v.Status == VacancyStatus.Referred).ToList();
            //TODO: Agree on closing soon range and make configurable
            var closingSoon = vacancies.Where(v => v.Status == VacancyStatus.Live && v.ClosingDate.HasValue && v.ClosingDate >= _dateTimeService.UtcNow.Date && v.ClosingDate.Value.AddDays(-5) < _dateTimeService.UtcNow).ToList();
            var closed = vacancies.Where(v => v.Status == VacancyStatus.Closed).ToList();
            var draft = vacancies.Where(v => v.Status == VacancyStatus.Draft).ToList();
            var newApplications = vacanciesSummarySearch.VacancyType == VacancyType.Apprenticeship ?
                vacancies.Where(v => v.Status == VacancyStatus.Live && _apprenticeshipApplicationService.GetNewApplicationCount(v.VacancyId) > 0).ToList() :
                vacancies.Where(v => v.Status == VacancyStatus.Live && _traineeshipApplicationService.GetNewApplicationCount(v.VacancyId) > 0).ToList();
            var newApplicationsCount = vacanciesSummarySearch.VacancyType == VacancyType.Apprenticeship ?
                _apprenticeshipApplicationService.GetNewApplicationsCount(liveVacancyIds):
                _traineeshipApplicationService.GetNewApplicationsCount(liveVacancyIds);
            var withdrawn = vacancies.Where(v => v.Status == VacancyStatus.Withdrawn).ToList();
            var completed = vacancies.Where(v => v.Status == VacancyStatus.Completed).ToList();

            switch (vacanciesSummarySearch.FilterType)
            {
                case VacanciesSummaryFilterTypes.Live:
                    vacancies = live;
                    break;
                case VacanciesSummaryFilterTypes.Submitted:
                    vacancies = submitted;
                    break;
                case VacanciesSummaryFilterTypes.Rejected:
                    vacancies = rejected;
                    break;
                case VacanciesSummaryFilterTypes.ClosingSoon:
                    vacancies = closingSoon.OrderBy(v => v.ClosingDate).ToList();
                    break;
                case VacanciesSummaryFilterTypes.Closed:
                    vacancies = closed.OrderByDescending(v => v.ClosingDate).ToList();
                    break;
                case VacanciesSummaryFilterTypes.Draft:
                    vacancies = draft;
                    break;
                case VacanciesSummaryFilterTypes.NewApplications:
                    vacancies = newApplications.OrderBy(v => v.ClosingDate).ToList();
                    break;
                case VacanciesSummaryFilterTypes.Withdrawn:
                    vacancies = withdrawn;
                    break;
                case VacanciesSummaryFilterTypes.Completed:
                    vacancies = completed.OrderByDescending(v => v.UpdatedDateTime).ToList();
                    break;
            }

            vacanciesSummarySearch.PageSizes = SelectListItemsFactory.GetPageSizes(vacanciesSummarySearch.PageSize);

            var vacancySummaries = vacancies.Select(v => _mapper.Map<VacancySummary, VacancySummaryViewModel>(v)).ToList();
            foreach (var vacancySummary in vacancySummaries)
            {
                vacancySummary.EmployerName = vacancyPartyToEmployerMap[vacancySummary.OwnerPartyId].Name;
            }

            if (isVacancySearch)
            {
                vacancySummaries = vacancySummaries.Where(v => (!string.IsNullOrEmpty(v.Title) && v.Title.IndexOf(vacanciesSummarySearch.SearchString, StringComparison.OrdinalIgnoreCase) >= 0) || v.EmployerName.IndexOf(vacanciesSummarySearch.SearchString, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            var vacancyPage = new PageableViewModel<VacancySummaryViewModel>
            {
                Page = vacancySummaries.Skip((vacanciesSummarySearch.CurrentPage - 1)*vacanciesSummarySearch.PageSize).Take(vacanciesSummarySearch.PageSize).ToList(),
                ResultsCount = vacancySummaries.Count,
                CurrentPage = vacanciesSummarySearch.CurrentPage,
                TotalNumberOfPages = vacancySummaries.Count == 0 ? 1 : (int)Math.Ceiling((double)vacancySummaries.Count/vacanciesSummarySearch.PageSize)
            };

            //TODO: This information will be returned from _apprenticeshipVacancyReadRepository.GetForProvider or similar once FAA has been migrated
            foreach (var vacancyViewModel in vacancyPage.Page.Where(v => v.Status.CanHaveApplicationsOrClickThroughs()))
            {
                if (vacancyViewModel.VacancyType == VacancyType.Apprenticeship)
                {
                    vacancyViewModel.ApplicationCount = _apprenticeshipApplicationService.GetApplicationCount(vacancyViewModel.VacancyId);
                }
                else if (vacancyViewModel.VacancyType == VacancyType.Traineeship)
                {
                    vacancyViewModel.ApplicationCount = _traineeshipApplicationService.GetApplicationCount(vacancyViewModel.VacancyId);
                }
            }

            foreach (var vacancyViewModel in vacancyPage.Page.Where(v => v.IsEmployerLocationMainApprenticeshipLocation.HasValue && !v.IsEmployerLocationMainApprenticeshipLocation.Value))
            {
                vacancyViewModel.LocationAddresses = _mapper.Map<List<VacancyLocation>, List<VacancyLocationAddressViewModel>>(_vacancyPostingService.GetVacancyLocations(vacancyViewModel.VacancyId));
            }

            var vacanciesSummary = new VacanciesSummaryViewModel
            {
                VacanciesSummarySearch = vacanciesSummarySearch,
                LiveCount = live.Count,
                SubmittedCount = submitted.Count,
                RejectedCount = rejected.Count,
                ClosingSoonCount = closingSoon.Count,
                ClosedCount = closed.Count,
                DraftCount = draft.Count,
                NewApplicationsCount = newApplicationsCount,
                WithdrawnCount = withdrawn.Count,
                CompletedCount = completed.Count,
                HasVacancies = hasVacancies,
                Vacancies = vacancyPage
            };

            return vacanciesSummary; 
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

            _vacancyPostingService.CreateApprenticeshipVacancy(vacancy);

            var vacancyParty = _providerService.GetVacancyParty(vacancy.OwnerPartyId);
            var employer = _employerService.GetEmployer(vacancyParty.EmployerId);
            var result = vacancyParty.Convert(employer);
            result.VacancyGuid = vacancy.VacancyGuid;

            return result;
        }

        public DashboardVacancySummariesViewModel GetPendingQAVacanciesOverview(DashboardVacancySummariesSearchViewModel searchViewModel)
        {
            var agencyUser = _userProfileService.GetAgencyUser(_currentUserService.CurrentUserName);
            var regionalTeam = agencyUser.RegionalTeam;

            var vacancies = _vacancyPostingService.GetWithStatus(VacancyStatus.Submitted, VacancyStatus.ReservedForQA).OrderBy(v => v.DateSubmitted).ToList();

            var utcNow = _dateTimeService.UtcNow;

            var submittedToday = vacancies.Where(v => v.DateSubmitted.HasValue && v.DateSubmitted >= utcNow.Date).ToList();
            var submittedYesterday = vacancies.Where(v => v.DateSubmitted.HasValue && v.DateSubmitted < utcNow.Date && v.DateSubmitted >= utcNow.Date.AddDays(-1)).ToList();
            var submittedMoreThan48Hours = vacancies.Where(v => v.DateSubmitted.HasValue && v.DateSubmitted < utcNow.Date.AddDays(-1)).ToList();
            var resubmitted = vacancies.Where(v => v.SubmissionCount > 1).ToList();

            var regionalTeamsMetrics = GetRegionalTeamsMetrics(vacancies, submittedToday, submittedYesterday, submittedMoreThan48Hours, resubmitted);

            vacancies = vacancies.Where(v => v.RegionalTeam == regionalTeam).ToList();
            submittedToday = submittedToday.Where(v => v.RegionalTeam == regionalTeam).ToList();
            submittedYesterday = submittedYesterday.Where(v => v.RegionalTeam == regionalTeam).ToList();
            submittedMoreThan48Hours = submittedMoreThan48Hours.Where(v => v.RegionalTeam == regionalTeam).ToList();
            resubmitted = resubmitted.Where(v => v.RegionalTeam == regionalTeam).ToList();

            if (vacancies.Count == 0)
            {
                //No vacancies for current team selection. Redirect to metrics
                searchViewModel.Mode = DashboardVacancySummariesMode.Metrics;
            }

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

            var viewModel = new DashboardVacancySummariesViewModel
            {
                SearchViewModel = searchViewModel,
                SubmittedTodayCount = submittedToday.Count,
                SubmittedYesterdayCount = submittedYesterday.Count,
                SubmittedMoreThan48HoursCount = submittedMoreThan48Hours.Count,
                ResubmittedCount = resubmitted.Count,
                Vacancies = vacancies.Select(ConvertToDashboardVacancySummaryViewModel).ToList(),
                RegionalTeamsMetrics = regionalTeamsMetrics
            };

            return viewModel;
        }

        public DashboardVacancySummaryViewModel GetNextAvailableVacancy()
        {
            var vacancies = GetTeamVacancySummaries();

            var nextVacancy = _vacancyLockingService.GetNextAvailableVacancy(_currentUserService.CurrentUserName,
                vacancies); 

            return nextVacancy != null ? ConvertToDashboardVacancySummaryViewModel(nextVacancy) : null;
        }

        public void UnReserveVacancyForQA(int vacancyReferenceNumber)
        {
            var vacancyToUnReserve = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);

            if (_vacancyLockingService.IsVacancyAvailableToQABy(_currentUserService.CurrentUserName, vacancyToUnReserve))
            {
                vacancyToUnReserve.QAUserName = null;
                vacancyToUnReserve.Status = VacancyStatus.Submitted;

                _vacancyPostingService.UpdateVacancy(vacancyToUnReserve);
            }
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

        private DashboardVacancySummaryViewModel ConvertToDashboardVacancySummaryViewModel(VacancySummary vacancy)
        {
            var provider = _providerService.GetProviderViaOwnerParty(vacancy.OwnerPartyId);
            var userName = _currentUserService.CurrentUserName;

            return new DashboardVacancySummaryViewModel
            {
                ClosingDate = vacancy.ClosingDate,
                DateSubmitted = vacancy.DateSubmitted,
                DateFirstSubmitted = vacancy.DateFirstSubmitted,
                ProviderName = provider.Name,
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

        private void CreateChildVacancy(Vacancy vacancy, VacancyLocation address, DateTime approvalTime)
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

            _vacancyPostingService.CreateApprenticeshipVacancy(newVacancy);
        }

        public QAActionResultCode ApproveVacancy(int vacancyReferenceNumber)
        {
            var qaApprovalDate = _dateTimeService.UtcNow;
            var submittedVacancy = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);

            if (!_vacancyLockingService.IsVacancyAvailableToQABy(_currentUserService.CurrentUserName, submittedVacancy))
            {
                return QAActionResultCode.InvalidVacancy;
            }

            if (submittedVacancy.IsEmployerLocationMainApprenticeshipLocation.HasValue && !submittedVacancy.IsEmployerLocationMainApprenticeshipLocation.Value)
            {
                var vacancyLocationAddresses = _vacancyPostingService.GetVacancyLocations(submittedVacancy.VacancyId);
                if (vacancyLocationAddresses != null && vacancyLocationAddresses.Any())
                {
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
            vacancy.HoursPerWeek = viewModel.HoursPerWeek;
            vacancy.WageType = viewModel.WageType;
            vacancy.Wage = viewModel.Wage;
            vacancy.WageUnit = viewModel.WageUnit;
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

            var vacancyParty =
                _providerService.GetVacancyParty(viewModel.OwnerParty.VacancyPartyId);
            var employer = _employerService.GetEmployer(vacancyParty.EmployerId);

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
            var addresses = viewModel.Addresses.Select(a => new VacancyLocation
            {
                Address = new PostalAddress
                {
                    AddressLine1 = a.Address.AddressLine1,
                    AddressLine2 = a.Address.AddressLine2,
                    AddressLine3 = a.Address.AddressLine3,
                    AddressLine4 = a.Address.AddressLine4,
                    Postcode = a.Address.Postcode,
                },
                NumberOfPositions = a.NumberOfPositions.Value
            });

            var vacancyParty =
                _providerService.GetVacancyParty(viewModel.ProviderSiteId, viewModel.EmployerEdsUrn);
            viewModel.VacancyPartyId = vacancyParty.VacancyPartyId;

            var employer = _employerService.GetEmployer(vacancyParty.EmployerId);

            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber);

            vacancy.IsEmployerLocationMainApprenticeshipLocation =
                viewModel.IsEmployerLocationMainApprenticeshipLocation;
            vacancy.NumberOfPositions = null;           
            vacancy.Address = employer.Address;
            vacancy.LocationAddressesComment = viewModel.LocationAddressesComment;
            vacancy.AdditionalLocationInformation = viewModel.AdditionalLocationInformation;
            vacancy.AdditionalLocationInformationComment = viewModel.AdditionalLocationInformationComment;
            
            if (addresses.Count() == 1)
            {
                //Set address
                vacancy.Address = addresses.Single().Address;
                vacancy.NumberOfPositions = addresses.Single().NumberOfPositions;
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

        public VacancyDatesViewModel GetVacancyDatesViewModel(int vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);

            var viewModel = _mapper.Map<Vacancy, VacancyDatesViewModel>(vacancy);

            viewModel.AutoSaveTimeoutInSeconds =
                    _configurationService.Get<RecruitWebConfiguration>().AutoSaveTimeoutInSeconds;

            return viewModel;
        }
    }
}
