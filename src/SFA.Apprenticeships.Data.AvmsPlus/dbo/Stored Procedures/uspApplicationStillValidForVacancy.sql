CREATE	PROCEDURE [dbo].[uspApplicationStillValidForVacancy]
	@applicationId int
AS
BEGIN  
 SET NOCOUNT ON  

	BEGIN TRY      
		Begin    
			Declare @secondsElapsedSinceExpiry int     
			Select @secondsElapsedSinceExpiry = DATEDIFF(day, ApplicationClosingDate, getDate())  
			From Vacancy    
			Where Vacancyid =     
			(Select Vacancyid    
			From Application     
			Where Applicationid = @applicationId)    
		   And VacancyStatusId = (Select VacancyStatusTypeId From VacancyStatusType    
				  Where CodeName = 'Lve')    
  
			If @secondsElapsedSinceExpiry > 0    
				return 0  -- False  
			Else    
				return 1  -- True  
		End    
    END TRY
  
    BEGIN CATCH  
	EXEC RethrowError;  
		END CATCH  
	SET NOCOUNT OFF  
END