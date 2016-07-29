CREATE PROCEDURE [dbo].[ReportGetGeoRegionsIncludingAll]
AS
SET NOCOUNT ON;
SELECT GeographicRegionID, GeographicFullName as GeoRegion
FROM vwRegions
UNION
SELECT -1, N'n/a';
