/*----------------------------------------------------------------------                  
Name  : ReportGetEmployer                  
Description :  returns ordered unique Employer 

                
History:                  
--------                  
Date			Version		Author			Comment
======================================================================
26-Aug-2008		1.0			Femma Ashraf	first version
03-Sep-2008		1.01		Ian Emery		added All option if the input parameter is blank
03-Sep-2008		1.02		Ian Emery		corrected the TrainingProviderId for All
---------------------------------------------------------------------- */                 

create procedure [dbo].[ReportGetTrainingProvider]
 @trainingProvider varchar(500)
as


set @trainingProvider=ltrim(rtrim(@trainingProvider))

BEGIN  
			SET NOCOUNT ON  
			SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 

		BEGIN TRY  
		if  len(@trainingProvider)=0 
		begin
		
		select 
				-1 as TrainingProviderId ,
				'All' as TradingName
		end

		else
		begin
			SELECT	ProviderSIteID as TrainingProviderId,
					TradingName + ' ('+ Town + ', ' + Postcode + ')' TradingName
					--,FullName
			FROM	dbo.ProviderSite
			WHERE
					TradingName like @trainingProvider+'%'
			ORDER BY TradingName
		end
		END TRY  
		BEGIN CATCH  
				EXEC RethrowError
		END CATCH  
      
    SET NOCOUNT OFF  
END