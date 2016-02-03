create procedure [sp_MSins_dboWorkExperience]
    @c1 int,
    @c2 int,
    @c3 nvarchar(50),
    @c4 datetime,
    @c5 datetime,
    @c6 nvarchar(200),
    @c7 bit,
    @c8 bit,
    @c9 int
as
begin  
	insert into [dbo].[WorkExperience](
		[WorkExperienceId],
		[CandidateId],
		[Employer],
		[FromDate],
		[ToDate],
		[TypeOfWork],
		[PartialCompletion],
		[VoluntaryExperience],
		[ApplicationId]
	) values (
    @c1,
    @c2,
    @c3,
    @c4,
    @c5,
    @c6,
    @c7,
    @c8,
    @c9	) 
end