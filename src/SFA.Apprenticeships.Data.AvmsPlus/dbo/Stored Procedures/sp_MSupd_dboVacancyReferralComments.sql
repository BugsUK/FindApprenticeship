create procedure [sp_MSupd_dboVacancyReferralComments]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 int = NULL,
		@c4 nvarchar(4000) = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[VacancyReferralComments] set
		[VacancyId] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [VacancyId] end,
		[FieldTypeId] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [FieldTypeId] end,
		[Comments] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [Comments] end
where [VacancyReferralCommentsID] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end