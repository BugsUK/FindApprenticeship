CREATE PROCEDURE [dbo].[uspVacancyOfflineApplicantsUpdate]
@VacancyId INT,@NoOfOfflineApplicants INT
AS
BEGIN        
        
 --The [dbo].[Person] table doesn't have a timestamp column. Optimistic concurrency logic cannot be generated        
 SET NOCOUNT ON        
        
 BEGIN TRY        
        
 /*If a field parameter passed to the stored procedure is null then then stored procedure        
    will not change the values already stored in that field.*/        
        
 UPDATE [dbo].[Vacancy]         
 SET         
         
  [NoOfOfflineApplicants ] = @NoOfOfflineApplicants      
     

 WHERE [VacancyId]=@VacancyId        
                
 IF @@ROWCOUNT = 0        
 BEGIN        
  RAISERROR('Concurrent update error. Updated aborted.', 16, 2)        
 END        
    END TRY        
        
    BEGIN CATCH        
  EXEC RethrowError;        
 END CATCH         
        
 SET NOCOUNT OFF        
END