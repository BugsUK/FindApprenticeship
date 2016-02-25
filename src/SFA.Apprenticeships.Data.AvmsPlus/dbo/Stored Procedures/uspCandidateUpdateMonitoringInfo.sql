CREATE PROCEDURE [dbo].[uspCandidateUpdateMonitoringInfo]      
	@candidateId int out,      
	@gender int,      
	@ethnicOrigin int,      
	@ethnicOriginOther nvarchar(50),    
	@disability int,    
	@disabilityOther nvarchar(256),  
	@HealthProblems nvarchar(256)  
AS      
BEGIN      
      
       
 SET NOCOUNT ON      
    
 BEGIN TRY      
     
UPDATE [dbo].[candidate]       
SET     
 [Gender] = @gender,  
 [EthnicOrigin] = @ethnicOrigin,  
 [EthnicOriginOther] = @ethnicOriginOther,  
 [Disability] = @disability,  
 [DisabilityOther] = @disabilityOther,  
 [HealthProblems] = @HealthProblems  
WHERE [CandidateId]=@candidateId  

	Return @candidateId
IF @@ROWCOUNT = 0      
 BEGIN      
  RAISERROR('Concurrent update error. Updated aborted.', 16, 2)      
 END      
END TRY      
      
BEGIN CATCH      
 EXEC RethrowError;      
END CATCH       
      
SET NOCOUNT OFF      
END