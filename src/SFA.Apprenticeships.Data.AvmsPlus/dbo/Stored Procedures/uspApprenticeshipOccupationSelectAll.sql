CREATE PROCEDURE [dbo].[uspApprenticeshipOccupationSelectAll]     
AS    
BEGIN    
    
	SET NOCOUNT ON    
	  
	Select     
		ApprenticeshipOccupationId,    
		isnull(Codename,'') as 'CodeName',    
		isnull(ShortName,'') as 'ShortName',    
		isnull(FullName,'') as 'ApprenticeshipOccupation',   
		ApprenticeshipOccupationStatusTypeId as 'StatusType',  
		ClosedDate as ClosedDate   
	from ApprenticeshipOccupation    
	order by ShortName, ApprenticeshipOccupation
	   
	SET NOCOUNT OFF    
   
END