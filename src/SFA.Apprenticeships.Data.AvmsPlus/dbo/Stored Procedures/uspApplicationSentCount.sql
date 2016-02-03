CREATE PROCEDURE [dbo].[uspApplicationSentCount]   
	@CandidateId int     
AS  
BEGIN  
  
 SET NOCOUNT ON  
   
 Select count(*) as 'ApplicationSent'
	from [Application]
	inner join ApplicationStatusType on  [Application].ApplicationStatusTypeId = ApplicationStatusType.ApplicationStatusTypeId
 where ApplicationStatusType.CodeName in ('NEW','APP','SUC')
	and CandidateId = @CandidateId
  
 SET NOCOUNT OFF  
END