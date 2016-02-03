CREATE PROCEDURE [dbo].[uspVacancyExistLiveForFrameworkStatus]
	@apprFrameworkId INT, 
	@apprFrameworkStatus INT  
AS  
Begin    
 SET NOCOUNT ON        
      
 Declare @vacancyCount int  
 Set @vacancyCount = 1  
 Begin Try    
  Select @vacancyCount = Count(*) from Vacancy v, ApprenticeshipFramework a  
  Where v.ApprenticeshipFrameworkId = a.ApprenticeshipFrameworkId   
  And v.VacancyStatusId In (Select VacancyStatusTypeId from VacancyStatusType Where CodeName In('Cld', 'Lve'))
  And v.ApprenticeshipFrameworkId = @apprFrameworkId  
  And a.ApprenticeshipFrameworkStatusTypeId = @apprFrameworkStatus  
    
 if @vacancyCount = 0   
  return 0  
 else  
  return @vacancyCount  
  
 End Try    
    
 Begin Catch    
  EXEC RethrowError;        
 End Catch    
         
 SET NOCOUNT OFF        
END