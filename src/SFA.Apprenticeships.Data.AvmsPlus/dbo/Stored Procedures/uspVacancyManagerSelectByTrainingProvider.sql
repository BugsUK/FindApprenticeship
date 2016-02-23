CREATE PROCEDURE [dbo].[uspVacancyManagerSelectByTrainingProvider]
 @TrainingProviderId INT
AS
BEGIN                  
SET NOCOUNT ON                  
           
select * from dbo.[VacancyOwnerRelationship] where [ProviderSiteID]=@TrainingProviderId and StatusTypeId =4
 SET NOCOUNT OFF                  

END