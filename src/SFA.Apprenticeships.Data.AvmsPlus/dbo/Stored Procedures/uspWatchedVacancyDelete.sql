CREATE PROCEDURE [dbo].[uspWatchedVacancyDelete]  
  @personId int,  
 @vacancyId int  
AS  
BEGIN  
 SET NOCOUNT ON  
   
    DELETE FROM [dbo].[WatchedVacancy]  
 WHERE [CandidateId]=@personId AND [VacancyId]=@vacancyId  
      
    SET NOCOUNT OFF  
END