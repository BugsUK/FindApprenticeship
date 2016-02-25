CREATE PROCEDURE [dbo].[uspCandidateUpdateReferralPoints]
	-- Add the parameters for the stored procedure here
	@rejectedReason int, 
	@candidateId int
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRY
		UPDATE candidate 
			SET referralPoints = ISNULL(referralPoints,0) + 
					(SELECT referralPoints FROM dbo.ApplicationUnsuccessfulReasonType 
						WHERE ApplicationUnsuccessfulReasonTypeId=@rejectedReason) 
			WHERE candidateId=@candidateId

		IF @@ROWCOUNT = 0
		BEGIN
			RAISERROR('Concurrent update error. Updated aborted.', 16, 2)
		END
    END TRY

    BEGIN CATCH
		EXEC RethrowError;
	END CATCH	
END