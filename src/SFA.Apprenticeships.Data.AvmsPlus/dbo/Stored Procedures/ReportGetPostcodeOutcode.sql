/*----------------------------------------------------------------------                  
                  
Name  : ReportGetEmployerTradingName                  
Description :  returns ordered unique Employeed trading names   

                
History:                  
--------                  
Date			Version		Author		Comment
20-Aug-2008		1.0			Ian Emery	first version
---------------------------------------------------------------------- */                 

create procedure [dbo].[ReportGetPostcodeOutcode]
as
BEGIN  
	SET NOCOUNT ON  
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
    BEGIN TRY  

		SELECT DISTINCT 
				RTRIM(left(postcode,charindex (' ',postcode)))   as 'Postcode Outcode'
		FROM 
				dbo.Employer emp (nolock)
		ORDER BY 
				RTRIM(left(postcode,charindex (' ',postcode)))

	END TRY  
	BEGIN CATCH  
		EXEC RethrowError
	END CATCH  
      
    SET NOCOUNT OFF  
END