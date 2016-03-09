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

CREATE procedure [dbo].[ReportGetRecruitmentAgency]
 @recruitmentAgency varchar(500)
as


set @recruitmentAgency=ltrim(rtrim(@recruitmentAgency))

BEGIN  
	
		BEGIN TRY  
		if  len(@recruitmentAgency)=0 
		begin
		
		select 
				-1 AS RecruitmentAgencyId,
				'All' AS RecruitmentAgencyName
		end

		else
		begin
			SELECT	
					ProviderSiteId AS RecruitmentAgencyId ,
					TradingName + ' (' + Town + ', ' + Postcode +')' AS RecruitmentAgencyName
			FROM	
					ProviderSite
			WHERE
					TradingName like @recruitmentAgency +'%'
					AND IsRecruitmentAgency = 1
			ORDER BY TradingName
		end
		END TRY  
		BEGIN CATCH  
				EXEC RethrowError
		END CATCH  
 
END