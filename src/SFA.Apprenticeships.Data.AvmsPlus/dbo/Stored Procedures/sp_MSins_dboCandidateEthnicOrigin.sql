create procedure [sp_MSins_dboCandidateEthnicOrigin]
    @c1 int,
    @c2 nvarchar(3),
    @c3 nvarchar(100),
    @c4 nvarchar(100)
as
begin  
	insert into [dbo].[CandidateEthnicOrigin](
		[CandidateEthnicOriginId],
		[CodeName],
		[ShortName],
		[FullName]
	) values (
    @c1,
    @c2,
    @c3,
    @c4	) 
end