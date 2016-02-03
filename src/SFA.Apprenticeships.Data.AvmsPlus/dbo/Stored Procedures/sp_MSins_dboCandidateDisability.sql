create procedure [sp_MSins_dboCandidateDisability]
    @c1 int,
    @c2 nvarchar(3),
    @c3 nvarchar(50),
    @c4 nvarchar(100)
as
begin  
	insert into [dbo].[CandidateDisability](
		[CandidateDisabilityId],
		[Codename],
		[ShortName],
		[FullName]
	) values (
    @c1,
    @c2,
    @c3,
    @c4	) 
end