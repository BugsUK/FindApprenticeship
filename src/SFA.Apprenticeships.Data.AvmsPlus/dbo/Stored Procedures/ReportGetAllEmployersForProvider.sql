/*----------------------------------------------------------------------                  
Name  : ReportGetAllEmployersForProvider                  
Description :  returns ordered unique Employers for providers

                
History:                  
--------                  
Date			Version		Author				Comment
======================================================================
23-Mar-2011		1.0			Ashish Yerunkar		first version
31/03/2011		1.1			Dmitriy Kraevoy		refactoring
---------------------------------------------------------------------- */                 

create procedure [dbo].[ReportGetAllEmployersForProvider]
	@trainingProviderID int 
as

BEGIN  

SET NOCOUNT ON  
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 

select	
	-1 as EmployerId,
	'All' as TradingName

union all 
						
select	
	 emp.EmployerId
	,emp.TradingName 
from dbo.Employer emp
join dbo.VacancyOwnerRelationship vpr on vpr.EmployerId = emp.EmployerId
join dbo.ProviderSite tp on tp.PRoviderSIteID = vpr.PRoviderSIteID
where
	tp.ProviderSiteID = @trainingProviderID
order by TradingName
      
SET NOCOUNT OFF  

END