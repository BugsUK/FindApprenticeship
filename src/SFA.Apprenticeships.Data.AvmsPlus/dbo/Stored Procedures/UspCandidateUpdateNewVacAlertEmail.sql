CREATE PROCEDURE [dbo].[UspCandidateUpdateNewVacAlertEmail]   
	 @CandidateId int ,
	 @NewVacancyAlertEmail bit   
AS  
BEGIN  
 SET NOCOUNT ON  
   
 BEGIN TRY  

		UPDATE 
			CANDIDATE
		SET
		   NewVacancyAlertEmail = @NewVacancyAlertEmail
		WHERE
			CANDIDATEID = @CandidateId
       
  END TRY  
  
    BEGIN CATCH  
    
  EXEC RethrowError;  
  
 END CATCH  
      
    SET NOCOUNT OFF  
END