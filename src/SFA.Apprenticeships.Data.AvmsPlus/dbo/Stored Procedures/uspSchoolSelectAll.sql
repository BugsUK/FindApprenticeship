CREATE PROCEDURE [dbo].[uspSchoolSelectAll]       

AS
   
BEGIN      
      
 SET NOCOUNT ON      
    SELECT   
        SchoolId,  
        URN,  
        SchoolName,  
        Address1,
        Address2,
        Area,
        Town,
        County,
        Postcode,
        SchoolNameForSearch 
        
    FROM 
        [dbo].[School]
     
SET NOCOUNT OFF      
      
END