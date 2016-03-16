namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;
    using Application.Interfaces.Applications;
    using Application.Interfaces.Employers;
    using SFA.Infrastructure.Interfaces;
    using Application.Interfaces.Providers;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.Vacancies;
    using Application.Interfaces.VacancyPosting;
    using ViewModels.Vacancy;
    using Web.Common.Configuration;
    using Converters;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Users;
    using Domain.Entities.Raa.Vacancies;
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
        private readonly IConfigurationService _configurationService;
        private readonly IMapper _mapper;

        public VacancyProvider( ILogService logService, 
                                IConfigurationService configurationService, 
                                IVacancyPostingService vacancyPostingService, 
                                IReferenceDataService referenceDataService, 
                                IProviderService providerService, 
                                IEmployerService employerService, 
                                IDateTimeService dateTimeService, 
                                IMapper mapper, 
                                IApprenticeshipApplicationService apprenticeshipApplicationService, 
                                ITraineeshipApplicationService traineeshipApplicationService,
                                IVacancyLockingService vacancyLockingService)
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
                return vacancyViewModel;
            }
            
            return new NewVacancyViewModel
            {
                OwnerParty = vacancyPartyViewModel,
                IsEmployerLocationMainApprenticeshipLocation = numberOfPositions.HasValue,
                NumberOfPositions = numberOfPositions
            };
        }

        public NewVacancyViewModel GetNewVacancyViewModel(int vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);
            var vacancyParty = _providerService.GetVacancyParty(vacancy.OwnerPartyId);
            var viewModel = _mapper.Map<Vacancy, NewVacancyViewModel>(vacancy);
            var employer = _employerService.GetEmployer(vacancyParty.EmployerId);
            viewModel.OwnerParty = vacancyParty.Convert(employer);

            viewModel.VacancyGuid = vacancy.VacancyGuid;
            return viewModel;
        }

        public LocationSearchViewModel CreateVacancy(LocationSearchViewModel locationSearchViewModel)
        {
            var vacancyReferenceNumber = _vacancyPostingService.GetNextVacancyReferenceNumber();
            var vacancyParty =
                _providerService.GetVacancyParty(locationSearchViewModel.ProviderSiteId, locationSearchViewModel.EmployerEdsUrn);

            locationSearchViewModel.VacancyPartyId = vacancyParty.VacancyPartyId;

            var vacancy = new Vacancy
            {
                VacancyGuid = locationSearchViewModel.VacancyGuid,
                VacancyReferenceNumber = vacancyReferenceNumber,
                OwnerPartyId = vacancyParty.VacancyPartyId,
                Status = VacancyStatus.Draft,
                AdditionalLocationInformation = locationSearchViewModel.AdditionalLocationInformation,
                IsEmployerLocationMainApprenticeshipLocation = locationSearchViewModel.IsEmployerLocationMainApprenticeshipLocation,
            };

            if (locationSearchViewModel.Addresses.Count == 1)
            {
                //Set address
                vacancy.Address = _mapper.Map<AddressViewModel, PostalAddress>(locationSearchViewModel.Addresses.Single().Address);
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
                    AdditionalLocationInformationComment = vacancy.AdditionalLocationInformationComment
                };

                var locationAddresses = _vacancyPostingService.GetVacancyLocations(vacancy.VacancyId);
                locationAddresses?.ForEach(v => viewModel.Addresses.Add(new VacancyLocationAddressViewModel
                {
                    Address = new AddressViewModel
                    {
                        AddressLine1 = v.Address.AddressLine1,
                        AddressLine2 = v.Address.AddressLine2,
                        AddressLine3 = v.Address.AddressLine3,
                        AddressLine4 = v.Address.AddressLine4,
                        Postcode = v.Address.Postcode,
                        //Uprn = v.Address.Uprn
                    },
                    NumberOfPositions = v.NumberOfPositions
                }));

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
                    Addresses = new List<VacancyLocationAddressViewModel>()
                };
            }
        }

        /// <summary>
        /// This method will create a new Vacancy record if the model provided does not have a vacancy reference number.
        /// Otherwise, it updates the existing one.
        /// </summary>
        /// <param name="newVacancyViewModel"></param>
        /// <returns></returns>
        public NewVacancyViewModel CreateVacancy(NewVacancyViewModel newVacancyViewModel)
        {
            if (VacancyExists(newVacancyViewModel))
            {
                return UpdateExistingVacancy(newVacancyViewModel);
            }

            _logService.Debug("Creating vacancy reference number");

            try
            {
                var vacancy = CreateNewVacancy(newVacancyViewModel);

                _logService.Debug("Created vacancy with reference number={0}", vacancy.VacancyReferenceNumber);

                return _mapper.Map<Vacancy, NewVacancyViewModel>(vacancy);
            }
            catch (Exception e)
            {
                _logService.Error("Failed to create vacancy", e);
                throw;
            }
        }

        private Vacancy CreateNewVacancy(NewVacancyViewModel newVacancyViewModel)
        {
            var offlineApplicationUrl = !string.IsNullOrEmpty(newVacancyViewModel.OfflineApplicationUrl) ? new UriBuilder(newVacancyViewModel.OfflineApplicationUrl).Uri.ToString() : newVacancyViewModel.OfflineApplicationUrl;
            var vacancyReferenceNumber = _vacancyPostingService.GetNextVacancyReferenceNumber();
            var vacancyParty =
                _providerService.GetVacancyParty(newVacancyViewModel.OwnerParty.VacancyPartyId);
            var employer = _employerService.GetEmployer(vacancyParty.EmployerId);

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
                // VacancyManagerId = vacancyParty.ProviderSiteId //TODO VGA: is this correct?
            });

            return vacancy;
        }
        
        private string GetFrameworkCodeName(TrainingDetailsViewModel trainingDetailsViewModel)
        {
            return trainingDetailsViewModel.TrainingType == TrainingType.Standards ? null : trainingDetailsViewModel.FrameworkCodeName;
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
            return trainingDetailsViewModel.VacancyType == VacancyType.Traineeship ? trainingDetailsViewModel.SectorCodeName : null;
        }

        private NewVacancyViewModel UpdateExistingVacancy(NewVacancyViewModel newVacancyViewModel)
        {
            var offlineApplicationUrl = !string.IsNullOrEmpty(newVacancyViewModel.OfflineApplicationUrl) ? new UriBuilder(newVacancyViewModel.OfflineApplicationUrl).Uri.ToString() : newVacancyViewModel.OfflineApplicationUrl;

            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(newVacancyViewModel.VacancyReferenceNumber.Value);

            vacancy.Title = newVacancyViewModel.Title;
            vacancy.ShortDescription = newVacancyViewModel.ShortDescription;
            vacancy.OfflineVacancy = newVacancyViewModel.OfflineVacancy.Value; // At this point we'll always have a value
            vacancy.OfflineApplicationUrl = offlineApplicationUrl;
            vacancy.OfflineApplicationInstructions = newVacancyViewModel.OfflineApplicationInstructions;
            vacancy.IsEmployerLocationMainApprenticeshipLocation = newVacancyViewModel.IsEmployerLocationMainApprenticeshipLocation;
            vacancy.NumberOfPositions = newVacancyViewModel.NumberOfPositions ?? 0;
            vacancy.VacancyType = newVacancyViewModel.VacancyType;

            vacancy = _vacancyPostingService.UpdateVacancy(vacancy);

            newVacancyViewModel = _mapper.Map<Vacancy, NewVacancyViewModel>(vacancy);

            return newVacancyViewModel;
        }

        private static bool VacancyExists(NewVacancyViewModel newVacancyViewModel)
        {
            return newVacancyViewModel.VacancyReferenceNumber.HasValue && newVacancyViewModel.VacancyReferenceNumber > 0;
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

            return viewModel;
        }

        public FurtherVacancyDetailsViewModel GetVacancySummaryViewModel(int vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);
            var viewModel = vacancy.ConvertToVacancySummaryViewModel();
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
            return viewModel;
        }

        public VacancyRequirementsProspectsViewModel GetVacancyRequirementsProspectsViewModel(int vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);
            var viewModel = vacancy.ConvertToVacancyRequirementsProspectsViewModel();
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
            return viewModel;
        }

        public VacancyQuestionsViewModel GetVacancyQuestionsViewModel(int vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);
            var viewModel = vacancy.ConvertToVacancyQuestionsViewModel();
            return viewModel;
        }

        public VacancyQuestionsViewModel UpdateVacancy(VacancyQuestionsViewModel viewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber);

            vacancy.FirstQuestion = viewModel.FirstQuestion;
            vacancy.SecondQuestion = viewModel.SecondQuestion;

            vacancy = _vacancyPostingService.UpdateVacancy(vacancy);

            viewModel = vacancy.ConvertToVacancyQuestionsViewModel();
            return viewModel;
        }

        public VacancyDatesViewModel UpdateVacancy(VacancyDatesViewModel viewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber);

            vacancy.ClosingDate = viewModel.ClosingDate.Date;
            vacancy.PossibleStartDate = viewModel.PossibleStartDate.Date;

            VacancyDatesViewModel result;

            try
            {
                vacancy = _vacancyPostingService.UpdateVacancy(vacancy);
            }
            catch (CustomException)
            {
                result = _mapper.Map<Vacancy, VacancyDatesViewModel>(vacancy);
                result.State = UpdateVacancyDatesState.InvalidState;
                return result;
            }
            
            result = _mapper.Map<Vacancy, VacancyDatesViewModel>(vacancy);
            result.State = _apprenticeshipApplicationService.GetApplicationCount((int)viewModel.VacancyReferenceNumber) > 0 ? UpdateVacancyDatesState.UpdatedHasApplications : UpdateVacancyDatesState.UpdatedNoApplications;

            return result;
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
            var vacancyParty = _providerService.GetVacancyParty(vacancy.OwnerPartyId);
            var employer = _employerService.GetEmployer(vacancyParty.EmployerId);
            viewModel.NewVacancyViewModel.OwnerParty = vacancyParty.Convert(employer);
            var providerSite = _providerService.GetProviderSite(vacancyParty.ProviderSiteId);
            viewModel.ProviderSite = providerSite.Convert();
            viewModel.FrameworkName = string.IsNullOrEmpty(vacancy.FrameworkCodeName)
                ? vacancy.FrameworkCodeName
                : _referenceDataService.GetSubCategoryByCode(vacancy.FrameworkCodeName).FullName;
            viewModel.SectorName = string.IsNullOrEmpty(vacancy.SectorCodeName)
                ? vacancy.SectorCodeName
                : _referenceDataService.GetCategoryByCode(vacancy.SectorCodeName).FullName;
            var standard = GetStandard(vacancy.StandardId);
            viewModel.StandardName = standard == null ? "" : standard.Name;
            if (viewModel.Status.CanHaveApplicationsOrClickThroughs() && viewModel.NewVacancyViewModel.OfflineVacancy == false)
            {
                //TODO: This information will be returned from _apprenticeshipVacancyReadRepository.GetForProvider or similar once FAA has been migrated
                if (viewModel.VacancyType == VacancyType.Apprenticeship)
                {
                    viewModel.ApplicationCount = _apprenticeshipApplicationService.GetApplicationCount(viewModel.VacancyReferenceNumber);
                }
                else if(viewModel.VacancyType == VacancyType.Traineeship)
                {
                    viewModel.ApplicationCount = _traineeshipApplicationService.GetApplicationCount(viewModel.VacancyReferenceNumber);
                }
            }

            viewModel.NewVacancyViewModel.LocationAddresses =
                _mapper.Map<List<VacancyLocation>, List<VacancyLocationAddressViewModel>>(
                    _vacancyPostingService.GetVacancyLocations(vacancy.VacancyId));

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
            var categories = _referenceDataService.GetCategories();

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

        #region Helpers

        private static string[] GetBlacklistedCategoryCodeNames(IConfigurationService configurationService)
        {
            var blacklistedCategoryCodeNames = configurationService.Get<CommonWebConfiguration>().BlacklistedCategoryCodes;
            
            if (string.IsNullOrWhiteSpace(blacklistedCategoryCodeNames))
            {
                return new string[] {};
            }

            return blacklistedCategoryCodeNames
                .Split(',')
                .Select(each => each.Trim())
                .ToArray();
        }

        #endregion

        public VacanciesSummaryViewModel GetVacanciesSummaryForProvider(int providerId, int providerSiteId,
            VacanciesSummarySearchViewModel vacanciesSummarySearch)
        {
            var isVacancySearch = !string.IsNullOrEmpty(vacanciesSummarySearch.SearchString);
            if (isVacancySearch)
            {
                //When searching the ﬁlters (lottery numbers) are ignored and the search applies to all vacancies
                vacanciesSummarySearch.FilterType = VacanciesSummaryFilterTypes.All;
            }

            //TODO: This filtering, aggregation and pagination should be done in the DAL once we've moved over to SQL Server
            //This means that we will need integration tests covering regression of the filtering and ordering. No point unit testing these at the moment
            var vacancyParties = _providerService.GetVacancyParties(providerSiteId).ToList();
            var employers = _employerService.GetEmployers(vacancyParties.Select(vp => vp.EmployerId));
            var vacancyPartyToEmployerMap = vacancyParties.ToDictionary(vp => vp.VacancyPartyId, vp => employers.Single(e => e.EmployerId == vp.EmployerId));
            var vacancies = _vacancyPostingService.GetByOwnerPartyIds(vacancyParties.Select(vp => vp.VacancyPartyId));
            var hasVacancies = vacancies.Count > 0;
            vacancies = vacancies.Where(v => v.VacancyType == vacanciesSummarySearch.VacancyType || v.VacancyType == VacancyType.Unknown).ToList();

            var live = vacancies.Where(v => v.Status == VacancyStatus.Live).ToList();
            var submitted = vacancies.Where(v => v.Status == VacancyStatus.Submitted || v.Status == VacancyStatus.ReservedForQA).ToList();
            var rejected = vacancies.Where(v => v.Status == VacancyStatus.Referred).ToList();
            //TODO: Agree on closing soon range and make configurable
            var closingSoon = vacancies.Where(v => v.Status == VacancyStatus.Live && v.ClosingDate.HasValue && v.ClosingDate >= _dateTimeService.UtcNow.Date && v.ClosingDate.Value.AddDays(-5) < _dateTimeService.UtcNow).ToList();
            var closed = vacancies.Where(v => v.Status == VacancyStatus.Closed).ToList();
            var draft = vacancies.Where(v => v.Status == VacancyStatus.Draft).ToList();
            var newApplications = vacancies.Where(v => v.Status == VacancyStatus.Live && _apprenticeshipApplicationService.GetNewApplicationCount((int)v.VacancyReferenceNumber) > 0).ToList();
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
                    vacancyViewModel.ApplicationCount = _apprenticeshipApplicationService.GetApplicationCount(vacancyViewModel.VacancyReferenceNumber);
                }
                else if (vacancyViewModel.VacancyType == VacancyType.Traineeship)
                {
                    vacancyViewModel.ApplicationCount = _traineeshipApplicationService.GetApplicationCount(vacancyViewModel.VacancyReferenceNumber);
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
                NewApplicationsCount = newApplications.Count,
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
            var vacancies = _vacancyPostingService.GetWithStatus(VacancyStatus.Submitted, VacancyStatus.ReservedForQA).OrderBy(v => v.DateSubmitted).ToList();

            var utcNow = _dateTimeService.UtcNow;

            var submittedToday = vacancies.Where(v => v.DateSubmitted.HasValue && v.DateSubmitted >= utcNow.Date).ToList();
            var submittedYesterday = vacancies.Where(v => v.DateSubmitted.HasValue && v.DateSubmitted < utcNow.Date && v.DateSubmitted >= utcNow.Date.AddDays(-1)).ToList();
            var submittedMoreThan48Hours = vacancies.Where(v => v.DateSubmitted.HasValue && v.DateSubmitted < utcNow.Date.AddDays(-1)).ToList();
            var resubmitted = vacancies.Where(v => v.SubmissionCount > 1).ToList();

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
                Vacancies = vacancies.Select(ConvertToDashboardVacancySummaryViewModel).ToList()
            };

            return viewModel;
        }

        private DashboardVacancySummaryViewModel ConvertToDashboardVacancySummaryViewModel(VacancySummary vacancy)
        {
            var provider = _providerService.GetProviderViaOwnerParty(vacancy.OwnerPartyId);
            var userName = Thread.CurrentPrincipal.Identity.Name; // TODO: move to service

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

        public void ApproveVacancy(int vacancyReferenceNumber)
        {
            var qaApprovalDate = _dateTimeService.UtcNow;
            var submittedVacancy = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);

            if (submittedVacancy.IsEmployerLocationMainApprenticeshipLocation.HasValue && !submittedVacancy.IsEmployerLocationMainApprenticeshipLocation.Value)
            {
                var vacancyLocationAddresses = _vacancyPostingService.GetVacancyLocations(submittedVacancy.VacancyId);

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

            submittedVacancy.Status = VacancyStatus.Live;
            submittedVacancy.DateQAApproved = qaApprovalDate;
            _vacancyPostingService.UpdateVacancy(submittedVacancy);

        }

        public void RejectVacancy(int vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(vacancyReferenceNumber);
            vacancy.Status = VacancyStatus.Referred;
            vacancy.QAUserName = null;

            _vacancyPostingService.UpdateVacancy(vacancy);
        }

        public VacancyViewModel ReserveVacancyForQA(int vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.ReserveVacancyForQA(vacancyReferenceNumber);
            //TODO: Cope with null, interprit as already reserved etc.
            var viewModel = _mapper.Map<Vacancy, VacancyViewModel>(vacancy);
            //TODO: Get from data layer via joins once we're on SQL
            var provider = _providerService.GetProviderViaOwnerParty(vacancy.OwnerPartyId);
            var vacancyParty = _providerService.GetVacancyParty(vacancy.OwnerPartyId);
            var employer = _employerService.GetEmployer(vacancyParty.EmployerId);
            viewModel.NewVacancyViewModel.OwnerParty = vacancyParty.Convert(employer);
            var providerSite = _providerService.GetProviderSite(vacancyParty.ProviderSiteId);

            var vacancyManager = default(ProviderUser); // TODO: AG: _userProfileService.GetProviderUser(vacancy.VacancyManagerId);

            viewModel.ProviderSite = providerSite.Convert();
            viewModel.FrameworkName = string.IsNullOrEmpty(vacancy.FrameworkCodeName) ? vacancy.FrameworkCodeName : _referenceDataService.GetSubCategoryByCode(vacancy.FrameworkCodeName).FullName;
            viewModel.SectorName = string.IsNullOrEmpty(vacancy.SectorCodeName) ? vacancy.SectorCodeName : _referenceDataService.GetCategoryByCode(vacancy.SectorCodeName).FullName;
            var standard = GetStandard(vacancy.StandardId);
            viewModel.StandardName = standard == null ? "" : standard.Name;
            viewModel.ContactDetailsAndVacancyHistory = ContactDetailsAndVacancyHistoryViewModelConverter.Convert(provider, vacancyManager, vacancy);

            viewModel.NewVacancyViewModel.LocationAddresses =
               _mapper.Map<List<VacancyLocation>, List<VacancyLocationAddressViewModel>>(
                   _vacancyPostingService.GetVacancyLocations(vacancy.VacancyId));

            return viewModel;
        }

        public FurtherVacancyDetailsViewModel UpdateVacancyWithComments(FurtherVacancyDetailsViewModel viewModel)
        {
            // TODO: merge with vacancypostingprovider? -> how we deal with comments. Add them as hidden fields in vacancy posting journey?
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

            vacancy.WageComment = viewModel.WageComment;
            vacancy.ClosingDateComment = viewModel.VacancyDatesViewModel.ClosingDateComment;
            vacancy.DurationComment = viewModel.DurationComment;
            vacancy.LongDescriptionComment = viewModel.LongDescriptionComment;
            vacancy.PossibleStartDateComment = viewModel.VacancyDatesViewModel.PossibleStartDateComment;
            vacancy.WorkingWeekComment = viewModel.WorkingWeekComment;

            vacancy = _vacancyPostingService.UpdateVacancy(vacancy);

            viewModel = vacancy.ConvertToVacancySummaryViewModel();
            return viewModel;
        }

        public NewVacancyViewModel UpdateVacancyWithComments(NewVacancyViewModel viewModel)
        {
            if (!viewModel.VacancyReferenceNumber.HasValue)
                throw new ArgumentNullException("viewModel.VacancyReferenceNumber", "VacancyReferenceNumber required for update");

            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber.Value);

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

            vacancy = _vacancyPostingService.UpdateVacancy(vacancy);

            viewModel = _mapper.Map<Vacancy, NewVacancyViewModel>(vacancy);
            return viewModel;
        }

        public TrainingDetailsViewModel UpdateVacancyWithComments(TrainingDetailsViewModel viewModel)
        {
            if (!viewModel.VacancyReferenceNumber.HasValue)
                throw new ArgumentNullException("viewModel.VacancyReferenceNumber", "VacancyReferenceNumber required for update");

            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber.Value);

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
            return viewModel;
        }

        public NewVacancyViewModel UpdateEmployerInformationWithComments(NewVacancyViewModel viewModel)
        {
            if (!viewModel.VacancyReferenceNumber.HasValue)
                throw new ArgumentNullException("viewModel.VacancyReferenceNumber", "VacancyReferenceNumber required for update");

            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber.Value);

            
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

                //vacancy.LocationAddresses = new List<VacancyLocation>();
                vacancy.LocationAddressesComment = null;
            }
            else
            {
                vacancy.NumberOfPositions = null;
                vacancy.NumberOfPositionsComment = null;
            }
            
            vacancy = _vacancyPostingService.UpdateVacancy(vacancy);

            viewModel = _mapper.Map<Vacancy, NewVacancyViewModel>(vacancy);
            return viewModel;
        }

        public VacancyRequirementsProspectsViewModel UpdateVacancyWithComments(VacancyRequirementsProspectsViewModel viewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber);

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

            vacancy = _vacancyPostingService.UpdateVacancy(vacancy);

            viewModel = vacancy.ConvertToVacancyRequirementsProspectsViewModel();
            return viewModel;
        }

        public VacancyQuestionsViewModel UpdateVacancyWithComments(VacancyQuestionsViewModel viewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber);

            vacancy.FirstQuestion = viewModel.FirstQuestion;
            vacancy.SecondQuestion = viewModel.SecondQuestion;
            vacancy.FirstQuestionComment = viewModel.FirstQuestionComment;
            vacancy.SecondQuestionComment = viewModel.SecondQuestionComment;

            vacancy = _vacancyPostingService.UpdateVacancy(vacancy);

            viewModel = vacancy.ConvertToVacancyQuestionsViewModel();
            return viewModel;
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

            var vacancy = _vacancyPostingService.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber);

            vacancy.IsEmployerLocationMainApprenticeshipLocation =
                viewModel.IsEmployerLocationMainApprenticeshipLocation;
            vacancy.NumberOfPositions = null;
            vacancy.LocationAddressesComment = viewModel.LocationAddressesComment;
            vacancy.AdditionalLocationInformation = viewModel.AdditionalLocationInformation;
            vacancy.AdditionalLocationInformationComment = viewModel.AdditionalLocationInformationComment;

            
            if (addresses.Count() == 1)
            {
                //Set address
                vacancy.Address = addresses.Single().Address;
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
                _vacancyPostingService.SaveVacancy(vacancy);

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

            return _mapper.Map<Vacancy, VacancyDatesViewModel>(vacancy);
        }
    }
}
