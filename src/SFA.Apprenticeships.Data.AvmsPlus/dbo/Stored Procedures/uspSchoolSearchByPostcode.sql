CREATE PROCEDURE [dbo].[uspSchoolSearchByPostcode]
 @SchoolName NVARCHAR (1000),
 @SchoolPostCode NVARCHAR (1000)  
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
		Postcode LIKE @SchoolPostCode + '%'
    )
    AND CONTAINS(*, @SchoolName)

     
SET NOCOUNT OFF      
      
END