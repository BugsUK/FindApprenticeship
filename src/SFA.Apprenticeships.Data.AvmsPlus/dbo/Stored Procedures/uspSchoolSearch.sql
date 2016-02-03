CREATE PROCEDURE [dbo].[uspSchoolSearch]       
 @SchoolName NVARCHAR (1000),
 @SchoolLocality NVARCHAR (1000)  
AS      
BEGIN      
      
 SET NOCOUNT ON      
    Select   
        SchoolId,  
        URN,  
        SchoolName,  
        Address1,
        Address2,
        Area,
        Town,
        County,
        Postcode
          
    FROM 
        School  
    WHERE
            
    (
        Address2 LIKE @SchoolLocality + '%'
    OR  Area LIKE @SchoolLocality + '%'
    OR  Town LIKE @SchoolLocality + '%'
    OR  County LIKE @SchoolLocality + '%'
    OR  Postcode LIKE @SchoolLocality + '%'
    )
    AND CONTAINS(*, @SchoolName)

     
SET NOCOUNT OFF      
      
END