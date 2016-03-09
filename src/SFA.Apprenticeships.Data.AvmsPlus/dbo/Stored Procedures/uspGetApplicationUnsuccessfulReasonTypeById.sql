CREATE PROCEDURE [dbo].[uspGetApplicationUnsuccessfulReasonTypeById]
@ApplicationUnsuccessfulReasonTypeId INT
AS
BEGIN  
 SET NOCOUNT ON  
	 SELECT ApplicationUnsuccessfulReasonTypeId, 
			CodeName,
			ShortName,
			FullName,
			ReferralPoints,
			CandidateDisplayText,
			CandidateFullName,
			Withdrawn
	 FROM	Applicationunsuccessfulreasontype  
	 WHERE  ApplicationUnsuccessfulReasonTypeId = @ApplicationUnsuccessfulReasonTypeId
	 
    SET NOCOUNT OFF  
END