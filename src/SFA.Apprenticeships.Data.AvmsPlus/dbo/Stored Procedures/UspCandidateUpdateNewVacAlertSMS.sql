CREATE PROCEDURE [dbo].[UspCandidateUpdateNewVacAlertSMS]
	 @CandidateId int ,
	 @NewVacancyAlertSMS bit   
AS  
BEGIN  
 SET NOCOUNT ON  
   
 BEGIN TRY  

		UPDATE 
			CANDIDATE
		SET
		   NewVacancyAlertSMS = @NewVacancyAlertSMS
		WHERE
			CANDIDATEID = @CandidateId
       
  END TRY  
  
    BEGIN CATCH  
    
  EXEC RethrowError;  
  
 END CATCH  
      
    SET NOCOUNT OFF  
END