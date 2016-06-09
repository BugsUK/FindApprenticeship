Create Procedure [dbo].[uspApprenticeshipFrameworkUpdateStatusType]
	@apprenticeshipFrameworkId int, 
	@newApprFramworkStatusType int,
	@closedDate DateTime
AS  
BEGIN  
	SET NOCOUNT ON  

	Begin Try

		UPDATE [dbo].[ApprenticeshipFramework]   
		SET	ApprenticeshipFrameworkStatusTypeId = @newApprFramworkStatusType,
			ClosedDate = @closedDate
		WHERE   [ApprenticeshipFrameworkId] = @ApprenticeshipFrameworkId   
	End Try

	Begin Catch  
		EXEC RethrowError;  
	END Catch
  
    SET NOCOUNT OFF  
END