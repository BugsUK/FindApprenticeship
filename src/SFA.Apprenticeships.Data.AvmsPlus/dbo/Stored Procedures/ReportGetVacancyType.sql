/*----------------------------------------------------------------------                  
Name  : ReportGetVacancyType                  
Description :  returns ordered unique Vacancy Type

                
History:                  
--------                  
Date			Version		Author			Comment
26-Aug-2008		1.0			Femma Ashraf	first version
03-Oct-2008		1.01		Ian Emery		changed description to VacDesc
---------------------------------------------------------------------- */                 

create procedure [dbo].[ReportGetVacancyType]
as

		BEGIN  
			SET NOCOUNT ON  
			SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
			BEGIN TRY  

			select 'All' as VacDesc, -1 as ID
			union all
			select 'Live' as VacDesc, 1 as ID
			union all
			select 'Posted' as VacDesc, 3 as ID
			union all
			select 'None' as VacDesc, 2 as ID
END TRY  
	BEGIN CATCH  
		EXEC RethrowError
	END CATCH  
      
    SET NOCOUNT OFF  
END