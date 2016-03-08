CREATE PROCEDURE [dbo].[uspSavedSearchCriteriaSelectByCandidateIdAndByName]  
 @candidateId int,   
 @name nvarchar(50)  
AS  
BEGIN  
  
 SET NOCOUNT ON;  
  
    SELECT  
  [SavedSearchCriteriaId]  
  
 FROM [dbo].[SavedSearchCriteria] [savedSearchCriteria]  
  
 WHERE   
   [CandidateId]=@CandidateId AND   
   [Name]=@Name  
END