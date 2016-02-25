CREATE PROCEDURE [dbo].[uspTEMPGetEmployerUrn]    
 @EmployerId int    
AS  
BEGIN  
 SET NOCOUNT ON  
  
 SELECT   
 [Employer].[EdsUrn] AS 'EdsUrn'  
 FROM [dbo].[Employer] [Employer]  
 WHERE   
 [Employer].[EmployerId] = @EmployerId  
  
 SET NOCOUNT OFF   
END