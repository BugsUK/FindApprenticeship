create procedure [sp_MSins_dboSchoolAttended]
    @c1 int,
    @c2 int,
    @c3 int,
    @c4 nvarchar(120),
    @c5 nvarchar(120),
    @c6 datetime,
    @c7 datetime,
    @c8 int
as
begin  
	insert into [dbo].[SchoolAttended](
		[SchoolAttendedId],
		[CandidateId],
		[SchoolId],
		[OtherSchoolName],
		[OtherSchoolTown],
		[StartDate],
		[EndDate],
		[ApplicationId]
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