CREATE PROCEDURE [dbo].[uspPersonDelete]
	 @personId INT
AS
BEGIN
	SET NOCOUNT ON
	
    DELETE FROM [dbo].[Person]
	WHERE [PersonId]=@personId
    
    SET NOCOUNT OFF
END