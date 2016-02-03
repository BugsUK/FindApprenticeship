CREATE PROCEDURE [dbo].[uspCandidateEthnicOriginSelectShortNamebyId]       
(  
@EthnicityId int
)  
AS      
BEGIN      
      
 SET NOCOUNT ON      
       
select CandidateEthnicOriginId, FullName, ShortName from CandidateEthnicOrigin  
where  CandidateEthnicOriginId = @EthnicityId  
      
SET NOCOUNT OFF      
      
END