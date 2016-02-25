CREATE PROCEDURE [dbo].[uspGetRegionsAndAssociatedLocalAuthorities]
	@TotalNumberOfRegions INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT TOP (100) PERCENT 
			LAG.LocalAuthorityGroupID AS 'GeographicRegionId', 
			LAG.CodeName AS LSCRegionCodeName, 
			LAG.ShortName AS LSCRegionShortName, 
			LAG.FullName AS LSCRegionFullName, 
			LA.LocalAuthorityId, 
			LA.CodeName AS LocalAuthorityCodeName, 
			LA.ShortName AS LocalAuthorityShortName, 
			LA.FullName AS LocalAuthorityFullName, 
			LA.CountyId
			FROM dbo.LocalAuthorityGroup LAG
			 INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID AND LocalAuthorityGroupTypeName = N'Region'
			 INNER JOIN dbo.LocalAuthorityGroupMembership LAGM ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID
			 INNER JOIN dbo.LocalAuthority LA ON LA.LocalAuthorityId = LAGM.LocalAuthorityID				

		 --FROM dbo.LSCRegion INNER JOIN
		 --dbo.LocalAuthority ON dbo.LSCRegion.LSCRegionId = dbo.LocalAuthority.LSCRegionId
		 --WHERE dbo.LSCRegion.Geographic <> 0
		ORDER BY LAG.LocalAuthorityGroupID

	SELECT @TotalNumberOfRegions = COUNT(*)
		FROM  (SELECT DISTINCT TOP (100) PERCENT LAG.LocalAuthorityGroupID
					FROM dbo.LocalAuthorityGroup LAG
					 INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID AND LocalAuthorityGroupTypeName = N'Region'
					 INNER JOIN dbo.LocalAuthorityGroupMembership LAGM ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID
					 INNER JOIN dbo.LocalAuthority LA ON LA.LocalAuthorityId = LAGM.LocalAuthorityID) X
END