create procedure [sp_MSupd_dboFAQ]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 int = NULL,
		@c4 nvarchar(100) = NULL,
		@c5 nvarchar(2000) = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[FAQ] set
		[SortOrder] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [SortOrder] end,
		[UserTypeId] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [UserTypeId] end,
		[Title] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [Title] end,
		[Content] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [Content] end
where [FAQId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end