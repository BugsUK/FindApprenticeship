CREATE PROCEDURE [dbo].[uspApplicationSelectByCandidateAndStatus]       
 @applicationStatusTypeId int,      
 @candidateId int      
AS      
BEGIN      
      
 SET NOCOUNT ON      
       
 SELECT      
  a.[ApplicationId] AS 'ApplicationId',      
  a.[ApplicationStatusTypeId] AS 'ApplicationStatusTypeId',      
  a.[CandidateId] AS 'CandidateId',      
  null AS 'UniqueApplicationReference',      
  Ah.[ApplicationHistoryEventDate] AS 'ApplicationHistoryEventDate',      
Case a.UnsuccessfulReasonId  
   When 0  Then ah.comment   
   else ur.CandidateDisplayText  
 End as 'Comment',  
 Case a.UnsuccessfulReasonId  
   When 0  Then NULL
   else ur.FullName  
 End as 'CommentHeadline',  

  a.[VacancyId] AS 'VacancyId',      
  v.[Title] AS 'VacancyTitle',      
  v.ApplicationClosingDate AS 'ApplicationClosingDate',
  v.VacancyStatusId AS 'VacancyStatus' -- Added by Manish 9/nov/08     
 FROM       
  ApplicationHistory ah right outer join application a       
   on a.applicationid = ah.applicationid       
  inner join vacancy v      
   on a.vacancyid = v.vacancyid      
  inner join ApplicationUnsuccessfulReasonType ur  
   on a.UnsuccessfulReasonId = ur.ApplicationUnsuccessfulReasonTypeId  
 WHERE       
 a.[ApplicationStatusTypeId]=@applicationStatusTypeId      
 AND a.[CandidateId]=@candidateId      
 and ah.[ApplicationHistoryEventDate] = (select max([ApplicationHistoryEventDate])       
           from [ApplicationHistory] t2       
           where t2.ApplicationId = ah.ApplicationId)       
 order by ah.[ApplicationHistoryEventDate] DESC      
      
 SET NOCOUNT OFF      
END