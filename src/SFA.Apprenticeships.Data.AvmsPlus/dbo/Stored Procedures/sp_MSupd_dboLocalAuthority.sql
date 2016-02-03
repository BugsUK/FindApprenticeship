create procedure [sp_MSupd_dboLocalAuthority]
		@c1 int = NULL,
		@c2 nchar(4) = NULL,
		@c3 nvarchar(50) = NULL,
		@c4 nvarchar(100) = NULL,
		@c5 int = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[LocalAuthority] set
		[CodeName] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [CodeName] end,
		[ShortName] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [ShortName] end,
		[FullName] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [FullName] end,
		[CountyId] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [CountyId] end
where [LocalAuthorityId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end