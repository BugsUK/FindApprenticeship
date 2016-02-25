CREATE PROCEDURE [dbo].[uspSchoolAttendedInsert]    
 @candidateId int,    
 @endDate datetime,    
 @otherSchoolName nvarchar(50) = NULL,    
 @otherSchoolTown nvarchar(50) = NULL,    
 @schoolAttendedId int OUT,    
 @startDate datetime,  
 @schoolId int  
AS    
BEGIN    
 SET NOCOUNT ON    
     
 BEGIN TRY    
    INSERT INTO [dbo].[SchoolAttended] ([CandidateId], [EndDate], [OtherSchoolName], [OtherSchoolTown], [SchoolId], [StartDate])    
 VALUES (@candidateId, @endDate, @otherSchoolName, @otherSchoolTown, @schoolId, @startDate)    
    SET @schoolAttendedId = SCOPE_IDENTITY()    
    END TRY    
    
    BEGIN CATCH    
  EXEC RethrowError;    
 END CATCH    
        
    SET NOCOUNT OFF    
END