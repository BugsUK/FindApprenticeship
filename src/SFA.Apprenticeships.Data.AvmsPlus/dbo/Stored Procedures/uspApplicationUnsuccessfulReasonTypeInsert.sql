CREATE PROCEDURE [dbo].[uspApplicationUnsuccessfulReasonTypeInsert]
@FullName NVARCHAR (100),
@CandidateDisplayText NVARCHAR (900),
@CandidateFullName NVARCHAR (100),
@ReferralPoints INT,
@Withdrawn BIT
AS
BEGIN	 
DECLARE @NewId INT
	
	INSERT INTO ApplicationUnsuccessfulReasonType
		(FullName,ReferralPoints,CandidateDisplayText,CandidateFullName,CodeName,ShortName,Withdrawn)
	VALUES
		(@FullName,@ReferralPoints,@CandidateDisplayText, CASE WHEN @CandidateFullName=''THEN NULL ELSE @CandidateFullName END,'','',@Withdrawn)
		
	--SELECT @NewId = MAX(ApplicationUnsuccessfulReasonTypeId)
	--FROM  ApplicationUnsuccessfulReasonType
	
	UPDATE ApplicationUnsuccessfulReasonType
	SET CodeName = ApplicationUnsuccessfulReasonTypeId,
	ShortName = ApplicationUnsuccessfulReasonTypeId
	WHERE ApplicationUnsuccessfulReasonTypeId = SCOPE_IDENTITY()

	return SCOPE_IDENTITY()
END