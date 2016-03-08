--Updates the ReminderMessageSent value of the candidates Table. 
CREATE PROCEDURE [dbo].[uspUpdateReminderStatus]
	@CandidateID int, 
	@ReminderStatus bit
AS
	BEGIN
		UPDATE Candidate
		SET ReminderMessageSent = @ReminderStatus
		WHERE CandidateID = @CandidateID
	END