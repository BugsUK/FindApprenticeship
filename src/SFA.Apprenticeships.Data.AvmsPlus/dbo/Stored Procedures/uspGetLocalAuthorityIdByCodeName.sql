-- =============================================    
-- Author:  Manish Poddar 
-- Create date: 03 DEC 2008    
-- Description: Select the LocalAuthorityId by CodeName  
-- =============================================    
CREATE PROCEDURE [dbo].[uspGetLocalAuthorityIdByCodeName]    
 @LocalAuthCode CHAR(4)  
AS    
    
BEGIN    
    
 SET NOCOUNT ON;    
    
 SELECT [LocalAuthorityId]    
 FROM [LocalAuthority]    
 WHERE [CodeName] = @LocalAuthCode    
    
END