create procedure [sp_MSupd_dboTermsAndConditions]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 nvarchar(200) = NULL,
		@c4 ntext = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[TermsAndConditions] set
		[UserTypeId] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [UserTypeId] end,
		[Fullname] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [Fullname] end,
		[Content] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [Content] end
where [TermsAndConditionsId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end