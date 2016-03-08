CREATE PROCEDURE [dbo].[uspEmployerSelectByUrnList]
@urnList VARCHAR (1000)
AS
BEGIN              
 -- SET NOCOUNT ON added to prevent extra result sets from              
 -- interfering with SELECT statements.              
 SET NOCOUNT ON;              
    
if @urnList is null or @urnList = ''    
set @urnList = 'NULL'              
              
EXEC (' SELECT               
     E.EmployerId              
    ,E.FullName               
    ,E.EdsUrn               
    ,E.AddressLine1              
    ,E.AddressLine2              
    ,E.AddressLine3              
    ,E.AddressLine4              
    ,E.Town              
    ,County.FullName as ''County''          
    ,E.CountyId              
    ,LAG.FullName as ''Region''         
    ,LAG.LocalAuthorityGroupID AS ''RegionId''            
    ,E.PostCode              
    ,E.Longitude              
    ,E.Latitude              
 FROM               
 dbo.Employer E
 INNER JOIN dbo.LocalAuthority LA ON E.LocalAuthorityId = LA.LocalAuthorityId
 INNER JOIN dbo.LocalAuthorityGroupMembership LAGM ON LA.LocalAuthorityId = LAGM.LocalAuthorityID
 INNER JOIN dbo.LocalAuthorityGroup LAG ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID
 INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
 AND LAGT.LocalAuthorityGroupTypeID = 4             
 Left Outer Join County on County.CountyId = E.CountyId 
 WHERE                
 EdsUrn in (' + @urnList + ') AND    E.EmployerStatusTypeId = 1   ')  
    

--SELECT       
--    E.EmployerId      
-- ,E.FullName       
--    ,E.EdsUrn       
--    ,E.AddressLine1      
--    ,E.AddressLine2      
--    ,E.AddressLine3      
--    ,E.AddressLine4      
--    ,E.Town      
--    ,County.FullName as 'County'
-- ,E.CountyId      
--    ,LSCRegion.FullName as 'Region'    
--    ,E.LSCRegionId
--    ,E.PostCode      
--    ,E.Longitude      
--    ,E.Latitude      
-- FROM       
-- dbo.Employer E      
-- Left Outer Join County on County.CountyId = E.CountyId    
-- left outer join LSCRegion on LSCRegion.LSCRegionId = E.LSCRegionId             
              
END