create procedure [sp_MSupd_dboSubVacancy]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 int = NULL,
		@c4 datetime = NULL,
		@c5 nchar(12) = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[SubVacancy] set
		[VacancyId] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [VacancyId] end,
		[AllocatedApplicationId] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [AllocatedApplicationId] end,
		[StartDate] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [StartDate] end,
		[ILRNumber] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [ILRNumber] end
where [SubVacancyId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end