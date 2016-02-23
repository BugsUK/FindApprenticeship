CREATE PROCEDURE [dbo].[uspGeographicRegionSelectByLocalAuthorityId]

	@LocalAuthId int
AS

BEGIN

	SET NOCOUNT ON;
	  
  SELECT 	 vwLA.GeographicRegionID 'GeographicRegionID',
			 vwLA.GeographicCodeName 'GeographicCodeName',
			 vwLA.GeographicShortName 'GeographicShortName',
			 vwLA.GeographicFullName 'GeographicFullName'
  FROM vwRegionsAndLocalAuthority vwLA
  where vwLA.LocalAuthorityId = @LocalAuthId

END