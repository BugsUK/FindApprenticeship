CREATE PROCEDURE [dbo].[uspSubVacancySelectByVacancyIdAndApplicationId]
 @vacancyId int,      
 @applicationId int 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT       
	[SubVacancy].[SubVacancyId] as 'SubVacancyId' ,    
	[SubVacancy].[StartDate] as 'ILRDate' ,    
	--[SubVacancy].[ILRNumber] as 'ILRNumber',
	[Candidate].[UniqueLearnerNumber] as 'UniqueLearnerNumber'    
      
 FROM     
	[SubVacancy]
 INNER JOIN [Application] ON [Application].[ApplicationId] = [SubVacancy].[AllocatedApplicationId]
 INNER JOIN [Candidate] ON [Application].[CandidateId] = [Candidate].[CandidateId]
 WHERE 
	[SubVacancy].[VacancyId] = @vacancyId and      
	[SubVacancy].[AllocatedApplicationId] = @applicationId  
 
SET NOCOUNT OFF      
 
END