CREATE PROCEDURE [dbo].[uspAttachedDocumentInsert]
    @attachedDocumentId int OUT,
	@attachment varbinary(8000),
	@mIMEType int,
	@title nvarchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON
	
	BEGIN TRY
    INSERT INTO [dbo].[AttachedDocument] ([Attachment], [MIMEType], [Title])
	VALUES (@attachment, @mIMEType, @title)
    SET @attachedDocumentId = SCOPE_IDENTITY()
    END TRY

    BEGIN CATCH
		EXEC RethrowError;
	END CATCH
    
    SET NOCOUNT OFF
END