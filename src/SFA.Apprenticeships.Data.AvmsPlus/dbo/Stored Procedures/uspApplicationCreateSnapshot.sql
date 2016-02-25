CREATE PROCEDURE [dbo].[uspApplicationCreateSnapshot]      
	@candidateId int,      
	@applicationId int
AS      
BEGIN      
	SET NOCOUNT ON      
	BEGIN TRY 

	-- Creates a copy in WorkExperience
	Insert Into WorkExperience 
		(CandidateId, ApplicationId, Employer, FromDate, ToDate, TypeOfWork, 
		PartialCompletion, VoluntaryExperience)
		Select @candidateId, @applicationId, Employer, FromDate, ToDate, TypeOfWork, 
			PartialCompletion, VoluntaryExperience 
		From WorkExperience
		Where CandidateId = @candidateId AND ApplicationID IS NULL

	-- Creates a copy in SchoolAttended
	Insert Into SchoolAttended
		(CandidateId, ApplicationId, EndDate, 
				OtherSchoolName, OtherSchoolTown, SchoolId, StartDate)
		Select @candidateId, @applicationId, EndDate, 
				OtherSchoolName, OtherSchoolTown, SchoolId, StartDate 
		From SchoolAttended
		Where CandidateId = @candidateId AND ApplicationID IS NULL

	-- Creates a copy in EducationResult 
	Insert Into EducationResult 
		(CandidateId, ApplicationId, Subject, Level, LevelOther, Grade, DateAchieved)
		Select @candidateId, @applicationId, Subject, Level, LevelOther, Grade, DateAchieved
		From EducationResult
		Where CandidateId = @candidateId AND ApplicationID IS NULL
		
		-- Creates a copy in CAFFields 
	Insert Into CAFFields 
		(CandidateId, ApplicationId, Field, Value)
		Select @candidateId, @applicationId, Field, Value
		From CAFFields
		Where CandidateId = @candidateId AND ApplicationID IS NULL
		

	END TRY      
      
	BEGIN CATCH      
		EXEC RethrowError;      
	END CATCH      
          
	SET NOCOUNT OFF      
END