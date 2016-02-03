create procedure [sp_MSins_dboAdditionalAnswer]
    @c1 int,
    @c2 int,
    @c3 int,
    @c4 nvarchar(4000)
as
begin  
	insert into [dbo].[AdditionalAnswer](
		[AdditionalAnswerId],
		[ApplicationId],
		[AdditionalQuestionId],
		[Answer]
	) values (
    @c1,
    @c2,
    @c3,
    @c4	) 
end