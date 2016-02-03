create procedure [sp_MSupd_dboDisplayText]
		@c1 int = NULL,
		@c2 nvarchar(250) = NULL,
		@c3 int = NULL,
		@c4 nvarchar(250) = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[DisplayText] set
		[Type] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [Type] end,
		[Id] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [Id] end,
		[StandardText] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [StandardText] end
where [DisplayTextId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end