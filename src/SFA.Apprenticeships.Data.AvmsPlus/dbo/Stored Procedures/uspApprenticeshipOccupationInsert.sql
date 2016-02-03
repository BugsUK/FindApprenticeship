CREATE PROCEDURE [dbo].[uspApprenticeshipOccupationInsert]
	@fullName nvarchar(200),
	@shortName nvarchar(6),
	@apprOccuStatusTypeID int,
	@closedDate DateTime
AS
BEGIN
	SET NOCOUNT ON
	
	BEGIN TRY
		Insert Into [dbo].[ApprenticeshipOccupation] (Codename, ShortName, FullName, ClosedDate, ApprenticeshipOccupationStatusTypeID)
		Values		(@shortName, @shortName, @fullName, @closedDate, @apprOccuStatusTypeID)
	END TRY

    BEGIN CATCH
		EXEC RethrowError;
	END CATCH
    
    SET NOCOUNT OFF
END