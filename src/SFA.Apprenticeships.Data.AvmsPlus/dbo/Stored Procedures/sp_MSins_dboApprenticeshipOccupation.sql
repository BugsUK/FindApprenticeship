create procedure [sp_MSins_dboApprenticeshipOccupation]
    @c1 int,
    @c2 nvarchar(3),
    @c3 nvarchar(50),
    @c4 nvarchar(100),
    @c5 int,
    @c6 datetime
as
begin  
	insert into [dbo].[ApprenticeshipOccupation](
		[ApprenticeshipOccupationId],
		[Codename],
		[ShortName],
		[FullName],
		[ApprenticeshipOccupationStatusTypeId],
		[ClosedDate]
	) values (
    @c1,
    @c2,
    @c3,
    @c4,
    @c5,
    @c6	) 
end