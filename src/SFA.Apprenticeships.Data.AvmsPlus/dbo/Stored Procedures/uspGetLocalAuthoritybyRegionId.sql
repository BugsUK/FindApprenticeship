CREATE PROCEDURE [dbo].[uspGetLocalAuthoritybyRegionId]  
    @RegionId int  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
     SET NOCOUNT ON;  
  
          
     SELECT   
        LA.LocalAuthorityId,  
        LA.CodeName,  
        LA.ShortName,  
        LA.FullName,  
        LAG.LocalAuthorityGroupID AS 'GeographicRegionId',
        LA.CountyId   
        from localauthority LA
        INNER JOIN dbo.LocalAuthorityGroupMembership LAGM ON LA.LocalAuthorityId = LAGM.LocalAuthorityID
		INNER JOIN dbo.LocalAuthorityGroup LAG ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID
		INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
		AND LocalAuthorityGroupTypeName = N'Region' 
        where LAG.LocalAuthorityGroupID = @RegionId  OR  LA.[LocalAuthorityId] = 0
		order by case when LA.[FullName] = 'Unspecified' then 0 else 1 end, LA.[FullName]
END