CREATE PROCEDURE [dbo].[uspBroadcastMessageSynchronise]
	@CandidateId int 

AS

BEGIN TRY
	BEGIN TRAN
	
		DECLARE @Messages TABLE
		(
			MessageId INT
		)

		INSERT INTO @Messages
		SELECT MessageId
		FROM Message
		WHERE Recipient = 0
		AND MessageId NOT IN 
			(	
				SELECT MessageId 
				FROM CandidateBroadcastMessage 
				WHERE CandidateId = @CandidateId
			)
		AND MessageDate > 
			(
				SELECT TOP 1 EventDate
				FROM CandidateHistory
				WHERE CandidateId = @CandidateId
				AND CandidateHistoryEventTypeId = 1
				AND CandidateHistorySubEventTypeId = 2 --Activated
				ORDER BY EventDate ASC
			)

		IF (SELECT COUNT(MessageId) FROM @Messages) > 0
		BEGIN

			INSERT INTO Message(Sender, SenderType, Recipient, RecipientType, MessageDate, MessageEventId, Text, Title, IsRead, IsDeleted)
			SELECT Sender, SenderType, @CandidateId, RecipientType, MessageDate, MessageEventId, Text, Title, IsRead, IsDeleted
			FROM Message
			WHERE MessageId IN (SELECT MessageId FROM @Messages)

			INSERT INTO CandidateBroadcastMessage(CandidateId, MessageId)
			SELECT @CandidateId, MessageId
			FROM @Messages

		END

	COMMIT TRAN
END TRY

BEGIN CATCH  
	ROLLBACK TRAN
	EXEC RethrowError;  
END CATCH