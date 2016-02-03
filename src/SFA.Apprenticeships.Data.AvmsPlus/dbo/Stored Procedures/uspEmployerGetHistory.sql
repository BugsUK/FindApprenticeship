create PROCEDURE [dbo].[uspEmployerGetHistory]
	@EmployerId int           
AS      
BEGIN
	SET NOCOUNT ON 
	
SELECT MyTable.*  FROM            
	( 
	select 
	TotalRows = count(1) over(),
	ROW_NUMBER() OVER( ORDER BY EH.[Date]   desc) as RowNum,

	EH.EmployerHistoryId as 'HistoryId',
	EH.EmployerId,
	EH.Event as 'EventTypeId',
	ISNULL(LEFT(EH.Comment,20),'') as 'Comment',
	EH.[Date] as 'EventDate',
	ISNULL(EH.UserName,'') as 'UserName'
	from EmployerHistory EH
	inner join EmployerHistoryEventType EHT on EH.[Event] = EHT.EmployerHistoryEventTypeId
	where EHT.CodeName = 'NTE' and EmployerId = @EmployerId
	) as MyTable

	     
	SET NOCOUNT OFF
END