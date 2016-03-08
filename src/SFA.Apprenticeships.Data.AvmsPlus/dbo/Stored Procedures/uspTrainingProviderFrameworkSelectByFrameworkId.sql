Create PROCEDURE [dbo].[uspTrainingProviderFrameworkSelectByFrameworkId]  
	@frameworkId INT  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
 SELECT tpf.ProviderSiteFrameworkID, tpf.ProviderSiteRelationshipID,   
   tpf.FrameworkId, --tpf.TrainingProviderSectorId,   
   af.ApprenticeshipOccupationId, af.ApprenticeshipFrameworkStatusTypeId  ,
   PSR.ProviderID
 FROM [ProviderSiteFramework] tpf
 JOIN  ApprenticeshipFramework af  
 ON tpf.FrameworkId = af.ApprenticeshipFrameworkId     
 JOIN dbo.ProviderSiteRelationship PSR
 ON PSR.ProviderSiteRelationshipID = tpf.ProviderSiteRelationshipID
 Where FrameworkId = @frameworkId    
END