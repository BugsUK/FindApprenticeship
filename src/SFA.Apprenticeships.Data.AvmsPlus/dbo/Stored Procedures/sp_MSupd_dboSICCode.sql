create procedure [sp_MSupd_dboSICCode]
		@c1 int = NULL,
		@c2 smallint = NULL,
		@c3 int = NULL,
		@c4 nvarchar(256) = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[SICCode] set
		[Year] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [Year] end,
		[SICCode] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [SICCode] end,
		[Description] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [Description] end
where [SICCodeId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end