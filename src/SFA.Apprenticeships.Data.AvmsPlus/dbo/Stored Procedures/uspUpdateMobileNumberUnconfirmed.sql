CREATE PROCEDURE [dbo].[uspUpdateMobileNumberUnconfirmed]
	@CandidateId int, 
	@Flag bit
AS
BEGIN

SET NOCOUNT ON  
   
BEGIN TRY
	UPDATE 
		Candidate
	SET
		MobileNumberUnconfirmed = @Flag
	WHERE
		CandidateId = @CandidateId
       
END TRY  
  
BEGIN CATCH  
    
	EXEC RethrowError;  
  
END CATCH  
      
SET NOCOUNT OFF  

END