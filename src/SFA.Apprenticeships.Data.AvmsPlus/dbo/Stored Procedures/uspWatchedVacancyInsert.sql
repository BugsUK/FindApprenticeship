CREATE PROCEDURE [dbo].[uspWatchedVacancyInsert]  
    @expiryDate datetime = NULL,  -- TODO: Remove this field
 @candidateId int,  
 @status nvarchar(20) = NULL,  --TODO: Remove this field
 @vacancyId int  
AS  
BEGIN  
  SET NOCOUNT ON  
  BEGIN TRY  
   UPDATE [dbo].[WatchedVacancy]   
   SET [CandidateId] = @candidateId, [VacancyId] = @vacancyId  
   WHERE [VacancyId] = @vacancyId AND [CandidateId] = @candidateId   
  
   IF @@ROWCOUNT = 0  
   BEGIN  
   INSERT INTO [dbo].[WatchedVacancy] ([CandidateId], [VacancyId])  
   VALUES (@candidateId, @vacancyId)  
   END  
  END TRY  
  
  BEGIN CATCH  
   EXEC RethrowError;  
  END CATCH  
  SET NOCOUNT OFF  
END