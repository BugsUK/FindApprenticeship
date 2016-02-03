/*----------------------------------------------------------------------                  
Name  : ReportGetLocalAuthority                  
Description :  returns ordered unique Employeed trading names   

                
History:                  
--------                  
Date			Version		Author		Comment
13-02-2012		1.1			Sanjeev    Change version
---------------------------------------------------------------------- */    

CREATE PROCEDURE [dbo].[ReportGetGeoRegions]  
(@type int)  -- if type<>1 then put n/a
  
as  
  
BEGIN    
 SET NOCOUNT ON    
 SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED   
    BEGIN TRY    
    
    
  if @type=1 
		begin

   SELECT    
     GeographicRegionID,  
     GeographicFullName AS GeoRegion  
   FROM   
     vwRegions  
   order by   
     GeographicFullName  
   END 
    else
		begin
	SELECT -1 as GeographicRegionID, 'n/a' as 'GeoRegion'
		end
  
 END TRY    
 BEGIN CATCH    
  EXEC RethrowError  
 END CATCH    
        
    SET NOCOUNT OFF    
END