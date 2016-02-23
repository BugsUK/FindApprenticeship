create PROCEDURE [dbo].[uspCandidateUpdateActivationValues]  
 @CandidateId as int  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  

--Comment the line because of activaton email ink does not working.
--BEGIN TRY      
  DECLARE @Activated int   
 SET @Activated=2  
  
 UPDATE dbo.Candidate 
 SET   CandidateStatusTypeId=@Activated  
 WHERE CandidateId=@CandidateId  
--IF @@ROWCOUNT = 0      
-- BEGIN      
--  RAISERROR('Concurrent update error. Updated aborted.', 16, 2)      
-- END      
--    END TRY      
--      
--    BEGIN CATCH      
--  EXEC RethrowError;      
-- END CATCH       
END