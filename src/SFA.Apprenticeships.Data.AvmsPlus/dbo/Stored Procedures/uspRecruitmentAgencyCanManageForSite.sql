CREATE PROCEDURE [dbo].[uspRecruitmentAgencyCanManageForSite]
	@RecruitmentAgencyId int , 
	@ProviderSiteId int 
AS
BEGIN
	declare @cnt as int    
    
    select @cnt = COUNT(*) from Provider P
	JOIN ProviderSiteRelationship R ON R.ProviderId = P.ProviderID
	JOIN ProviderSiteRelationship R2 ON R2.ProviderId = P.ProviderID
	WHERE R.ProviderId = R2.ProviderId
	AND R.ProviderSiteID = @ProviderSiteId
	AND R2.ProviderSiteID = @RecruitmentAgencyId
	AND R.ProviderSiteRelationshipTypeID = 1
	AND R2.ProviderSiteRelationshipTypeID = 3
     
	 
	 if (@cnt > 0)    
  select 1 as Result    
     else    
  select 0 as Result  

END