Create PROCEDURE [dbo].[uspUniqueKeyRegisterInsert] 
	
	@KeyType nchar(2),
	@KeyValue nvarchar(30),	
	@DateTimeStamp datetime		  
	
AS    
BEGIN    
	SET NOCOUNT ON

	BEGIN TRY
		INSERT INTO [dbo].[UniqueKeyRegister]
			(
				[KeyType],
				[KeyValue],
				[DateTimeStamp]
			)
  
		VALUES 
			(  

				@KeyType,
				@KeyValue,
				@DateTimeStamp
			)	

	END TRY    
	BEGIN CATCH    
		EXEC RethrowError;    
	END CATCH    

	SET NOCOUNT OFF    
END