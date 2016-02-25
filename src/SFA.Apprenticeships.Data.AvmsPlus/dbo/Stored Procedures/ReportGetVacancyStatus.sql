/*----------------------------------------------------------------------                  
Name  : ReportGetVacancyStatus                  
Description :  returns ordered unique vacancy status 

                
History:                  
--------                  
Date			Version		Author			Comment
26-Aug-2008		1.0			Femma Ashraf	first version
---------------------------------------------------------------------- */                 

Create procedure [dbo].[ReportGetVacancyStatus]

as



		BEGIN  
			SET NOCOUNT ON  
			SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
			BEGIN TRY  

	
	SELECT 	-1 AS VacancyStatusTypeId ,'All' FullName,	0 AS ord	
	UNION ALL		
	SELECT 	VacancyStatusTypeId,'Awaiting Approval' FullName, 1 AS ord	FROM dbo.VacancyStatusType WHERE FullName ='Submitted'
	UNION ALL			
	SELECT 	VacancyStatusTypeId,'Requires Rework'FullName, 2 AS ord	FROM dbo.VacancyStatusType WHERE FullName ='Referred'
	UNION ALL			
	SELECT 	VacancyStatusTypeId,'Live' FullName, 3 AS ord	FROM dbo.VacancyStatusType WHERE FullName ='Live'
	UNION ALL			
	SELECT 	VacancyStatusTypeId,'Closing Date Passed' FullName, 4 AS ord	FROM dbo.VacancyStatusType WHERE FullName ='Closed'
	UNION ALL			
	SELECT 	VacancyStatusTypeId,'Completed' FullName, 5 AS ord 	FROM dbo.VacancyStatusType WHERE FullName ='Completed'
	UNION ALL			
	SELECT 	VacancyStatusTypeId,'Withdrawn' FullName, 6 AS ord	FROM dbo.VacancyStatusType WHERE FullName ='Withdrawn'

	
	
	
	
END TRY  
	BEGIN CATCH  
		EXEC RethrowError
	END CATCH  
      
    SET NOCOUNT OFF  
END