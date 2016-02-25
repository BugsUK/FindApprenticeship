CREATE PROCEDURE [dbo].[uspApprenticeshipOccupationUpdate]
@occupationId INT, @description nvarchar(200), @closedDate datetime
AS
BEGIN
	SET NOCOUNT ON
	
	BEGIN TRY
		UPDATE [dbo].[ApprenticeshipOccupation] 
		SET [FullName] = @description,
		[ClosedDate] = @closedDate
		WHERE   [ApprenticeshipOccupationId] = @occupationId		
	END TRY

    BEGIN CATCH
		EXEC RethrowError;
	END CATCH
    
    SET NOCOUNT OFF
END