/*----------------------------------------------------------------------                  
Name  : ReportGetEmployerTradingName                  
Description :  returns ordered unique Employeed trading names   

                
History:                  
--------                  
Date			Version		Author		Comment
20-Aug-2008		1.0			Ian Emery	first version
08-Aug-2008		1.01		Ian Emery   allowed all and filtering on employername
---------------------------------------------------------------------- */                 

CREATE procedure [dbo].[ReportGetEmployerTradingName]
 @EmployerName varchar(500)
as


set @EmployerName=ltrim(rtrim(@EmployerName))



BEGIN  
			SET NOCOUNT ON  
			SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 

		BEGIN TRY  
		if  len(@EmployerName)=0 
		begin
		
		select 
				-1 as EmployerId,
				'All' as TradingName
		end

		else
		begin
				SELECT 
					EmployerId,
					TradingName  + ' (' + Town + ', ' + Postcode +')' TradingName
			FROM 
					Employer 
			Where
					TradingName like @EmployerName+'%'
			order by 
					TradingName
		end
		END TRY  
		BEGIN CATCH  
				EXEC RethrowError
		END CATCH  
      
    SET NOCOUNT OFF  
END