CREATE PROCEDURE [dbo].[uspApplicationUnsuccessfulReasonTypeUpdate]
@ApplicationUnsuccessfulReasonTypeId INT,
@FullName NVARCHAR (100),
@CandidateDisplayText NVARCHAR (900),
@CandidateFullName NVARCHAR (100),
@ReferralPoints INT
AS
BEGIN	
	UPDATE Applicationunsuccessfulreasontype
	SET		FullName = @FullName,
			ReferralPoints = @ReferralPoints,
			CandidateDisplayText = @CandidateDisplayText,
			CandidateFullName = @CandidateFullName
	WHERE 	ApplicationUnsuccessfulReasonTypeId = @ApplicationUnsuccessfulReasonTypeId	
END