/*----------------------------------------------------------------------                  
Name  : ReportGetEmployerProvider                  
Description :  returns ordered unique Employer Provider

                
History:                  
--------                  
Date			Version		Author			Comment
======================================================================
24-Nov-2008		1.0			Femma Ashraf	first version
---------------------------------------------------------------------- */                 

CREATE procedure [dbo].[ReportGetEmployerProvider]
 @employerName varchar(500),
 @ID int,
 @TypeID int
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
					emp.EmployerId,
					emp.TradingName + ' (' + emp.Town + ', ' + emp.Postcode +')' TradingName
			FROM	
					dbo.Employer emp
					join dbo.VacancyOwnerRelationship vpr 
						on vpr.EmployerId = emp.EmployerId
					join dbo.ProviderSite tp
						on tp.ProviderSiteID = vpr.ProviderSiteID
			WHERE
					emp.TradingName like @employerName+'%'
					AND ((@TypeId = 99)
					OR (@TypeId = 4 AND emp.EmployerId = @Id)
					OR (@TypeId = 2 AND tp.ProviderSiteID = @Id))
			ORDER BY TradingName
		end
		END TRY  
		BEGIN CATCH  
				EXEC RethrowError
		END CATCH  
      
    SET NOCOUNT OFF  
END