create procedure [sp_MSins_dboSchool]
    @c1 int,
    @c2 nvarchar(100),
    @c3 nvarchar(120),
    @c4 nvarchar(2000),
    @c5 nvarchar(100),
    @c6 nvarchar(100),
    @c7 nvarchar(100),
    @c8 nvarchar(100),
    @c9 nvarchar(100),
    @c10 nvarchar(10),
    @c11 nvarchar(120)
as
begin  
	insert into [dbo].[School](
		[SchoolId],
		[URN],
		[SchoolName],
		[Address],
		[Address1],
		[Address2],
		[Area],
		[Town],
		[County],
		[Postcode],
		[SchoolNameForSearch]
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
    @c10,
    @c11	) 
end