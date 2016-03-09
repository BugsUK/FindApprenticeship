CREATE PROCEDURE [dbo].[uspSavedSearchCriteriaDelete]  
  @savedSearchCriteriaId int  
AS  
BEGIN  
 SET NOCOUNT ON  
   
	DELETE FROM [dbo].[SearchFrameworks]
	WHERE [SavedSearchCriteriaId]=@savedSearchCriteriaId

    DELETE FROM [dbo].[SavedSearchCriteria]  
 WHERE [SavedSearchCriteriaId]=@savedSearchCriteriaId   
      
    SET NOCOUNT OFF  
END