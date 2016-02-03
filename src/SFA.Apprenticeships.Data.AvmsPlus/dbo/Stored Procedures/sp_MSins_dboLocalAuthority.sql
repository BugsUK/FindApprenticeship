create procedure [sp_MSins_dboLocalAuthority]
    @c1 int,
    @c2 nchar(4),
    @c3 nvarchar(50),
    @c4 nvarchar(100),
    @c5 int
as
begin  
	insert into [dbo].[LocalAuthority](
		[LocalAuthorityId],
		[CodeName],
		[ShortName],
		[FullName],
		[CountyId]
	) values (
    @c1,
    @c2,
    @c3,
    @c4,
    @c5	) 
end