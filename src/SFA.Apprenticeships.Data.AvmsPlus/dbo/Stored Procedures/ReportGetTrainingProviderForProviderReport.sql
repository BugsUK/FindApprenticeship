/*----------------------------------------------------------------------                  
Name  : ReportGetTrainingProviderForProviderReport                  
Description :  returns ordered unique Employer 

                
History:                  
--------                  
Date			Version		Author			Comment
======================================================================
26-Aug-2008		1.0			Femma Ashraf	first version
03-Sep-2008		1.01		Ian Emery		added All option if the input parameter is blank
03-Sep-2008		1.02		Ian Emery		corrected the TrainingProviderId for All
---------------------------------------------------------------------- */                 

create procedure [dbo].[ReportGetTrainingProviderForProviderReport]
 @trainingProvider varchar(500),
 @TypeID INT,
 @ID INT
as


---- ****************** debug start **********************************
--DECLARE	@trainingProvider varchar(500),
--		@TypeID INT,
--		@ID INT
-- 
-- SET @trainingProvider= 'a'
-- SET @typeid =2   -- id=2 Tp    id=4 emp  id=99 for nas support
-- set @id =59
--
---- ****************** debug end    **********************************


set @trainingProvider=ltrim(rtrim(@trainingProvider))

BEGIN  
			SET NOCOUNT ON  
			SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 

		BEGIN TRY  
		if  len(@trainingProvider)=0 
		begin
		
		select 
				-1 as TrainingProviderId,
				'All' as TradingName
		end

		else
		begin
			SELECT	distinct
					tp.ProviderSiteID,
					tp.TradingName + ' ('+ tp.Town + ', ' + tp.Postcode + ')' TradingName
					--,FullName
			FROM	dbo.ProviderSIte tp
			JOIN dbo.VacancyOwnerRelationship vpr 
					ON vpr.ProviderSiteID = tp.ProviderSiteID
			WHERE
					
					((@typeid = 2) and  (tp.TradingName like @trainingProvider+'%') AND  (tp.ProviderSiteID= @id  )) OR
					((@typeid = 4) and  (tp.TradingName like @trainingProvider+'%') AND  (vpr.EmployerId= @id)) OR 
					((@typeid = 99) and  (tp.TradingName like @trainingProvider+'%'))
			ORDER BY TradingName
		end
		END TRY  
		BEGIN CATCH  
				EXEC RethrowError
		END CATCH  
      
    SET NOCOUNT OFF  
END