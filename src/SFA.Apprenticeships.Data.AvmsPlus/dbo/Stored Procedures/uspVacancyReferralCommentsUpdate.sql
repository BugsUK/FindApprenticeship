CREATE PROCEDURE [dbo].[uspVacancyReferralCommentsUpdate]         
 @vacancyId int,        
 @FieldId int,        
 @comments varchar(8000)         
AS        
BEGIN        
 SET NOCOUNT ON        
    
--  Declare @FieldId int  
--  Select @FieldId = VacancyReferralCommentsFieldTypeId from VacancyReferralCommentsFieldType where FullName = @Field  
  
 declare @VacancyReferralComments int      
 if exists(select 1 from VacancyReferralComments where VacancyId = @vacancyId and FieldTypeId = @FieldId)        
 Begin        
  delete from  VacancyReferralComments         
  where VacancyId = @vacancyId and FieldTypeId = @FieldId        
 End        
         
-- SET @VacancyReferralComments = NEWID()        
        
 insert into VacancyReferralComments        
 (        
  VacancyId,FieldTypeId,Comments        
 )        
 Values        
 (        
  @vacancyId,@FieldId,@comments        
 )         
        
SET NOCOUNT OFF        
        
END