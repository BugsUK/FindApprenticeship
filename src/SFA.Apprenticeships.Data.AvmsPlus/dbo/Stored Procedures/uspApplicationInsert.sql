CREATE PROCEDURE [dbo].[uspApplicationInsert]
	@candidateId int,  
	@vacancyId int,
	@ApplicationStatusTypeId int,
	@ApplicationId int out

AS
BEGIN
	SET NOCOUNT ON
	
	BEGIN TRY

		-- Check for a pre-existing application and if found use that one instead 
		IF EXISTS (
			SELECT 1 FROM [Application] 
			WHERE CandidateId = @candidateId AND VacancyId = @vacancyId AND ApplicationStatusTypeId = @ApplicationStatusTypeId
			)
		BEGIN
			SELECT @applicationId = ApplicationId
			FROM [Application] 
			WHERE CandidateId = @candidateId AND VacancyId = @vacancyId AND ApplicationStatusTypeId = @ApplicationStatusTypeId
		END 		
		-- Check for a application for the same vacancy and but a different status 
		--  if found raise an error
		ELSE IF EXISTS (
			SELECT 1 FROM [Application] 
			WHERE CandidateId = @candidateId AND VacancyId = @vacancyId
			)
		BEGIN
			SET @applicationId = -1
		END 		
		ELSE
		BEGIN
			INSERT INTO [dbo].[Application] 
			([CandidateId],[VacancyId],[ApplicationStatusTypeId])
			VALUES (@candidateId, @vacancyId, @ApplicationStatusTypeId)
	    
			SET @applicationId = SCOPE_IDENTITY()
	    
			insert into applicationhistory(ApplicationId, UserName, ApplicationHistoryEventDate, ApplicationHistoryEventTypeId, ApplicationHistoryEventSubTypeId ,Comment)
			values (@applicationId, NULL, getdate(),1, @ApplicationStatusTypeId, '')
		END


		

	END TRY

    BEGIN CATCH
		EXEC RethrowError;
	END CATCH
    
    SET NOCOUNT OFF
END