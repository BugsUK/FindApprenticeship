CREATE PROCEDURE [dbo].[uspApplicationUpdateAppStatus]
    @applicationId int,
	@applicationStatusTypeId int,
	@userName varchar(100),
	@applicationHistoryEventDate datetime,
	@comment varchar(400)
	
AS
BEGIN
	SET NOCOUNT ON
	
	BEGIN TRY
    UPDATE [dbo].[application] SET applicationStatusTypeId = @applicationStatusTypeId WHERE ApplicationId = @applicationId
    
	INSERT INTO [dbo].[ApplicationHistory] ([ApplicationId],[UserName],[ApplicationHistoryEventDate],[ApplicationHistoryEventTypeId],[ApplicationHistoryEventSubTypeId],[Comment])
		VALUES (@applicationId,@userName,getdate(),1,@applicationStatusTypeId,@comment)
    END TRY

    BEGIN CATCH
		EXEC RethrowError;
	END CATCH
    
    SET NOCOUNT OFF
END