create procedure [sp_MSins_dboApplicationUnsuccessfulReasonType]
    @c1 int,
    @c2 nvarchar(3),
    @c3 nvarchar(10),
    @c4 nvarchar(100),
    @c5 int,
    @c6 nvarchar(900),
    @c7 nvarchar(100),
    @c8 bit
as
begin  
	insert into [dbo].[ApplicationUnsuccessfulReasonType](
		[ApplicationUnsuccessfulReasonTypeId],
		[CodeName],
		[ShortName],
		[FullName],
		[ReferralPoints],
		[CandidateDisplayText],
		[CandidateFullName],
		[Withdrawn]
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