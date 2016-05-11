CREATE PROCEDURE [dbo].[ReportGetGeoRegionsIncludingAll]
AS
SET NOCOUNT ON;
SELECT GeographicRegionID, GeoGraphicFullName as GeoRegion
FROM vwRegions
UNION
SELECT -1, N'n/a';
