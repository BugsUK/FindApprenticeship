CREATE PROCEDURE [dbo].[uspGetApplicationCountByCandidateId]    
 @candidateId int,
 @ApplicationCount int OUT  


AS


BEGIN
	SET NOCOUNT ON
    
    SELECT @ApplicationCount = COUNT(ApplicationId) 
        FROM 
    APPLICATION 
        WHERE 
    CANDIDATEID = @candidateId


	SET NOCOUNT OFF
END