CREATE PROCEDURE [dbo].[uspGetCandidatesForAlertSetupPrompt]
AS
	BEGIN
	SET NOCOUNT ON;
		SELECT Candidate.CandidateId, Candidate.AllowMarketingMessages FROM Candidate 
		LEFT JOIN CandidateHistory History ON Candidate.CandidateId = History.CandidateID
		LEFT JOIN SavedSearchCriteria SavedSearch ON Candidate.CandidateID = SavedSearch.CandidateID
		WHERE Candidate.CandidateStatusTypeID = 2 AND (Candidate.NewVacancyAlertEmail = 0 OR Candidate.NewVacancyAlertEmail IS NULL)
		AND (Candidate.NewVacancyAlertSMS = 0 OR Candidate.NewVacancyAlertSMS IS NULL) AND Candidate.ReminderMessageSent = 0 
		AND History.CandidateHistoryEventTypeID = 1 AND History.CandidateHistorySubEventTypeId = 2
		AND DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())) > DATEADD(dd, 0, DATEDIFF(dd, -7, EventDate)) 
		AND SavedSearch.SavedSearchCriteriaId IS NULL 
		ORDER BY Candidate.CandidateId DESC
	END