CREATE PROCEDURE [dbo].[uspEducationResultSelectByCandidateId]   
	@candidateId int,
	@applicationId Int
AS
BEGIN  
  
 SET NOCOUNT ON  
   
DECLARE @applicationStatus int
	
	-- get the application status.
	select @applicationStatus = applicationStatusTypeId
	from Application 
	where ApplicationId = @applicationId

	-- Return the rows held against the candidate
If @applicationId = 0

	 SELECT  
		 [educationResult].[CandidateId] AS 'CandidateId',  
		 [educationResult].[ApplicationId] AS 'ApplicationId',  
		 [educationResult].[DateAchieved] AS 'DateAchieved',  
		 [educationResult].[EducationResultId] AS 'EducationResultId',  
		 [educationResult].[Grade] AS 'Grade',  
		 [educationResult].[Level] AS 'Level',  
		 [educationResult].[LevelOther] AS 'LevelOther',  
		 [educationResult].[Subject] AS 'Subject'  
	 FROM [dbo].[EducationResult] [educationResult]  
	 WHERE [CandidateId]=@candidateId  
	 AND ApplicationId IS NULL
	 
	 ORDER BY DateAchieved Desc

else -- application id exists
		BEGIN
			-- Return rows held against UNSENT applications
			if @applicationStatus = 1
	SELECT  
		 [educationResult].[CandidateId] AS 'CandidateId',  
		 [educationResult].[ApplicationId] AS 'ApplicationId',  
		 [educationResult].[DateAchieved] AS 'DateAchieved',  
		 [educationResult].[EducationResultId] AS 'EducationResultId',  
		 [educationResult].[Grade] AS 'Grade',  
		 [educationResult].[Level] AS 'Level',  
		 [educationResult].[LevelOther] AS 'LevelOther',  
		 [educationResult].[Subject] AS 'Subject'  
	 FROM [dbo].[EducationResult] [educationResult]  
	 WHERE [CandidateId]=@candidateId  
	 AND ApplicationId IS NULL 
	 ORDER BY DateAchieved Desc

else
			-- Return rows held against SENT applications

	SELECT  
		 [educationResult].[CandidateId] AS 'CandidateId',  
		 [educationResult].[ApplicationId] AS 'ApplicationId',  
		 [educationResult].[DateAchieved] AS 'DateAchieved',  
		 [educationResult].[EducationResultId] AS 'EducationResultId',  
		 [educationResult].[Grade] AS 'Grade',  
		 [educationResult].[Level] AS 'Level',  
		 [educationResult].[LevelOther] AS 'LevelOther',  
		 [educationResult].[Subject] AS 'Subject'  
	 FROM [dbo].[EducationResult] [educationResult]  
	 WHERE [CandidateId]=@candidateId  
	 AND ApplicationId =@applicationId 
	 ORDER BY DateAchieved Desc

END

 SET NOCOUNT OFF  
END