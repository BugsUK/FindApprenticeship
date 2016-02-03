CREATE PROCEDURE [dbo].[uspGetManagingAreaAndAssociatedLocalAuthority]
	@TotalNumberOfRegions INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   SELECT 
			LAG.LocalAuthorityGroupID AS 'ManagingAreaId', 
			LAG.CodeName AS ManagingAreaCodeName, 
			LAG.ShortName AS ManagingAreaShortName, 
			LAG.FullName AS ManagingAreaFullName, 
			LA.LocalAuthorityId, 
			LA.CodeName AS LocalAuthorityCodeName, 
			LA.ShortName AS LocalAuthorityShortName, 
			LA.FullName AS LocalAuthorityFullName, 
			LA.CountyId
			FROM dbo.LocalAuthorityGroup LAG
			 INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID AND LocalAuthorityGroupTypeName = N'Managing Area'
			 INNER JOIN dbo.LocalAuthorityGroupMembership LAGM ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID
			 INNER JOIN dbo.LocalAuthority LA ON LA.LocalAuthorityId = LAGM.LocalAuthorityID				

		ORDER BY LA.LocalAuthorityId

	SELECT @TotalNumberOfRegions = COUNT(*)
		FROM  (SELECT DISTINCT TOP (100) PERCENT LAG.LocalAuthorityGroupID
					FROM dbo.LocalAuthorityGroup LAG
					 INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID AND LocalAuthorityGroupTypeName = N'Managing Area'
					 INNER JOIN dbo.LocalAuthorityGroupMembership LAGM ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID
					 INNER JOIN dbo.LocalAuthority LA ON LA.LocalAuthorityId = LAGM.LocalAuthorityID) X
END