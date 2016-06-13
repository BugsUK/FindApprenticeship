/*----------------------------------------------------------------------                  
Name  : ReportGetEmployerTrainingProviderStatus
Description :  returns ordered unique Employeed status  

                
History:                  
--------                  
Date			Version		Author		Comment
15-Jan-2010		1.0			Hitesh      first version

---------------------------------------------------------------------- */                 

create procedure [dbo].[ReportGetEmployerTrainingProviderStatus]  

as  
  
BEGIN    
 SET NOCOUNT ON    
 SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED   
    BEGIN TRY    
  
  SELECT  -1 AS EmployerTrainingProviderStatusId ,'All' EmplyerProviderStatus, 0 AS ord  
  Union all  
  Select EmployerTrainingProviderStatusId, 'Active' EmplyerProviderStatus, 1 from  EmployerTrainingProviderStatus where CodeName = 'ATV'  
  Union all  
  Select EmployerTrainingProviderStatusId, 'Deleted' EmplyerProviderStatus, 2 from  EmployerTrainingProviderStatus where CodeName = 'DEL'  
   Union all  
  Select EmployerTrainingProviderStatusId, 'Suspended' EmplyerProviderStatus, 3 from  EmployerTrainingProviderStatus where CodeName = 'SUS'  
 
 END TRY    
 BEGIN CATCH    
  EXEC RethrowError  
 END CATCH    
        
    SET NOCOUNT OFF    
END