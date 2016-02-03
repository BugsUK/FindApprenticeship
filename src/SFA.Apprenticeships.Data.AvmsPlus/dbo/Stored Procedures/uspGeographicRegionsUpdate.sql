CREATE PROCEDURE [dbo].[uspGeographicRegionsUpdate]
@RegionId INT, @FullName NVARCHAR (200)
AS
BEGIN
	UPDATE dbo.LocalAuthorityGroup
	SET FullName = @FullName
	WHERE LocalAuthorityGroupID = @RegionId
	
END