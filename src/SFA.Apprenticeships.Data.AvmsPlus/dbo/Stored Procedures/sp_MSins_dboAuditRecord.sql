create procedure [sp_MSins_dboAuditRecord]
    @c1 int,
    @c2 nvarchar(50),
    @c3 datetime,
    @c4 int,
    @c5 int
as
begin  
	insert into [dbo].[AuditRecord](
		[AuditRecordId],
		[Author],
		[ChangeDate],
		[AttachedtoItem],
		[AttachedtoItemType]
	) values (
    @c1,
    @c2,
    @c3,
    @c4,
    @c5	) 
end