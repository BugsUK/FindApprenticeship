CREATE PROCEDURE [dbo].[uspWorkExperienceSelectByCandidateId]  
	@candidateId INT,
	@applicationId Int
AS
BEGIN    
    
	SET NOCOUNT ON    

	DECLARE @applicationStatus int
	
	-- get the application status.
	select @applicationStatus = applicationStatusTypeId
	from application 
	where applicationId = @applicationId

	-- Return the rows held against the candidate
If @applicationId = 0
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
		WHERE [CandidateId]=@candidateId 
		AND ApplicationId IS NULL   
		ORDER BY FromDate Desc  
else -- application id exists
		BEGIN
			-- Return rows held against UNSENT applications
		if @applicationStatus = 1
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
				WHERE [CandidateId]=@candidateId  
				AND ApplicationId IS NULL
				ORDER BY FromDate Desc
		else
			-- Return rows held against SENT applications
			
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
				WHERE [CandidateId]=@candidateId    
				AND ApplicationId =@applicationId
				ORDER BY FromDate Desc  
		END
	
	SET NOCOUNT OFF    
END