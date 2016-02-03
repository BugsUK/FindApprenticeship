create procedure [sp_MSupd_dboVacancyTextField]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 int = NULL,
		@c4 nvarchar(max) = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[VacancyTextField] set
		[VacancyId] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [VacancyId] end,
		[Field] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [Field] end,
		[Value] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [Value] end
where [VacancyTextFieldId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end