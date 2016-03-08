/*----------------------------------------------------------------------                  
Name  : ReportGetEmployer                  
Description :  returns ordered unique Employer 

                
History:                  
--------                  
Date			Version		Author			Comment
======================================================================
26-Aug-2008		1.0			Femma Ashraf	first version
03-Sep-2008		1.01		Ian Emery		added All option if the input parameter is blank
---------------------------------------------------------------------- */                 

CREATE procedure [dbo].[ReportGetEmployer]
 @employerName varchar(500)
as


set @employerName=ltrim(rtrim(@employerName))

BEGIN  
			SET NOCOUNT ON  
			SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 

		BEGIN TRY  
		if  len(@employerName)=0 
		begin
		
		select 
				-1 as EmployerId,
				'All' as TradingName
		end

		else
		begin
			SELECT	
					EmployerId,
					TradingName + ' (' + Town + ', ' + Postcode +')' TradingName
			FROM	
					dbo.Employer
			WHERE
					TradingName like @employerName+'%'
			ORDER BY TradingName
		end
		END TRY  
		BEGIN CATCH  
				EXEC RethrowError
		END CATCH  
      
    SET NOCOUNT OFF  
END