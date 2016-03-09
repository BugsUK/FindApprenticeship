CREATE PROCEDURE [dbo].[uspTrainingProviderSelectCountByEmployerId]       
@EmployerId INT      
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;      
      
 SELECT Count(EmployerId) as column1
 FROM dbo.[VacancyOwnerRelationship] VM       
 WHERE EmployerId=@EmployerId      
END