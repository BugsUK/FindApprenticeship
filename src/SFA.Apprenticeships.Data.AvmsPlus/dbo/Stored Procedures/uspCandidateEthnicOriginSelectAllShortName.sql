CREATE PROCEDURE [dbo].[uspCandidateEthnicOriginSelectAllShortName]     
   
AS    
BEGIN    
    
 SET NOCOUNT ON    
     
----select distinct shortName As 'ShortName' from CandidateEthnicOrigin
--select shortname,min(candidateethnicoriginid) as tt from (
--			select distinct shortname,candidateethnicoriginid from dbo.CandidateEthnicOrigin  
--		) as d group by shortname order by tt
--

SELECT ShortName
FROM CandidateEthnicOrigin 
GROUP BY ShortName 
order by Min(CandidateEthnicOriginId)
    
    
SET NOCOUNT OFF    
    
END