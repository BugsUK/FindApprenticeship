CREATE PROCEDURE [dbo].[uspApprenticeshipFrameworkUpdatePreviousOccupation]
	@FrameworkId int,
	@NewPreviousOccupationId int = Null
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
	SET NOCOUNT ON;  
  
	Update	ApprenticeshipFramework
	Set		PreviousApprenticeshipOccupationId = @NewPreviousOccupationId
	Where	ApprenticeshipFrameworkId = @FrameworkId

END