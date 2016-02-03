create procedure [sp_MSupd_dboSearchAuditRecord]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 datetime = NULL,
		@c4 nvarchar(500) = NULL,
		@c5 datetime = NULL,
		@c6 int = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[SearchAuditRecord] set
		[CandidateId] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [CandidateId] end,
		[RunDate] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [RunDate] end,
		[SearchCriteria] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [SearchCriteria] end,
		[RunTime] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [RunTime] end,
		[RecordCount] = case substring(@bitmap,1,1) & 32 when 32 then @c6 else [RecordCount] end
where [SearchAuditRecordId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end