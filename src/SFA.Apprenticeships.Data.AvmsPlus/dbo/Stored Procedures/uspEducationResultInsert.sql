CREATE PROCEDURE [dbo].[uspEducationResultInsert]        
@CandidateId int,    
@Subject nvarchar(100),    
@Level int,    
@LevelOther nvarchar(200),    
@Grade nvarchar(15),    
@DateAchieved datetime,    
@EducationResultId int out    
AS            
BEGIN            
SET NOCOUNT ON            
    
 BEGIN TRY   
  Insert into EducationResult    
  (    
   CandidateId,    
   Subject,    
   [Level],    
   LevelOther,    
   Grade,    
   DateAchieved    
  )    
  Values    
  (    
   @CandidateId,    
   @Subject,    
   @Level,    
   @LevelOther,    
   @Grade,    
   @DateAchieved  
  )    
     
SET @EducationResultId = SCOPE_IDENTITY()    
    
    END TRY    
    
    BEGIN CATCH    
  EXEC RethrowError;    
 END CATCH    
   
SET NOCOUNT OFF            
            
END