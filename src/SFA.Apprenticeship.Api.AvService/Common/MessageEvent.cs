namespace SFA.Apprenticeship.Api.AvService.Common
{
    public enum MessageEvent
    {
        VacancyProvisionRelationshipModified = 1,
        VacanyMadeLive = 2,
        VacancyMadeLiveWithEdits = 3,
        VacancyRevised = 4,
        VacancyAboutToClose = 5,
        YouHaveSuccessfullyRegistered = 6,
        VacancyProvisionRelationshipDeleted = 7,
        WelcomeToTheApprenticeshipMatchingSystem = 8,
        NewVacanciesHaveBeenPostedThatMatchYourSearchCriteria = 9,
        ClosingDateForOneofYourVacanciesIsOneWeekAway = 10,
        VacancyDetailsForOneOfYourApplicationsHasBeenChanged = 11,
        SystemMaintenanceMessage = 12,
        BroadcastMessage = 13,
        VacancyTransferToLP = 14,
        VacancyTransferFromLP = 15,
        VacancyTransferToNASRegion = 16,
        VacancyTransferFromNASRegion = 17,
        SubContractorActivation = 18,
        SubContractorDectivation = 19,
        SectorSuccessRateChange = 20,
        ProfiledChanged = 21,
        VacancyTransferToCandidate = 22
    }
}