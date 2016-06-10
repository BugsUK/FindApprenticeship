CREATE PROCEDURE [dbo].[uspEmployerSICCodesByEmployerId]  
 @EmployerId int  
AS  
BEGIN  
  
 SET NOCOUNT ON;  
  
  
  
SELECT  
    ESC.EmployerID,  
    ESC.SICId,
	SC.SICCode,
	SC.Description,
	SC.[Year]  
FROM   
    EmployerSICCodes ESC inner join SICCode SC on SC.SICCodeId = ESC.SICId
WHERE   
    ESC.EmployerId = @EmployerId  
   
   
END