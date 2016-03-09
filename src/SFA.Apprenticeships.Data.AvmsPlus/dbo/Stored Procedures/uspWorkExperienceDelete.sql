CREATE PROCEDURE [dbo].[uspWorkExperienceDelete]  
  @workExperienceId int  
AS  
BEGIN  
 SET NOCOUNT ON  
   
    DELETE FROM [dbo].[WorkExperience]  
 WHERE [WorkExperienceId]=@workExperienceId  
      
    SET NOCOUNT OFF  
END