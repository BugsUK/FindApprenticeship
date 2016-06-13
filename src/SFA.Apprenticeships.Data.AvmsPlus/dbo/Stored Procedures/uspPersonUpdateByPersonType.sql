CREATE PROCEDURE [dbo].[uspPersonUpdateByPersonType]        
     @PersonId INT,        
        @ADUsername nvarchar(50) = NULL,        
        @Title int = NULL,        
        @OtherTitle nvarchar(10) = NULL,        
        @FirstName nvarchar(35) = NULL,        
        @MiddleNames nvarchar(35)= NULL,        
        @Surname nvarchar(35)= NULL,        
        @LandlineNumber nvarchar(30) = NULL,        
        @MobileNumber nvarchar(30) = NULL,        
        @TextFailureCount smallint = NULL,        
        @Email nvarchar(70) = NULL,        
        @EmailFailureCount smallint = NULL,        
        @EmailAlertSent bit = NULL,
        @PersonTypeId int = NULL           
        
AS        
BEGIN        
        
 --The [dbo].[Person] table doesn't have a timestamp column. Optimistic concurrency logic cannot be generated        
 SET NOCOUNT ON        
        
 BEGIN TRY        
        
 /*If a field parameter passed to the stored procedure is null then then stored procedure        
    will not change the values already stored in that field.*/        
        
 UPDATE [dbo].[Person]         
 SET         
  [Title] = ISNULL(@Title,Title),         
  [OtherTitle] = ISNULL(@OtherTitle,OtherTitle),        
  [FirstName] = ISNULL(@FirstName,FirstName),         
  [MiddleNames] = ISNULL(@MiddleNames,MiddleNames),         
  [Surname] = ISNULL(@Surname,Surname),        
  [LandlineNumber] = ISNULL(@LandlineNumber,LandlineNumber),         
  [MobileNumber] = ISNULL(@MobileNumber,MobileNumber),        
  [Email] = ISNULL(@Email,Email),         
  [PersonTypeId]= ISNULL(@PersonTypeId, PersonTypeId)
 WHERE [PersonId]=@PersonId        
                
 IF @@ROWCOUNT = 0        
 BEGIN        
  RAISERROR('Concurrent update error. Updated aborted.', 16, 2)        
 END        
    END TRY        
        
    BEGIN CATCH        
  EXEC RethrowError;        
 END CATCH         
        
 SET NOCOUNT OFF        
END        
         
        
        
        
        
   -- select * from candidate*/