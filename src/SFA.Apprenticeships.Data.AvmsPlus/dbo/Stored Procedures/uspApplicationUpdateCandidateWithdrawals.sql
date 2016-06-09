Create Procedure [dbo].[uspApplicationUpdateCandidateWithdrawals]
	@applicationId int,
	@unsuccessfulReasonId int,
	@userName nvarchar(50) = NULL,
	@appHistoryEventSubTypeId int, 
	@comment nvarchar(4000) = NULL
AS
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
	SET NOCOUNT ON;  

	-- Insert statements for procedure here  
	Update Application
	Set	WithdrawalAcknowledged = 0,
		UnsuccessfulReasonId = @unsuccessfulReasonId,
		ApplicationStatusTypeId = (Select ApplicationStatusTypeId From ApplicationStatusType 
									Where CodeName = 'WTD')
	Where ApplicationId = @applicationId

	-- Audit in History table as well
	Insert Into ApplicationHistory(ApplicationId, UserName, ApplicationHistoryEventDate, 
		ApplicationHistoryEventTypeId, ApplicationHistoryEventSubTypeId, Comment)  
	Values (@applicationId, @userName, getdate(), 1, @appHistoryEventSubTypeId, @comment)
END