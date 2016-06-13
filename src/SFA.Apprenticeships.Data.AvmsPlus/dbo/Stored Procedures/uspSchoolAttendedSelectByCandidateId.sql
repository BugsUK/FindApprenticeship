CREATE PROCEDURE [dbo].[uspSchoolAttendedSelectByCandidateId]           
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
		SchoolAttendedId,      
		CandidateId,      
		ApplicationId,     
		[SchoolAttended].[SchoolId],    
		OtherSchoolName,      
		OtherSchoolTown,      
		StartDate,      
		EndDate      
	FROM SchoolAttended      
		LEFT OUTER JOIN School ON [SchoolAttended].[SchoolId] = [School].[SchoolId]
	WHERE CandidateId = @candidateId      
	AND ApplicationId IS NULL

else -- application id exists
		BEGIN
			-- Return rows held against UNSENT applications
			if @applicationStatus = 1

	SELECT       
		SchoolAttendedId,      
		CandidateId,      
		ApplicationId,     
		[SchoolAttended].[SchoolId],    
		OtherSchoolName,      
		OtherSchoolTown,      
		StartDate,      
		EndDate      
	FROM SchoolAttended      
		LEFT OUTER JOIN School ON [SchoolAttended].[SchoolId] = [School].[SchoolId]
	WHERE CandidateId = @candidateId      
	AND ApplicationId IS NULL

else
			-- Return rows held against SENT applications


	SELECT       
		SchoolAttendedId,      
		CandidateId,      
		ApplicationId,     
		[SchoolAttended].[SchoolId],    
		OtherSchoolName,      
		OtherSchoolTown,      
		StartDate,      
		EndDate      
	FROM SchoolAttended      
	LEFT OUTER JOIN School ON [SchoolAttended].[SchoolId] = [School].[SchoolId]
	WHERE CandidateId = @candidateId      
	AND ApplicationId =@applicationId 

END
         
	SET NOCOUNT OFF          
END