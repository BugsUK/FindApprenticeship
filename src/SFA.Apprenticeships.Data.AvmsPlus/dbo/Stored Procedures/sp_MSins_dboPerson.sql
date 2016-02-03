create procedure [sp_MSins_dboPerson]
    @c1 int,
    @c2 int,
    @c3 nvarchar(10),
    @c4 nvarchar(35),
    @c5 nvarchar(35),
    @c6 nvarchar(35),
    @c7 nvarchar(16),
    @c8 nvarchar(16),
    @c9 nvarchar(100),
    @c10 int
as
begin  
	insert into [dbo].[Person](
		[PersonId],
		[Title],
		[OtherTitle],
		[FirstName],
		[MiddleNames],
		[Surname],
		[LandlineNumber],
		[MobileNumber],
		[Email],
		[PersonTypeId]
	) values (
    @c1,
    @c2,
    @c3,
    @c4,
    @c5,
    @c6,
    @c7,
    @c8,
    @c9,
    @c10	) 
end