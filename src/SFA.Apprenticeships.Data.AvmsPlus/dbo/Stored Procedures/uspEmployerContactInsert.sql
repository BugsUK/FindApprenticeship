CREATE PROCEDURE [dbo].[uspEmployerContactInsert]
	@employerContactId int out,
	@personId int,	 
    @AddressLine1	nvarchar(50)	,
    @AddressLine2	nvarchar(50)	,
    @AddressLine3	nvarchar(50)	,
    @AddressLine4	nvarchar(50)	,
    @Town	nvarchar(50)	,
    @CountyId	int,
    @PostCode	nvarchar(50),
	@LocalAuthorityId int,
	@ContactPreferenceTypeId int,
	@JobTitle	nvarchar(50),
	@FaxNumber nvarchar(16)	
AS
BEGIN
	SET NOCOUNT ON

	
	BEGIN TRY
    INSERT INTO [dbo].[EmployerContact] 
			(	[PersonId],
				[AddressLine1],
				[AddressLine2],
				[AddressLine3],
				[AddressLine4],
				[Town],
				[CountyId],
				[Postcode],
				[LocalAuthorityId],
				[ContactPreferenceTypeId],
				[JobTitle],
				[FaxNumber] )

	 VALUES (	@personId ,
				@AddressLine1,
				@AddressLine2,
				@AddressLine3,
				@AddressLine4,
				@Town,
				@CountyId,
				@Postcode,
				@LocalAuthorityId,
				@ContactPreferenceTypeId,
				@JobTitle,
				@FaxNumber )
    
	SET @employerContactId=SCOPE_IDENTITY()
	
    END TRY

    BEGIN CATCH
		EXEC RethrowError;
	END CATCH
    
    SET NOCOUNT OFF
END