CREATE PROCEDURE [dbo].[uspCandidateInsertPreRegistrationDetails]
    @additionalEmail nvarchar(50) = NULL,
	@addressLine1 nvarchar(50),
	@addressLine2 nvarchar(50) = NULL,
	@addressLine3 nvarchar(50) = NULL,
	@addressLine4 nvarchar(50) = NULL,
	@candidateId int OUT,
	@candidateStatusTypeId int,
	@county int=NULL,
	@region nvarchar(10) = NULL,
	@dateofBirth datetime,
	@UnconfirmedEmailAddress nvarchar(200)= NULL,
	@emailAlertSent bit = NULL,
	@emailFailureCount smallint = NULL,
	@mobileNumberUnconfirmed bit = NULL,
	@personId int,
	@postcode nvarchar(10),
	@town nvarchar(50),	
	@uniqueLearnerNumber int,
	@gender int,
	@disability int,
	@healthproblems nvarchar(50), 
	@firstOccupation INT, 
	@firstFramework INT,
	@secondOccupation INT, 
	@secondFramework INT , 
	@allowMarketingMessages BIT
AS
BEGIN
	SET NOCOUNT ON
	BEGIN TRY
 DECLARE @LocalAuthorityId int,
		 @RegionId int

select @LocalAuthorityId=LocalAuthorityId 
		from LocalAuthority where codename=@region

If @LocalAuthorityId Is Null OR @LocalAuthorityId = 0
BEGIN
    SET @LocalAuthorityId = 0
    SET @RegionId = (Select LAG.LocalAuthorityGroupID from dbo.LocalAuthorityGroup LAG 
						Where LAG.CodeName = 'NAC')
END
    
INSERT INTO [dbo].[Candidate] 
		([AdditionalEmail], 
		[AddressLine1], 
		[AddressLine2], 
		[AddressLine3], 
		[AddressLine4], 
		[CandidateStatusTypeId], 
		[CountyId], 
		[DateofBirth],
		[UnconfirmedEmailAddress], 
		[EmailFailureCount],  
		[MobileNumberUnconfirmed], 
		[PersonId], 
		[Postcode],  
		[Town],  
		[UniqueLearnerNumber],
		[NiReference],
		[Gender],
		[LocalAuthorityid],
		--[LSCRegionid],
		[Disability],
		[HealthProblems],
		[AllowMarketingMessages]	
		)
	VALUES 
		(@additionalEmail, 
		@addressLine1, 
		@addressLine2, 
		@addressLine3, 
		@addressLine4, 
		@candidateStatusTypeId, 
		@county,
		@dateofBirth, 
		@UnconfirmedEmailAddress, 
		@emailFailureCount, 
		@mobileNumberUnconfirmed,
		@personId, 
		@postcode, 
		@town, 
		@uniqueLearnerNumber,
		'',
		@gender,
		@LocalAuthorityId,
		--@RegionId,
		@disability,
		@healthproblems, 
		@allowMarketingMessages
		)

    SET @candidateId = SCOPE_IDENTITY()

INSERT INTO [dbo].[CandidatePreferences]
		([CandidateId], 
		[FirstFrameworkId], 
		[FirstOccupationId], 
		[SecondFrameworkId], 
		[SecondOccupationId]
		)
VALUES 
		(@candidateId, 
		@firstFramework, 
		@firstOccupation, 
		@secondFramework, 
		@secondOccupation
		)


    END TRY

    BEGIN CATCH
		EXEC RethrowError;
	END CATCH
    
    SET NOCOUNT OFF
END