CREATE PROCEDURE [dbo].[ReportGetAllRegions]  
  
as  
  
BEGIN    
 SET NOCOUNT ON    
 SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED   
    BEGIN TRY    
    
    
BEGIN
   SELECT '-1' as GeographicRegionID,
   'All' as GeoRegion
   UNION
   SELECT    
     GeographicRegionID,  
     GeographicFullName AS GeoRegion  
   FROM   
     vwRegions
   WHERE GeographicRegionID <> 0  
 
   END 

  
 END TRY    
 BEGIN CATCH    
  EXEC RethrowError  
 END CATCH    
        
    SET NOCOUNT OFF    
END