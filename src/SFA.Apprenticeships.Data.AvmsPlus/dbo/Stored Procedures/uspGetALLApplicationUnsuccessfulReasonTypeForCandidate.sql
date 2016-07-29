CREATE PROCEDURE [dbo].[uspGetALLApplicationUnsuccessfulReasonTypeForCandidate]  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
	SET NOCOUNT ON;  

	-- Insert statements for procedure here  
	SELECT
		ApplicationUnsuccessfulReasonTypeId,
		CodeName,
		ShortName,
		FullName,
		CandidateFullName,
		ReferralPoints,
		CandidateDisplayText
	FROM ApplicationUnsuccessfulReasonType		
	WHERE CandidateFullName IS NOT NULL

END