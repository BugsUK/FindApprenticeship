CREATE PROCEDURE [dbo].[uspApplicationSelectByStatusandVacId]
@vacancyId INT, @status INT, @firstName VARCHAR (70)=NULL, @surname VARCHAR (70)=NULL, @candidateId INT=0
AS
BEGIN        
	SET NOCOUNT ON  
  
DECLARE @temp TABLE
(Applicationid int,
[Vacancy Id] int,
[Status Change Date] datetime,
[Application Id] int,
[Fisrt Name] nvarchar(35),
[Middle Name] nvarchar(35),
[SurName] nvarchar(35),
[AllocatedTo] varchar(200),
[ILRDate] datetime, 
[ILRNumber] varchar(200) ,
[UniqueLearnerNumber] bigint,
[CandidateId] int, 
[CandidateStatusTypeId] int, 
[ApplicationStatusTypeId] int

)

		INSERT INTO @temp(Applicationid,[Vacancy Id],[Status Change Date],[Application Id],[Fisrt Name],[Middle Name], [SurName], [AllocatedTo], [ILRDate], [ILRNumber], [UniqueLearnerNumber],[CandidateId], [CandidateStatusTypeId], [ApplicationStatusTypeId] ) 
		 SELECT DISTINCT  
			[Application].Applicationid,      
			[Application].Vacancyid AS 'Vacancy Id',      
			( CASE WHEN @status = 3 THEN -- In Progress
			(SELECT TOP 1 ApplicationHistoryEventDate   
				FROM ApplicationHistory   
				WHERE ApplicationHistory.ApplicationId = Application.ApplicationId 
				AND   ApplicationHistoryEventSubTypeId = 2 --Sent
				ORDER BY ApplicationHistory.ApplicationHistoryEventDate)
			ELSE
			(SELECT TOP 1 ApplicationHistoryEventDate   
				FROM ApplicationHistory   
				WHERE ApplicationHistory.ApplicationId = Application.ApplicationId 
				AND   ApplicationHistoryEventSubTypeId = @status
				ORDER BY ApplicationHistory.ApplicationHistoryEventDate)
			END)
												AS 'Status Change Date',  
			[Application].[ApplicationId]		As 'Application Id',        
			[Person].[FirstName]				As 'Fisrt Name',        
			[Person].[MiddleNames]				As 'Middle Name',        
			[Person].[SurName]					As 'SurName',  
			CASE WHEN @status=4 or @status=5  THEN --WITHDRAWN/--UNSUCCESSFUL
				(CASE WHEN [Application].[OutcomeReasonOther]<>''  THEN 
					(SELECT ART.FullName FROM ApplicationUnsuccessfulReasonType ART
						WHERE ART.ApplicationUnsuccessfulReasonTypeId = [Application].[UnsuccessfulReasonId]) 
							+' - '+ [Application].[OutcomeReasonOther] 
				  ELSE 
					(SELECT ART.FullName FROM ApplicationUnsuccessfulReasonType ART
						WHERE ART.ApplicationUnsuccessfulReasonTypeId = [Application].[UnsuccessfulReasonId])
				 END)

			ELSE
			(CASE WHEN LEN(ISNULL([Application].[AllocatedTo],''))>50 THEN
				substring([Application].[AllocatedTo],0,55) + ' ...'
			 ELSE
				[Application].[AllocatedTo]
			 END)
			END
												As 'AllocatedTo', 
			     
			[SubVacancy].[StartDate]			As 'ILRDate' ,      
			[SubVacancy].[ILRNumber]			As 'ILRNumber',  
			[Candidate].[UniqueLearnerNumber]	As 'UniqueLearnerNumber',  
			[Candidate].[CandidateId]			As 'CandidateId',
			[Candidate].[CandidateStatusTypeId]			As 'CandidateStatusTypeId',
			[Application].ApplicationStatusTypeId As 'ApplicationStatusTypeId'
		FROM [dbo].[Application] 
			LEFT OUTER JOIN ApplicationHistory ON   
				[Application].[ApplicationId] = [ApplicationHistory].[ApplicationId] 
				AND [ApplicationHistory].[ApplicationHistoryEventSubTypeId] = @status        
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
				
		--APPLY SORTING
		IF(@status = 2)
			BEGIN		
				select * from @temp order by [Status Change Date]
			END
		ELSE
			BEGIN
				select * from @temp order by [SurName], [Fisrt Name]
			END
		
	SET NOCOUNT OFF
END