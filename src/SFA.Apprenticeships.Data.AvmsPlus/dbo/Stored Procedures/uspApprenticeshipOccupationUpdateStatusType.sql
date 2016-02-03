Create Procedure [dbo].[uspApprenticeshipOccupationUpdateStatusType]
	@apprenticeshipOccupationId int,
	@newApprOccpStatusType int,
	@closedDate DateTime
AS
BEGIN
	SET NOCOUNT ON
	
	BEGIN TRY
		UPDATE [dbo].[ApprenticeshipOccupation] 
		SET ApprenticeshipOccupationStatusTypeID = @newApprOccpStatusType, 
			ClosedDate = @closedDate
		WHERE   [ApprenticeshipOccupationId] = @apprenticeshipOccupationId 
	END TRY

    BEGIN CATCH
		EXEC RethrowError;
	END CATCH
    
    SET NOCOUNT OFF
END