create procedure [sp_MSins_dboApprenticeshipFramework]
    @c1 int,
    @c2 int,
    @c3 nvarchar(3),
    @c4 nvarchar(100),
    @c5 nvarchar(200),
    @c6 int,
    @c7 datetime,
    @c8 int
as
begin  
	insert into [dbo].[ApprenticeshipFramework](
		[ApprenticeshipFrameworkId],
		[ApprenticeshipOccupationId],
		[CodeName],
		[ShortName],
		[FullName],
		[ApprenticeshipFrameworkStatusTypeId],
		[ClosedDate],
		[PreviousApprenticeshipOccupationId]
	) values (
    @c1,
    @c2,
    @c3,
    @c4,
    @c5,
    @c6,
    @c7,
    @c8	) 
end