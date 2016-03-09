CREATE PROCEDURE [dbo].[uspTrainingProviderSelectActiveCountByEmployerId]
@EmployerId INT
AS
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;      
 -- Checks for the employers where Relationship exists & is not set to Delete
      
 SELECT Count(EmployerId) as Column1
 FROM dbo.[VacancyOwnerRelationship] VM       
 WHERE EmployerId=@EmployerId    
 AND VM.StatusTypeId != 3
    
END