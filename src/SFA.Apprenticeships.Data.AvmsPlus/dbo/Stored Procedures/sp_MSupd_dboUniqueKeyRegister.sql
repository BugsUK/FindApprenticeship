create procedure [sp_MSupd_dboUniqueKeyRegister]
		@c1 int = NULL,
		@c2 nchar(2) = NULL,
		@c3 nvarchar(30) = NULL,
		@c4 datetime = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[UniqueKeyRegister] set
		[KeyType] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [KeyType] end,
		[KeyValue] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [KeyValue] end,
		[DateTimeStamp] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [DateTimeStamp] end
where [UniqueKeyRegisterId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end