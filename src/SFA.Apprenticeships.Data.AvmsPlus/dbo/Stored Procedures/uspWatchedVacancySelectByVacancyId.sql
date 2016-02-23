CREATE PROCEDURE [dbo].[uspWatchedVacancySelectByVacancyId]     
 @vacancyId int    
AS    
BEGIN    
    
 SET NOCOUNT ON    
    
   
	SELECT
		w.WatchedVacancyId,
		w.CandidateId,
		w.VacancyId	
	FROM 
		dbo.WatchedVacancy w
	WHERE
		w.VacancyId =@vacancyId

 SET NOCOUNT OFF    
END