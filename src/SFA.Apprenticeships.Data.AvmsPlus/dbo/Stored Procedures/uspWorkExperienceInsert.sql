CREATE PROCEDURE [dbo].[uspWorkExperienceInsert]    
	@candidateId int,    
	@employer nvarchar(50),    
	@fromDate datetime,    
	@toDate datetime = NULL,    
	@typeOfWork nvarchar(200) = NULL,    
	@PartialCompletion int,
	@VoluntaryExperience int,
	@workExperienceId int OUT
AS    
BEGIN    
 SET NOCOUNT ON    
     
 BEGIN TRY    
	INSERT INTO [dbo].[WorkExperience] 
		([CandidateId], [Employer], [FromDate], [ToDate], 
		[TypeOfWork], [PartialCompletion], [VoluntaryExperience])    
	VALUES 
		(@candidateId, @employer, @fromDate, @toDate, 
		@typeOfWork, @PartialCompletion, @VoluntaryExperience)    
        
	SET @workExperienceId = SCOPE_IDENTITY()    
END TRY    
    
 BEGIN CATCH    
	EXEC RethrowError;    
 END CATCH    
        
 SET NOCOUNT OFF    
END