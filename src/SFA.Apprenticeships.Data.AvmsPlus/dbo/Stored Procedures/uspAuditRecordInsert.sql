CREATE PROCEDURE [dbo].[uspAuditRecordInsert] 
	
	@Author nvarchar(50),  
	@ChangeDate datetime,
	@AttachedtoItem int,
	@AttachedtoItemType int
AS
BEGIN
	
	SET NOCOUNT ON;

   INSERT INTO [dbo].[AuditRecord] ([Author], [ChangeDate], [attachedtoItem], [AttachedtoItemType])  
			VALUES (@Author, @ChangeDate, @AttachedtoItem,@AttachedtoItemType)
	
END