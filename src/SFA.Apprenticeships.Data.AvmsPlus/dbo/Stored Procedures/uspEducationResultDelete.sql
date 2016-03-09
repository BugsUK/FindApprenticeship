CREATE PROCEDURE [dbo].[uspEducationResultDelete]        
@EducationResultId int        
AS            
BEGIN            
SET NOCOUNT ON            
    
  delete from EducationResult
  where EducationResultId=@EducationResultId
   
SET NOCOUNT OFF            
            
END