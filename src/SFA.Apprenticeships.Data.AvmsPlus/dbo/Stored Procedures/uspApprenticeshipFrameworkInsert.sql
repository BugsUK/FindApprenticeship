Create Procedure [dbo].[uspApprenticeshipFrameworkInsert]
	@occupationId int,
	@shortName nVarchar (100),
	@fullName nVarchar (200),
	@closedDate DateTime,
	@apprFrameworkStatusTypeID int
AS  
BEGIN    
	SET NOCOUNT ON    

	Begin Try

		Insert Into ApprenticeshipFramework
			(ApprenticeshipOccupationId, CodeName, ShortName, FullName, ClosedDate, ApprenticeshipFrameworkStatusTypeId)
		Values
			(@occupationId, @shortName, @shortName, @fullName, @closedDate, @apprFrameworkStatusTypeID)    

	End Try  

	Begin Catch
		EXEC RethrowError;    
	End Catch

	SET NOCOUNT OFF
END