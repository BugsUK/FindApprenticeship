CREATE PROCEDURE [dbo].[uspEmployerContactUpdate]  
 -- Add the parameters for the stored procedure here  
 @employerContactId int,   
    @AddressLine1 nvarchar(50) ,  
    @AddressLine2 nvarchar(50) ,  
    @AddressLine3 nvarchar(50) ,  
    @AddressLine4 nvarchar(50) ,  
    @Town nvarchar(50) ,  
    @CountyId int,  
    @PostCode nvarchar(50),  
	@LocalAuthorityId int,  
 @ContactPreferenceTypeId int,  
 @JobTitle nvarchar(50),  
 @FaxNumber nvarchar(16),  
 @PersonId int output  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
BEGIN TRY  
  
    -- Insert statements for procedure here  
 UPDATE EmployerContact  
 SET   
  AddressLine1=ISNULL(@AddressLine1,AddressLine1),  
  AddressLine2=ISNULL(@AddressLine2,AddressLine2),  
  AddressLine3=ISNULL(@AddressLine3,AddressLine3),  
  AddressLine4=ISNULL(@AddressLine4,AddressLine4),  
  Town=ISNULL(@Town,Town),  
  CountyId=ISNULL(@CountyId,CountyId),  
  Postcode=ISNULL(@Postcode,Postcode),  
  ContactPreferenceTypeId=ISNULL(@ContactPreferenceTypeId,ContactPreferenceTypeId),  
  JobTitle=ISNULL(@JobTitle,JobTitle),  
  FaxNumber=ISNULL(@FaxNumber,FaxNumber),  
  LocalAuthorityId = ISNULL(@LocalAuthorityId,LocalAuthorityId)
 WHERE  
  EmployerContactId=@EmployerContactId   
  
 IF @@ROWCOUNT = 0  
 BEGIN  
  RAISERROR('Concurrent update error. Updated aborted.', 16, 2)  
 END  
  
 SELECT @PersonId=PersonId FROM EmployerContact WHERE EmployerContactId=@EmployerContactId   
      
 END TRY  
  
    BEGIN CATCH  
  EXEC RethrowError;  
 END CATCH   
  
 SET NOCOUNT OFF  
   
END