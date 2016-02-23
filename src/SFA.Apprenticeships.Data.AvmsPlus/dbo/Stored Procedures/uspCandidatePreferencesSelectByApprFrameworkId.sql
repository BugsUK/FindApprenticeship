CREATE PROCEDURE [dbo].[uspCandidatePreferencesSelectByApprFrameworkId] 
	@apprFrameworkId int
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
	Select * from CandidatePreferences  
	Where	FirstFrameworkId = @apprFrameworkId
	Or		SecondFrameworkId = @apprFrameworkId
END