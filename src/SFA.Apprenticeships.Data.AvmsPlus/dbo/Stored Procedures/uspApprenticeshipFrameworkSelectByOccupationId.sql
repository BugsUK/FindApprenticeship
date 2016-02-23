CREATE PROCEDURE [dbo].[uspApprenticeshipFrameworkSelectByOccupationId]  
	@ApprenticeshipOccupationId INT  
AS  
BEGIN    
    
	SET NOCOUNT ON    
  
	Select   
		ApprenticeshipFrameworkId,  
		ApprenticeshipFramework.ApprenticeshipOccupationId as 'ApprenticeshipOccupationId',  
		CodeName, ShortName, 
		isnull(Fullname,'') as 'FrameworkName',
		ApprenticeshipFrameworkStatusTypeId as 'StatusType',
		ClosedDate as 'ClosedDate',
		PreviousApprenticeshipOccupationId as 'PreviousApprenticeshipOccupationId'
	From ApprenticeshipFramework 
	Where ApprenticeshipFramework.ApprenticeshipOccupationId = @ApprenticeshipOccupationId 
	Order by FrameworkName  


	SET NOCOUNT OFF    

END