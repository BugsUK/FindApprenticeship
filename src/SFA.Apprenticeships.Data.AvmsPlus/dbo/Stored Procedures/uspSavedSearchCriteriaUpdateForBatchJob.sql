Create PROCEDURE [dbo].[uspSavedSearchCriteriaUpdateForBatchJob]        
 @alertSent bit = NULL,        
 @backgroundSearch bit = NULL,        
 @countBackgroundMatches int = NULL,        
 @dateSearched datetime = NULL,        
 @savedSearchCriteriaId int = null       
 
AS        
BEGIN        
        
 --The [dbo].[SavedSearchCriteria] table doesn't have a timestamp column. Optimistic concurrency logic cannot be generated        
 SET NOCOUNT ON        
        
 --BEGIN TRY        
 --if @originalName = NULL         
 --BEGIN        
 -- SET @OriginalName = @name        
 --END        
 UPDATE [dbo].[SavedSearchCriteria]         

 SET         
  [AlertSent] = @alertSent,         
  [BackgroundSearch] = @backgroundSearch,         
  [CountBackgroundMatches] = @countBackgroundMatches,         
  [DateSearched] = @dateSearched       

 WHERE         
 [SavedSearchCriteriaId]=@savedSearchCriteriaId        
  
 IF @@ROWCOUNT = 0        
 BEGIN        
  RAISERROR('Concurrent update error. Updated aborted.', 16, 2)        
 END          
              
 SET NOCOUNT OFF        
END