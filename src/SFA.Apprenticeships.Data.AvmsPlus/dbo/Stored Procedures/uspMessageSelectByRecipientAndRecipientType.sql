CREATE PROCEDURE [dbo].[uspMessageSelectByRecipientAndRecipientType]      
 @recipientType int,    
 @recipient int,    
 @showDeleted bit    
AS    
BEGIN      
 SET NOCOUNT ON      
    
 BEGIN TRY      
  Begin    
       
   Select *    
   From Message    
   Where RecipientType = @recipientType     
   And  Recipient = @recipient    
   And  IsDeleted = @showDeleted    
	Order By MessageDate Desc
  End    
    END TRY      
      
    BEGIN CATCH      
 EXEC RethrowError;      
  END CATCH      
 SET NOCOUNT OFF      
END