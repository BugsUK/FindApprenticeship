CREATE PROCEDURE [dbo].[uspStakeHolderInsertPreRegistrationDetails]
@PersonId INT, @AddressLine1 NVARCHAR (50), @AddressLine2 NVARCHAR (50)=NULL, @AddressLine3 NVARCHAR (50)=NULL, @AddressLine4 NVARCHAR (50)=NULL, @AddressLine5 NVARCHAR (50)=NULL, @StakeHolderStatusId INT, @OrganisationId INT, @OrganisationOther NVARCHAR (50)=NULL, @Postcode NVARCHAR (10), @Town NVARCHAR (50), @CountyId INT=NULL, @UnconfirmedEmailAddress NVARCHAR (200)=NULL, @EmailAlertSent BIT=NULL,@region NVARCHAR (10), @StakeHolderId INT OUTPUT
AS
BEGIN  

 -- Declaration  
 Declare       
   @LocalAuthorityId int      
     
  
 -- Getting the Candidate Region Information  
 Select @LocalAuthorityId = LocalAuthorityId       
 From LocalAuthority  
 Where codename = @region    
    
 If @LocalAuthorityId Is Null OR @LocalAuthorityId = 0    
 BEGIN    
  SET @LocalAuthorityId = 0      
 END   

	SET NOCOUNT ON  
	BEGIN TRY  
		INSERT INTO [StakeHolder](
			PersonId, AddressLine1, AddressLine2, AddressLine3, 
			AddressLine4, AddressLine5, StakeHolderStatusId, 
			OrganisationId, OrganisationOther, Postcode, Town, 
			CountyId, UnconfirmedEmailAddress, EmailAlertSent,LocalAuthorityid)  
		 VALUES(
			@PersonId, @AddressLine1, @AddressLine2, @AddressLine3, 
			@AddressLine4, @AddressLine5, @StakeHolderStatusId, 
			@OrganisationId, @OrganisationOther, @Postcode, @Town, 
			@CountyId, @UnconfirmedEmailAddress, @EmailAlertSent,@LocalAuthorityId)  
		SET @StakeHolderId = SCOPE_IDENTITY()  
    END TRY  
  
    BEGIN CATCH  
		EXEC RethrowError;  
	END CATCH  
      
	SET NOCOUNT OFF  
END