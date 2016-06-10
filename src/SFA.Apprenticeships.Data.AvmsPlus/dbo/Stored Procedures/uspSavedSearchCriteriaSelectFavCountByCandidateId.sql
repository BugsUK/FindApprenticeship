CREATE PROCEDURE [dbo].[uspSavedSearchCriteriaSelectFavCountByCandidateId]  
 -- Add the parameters for the stored procedure here  
 @candidateId int  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    SELECT COUNT(SavedSearchCriteriaId) AS [Count] FROM dbo.SavedSearchCriteria   
 WHERE CandidateId=@candidateId AND BackgroundSearch=1  
END