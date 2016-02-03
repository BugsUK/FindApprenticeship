CREATE PROCEDURE [dbo].[uspAttachedDocumentDelete]
	 @attachedDocumentId int
AS
BEGIN
	SET NOCOUNT ON
	
    DELETE FROM [dbo].[AttachedDocument]
	WHERE [AttachedDocumentId]=@attachedDocumentId
    
    SET NOCOUNT OFF
END