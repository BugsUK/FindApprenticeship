create procedure [sp_MSins_dboCAFFields]
    @c1 int,
    @c2 int,
    @c3 int,
    @c4 smallint,
    @c5 nvarchar(4000)
as
begin  
	insert into [dbo].[CAFFields](
		[CAFFieldsId],
		[CandidateId],
		[ApplicationId],
		[Field],
		[Value]
	) values (
    @c1,
    @c2,
    @c3,
    @c4,
    @c5	) 
end