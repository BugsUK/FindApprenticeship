namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Application.Interfaces.Communications;
    using SFA.Infrastructure.Interfaces;
    using Common.Configuration;
    using Common.Providers;
    using Common.Services;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Users;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Users;
    using Constants.Pages;
    using ViewModels;
    using ViewModels.Candidate;
    using ViewModels.Home;
    using ViewModels.Login;
    using ViewModels.Register;
    using Common.Constants;
    using Domain.Entities.Communication;
    using Helpers;
    using Infrastructure.Presentation;
    using Mappers;

    using SFA.Apprenticeships.Application.Interfaces;

    using ViewModels.Account;
    using ViewModels.VacancySearch;
    using CandidateErrorCodes = Application.Interfaces.Candidates.ErrorCodes;
    using UserErrorCodes = Application.Interfaces.Users.ErrorCodes;

    public class CandidateServiceProvider : ICandidateServiceProvider
    {
        private readonly ILogService _logger;
        private readonly ICandidateService _candidateService;
        private readonly IConfigurationService _configurationService;
        private readonly IMapper _mapper;
        private readonly IUserAccountService _userAccountService;
        private readonly IUserDataProvider _userDataProvider;
        private readonly IAuthenticationTicketService _authenticationTicketService;

        public CandidateServiceProvider(
            ICandidateService candidateService,
            IUserAccountService userAccountService,
            IUserDataProvider userDataProvider,
            IAuthenticationTicketService authenticationTicketService,
            IMapper mapper,
            IConfigurationService configurationService, ILogService logger)
        {
            _candidateService = candidateService;
            _userAccountService = userAccountService;
            _userDataProvider = userDataProvider;
            _authenticationTicketService = authenticationTicketService;
            _mapper = mapper;
            _configurationService = configurationService;
            _logger = logger;
        }

        public UserNameAvailability IsUsernameAvailable(string username)
        {
            _logger.Debug("Calling CandidateServiceProvider to if the username {0} is available.", username);

            var userNameAvailability = new UserNameAvailability();

            try
            {
                userNameAvailability.IsUserNameAvailable = _userAccountService.IsUsernameAvailable(username);
            }
            catch (Exception ex)
            {
                const string errorMessage = "Error checking user name availability for {0}";
                var message = string.Format(errorMessage, username);
                _logger.Error(message, ex);

                userNameAvailability.HasError = true;
                userNameAvailability.ErrorMessage = errorMessage;
            }

            return userNameAvailability;
        }

        public ApplicationStatuses? GetApplicationStatus(Guid candidateId, int vacancyId)
        {
            _logger.Debug(
                "Calling CandidateServiceProvider to get the application status for CandidateID={0}, VacancyId={1}.",
                candidateId, vacancyId);

            try
            {
                var application = _candidateService.GetApprenticeshipApplications(candidateId)
                    .SingleOrDefault(a => a.LegacyVacancyId == vacancyId);

                if (application == null)
                {
                    return null;
                }

                return application.Status;
            }
            catch (Exception e)
            {
                var message = string.Format("Get Application status failed for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                _logger.Error(message, e);
                throw;
            }
        }

        public bool Register(RegisterViewModel model)
        {
            _logger.Debug("Calling CandidateServiceProvider to register a new candidate");

            try
            {
                var candidate = _mapper.Map<RegisterViewModel, Candidate>(model);
                candidate.RegistrationDetails.AcceptedTermsAndConditionsVersion = _configurationService.Get<CommonWebConfiguration>().TermsAndConditionsVersion;

                _candidateService.Register(candidate, model.Password);
                _authenticationTicketService.SetAuthenticationCookie(candidate.EntityId.ToString(), UserRoleNames.Unactivated);

                return true;
            }
            catch (CustomException e)
            {
                var message = string.Format("Candidate registration failed for {0}.", model.EmailAddress);

                if (e.Code == Domain.Entities.ErrorCodes.EntityStateError)
                {
                    _logger.Info(message, e);
                }
                else
                {
                    _logger.Error(message, e);
                }
                return false;
            }
            catch (Exception e)
            {
                _logger.Error("Candidate registration failed for " + model.EmailAddress, e);
                return false;
            }
        }

        public ActivationViewModel Activate(ActivationViewModel model, Guid candidateId)
        {
            _logger.Info(
                "Calling CandidateServiceProvider to activate user with Id={0}",
                candidateId);

            try
            {
                var user = _userAccountService.GetUser(candidateId);
                if (user.Status == UserStatuses.Active)
                {
                    _authenticationTicketService.SetAuthenticationCookie(candidateId.ToString(), UserRoleNames.Activated);

                    return new ActivationViewModel(model.EmailAddress, model.ActivationCode, ActivateUserState.AlreadyActivated);
                }

                var candidate = _candidateService.GetCandidate(candidateId);
                if (!String.Equals(candidate.RegistrationDetails.EmailAddress, model.EmailAddress, StringComparison.CurrentCultureIgnoreCase))
                {
                    _logger.Warn("Candidate email address {0} does not match supplied email address {1}", candidate.RegistrationDetails.EmailAddress, model.EmailAddress);
                }

                _candidateService.Activate(candidateId, model.ActivationCode);
                _authenticationTicketService.SetAuthenticationCookie(candidateId.ToString(), UserRoleNames.Activated);

                return new ActivationViewModel(model.EmailAddress, model.ActivationCode, ActivateUserState.Activated);
            }
            catch (CustomException e)
            {
                switch (e.Code)
                {
                    case Domain.Entities.ErrorCodes.EntityStateError:
                        _logger.Error("Candidate was in an invalid state for activation:" + model.EmailAddress, e);
                        return new ActivationViewModel(model.EmailAddress, model.ActivationCode, ActivateUserState.Error, ActivationPageMessages.ActivationFailed);

                    default:
                        _logger.Info("Candidate activation failed for " + model.EmailAddress, e);
                        return new ActivationViewModel(model.EmailAddress, model.ActivationCode, ActivateUserState.InvalidCode, ActivationPageMessages.ActivationCodeIncorrect);
                }
            }
            catch (Exception e)
            {
                _logger.Error("Candidate activation failed for " + model.EmailAddress, e);
                return new ActivationViewModel(model.EmailAddress, model.ActivationCode, ActivateUserState.Error, ActivationPageMessages.ActivationFailed);
            }
        }

        public LoginResultViewModel Login(LoginViewModel model)
        {
            _logger.Debug("Calling CandidateServiceProvider to log the user {0}",
                model.EmailAddress);

            try
            {
                var user = _userAccountService.GetUser(model.EmailAddress, false);

                if (user == null || user.Status == UserStatuses.PendingDeletion)
                {
                    return GetAuthenticationFailedViewModel(model);
                }

                if (user.Status == UserStatuses.Locked)
                {
                    return GetLoginResultViewModel(model, user.Status);
                }

                var candidate = _candidateService.Authenticate(model.EmailAddress, model.Password);

                if (candidate != null)
                {
                    // User is authentic.
                    _authenticationTicketService.SetAuthenticationCookie(candidate.EntityId.ToString(), _userAccountService.GetRoleNames(candidate.EntityId));

                    _userDataProvider.Push(UserDataItemNames.LastApplicationStatusNotification,
                        user.LastLogin.HasValue ? user.LastLogin.Value.Ticks.ToString(CultureInfo.InvariantCulture) : DateTime.UtcNow.Ticks.ToString(CultureInfo.InvariantCulture));

                    return new LoginResultViewModel
                    {
                        EmailAddress = candidate.RegistrationDetails.EmailAddress,
                        FullName = candidate.RegistrationDetails.FirstName + " " + candidate.RegistrationDetails.LastName,
                        UserStatus = user.Status,
                        IsAuthenticated = true,
                        AcceptedTermsAndConditionsVersion = candidate.RegistrationDetails.AcceptedTermsAndConditionsVersion,
                        PhoneNumber = candidate.RegistrationDetails.PhoneNumber,
                        MobileVerificationRequired = candidate.MobileVerificationRequired(),
                        PendingUsernameVerificationRequired = user.PendingUsernameVerificationRequired()
                    };
                }

                user = _userAccountService.GetUser(model.EmailAddress);

                if (user.Status == UserStatuses.Locked)
                {
                    // Authentication failed, user just locked their account.
                    return GetLoginResultViewModel(model, user.Status);
                }

                return GetAuthenticationFailedViewModel(model, user.Status);
            }
            catch (Exception e)
            {
                _logger.Error("Candidate login failed for " + model.EmailAddress, e);

                return new LoginResultViewModel(LoginPageMessages.LoginFailedErrorText)
                {
                    EmailAddress = model.EmailAddress
                };
            }
        }

        public bool RequestForgottenPasswordResetCode(ForgottenPasswordViewModel model)
        {
            _logger.Debug("Calling CandidateServiceProvider to request a password reset code for user {0}",
                model.EmailAddress);

            try
            {
                _userAccountService.SendPasswordResetCode(model.EmailAddress);

                return true;
            }
            catch (CustomException e)
            {
                switch (e.Code)
                {
                    case Domain.Entities.ErrorCodes.EntityStateError:
                    case Application.Interfaces.Users.ErrorCodes.UnknownUserError:
                        _logger.Info(e.Message, e);
                        break;
                    default:
                        _logger.Error(e.Message, e);
                        break;
                }

                return false;
            }
            catch (Exception e)
            {
                _logger.Error("Send password reset code failed for " + model.EmailAddress, e);

                return false;
            }
        }

        public AccountUnlockViewModel RequestAccountUnlockCode(AccountUnlockViewModel model)
        {
            _logger.Debug("Calling CandidateServiceProvider to request an account unlock code for user {0}",
                model.EmailAddress);

            try
            {
                _userAccountService.ResendAccountUnlockCode(model.EmailAddress);
                return new AccountUnlockViewModel { EmailAddress = model.EmailAddress };
            }
            catch (CustomException e)
            {
                AccountUnlockState status;
                switch (e.Code)
                {
                    case Domain.Entities.ErrorCodes.EntityStateError:
                        status = AccountUnlockState.UserInIncorrectState;
                        _logger.Info(e.Message, e);
                        break;
                    case Application.Interfaces.Users.ErrorCodes.UnknownUserError:
                        status = AccountUnlockState.AccountEmailAddressOrUnlockCodeInvalid;
                        _logger.Info(e.Message, e);
                        break;
                    default:
                        status = AccountUnlockState.Error;
                        _logger.Error(e.Message, e);
                        break;
                }
                return new AccountUnlockViewModel(e.Message) { EmailAddress = model.EmailAddress, Status = status };
            }
            catch (Exception e)
            {
                var message = string.Format("Send account unlock code failed for " + model.EmailAddress);
                _logger.Error(message, e);
                return new AccountUnlockViewModel(message) { EmailAddress = model.EmailAddress, Status = AccountUnlockState.Error };
            }
        }

        public PasswordResetViewModel VerifyPasswordReset(PasswordResetViewModel passwordResetViewModel)
        {
            _logger.Debug("Calling CandidateServiceProvider to verify password reset for user {0}",
                passwordResetViewModel.EmailAddress);

            passwordResetViewModel.IsPasswordResetCodeValid = false;

            try
            {
                _candidateService.ResetForgottenPassword(passwordResetViewModel.EmailAddress, passwordResetViewModel.PasswordResetCode, passwordResetViewModel.Password);

                passwordResetViewModel.IsPasswordResetCodeValid = true;
                passwordResetViewModel.UserStatus = UserStatuses.Active;
            }
            catch (CustomException e)
            {
                _logger.Info("Reset forgotten password failed for " + passwordResetViewModel.EmailAddress, e);

                switch (e.Code)
                {
                    case UserErrorCodes.UnknownUserError:
                    case Domain.Entities.ErrorCodes.EntityStateError:
                    case Application.Interfaces.Users.ErrorCodes.UserPasswordResetCodeExpiredError:
                    case Application.Interfaces.Users.ErrorCodes.UserPasswordResetCodeIsInvalid:
                        passwordResetViewModel.IsPasswordResetCodeValid = false;
                        break;

                    case Application.Interfaces.Users.ErrorCodes.UserAccountLockedError:
                        passwordResetViewModel.UserStatus = UserStatuses.Locked;
                        break;

                    default:
                        passwordResetViewModel.ViewModelMessage = PasswordResetPageMessages.FailedPasswordReset;
                        break;
                }
            }
            catch (Exception e)
            {
                _logger.Error("Reset forgotten password failed for " + passwordResetViewModel.EmailAddress, e);

                passwordResetViewModel.ViewModelMessage = PasswordResetPageMessages.FailedPasswordReset;
            }

            return passwordResetViewModel;
        }

        public AccountUnlockViewModel VerifyAccountUnlockCode(AccountUnlockViewModel model)
        {
            _logger.Debug("Calling CandidateServiceProvider to verify account unlock code for user {0}",
                model.EmailAddress);

            try
            {
                _candidateService.UnlockAccount(model.EmailAddress, model.AccountUnlockCode);
                return new AccountUnlockViewModel { Status = AccountUnlockState.Ok };
            }
            catch (CustomException e)
            {
                switch (e.Code)
                {
                    case Domain.Entities.ErrorCodes.EntityStateError:
                        _logger.Info(e.Message, e);
                        return new AccountUnlockViewModel { Status = AccountUnlockState.UserInIncorrectState };
                    case Application.Interfaces.Users.ErrorCodes.AccountUnlockCodeExpired:
                        _logger.Info(e.Message, e);
                        return new AccountUnlockViewModel { Status = AccountUnlockState.AccountUnlockCodeExpired };
                    case Application.Interfaces.Users.ErrorCodes.AccountUnlockCodeInvalid:
                    case Application.Interfaces.Users.ErrorCodes.UnknownUserError:
                        _logger.Info(e.Message, e);
                        return new AccountUnlockViewModel
                        {
                            Status = AccountUnlockState.AccountEmailAddressOrUnlockCodeInvalid
                        };
                    default:
                        _logger.Error(e.Message, e);
                        return new AccountUnlockViewModel { Status = AccountUnlockState.Error };
                }
            }
            catch (Exception e)
            {
                _logger.Error("Account unlock failed for " + model.EmailAddress, e);
                return new AccountUnlockViewModel { Status = AccountUnlockState.Error };
            }
        }

        public bool ResendActivationCode(string username)
        {
            _logger.Debug("Calling CandidateServiceProvider to request activation code for user {0}.", username);

            try
            {
                _userAccountService.ResendActivationCode(username);
                return true;
            }
            catch (CustomException e)
            {
                _logger.Info("Reset activation code failed for " + username, e);
                return false;
            }
            catch (Exception e)
            {
                _logger.Error("Reset activation code failed for " + username, e);
                return false;
            }
        }

        public Candidate GetCandidate(string username)
        {
            _logger.Debug("Calling CandidateServiceProvider to get Candidate for user {0}.", username);

            try
            {
                return _candidateService.GetCandidate(username);
            }
            catch (Exception e)
            {
                var message = string.Format("GetCandidate for user {0} failed.", username);
                _logger.Error(message, e);
                throw;
            }
        }

        public Candidate GetCandidate(Guid candidateId)
        {
            try
            {
                return _candidateService.GetCandidate(candidateId);
            }
            catch (Exception e)
            {
                var message = string.Format("GetCandidate for user with Id={0} failed.", candidateId);
                _logger.Error(message, e);
                throw;
            }
        }

        public bool AcceptTermsAndConditions(Guid candidateId, string currentVersion)
        {
            try
            {
                var candidate = _candidateService.GetCandidate(candidateId);
                candidate.RegistrationDetails.AcceptedTermsAndConditionsVersion = currentVersion;
                _candidateService.SaveCandidate(candidate);
                _userDataProvider.SetUserContext(candidate.RegistrationDetails.EmailAddress, candidate.RegistrationDetails.FirstName + " " + candidate.RegistrationDetails.LastName, currentVersion);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("Error updating terms and conditions version", ex);
                return false;
            }
        }

        public bool SendContactMessage(Guid? candidateId, ContactMessageViewModel viewModel)
        {
            try
            {
                var candidate = _mapper.Map<ContactMessageViewModel, ContactMessage>(viewModel);

                candidate.UserId = candidateId;
                _candidateService.SubmitContactMessage(candidate);
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SendFeedback(Guid? candidateId, FeedbackViewModel viewModel)
        {
            try
            {
                var candidate = _mapper.Map<FeedbackViewModel, ContactMessage>(viewModel);
                
                candidate.UserId = candidateId;
                _candidateService.SubmitContactMessage(candidate);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<ApprenticeshipApplicationSummary> GetApprenticeshipApplications(Guid candidateId, bool refresh = true)
        {
            return _candidateService.GetApprenticeshipApplications(candidateId, refresh);
        }

        public ApprenticeshipSearchViewModel CreateSavedSearch(Guid candidateId, ApprenticeshipSearchViewModel viewModel)
        {
            var categoryFullName = default(string);
            var subCategoriesFullName = default(string);
            string[] subCategories = null;
            if (!string.IsNullOrEmpty(viewModel.Category))
            {
                var category = viewModel.Categories.SingleOrDefault(c => c.CodeName == viewModel.Category);
                if (category != null)
                {
                    categoryFullName = FullNameFormatter.Format(category.FullName);

                    if (viewModel.SubCategories != null && viewModel.SubCategories.Length > 0)
                    {
                        var selectedSubCategories = category.SubCategories.Where(sc => viewModel.SubCategories.Contains(sc.CodeName)).ToList();
                        subCategories = selectedSubCategories.Select(sc => sc.CodeName).ToArray();
                        var subCategoryFullNames = selectedSubCategories.Select(sc => FullNameFormatter.Format(sc.FullName));
                        subCategoriesFullName = string.Join("|", subCategoryFullNames);
                    }
                }
            }

            var savedSearch = new SavedSearch
            {
                CandidateId = candidateId,
                SearchMode = viewModel.SearchMode,
                Keywords = viewModel.Keywords,
                Location = viewModel.Location,
                Latitude = viewModel.Latitude,
                Longitude = viewModel.Longitude,
                Hash = viewModel.Hash,
                WithinDistance = viewModel.WithinDistance,
                ApprenticeshipLevel = viewModel.ApprenticeshipLevel,
                Category = viewModel.Category,
                CategoryFullName = categoryFullName,
                SubCategories = subCategories,
                SubCategoriesFullName = subCategoriesFullName,
                SearchField = viewModel.SearchField,
                DisplaySubCategory = viewModel.DisplaySubCategory,
                DisplayDescription = viewModel.DisplayDescription,
                DisplayDistance = viewModel.DisplayDistance,
                DisplayClosingDate = viewModel.DisplayClosingDate,
                DisplayStartDate = viewModel.DisplayStartDate,
                DisplayApprenticeshipLevel = viewModel.DisplayApprenticeshipLevel,
                DisplayWage = viewModel.DisplayWage
            };

            try
            {
                var candidate = _candidateService.GetCandidate(candidateId);

                var searchHash = savedSearch.GetSearchHash();
                var existingSavedSearches = _candidateService.GetSavedSearches(candidateId);

                if (existingSavedSearches == null || !existingSavedSearches.Select(s => s.GetSearchHash()).Contains(searchHash))
                {
                    _candidateService.CreateSavedSearch(savedSearch);
                }

                var saveCandidate = false;

                if (!candidate.CommunicationPreferences.SavedSearchPreferences.EnableEmail && !candidate.CommunicationPreferences.SavedSearchPreferences.EnableText)
                {
                    candidate.CommunicationPreferences.SavedSearchPreferences.EnableEmail = true;
                    saveCandidate = true;
                }

                if (saveCandidate)
                {
                    _candidateService.SaveCandidate(candidate);
                    if (candidate.MobileVerificationRequired())
                    {
                        _candidateService.SendMobileVerificationCode(candidate);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error creating saved search", ex);
                viewModel.ViewModelMessage = VacancySearchResultsPageMessages.SaveSearchFailed;
            }

            return viewModel;
        }

        public SavedSearchViewModel DeleteSavedSearch(Guid candidateId, Guid savedSearchId)
        {
            try
            {
                var deletedSavedSearch = _candidateService.DeleteSavedSearch(candidateId, savedSearchId);

                return deletedSavedSearch.ToViewModel(_configurationService.Get<CommonWebConfiguration>().SubCategoriesFullNamesLimit);
            }
            catch (Exception ex)
            {
                _logger.Error("Error deleting saved search", ex);
                return new SavedSearchViewModel { ViewModelMessage = AccountPageMessages.DeleteSavedSearchFailed };
            }
        }

        public IEnumerable<SavedSearchViewModel> GetSavedSearches(Guid candidateId)
        {
            try
            {
                return _candidateService.GetSavedSearches(candidateId)
                    .Select(each => each.ToViewModel(_configurationService.Get<CommonWebConfiguration>().SubCategoriesFullNamesLimit));
            }
            catch (Exception ex)
            {
                _logger.Error("Error retrieving saved searches for candidate id='{0}'", ex, candidateId);
                return null;
            }
        }

        public SavedSearchViewModel GetSavedSearch(Guid candidateId, Guid savedSearchId)
        {
            try
            {
                var savedSearch = _candidateService.GetSavedSearch(candidateId, savedSearchId);

                return savedSearch == null ? null : savedSearch.ToViewModel(_configurationService.Get<CommonWebConfiguration>().SubCategoriesFullNamesLimit);
            }
            catch (Exception ex)
            {
                _logger.Error("Error retrieving saved search id='{0}'", ex, savedSearchId);

                return new SavedSearchViewModel
                {
                    ViewModelMessage = ApprenticeshipsSearchPageMessages.RunSavedSearchFailed
                };
            }
        }

        public bool Unsubscribe(Guid subscriberId, SubscriptionTypes subscriptionType)
        {
            return _candidateService.Unsubscribe(subscriberId, subscriptionType);
        }

        public void UpdateMonitoringInformation(Guid candidateId, MonitoringInformationViewModel monitoringInformationViewModel)
        {
            try
            {
                var monitoringInformation = _mapper.Map<MonitoringInformationViewModel, MonitoringInformation>(monitoringInformationViewModel);
                var candidate = _candidateService.GetCandidate(candidateId);

                candidate.MonitoringInformation = monitoringInformation;
                candidate.ApplicationTemplate.AboutYou.Support = monitoringInformationViewModel.AnythingWeCanDoToSupportYourInterview;

                _candidateService.SaveCandidate(candidate);
            }
            catch (Exception ex)
            {
                _logger.Error("Error updating monitoring information", ex);
            }
        }

        public string SendMobileVerificationCode(Guid candidateId)
        {
            var candidate = _candidateService.GetCandidate(candidateId);
            _candidateService.SendMobileVerificationCode(candidate);
            return candidate.RegistrationDetails.PhoneNumber;
        }

        public bool RequestEmailReminder(ForgottenEmailViewModel forgottenEmailViewModel)
        {
            try
            {
                _candidateService.RequestEmailReminder(forgottenEmailViewModel.PhoneNumber);

                return true;
            }
            catch (CustomException e)
            {
                switch (e.Code)
                {
                    case Domain.Entities.ErrorCodes.EntityStateError:
                    case Application.Interfaces.Candidates.ErrorCodes.CandidateNotFoundError:
                        _logger.Info(e.Message, e);
                        break;
                    default:
                        _logger.Error(e.Message, e);
                        break;
                }

                return false;
            }
            catch (Exception e)
            {
                _logger.Error("Send email reminder failed for " + forgottenEmailViewModel.PhoneNumber, e);

                return false;
            }
        }

        #region Helper


        private static LoginResultViewModel GetLoginResultViewModel(LoginViewModel model, UserStatuses userStatus)
        {
            return new LoginResultViewModel
            {
                EmailAddress = model.EmailAddress,
                UserStatus = userStatus
            };
        }

        private static LoginResultViewModel GetAuthenticationFailedViewModel(LoginViewModel model, UserStatuses? userStatus = null)
        {
            return new LoginResultViewModel(LoginPageMessages.InvalidUsernameAndOrPasswordErrorText)
            {
                EmailAddress = model.EmailAddress,
                UserStatus = userStatus
            };
        }

        #endregion
    }
}