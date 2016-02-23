CREATE PROCEDURE [dbo].[uspSystemParametersUpdate]
@SystemParametersId INT, @SystemParameterValue NVARCHAR (600)
AS
BEGIN
	SET NOCOUNT ON
	
	BEGIN TRY 
	
		UPDATE SystemParameters
		SET ParameterValue = @SystemParameterValue
		WHERE SystemParametersId = @SystemParametersId
	
	
	END TRY      
      
    BEGIN CATCH      
		EXEC RethrowError;      
	END CATCH       
      
	
	SET NOCOUNT OFF
END