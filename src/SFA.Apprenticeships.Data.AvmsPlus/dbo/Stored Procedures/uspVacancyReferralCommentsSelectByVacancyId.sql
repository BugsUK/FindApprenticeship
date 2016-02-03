CREATE PROCEDURE [dbo].[uspVacancyReferralCommentsSelectByVacancyId]           
 @vacancyId int      
AS          
BEGIN          
          
 SET NOCOUNT ON          
          
  select Vacancy.VacancyId, ft.VacancyReferralCommentsFieldTypeId as 'Field', isnull(Comments,'') as 'Comments'          
  from Vacancy          
  inner  join VacancyReferralComments on Vacancy.VacancyId = VacancyReferralComments.VacancyId   
  inner join   
 VacancyReferralCommentsFieldType ft ON   
  ft.VacancyReferralCommentsFieldTypeId = VacancyReferralComments.FieldTypeId  
  where [vacancy].[VacancyId]  = @VacancyId           
            
 SET NOCOUNT OFF          
          
END