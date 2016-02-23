create  PROCEDURE [dbo].[uspEmployerHistorySelectByHistoryId]         
 @historyId INT
 
AS        
BEGIN        
        
 SET NOCOUNT ON        
         
select 
EmployerHistoryId as 'HistoryId',
EmployerId,
Event,
[Date] as EventDate,
Comment AS 'Comment',
ISNULL(UserName,'') AS 'UserName'
FROM dbo.EmployerHistory
WHERE EmployerHistoryId = @historyId 
	
SET NOCOUNT OFF        
END