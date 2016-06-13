CREATE PROCEDURE [dbo].[uspCandidateUpdateMyInformationDetails]    
@candidateId INT, @firstName VARCHAR (60), @middleName VARCHAR (60), @surName VARCHAR (60), @dateofBirth DATETIME, @addressLine1 VARCHAR (50), @addressLine2 VARCHAR (50), @addressLine3 VARCHAR (50), @addressLine4 VARCHAR (50), @addressLine5 VARCHAR (50), @town VARCHAR (50), @countyId INT, @region NVARCHAR (10)=NULL, @postCode VARCHAR (8), @landlineNumber VARCHAR (32), @mobileNumber VARCHAR (32), @email VARCHAR (100), @UnconfirmedEmailAddress NVARCHAR (100), @mobileNumberUnconfirmed BIT,
@firstOccupation INT, @firstFramework INT,
@secondOccupation INT, @secondFramework INT, @allowMarketingMessages BIT
AS
BEGIN          
	SET NOCOUNT ON          

	-- Declaration
	Declare	@personId int,    
			@LocalAuthorityId int,    
			@RegionId int,
			@ContactPreferenceCandidateId int    

	-- Getting the Candidate Region Information
	Select	@LocalAuthorityId = LocalAuthorityid     
	From	LocalAuthority
	Where	CodeName = @region  
  
	If @LocalAuthorityId Is Null OR @LocalAuthorityId = 0  
	BEGIN  
		SET @LocalAuthorityId = 0  
		SET @RegionId = (Select LAG.LocalAuthorityGroupID from dbo.LocalAuthorityGroup LAG   
							Where LAG.CodeName = 'NAC') 
	END  

	-- Getting candidates PersonId
	Select	@personId = PersonId From Candidate 
	Where	CandidateId = @candidateId    

--  Start Add for existing PreRegister candidarte
Select @ContactPreferenceCandidateId = CandidateId
From CandidatePreferences
Where [CandidateId]=@candidateId

If @ContactPreferenceCandidateId Is Null OR @ContactPreferenceCandidateId = 0 
BEGIN
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
ENd

--  END Add for existing PreRegister candidarte

	-- Attempting to update candidate information
	BEGIN TRY          
	BEGIN TRAN    
		UPDATE [dbo].[Candidate]           
		SET [DateofBirth] = @dateofBirth,           
			[AddressLine1] = @addressLine1,           
			[AddressLine2] = @addressLine2,          
			[AddressLine3] = @addressLine3,          
			[AddressLine4] = @addressLine4,          
			[AddressLine5] = @addressLine5,          
			[Town] = @town,     
			[CountyId] = @countyId,     
			[Postcode] = @postCode,          
			[UnconfirmedEmailAddress] = @UnconfirmedEmailAddress,          
			[MobileNumberUnconfirmed] = @mobileNumberUnconfirmed,    
			[LocalAuthorityid]=@LocalAuthorityId, 
			[AllowMarketingMessages] = @allowMarketingMessages  
			--[LSCRegionid] = @RegionId    
		WHERE [CandidateId]=@candidateId    

		UPDATE [dbo].[person]
		SET   [FirstName] = @firstName,          
			[MiddleNames] = @middleName,          
			[Surname] = @surName,          
			[LandlineNumber] = @landlineNumber,          
			[MobileNumber] = @mobileNumber,          
			[Email] = @email    
		WHERE PersonId = @personId    

		UPDATE [dbo].[CandidatePreferences]		 
		SET	[FirstFrameworkId] = @firstFramework, 
			[FirstOccupationId] = @firstOccupation, 
			[SecondFrameworkId] = @secondFramework, 
			[SecondOccupationId] = @secondOccupation
		where [CandidateId]=@candidateId

	COMMIT TRAN           
	END TRY          

	BEGIN CATCH          
	If @@TRANCOUNT > 0 ROLLBACK TRAN     
		RAISERROR('Unable to update Candidate Information as no relevant records found in the database.  Updated aborted.', 16, 2)          
		EXEC RethrowError;          
	END CATCH           
	SET NOCOUNT OFF          
END