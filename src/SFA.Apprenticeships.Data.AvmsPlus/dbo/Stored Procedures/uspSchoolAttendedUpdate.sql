CREATE PROCEDURE [dbo].[uspSchoolAttendedUpdate]      
 @candidateId int,      
 @schoolId int,      
 @otherSchoolName nvarchar(120),      
 @otherSchoolTown nvarchar(120),      
 @startDate dateTime,      
 @endDate dateTime,      
 @schoolAttendedId int OUT      
AS      
BEGIN      
 SET NOCOUNT ON      
       
 BEGIN TRY      
    UPDATE [dbo].[SchoolAttended] SET        
      [OtherSchoolName] = @otherSchoolName,      
      [OtherSchoolTown]  = @otherSchoolTown,      
      [StartDate] = @startDate,      
      [EndDate] = @endDate,      
      [SchoolId] = @schoolId      
   WHERE SchoolAttendedId = @schoolAttendedId      
 Return @schoolAttendedId  
END TRY      
      
 BEGIN CATCH      
  EXEC RethrowError;      
 END CATCH      
          
    SET NOCOUNT OFF      
END