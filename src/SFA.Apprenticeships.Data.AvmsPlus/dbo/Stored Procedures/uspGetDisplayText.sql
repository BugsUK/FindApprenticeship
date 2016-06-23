CREATE PROCEDURE [dbo].[uspGetDisplayText]      
  
AS  
  
BEGIN  
 SET NOCOUNT ON  
   
 SELECT  
Type AS 'Key',  
 StandardText AS 'Value'  
   
FROM [dbo].[DisplayText]   
  
 SET NOCOUNT OFF  
END