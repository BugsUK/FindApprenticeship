/*----------------------------------------------------------------------                  
Name  : ReportGetStartDatePresent                  
Description :  Returns ID for start date present 
ID			Description
0			All
1			With Start Date
2			Without Start Date

                
History:                  
--------                  
Date			Version		Author			Comment
28-Aug-2008		1.0			Femma Ashraf	first version
---------------------------------------------------------------------- */                 

CREATE procedure [dbo].[ReportGetStartDatePresent]
as

BEGIN  
	SET NOCOUNT ON  
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
    BEGIN TRY  	


		select 'All' as startDate, 0 as ID
		union all
		select 'With Start Date' as startDate, 1 as ID
		union all
		select 'Without Start Date' as startDate, 2 as ID
		ORDER BY
		ID
END TRY  
	BEGIN CATCH  
		EXEC RethrowError
	END CATCH  
      
    SET NOCOUNT OFF  
END