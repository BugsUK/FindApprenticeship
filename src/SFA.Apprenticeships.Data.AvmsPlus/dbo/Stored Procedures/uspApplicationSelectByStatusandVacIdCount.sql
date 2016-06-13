CREATE PROCEDURE [dbo].[uspApplicationSelectByStatusandVacIdCount]
@vacancyId INT, 
@status INT, 
@firstName VARCHAR (70)=NULL, 
@surname VARCHAR (70)=NULL, 
@candidateId INT=0
AS
BEGIN        
		SET NOCOUNT ON  
 
		
		SELECT count(DISTINCT  
			[Application].ApplicationId) as [Count]
		FROM [dbo].[Application] 
			LEFT OUTER JOIN ApplicationHistory ON   
				[Application].[ApplicationId] = [ApplicationHistory].[ApplicationId] 
				AND @status =  [ApplicationHistory].[ApplicationHistoryEventSubTypeId] 
		   INNER JOIN Candidate ON   
				[Application].[CandidateId] = [Candidate].[CandidateId]        
		   INNER JOIN Person ON   
				[Candidate].[PersonId] = [Person].[PersonId]        
		   LEFT OUTER JOIN SubVacancy ON   
				[SubVacancy].[VacancyId] =[Application].[VacancyId] 
				AND [SubVacancy].[AllocatedApplicationId] = [Application].[ApplicationId]  
		WHERE	[Application].[VacancyId] = @vacancyId 
				AND ApplicationHistoryEventSubTypeId = @status 
				AND [Application].ApplicationStatusTypeId = @status  
				AND (ISNULL(@firstName,'') = '' OR [Person].[FirstName] like '%' + @firstName + '%')
				AND (ISNULL(@surname,'') = '' OR [Person].[Surname] like '%' + @surname + '%')
				AND (ISNULL(@candidateId,0) = 0 OR [Candidate].CandidateId = @candidateId)				

		
	SET NOCOUNT OFF
END