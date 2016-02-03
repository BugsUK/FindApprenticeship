CREATE PROCEDURE [dbo].[uspSchoolSelectById]        
 @schoolId int    
AS    
BEGIN    
 SET NOCOUNT ON    
     
SELECT    
	URN,
	SchoolName,    
	Address1,
	Address2,
	Area,
	Town,
	County,
	Postcode  
 FROM [dbo].[School]    
 WHERE     
	SchoolId = @schoolId    
    
 SET NOCOUNT OFF    
END