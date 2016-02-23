CREATE PROCEDURE [dbo].[uspSchoolAttendedSelectBySchoolAttendedId]   
 @schoolAttendedId int  
AS  
BEGIN  
  
 SET NOCOUNT ON  
   
 SELECT  
 [schoolAttended].[CandidateId] AS 'CandidateId',  
 [schoolAttended].[ApplicationId] AS 'ApplicationId',  
 [schoolAttended].[EndDate] AS 'EndDate',  
 [schoolAttended].[OtherSchoolName] AS 'OtherSchoolName',  
 [schoolAttended].[OtherSchoolTown] AS 'OtherSchoolTown',  
 [schoolAttended].[SchoolAttendedId] AS 'SchoolAttendedId',  
 [schoolAttended].[StartDate] AS 'StartDate'  
 FROM [dbo].[SchoolAttended] [schoolAttended]  
 WHERE [SchoolAttendedId]=@schoolAttendedId  
 AND ApplicationId IS NULL
  
 SET NOCOUNT OFF  
END