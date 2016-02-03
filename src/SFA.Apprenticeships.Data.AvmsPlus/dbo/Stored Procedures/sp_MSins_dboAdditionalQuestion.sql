create procedure [sp_MSins_dboAdditionalQuestion]
    @c1 int,
    @c2 int,
    @c3 smallint,
    @c4 nvarchar(4000)
as
begin  
	insert into [dbo].[AdditionalQuestion](
		[AdditionalQuestionId],
		[VacancyId],
		[QuestionId],
		[Question]
	) values (
    @c1,
    @c2,
    @c3,
    @c4	) 
end