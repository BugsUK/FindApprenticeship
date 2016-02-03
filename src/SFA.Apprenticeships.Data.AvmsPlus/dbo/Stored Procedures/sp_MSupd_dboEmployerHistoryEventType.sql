create procedure [sp_MSupd_dboEmployerHistoryEventType]
		@c1 int = NULL,
		@c2 nvarchar(3) = NULL,
		@c3 nvarchar(100) = NULL,
		@c4 nvarchar(200) = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[EmployerHistoryEventType] set
		[CodeName] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [CodeName] end,
		[ShortName] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [ShortName] end,
		[FullName] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [FullName] end
where [EmployerHistoryEventTypeId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end