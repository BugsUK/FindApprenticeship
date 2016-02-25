/*----------------------------------------------------------------------                  
Name  : ReportGetUnsuccessfulCategory                  
Description :  Returns ID for age ranges 
ID			Description
1			<16
2			16-18
3			19-24
4			25+ 

                
History:                  
--------                  
Date			Version		Author			Comment
26-Aug-2008		1.0			Femma Ashraf	first version
05-Oct-2008		1.1			Ian Emery		removed parameter @type
---------------------------------------------------------------------- */   

CREATE procedure [dbo].[ReportGetUnsuccessfulCatrgory]

as

BEGIN  
	SET NOCOUNT ON  
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
    BEGIN TRY 
SELECT unsuccessfulCat, ID
FROM (
		select 'All' as unsuccessfulCat, -1 as ID, 0 ORD
				union all
				select '1' as unsuccessfulCat, 3 as ID, 1 ORD
				union all
				select '2' as unsuccessfulCat, 1 as ID, 2 ORD
				union all
				select '3' as unsuccessfulCat, 0 as ID, 3 ORD
)a
		ORDER BY ORD

END TRY  
	BEGIN CATCH  
		EXEC RethrowError
	END CATCH  
      
    SET NOCOUNT OFF  
END