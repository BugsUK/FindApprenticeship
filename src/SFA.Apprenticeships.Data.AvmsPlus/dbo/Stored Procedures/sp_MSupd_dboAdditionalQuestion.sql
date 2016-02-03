create procedure [sp_MSupd_dboAdditionalQuestion]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 smallint = NULL,
		@c4 nvarchar(4000) = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[AdditionalQuestion] set
		[VacancyId] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [VacancyId] end,
		[QuestionId] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [QuestionId] end,
		[Question] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [Question] end
where [AdditionalQuestionId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end