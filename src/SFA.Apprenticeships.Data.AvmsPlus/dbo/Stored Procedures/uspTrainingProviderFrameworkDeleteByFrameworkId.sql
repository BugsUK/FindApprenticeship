CREATE PROCEDURE [dbo].[uspTrainingProviderFrameworkDeleteByFrameworkId]
	@frameworkId int
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
 Delete from [ProviderSiteFramework]  
 WHERE FrameworkId = @frameworkId  
END