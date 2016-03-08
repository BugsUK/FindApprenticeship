CREATE PROCEDURE [dbo].[uspVacancyReferralCommentsDelete]     
 @vacancyId int  
AS    
BEGIN    
 SET NOCOUNT ON    
     
  delete from VacancyReferralComments     
  where VacancyId=@vacancyId    
    
 SET NOCOUNT OFF    
    
END