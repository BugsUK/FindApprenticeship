CREATE PROCEDURE [dbo].[uspGetNoiseWords]       
AS      
BEGIN      
      
 SET NOCOUNT ON     
  
    SELECT   
        NoiseWordId,  
        NoiseWord
    FROM 
        NoiseWord  

 SET NOCOUNT OFF      
      
END