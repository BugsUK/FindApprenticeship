CREATE PROCEDURE [dbo].[uspCandidateInsert]
    @additionalEmail nvarchar(50) = NULL,
	@addressLine1 nvarchar(50),
	@addressLine2 nvarchar(50) = NULL,
	@addressLine3 nvarchar(50) = NULL,
	@addressLine4 nvarchar(50) = NULL,
	@applicationLimitEnforced bit = NULL,
	@candidateId int OUT,
	@candidateStatusTypeId int,	
	@countyId int,
	@dateofBirth datetime,	
	@disability nvarchar(256) = NULL,
	@disabilityOther nvarchar(256) = NULL,
	@disableAlerts bit = NULL,
	@doBFailureCount smallint = NULL,
	@UnconfirmedEmailAddress nvarchar(200) = NULL,
	@emailFailureCount smallint = NULL,
	@ethnicOrigin smallint = NULL,
	@ethnicOriginOther nvarchar(50) = NULL,
	@forgottenPasswordRequested bit = NULL,
	@forgottenUsernameRequested bit = NULL,
	@gender int,
	@geocodeEasting decimal(20,15) = NULL,
	@geocodeNorthing decimal(20,15) = NULL,
	@healthProblems bit = NULL,
	@lastAccessedDate datetime = NULL,
	@latitude decimal(20,15) = NULL,
	@longitude decimal(20,15) = NULL,
	@mobileNumberUnconfirmed bit = NULL,	
	@niReference nvarchar(10),
	@personId int,
	@postcode nvarchar(10),
	@receivePushedContent bit = NULL,
	@referralAgent bit = NULL,			
	@textFailureCount smallint = NULL,
	@town nvarchar(50),
	@ulnStatusId int,
	@uniqueLearnerNumber int
AS
BEGIN
	SET NOCOUNT ON
	
	BEGIN TRY
    INSERT INTO [dbo].[Candidate] ([AdditionalEmail], [AddressLine1], [AddressLine2], [AddressLine3], [AddressLine4], [ApplicationLimitEnforced], [CandidateStatusTypeId], [CountyId], [DateofBirth], [Disability], [DisabilityOther], [DisableAlerts], [DoBFailureCount], [UnconfirmedEmailAddress],  [EmailFailureCount], [EthnicOrigin], [EthnicOriginOther], [ForgottenPasswordRequested], [ForgottenUsernameRequested], [Gender], [GeocodeEasting], [GeocodeNorthing], [HealthProblems], [LastAccessedDate], [Latitude], [Longitude], [MobileNumberUnconfirmed], [NiReference], [PersonId], [Postcode], [ReceivePushedContent], [ReferralAgent],  [TextFailureCount], [Town], [UlnStatusId], [UniqueLearnerNumber])
	VALUES (@additionalEmail, @addressLine1, @addressLine2, @addressLine3, @addressLine4, @applicationLimitEnforced, @candidateStatusTypeId, @countyId, @dateofBirth,  @disability, @disabilityOther, @disableAlerts, @doBFailureCount, @UnconfirmedEmailAddress, @emailFailureCount, @ethnicOrigin, @ethnicOriginOther, @forgottenPasswordRequested, @forgottenUsernameRequested, @gender, @geocodeEasting, @geocodeNorthing, @healthProblems, @lastAccessedDate, @latitude, @longitude, @mobileNumberUnconfirmed,  @niReference, @personId, @postcode, @receivePushedContent, @referralAgent,   @textFailureCount, @town, @ulnStatusId, @uniqueLearnerNumber)
    SET @candidateId = SCOPE_IDENTITY()
    END TRY

    BEGIN CATCH
		EXEC RethrowError;
	END CATCH
    
    SET NOCOUNT OFF
END