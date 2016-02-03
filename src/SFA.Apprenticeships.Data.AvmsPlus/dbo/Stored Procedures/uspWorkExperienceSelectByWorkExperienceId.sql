CREATE PROCEDURE [dbo].[uspWorkExperienceSelectByWorkExperienceId]  
	@workExperienceId INT  
AS  
BEGIN    
    
	SET NOCOUNT ON    
     
	SELECT    
		[workExperience].[CandidateId] AS 'CandidateId',    
		[workExperience].[ApplicationId] AS 'ApplicationId',      	
		[workExperience].[Employer] AS 'Employer',    
		[workExperience].[FromDate] AS 'FromDate',    
		[workExperience].[ToDate] AS 'ToDate',    
		[workExperience].[TypeOfWork] AS 'TypeOfWork',    
		[workExperience].[PartialCompletion] AS 'PartialCompletion',
		[workExperience].[VoluntaryExperience] AS 'VoluntaryExperience',
		[workExperience].[WorkExperienceId] AS 'WorkExperienceId'    
	FROM [dbo].[WorkExperience] [workExperience]    
	WHERE [WorkExperienceId]=@workExperienceId
	AND ApplicationId IS NULL    
    
	SET NOCOUNT OFF    
END