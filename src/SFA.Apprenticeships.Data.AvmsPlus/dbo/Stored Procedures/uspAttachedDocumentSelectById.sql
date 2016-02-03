CREATE PROCEDURE [dbo].[uspAttachedDocumentSelectById] 
	@attachedDocumentId int
AS
BEGIN

	SET NOCOUNT ON
	
	SELECT
	[attachedDocument].[AttachedDocumentId] AS 'AttachedDocumentId',
	[attachedDocument].[Attachment] AS 'Attachment',
	[attachedDocument].[MIMEType] AS 'MIMEType',
	[attachedDocument].[Title] AS 'Title'
	FROM [dbo].[AttachedDocument] [attachedDocument]
	WHERE [AttachedDocumentId]=@attachedDocumentId

	SET NOCOUNT OFF
END