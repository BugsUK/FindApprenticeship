CREATE PROCEDURE [dbo].[uspAdditionalAnswerInsert]  
 @AdditionalAnswerId int out,  
 @ApplicationId int,  
 @AdditionalQuestionId int,  
 @Answer nvarchar(4000)   
   
AS  
BEGIN  
 SET NOCOUNT ON  
   
 BEGIN TRY  
	IF EXISTS (
			SELECT 1 
			FROM [AdditionalAnswer] 
			WHERE ApplicationId = @ApplicationId AND AdditionalQuestionId = @AdditionalQuestionId
			)
	BEGIN
		SELECT @AdditionalAnswerId = AdditionalAnswerId 
		FROM [AdditionalAnswer] 
		WHERE ApplicationId = @ApplicationId AND AdditionalQuestionId = @AdditionalQuestionId

	END
	ELSE
	BEGIN
	  INSERT INTO [dbo].[AdditionalAnswer]   
	  ([ApplicationId],[AdditionalQuestionId], [Answer])  
	  VALUES (@ApplicationId, @AdditionalQuestionId, @Answer)  
       
	  SET @AdditionalAnswerId = SCOPE_IDENTITY()  
	END
 END TRY  
  
    BEGIN CATCH  
  EXEC RethrowError;  
 END CATCH  
      
    SET NOCOUNT OFF  
END