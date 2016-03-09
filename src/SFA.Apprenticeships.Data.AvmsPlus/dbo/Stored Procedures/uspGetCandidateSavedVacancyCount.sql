create PROCEDURE [dbo].[uspGetCandidateSavedVacancyCount]    
 @candidateId int,
 @candidateCount int OUT  


AS


BEGIN
	SET NOCOUNT ON
    
    SELECT @candidateCount = COUNT(WATCHEDVACANCYID) 
        FROM 
    WATCHEDVACANCY wv
	inner join vacancy v on wv.vacancyid = v.vacancyid
        WHERE 
    CANDIDATEID = @candidateId 
	and datediff(dd,v.ApplicationClosingDate , getdate()) <= 30
	AND wv.VacancyId NOT IN
	(SELECT vacancyid FROM [Application] INNER JOIN ApplicationStatusType ON 
	[Application].ApplicationStatusTypeId = ApplicationStatusType.ApplicationStatusTypeId
	WHERE CandidateId=@candidateId AND ApplicationStatusType.FullName IN ('New','Applied','Successful'))


	SET NOCOUNT OFF
END