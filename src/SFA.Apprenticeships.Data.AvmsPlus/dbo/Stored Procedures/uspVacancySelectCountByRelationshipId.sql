CREATE PROCEDURE [dbo].[uspVacancySelectCountByRelationshipId]       
 @RelationshipId int,  
 @VacancyManagerId int = 0,
 @Impersonated bit = 0
AS   
  
BEGIN      
      
 SET NOCOUNT ON      
  
 IF(@Impersonated <>0)
 BEGIN
		 select count(VacancyId) as 'VacancyCount'
	
		 from Vacancy v  
		 inner join [VacancyOwnerRelationship] VPR on VPR.[VacancyOwnerRelationshipId] = v.[VacancyOwnerRelationshipId]     
		 INNER JOIN ProviderSite PS ON PS.ProviderSiteId = VPR.ProviderSiteId  
			 LEFT JOIN PROVIDER CO ON v.contractownerid = CO.ProviderID  
		 left outer join   
		  vacancyStatustype on   
		  V.VacancyStatusId = vacancyStatustype.vacancyStatusTypeId                  
		  where VPR.[VacancyOwnerRelationshipId] = @RelationshipId		  
			and VacancyStatusType.FullName != 'Deleted'   
			and VacancyStatusType.FullName != 'Posted In Error'  
		 and (@VacancyManagerId = 0 OR v.VacancyManagerId = @VacancyManagerId)  
	
   END
 ELSE
   BEGIN
      select count(VacancyId) as 'VacancyCount'
		
		 from Vacancy v  
		 inner join [VacancyOwnerRelationship] VPR on VPR.[VacancyOwnerRelationshipId] = v.[VacancyOwnerRelationshipId]     
		 INNER JOIN ProviderSite PS ON PS.ProviderSiteId = VPR.ProviderSiteId  
			 LEFT JOIN PROVIDER CO ON v.contractownerid = CO.ProviderID  
		 left outer join   
		  vacancyStatustype on   
		  V.VacancyStatusId = vacancyStatustype.vacancyStatusTypeId                  
		  where VPR.[VacancyOwnerRelationshipId] = @RelationshipId
		AND (PS.TrainingProviderStatusTypeId != 3 
		 AND (co.ProviderStatusTypeID IS NULL )
		 OR co.ProviderStatusTypeID != 3)		  
			and VacancyStatusType.FullName != 'Deleted'   
			and VacancyStatusType.FullName != 'Posted In Error'  
		 and (@VacancyManagerId = 0 OR v.VacancyManagerId = @VacancyManagerId)  
	
   END
   
  
SET NOCOUNT OFF      
      
END