/*----------------------------------------------------------------------                  
Name  : ReportGetVacancyTitle                  
Description :  Returns vacancy title 
                
History:                  
--------                  
Date			Version		Author			Comment
26-Aug-2008		1.0			Femma Ashraf	first version
05-Oct-2008		1.1			Ian Emery		removed parameter @type
---------------------------------------------------------------------- */   

create procedure [dbo].[ReportGetVacancyTitle]

as

BEGIN  
	SET NOCOUNT ON  
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
    BEGIN TRY 

SELECT DISTINCT title,
				VacancyId
FROM Vacancy
ORDER BY title

END TRY  
	BEGIN CATCH  
		EXEC RethrowError
	END CATCH  
      
    SET NOCOUNT OFF  
END