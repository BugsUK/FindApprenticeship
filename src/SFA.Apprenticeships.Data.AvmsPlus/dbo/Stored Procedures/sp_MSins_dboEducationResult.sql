create procedure [sp_MSins_dboEducationResult]
    @c1 int,
    @c2 int,
    @c3 nvarchar(50),
    @c4 int,
    @c5 nvarchar(100),
    @c6 nvarchar(20),
    @c7 datetime,
    @c8 int
as
begin  
	insert into [dbo].[EducationResult](
		[EducationResultId],
		[CandidateId],
		[Subject],
		[Level],
		[LevelOther],
		[Grade],
		[DateAchieved],
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