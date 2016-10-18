namespace SFA.Apprenticeships.Web.Recruit.Constants
{
    public class RecruitmentRouteNames
    {
        public const string RecruitmentHome = "RecruitmentHome";

        // Account set up
        public const string Settings = "Settings";
        public const string ManageProviderSites = "ManageProviderSites";
        public const string VerifyEmail = "VerifyEmail";
        public const string ResendVerificationCode = "ResendVerificationCode";
        public const string OnBoardingComplete = "OnBoardingComplete";
        public const string DismissReleaseNotes = "DismissReleaseNotes";

        // Authetication
        public const string SignIn = "SignIn";
        public const string SignOut = "SignOut";
        public const string Authorize = "Authorize";
        public const string AuthorizationError = "AuthorizationError";
        public const string SessionTimeout = "SessionTimeout";
        public const string SignOutCallback = "SignOutCallback";

        // Site furniture
        public const string LandingPage = "Landing";
        public const string Privacy = "Privacy";
        public const string TermsAndConditions = "TermsAndConditions";
        public const string ContactUs = "ContactUs";
        public const string WebTrendsOptOut = "WebTrendsOptOut";
        public const string Cookies = "Cookies";

        // Vacancy posting
        public const string CreateVacancy = "CreateVacancy";
        public const string AutoSaveCreateVacancy = "AutoSaveCreateVacancy";
        public const string ReviewCreateVacancy = "ReviewCreateVacancy";
        public const string TrainingDetails = "TrainingDetails";
        public const string AutoSaveTrainingDetails = "AutoSaveTrainingDetails";
        public const string ReviewTrainingDetails = "ReviewTrainingDetails";
        public const string VacancySummary = "VacancySummary";
        public const string AutoSaveVacancySummary = "AutoSaveVacancySummary";
        public const string ReviewVacancySummary = "ReviewVacancySummary";
        public const string VacancyRequirementsProspects = "VacancyRequirementsProspects";
        public const string AutoSaveRequirementsProspects = "AutoSaveRequirementsProspects";
        public const string ReviewVacancyRequirementsProspects = "ReviewVacancyRequirementsProspects";
        public const string VacancyQuestions = "VacancyQuestions";
        public const string AutoSaveVacancyQuestions = "AutoSaveVacancyQuestions";
        public const string ReviewVacancyQuestions = "ReviewVacancyQuestions";
        public const string PreviewVacancy = "PreviewVacancy";
        public const string SubmitVacancy = "SubmitVacancy";
        public const string VacancySubmitted = "VacancySubmitted";
        public const string CloneVacancy = "CloneVacancy";
        public const string DeleteVacancy = "DeleteVacancy";
        public const string ConfirmEmployerSelection = "ConfirmEmployerSelection";
        public const string ConfirmNewEmployerSelection = "ConfirmNewEmployerSelection";
        public const string SearchAddresses = "SearchAddresses";
        public const string AddLocations = "AddLocations";
        public const string AutoSaveLocations = "AutoSaveLocations";
        public const string ShowLocations = "ShowLocations";
        public const string ManageDates = "ManageDates";
        public const string AutoSaveManageDates = "AutoSaveManageDates";
        public const string ArchiveVacancy = "Archive";
        public const string ConfirmArchiveVacancy = "ConfirmArchive";

        // Vacancy posting - existing employer
        public const string SelectExistingEmployer = "SelectExistingEmployer";
        public const string SearchExistingEmployer = "SearchExistingEmployer";
        public const string PageExistingEmployer = "PageExistingEmployer";
        public const string SearchExistingEmployerByErn = "SearchExistingEmployerByErn";
        public const string SearchExistingEmployerByNameAndOrLocation = "SearchExistingEmployerByNameAndOrLocation";
        public const string AddEmployer = "AddEmployer";
        public const string ConfirmEmployer = "ConfirmEmployer";

        // Vacancy posting - new employer
        public const string SelectNewEmployer = "SelectNewEmployer";
        public const string AddNewEmployer = "AddNewEmployer";
        public const string ComfirmNewEmployer = "ConfirmNewEmployer";

        // Applications
        public const string VacancyApplications = "VacancyApplications";
        public const string ShareApplications = "ShareApplications";

        // Apprenticeships
        public const string ReviewApprenticeshipApplication = "ReviewApprenticeshipApplication";
        public const string ConfirmSuccessfulApprenticeshipApplication = "ConfirmSuccessfulApprenticeshipApplication";
        public const string ConfirmUnsuccessfulApprenticeshipApplication = "ConfirmUnsuccessfulApprenticeshipApplication";
        public const string ViewAnonymousApprenticeshipApplication = "ViewAnonymousApprenticeshipApplication";
        public const string ConfirmRevertToInProgress = "ConfirmRevertToInProgress";

        // Traineeships
        public const string ReviewTraineeshipApplication = "ReviewTraineeshipApplication";
        public const string ViewAnonymousTraineeshipApplication = "ViewAnonymousTraineeshipApplication";

        //Reports
        public const string ReportList = "ReportList";
        public const string ReportApplicationsReceived = "ReportApplicationsReceived";
        public const string ReportCandidatesWithApplications = "ReportCandidatesWithApplications";

        //Admin
        public const string AdminList = "AdminList";
        public const string AdminProviderUsers = "AdminProviderUsers";
        public const string AdminViewProviderUser = "AdminViewProviderUser";
        public const string AdminChangeUkprn = "AdminChangeUkprn";
        public const string AdminResetUkprn = "AdminResetUkprn";
        public const string TransferVacancies = "TransferVacancies";
        public const string GetVacancies = "GetVacancies";
        public const string TransferToProvider = "TransferToProvider";

        //Candidates
        public const string CandidateSearch = "CandidateSearch";
        public const string SearchCandidates = "SearchCandidates";
        public const string ViewCandidate = "ViewCandidate";
        public const string SortCandidate = "SortCandidate";

        //Service status
        public static string InformationRadiator = "InformationRadiator";
        public static string ChooseProvider = "ChooseProvider";
        public static string AdminViewProvider = "AdminViewProvider";
        public static string ConfirmVacancies = "ConfirmVacancies";
        public static string SearchProvider = "SearchProvider";
        public static string AdminProviders = "AdminProviders";
        public static string AdminViewProviderSite = "AdminViewProviderSite";
        public static string AdminProviderSites = "AdminProviderSites";
        public static string AdminEditProviderSite = "AdminEditProviderSite";
        public static string ManageVacanciesTransfers = "ManageVacanciesTransfers";
    }
}
