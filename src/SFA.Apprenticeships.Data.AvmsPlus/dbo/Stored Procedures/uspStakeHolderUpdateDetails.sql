CREATE PROCEDURE [dbo].[uspStakeHolderUpdateDetails]
@PersonID INT, @StakeHolderId INT, @AddressLine1 NVARCHAR (50), @AddressLine2 NVARCHAR (50)=NULL, @AddressLine3 NVARCHAR (50)=NULL, @AddressLine4 NVARCHAR (50)=NULL, @AddressLine5 NVARCHAR (50)=NULL, @OrganisationId INT, @OrganisationOther NVARCHAR (50)=NULL, @Postcode NVARCHAR (10), @Town NVARCHAR (50), @CountyId INT=NULL, @UnconfirmedEmailAddress NVARCHAR (100)=NULL, @EmailAlertSent BIT=NULL, @TitleID INT, @OtherTitle NVARCHAR (10)=NULL, @FirstName NVARCHAR (35)=NULL, @MiddleNames NVARCHAR (35)=NULL, @Surname NVARCHAR (35), @LandlineNumber NVARCHAR (16)=NULL, @MobileNumber NVARCHAR (16)=NULL, @Email NVARCHAR (100)=NULL,@region NVARCHAR(10)
AS
BEGIN  
	SET NOCOUNT ON  
	
  Declare @RegionId int, @LocalAuthorityId int      
  
 -- Getting the Candidate Region Information  
 Select @LocalAuthorityId = LocalAuthorityid       
 From LocalAuthority  
 Where codename = @region    
    
 If @LocalAuthorityId Is Null OR @LocalAuthorityId = 0    
 BEGIN    
  SET @LocalAuthorityId = 0    
  SET @RegionId = (Select LAG.LocalAuthorityGroupID from LocalAuthorityGroup LAG     
       Where LAG.CodeName = 'NAC')   
 END

	--Update StakeHolder Details
	BEGIN TRY  
		Update	StakeHolder
		Set		AddressLine1 = @AddressLine1, 
				AddressLine2 = @AddressLine2, 
				AddressLine3 = @AddressLine3, 
				AddressLine4 = @AddressLine4, 
				AddressLine5 = @AddressLine5, 
				OrganisationId = @OrganisationId, 
				OrganisationOther = @OrganisationOther, 
				Postcode = @Postcode, 
				Town = @Town, 
				CountyId = @CountyId, 
				UnconfirmedEmailAddress = @UnconfirmedEmailAddress, 
				EmailAlertSent = @EmailAlertSent,
				LocalAuthorityId=@LocalAuthorityId
								
		Where	StakeHolderId = @StakeHolderId 
	
		--Update Person Details
		Execute uspPersonUpdate @PersonID, NULL, @TitleID, @OtherTitle, 
								@FirstName, @MiddleNames, @Surname, 
								@LandlineNumber, @MobileNumber, NULL, @Email 
    END TRY  
  
    BEGIN CATCH  
		EXEC RethrowError;  
	END CATCH  
      
	SET NOCOUNT OFF  
END