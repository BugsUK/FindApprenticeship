create procedure [sp_MSins_dboCandidateULNStatus]
    @c1 int,
    @c2 nvarchar(3),
    @c3 nvarchar(10),
    @c4 nvarchar(100)
as
begin  
	insert into [dbo].[CandidateULNStatus](
		[CandidateULNStatusId],
		[Codename],
		[Shortname],
		[Fullname]
	) values (
    @c1,
    @c2,
    @c3,
    @c4	) 
end