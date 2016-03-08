CREATE PROCEDURE [dbo].[uspExternalSystemRemove]
	@SystemID INT
AS
BEGIN
	DELETE FROM
		[dbo].[ExternalSystem]
	WHERE
		ID = @SystemID
END