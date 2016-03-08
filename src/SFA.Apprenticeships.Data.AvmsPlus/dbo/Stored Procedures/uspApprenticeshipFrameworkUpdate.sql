Create Procedure [dbo].[uspApprenticeshipFrameworkUpdate]  
	@frameworkId int, 
	@occupationId int, 
	@fullName nVarchar (200), 
	@closedDate DateTime = null,
	@previousOccupationId int = null
AS  
Begin
	SET NOCOUNT ON    
	 if @previousOccupationId = 0
		Set @previousOccupationId = null
	Begin Try
		Update ApprenticeshipFramework     
		Set	ApprenticeshipOccupationId = @occupationId, 
			FullName = @fullName, 
			ClosedDate = @closedDate,
			PreviousApprenticeshipOccupationId = @previousOccupationId
		Where ApprenticeshipFrameworkId = @frameworkId     
	End Try

	Begin Catch
		EXEC RethrowError;    
	End Catch
	    
	SET NOCOUNT OFF    
END