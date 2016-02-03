CREATE PROCEDURE [dbo].[UspCandidateSelectAlertPreferences]   
 @CandidateId int  
   
AS  
BEGIN  
 SET NOCOUNT ON  
   
 BEGIN TRY  
		SELECT
			  C.CandidateId
			  ,P.PersonId
			  ,P.Email
			  ,C.UnconfirmedEmailAddress
			  ,P.MobileNumber
			  ,C.MobileNumberUnconfirmed
			  ,C.NewVacancyAlertEmail
			  ,C.NewVacancyAlertSMS
		FROM 
			Candidate C
		INNER JOIN Person P on P.PersonId = C.PersonId
		WHERE
			C.CandidateId = @CandidateId
       
  END TRY  
  
    BEGIN CATCH  
  EXEC RethrowError;  
 END CATCH  
      
    SET NOCOUNT OFF  
END