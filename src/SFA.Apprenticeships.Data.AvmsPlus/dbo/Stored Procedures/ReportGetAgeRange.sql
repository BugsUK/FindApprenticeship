/*----------------------------------------------------------------------                  
Name  : ReportGetAgeRange                  
Description :  Returns ID for age ranges 
ID			Description
1			<16
2			16-18
3			Up to 20
4			19-24
5			25+ 

                
History:                  
--------                  
Date			Version		Author				Comment
26-Aug-2008		1.0			Femma Ashraf		first version
03-Sep-2008		1.01		Ian Emery			Added all option
09-Oct-2008		1.02		Juandre Germishuys	Added "up to 20" as value
12-Nov-2008		1.03		Ian Emery			Changed the names of the ages descriptions
---------------------------------------------------------------------- */   

CREATE procedure [dbo].[ReportGetAgeRange]
as

BEGIN  
	SET NOCOUNT ON  
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
    BEGIN TRY
		Select 'All' as ageDesc, -1 as ID
		union all
		select 'Up to 16' as ageDesc, 1 as ID
		union all
		select '16 - 18' as ageDesc, 2 as ID
		union all
		select 'Up to 20' as ageDesc, 3 as ID
		union all
		select '19 - 24' as ageDesc, 4 as ID
		union all
		select '25 +' as ageDesc, 5 as ID

END TRY  
	BEGIN CATCH  
		EXEC RethrowError
	END CATCH  
      
    SET NOCOUNT OFF  
END