create procedure [sp_MSupd_dboAuditRecord]
		@c1 int = NULL,
		@c2 nvarchar(50) = NULL,
		@c3 datetime = NULL,
		@c4 int = NULL,
		@c5 int = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[AuditRecord] set
		[Author] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [Author] end,
		[ChangeDate] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [ChangeDate] end,
		[AttachedtoItem] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [AttachedtoItem] end,
		[AttachedtoItemType] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [AttachedtoItemType] end
where [AuditRecordId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end