CREATE PROCEDURE [dbo].[uspCandidateHistoryInsert]    
	@candidateHistoryEventTypeId int,    
	@candidateHistoryId int output ,    
	@candidateId int,    
	@comment nvarchar(4000) = NULL,    
	@eventDate datetime,    
	@userName nvarchar(50) = NULL,  
	@CandidateHistorySubEventTypeId  int=0  
AS    
BEGIN    
	SET NOCOUNT ON    
     
	 BEGIN TRY    
		INSERT INTO [dbo].[CandidateHistory] ([CandidateHistoryEventTypeId], [CandidateId], [Comment], [EventDate], [UserName],CandidateHistorySubEventTypeId)    
			VALUES (@candidateHistoryEventTypeId, @candidateId, @comment, @eventDate, @userName,@CandidateHistorySubEventTypeId)    
		SET @candidateHistoryId = SCOPE_IDENTITY()    
	END TRY    
    
	BEGIN CATCH    
		EXEC RethrowError;    
	END CATCH    
        
    SET NOCOUNT OFF    
END