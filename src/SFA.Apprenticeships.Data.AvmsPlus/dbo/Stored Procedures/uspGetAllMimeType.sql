CREATE PROCEDURE  [dbo].[uspGetAllMimeType]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    
	SELECT * from MIMEType
END