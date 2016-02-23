CREATE PROCEDURE [dbo].[uspSchoolSelectByName]       
 @SchoolName nvarchar(240)  
AS      
BEGIN      
      
 SET NOCOUNT ON      
     Select   
  SchoolId,  
  URN,  
  SchoolName,  
     Address  
     from School  
  Where SchoolName like  @SchoolName + '%'  
     
SET NOCOUNT OFF      
      
END