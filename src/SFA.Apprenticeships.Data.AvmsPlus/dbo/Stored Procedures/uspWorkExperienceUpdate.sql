CREATE PROCEDURE [dbo].[uspWorkExperienceUpdate]  
	@candidateId INT, 
	@employer NVARCHAR (50), 
	@fromDate DATETIME, 
	@toDate DATETIME=NULL, 
	@typeOfWork NVARCHAR (200)=NULL, 
	@PartialCompletion int,
	@VoluntaryExperience int,
	@workExperienceId INT
AS  
BEGIN    
    
	SET NOCOUNT ON    
    
	BEGIN TRY    
		UPDATE [dbo].[WorkExperience]     
		SET [Employer] = @employer, 
			[FromDate] = @fromDate, 
			[ToDate] = @toDate, 
			[TypeOfWork] = @typeOfWork,
			[PartialCompletion] = @PartialCompletion, 
			[VoluntaryExperience] = @VoluntaryExperience
		WHERE [WorkExperienceId]=@workExperienceId    

	IF @@ROWCOUNT = 0    
		BEGIN    
			RAISERROR('Concurrent update error. Updated aborted.', 16, 2)    
		END    
	END TRY    

	BEGIN CATCH    
		EXEC RethrowError;    
	END CATCH     
    
	SET NOCOUNT OFF    
END