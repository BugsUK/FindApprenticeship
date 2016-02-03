create procedure [sp_MSupd_dboPostcodeOutcode]
		@c1 int = NULL,
		@c2 nchar(4) = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[PostcodeOutcode] set
		[Outcode] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [Outcode] end
where [PostcodeOutcodeId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end