CREATE PROCEDURE [dbo].[uspCandidateEthnicOriginSelectFullNamebyShortName]       
(  
@ShortName nvarchar(200)     
)  
AS      
BEGIN      
      
 SET NOCOUNT ON      
       
select CandidateEthnicOriginId, FullName 
from CandidateEthnicOrigin
where FullName = 'Please Select'
and ShortName = @ShortName
union all
select CandidateEthnicOriginId, FullName from CandidateEthnicOrigin  
where  ShortName = @ShortName
and FullName != 'Please Select'
SET NOCOUNT OFF      
      
END