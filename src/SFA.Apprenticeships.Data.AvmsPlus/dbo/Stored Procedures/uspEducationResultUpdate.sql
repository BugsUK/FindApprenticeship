CREATE PROCEDURE [dbo].[uspEducationResultUpdate]      
@EducationResultId int,  
@CandidateId int,  
@Subject nvarchar(100),  
@Level int,  
@LevelOther nvarchar(200),  
@Grade nvarchar(15),    
@DateAchieved datetime  

AS          
BEGIN          
SET NOCOUNT ON          
  
BEGIN TRY  
 Update EducationResult  
 set   
     CandidateId = @CandidateId,  
  Subject=@Subject,  
  [Level]=@Level,  
  LevelOther=@LevelOther,  
  Grade=@Grade,  
  DateAchieved=@DateAchieved

 Where EducationResultId =@EducationResultId   
  
IF @@ROWCOUNT = 0  
 BEGIN  
  RAISERROR('Updated aborted.', 16, 2)  
 END  
  
END TRY  
  
BEGIN CATCH  
 EXEC RethrowError;  
END CATCH   
  
SET NOCOUNT OFF          
          
END