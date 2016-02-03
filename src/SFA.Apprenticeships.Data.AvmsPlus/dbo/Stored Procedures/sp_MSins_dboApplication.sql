create procedure [sp_MSins_dboApplication]
    @c1 int,
    @c2 int,
    @c3 int,
    @c4 int,
    @c5 int,
    @c6 int,
    @c7 nvarchar(100),
    @c8 int,
    @c9 nvarchar(100),
    @c10 nvarchar(200),
    @c11 int,
    @c12 nvarchar(50),
    @c13 datetime,
    @c14 bit
as
begin  
	insert into [dbo].[Application](
		[ApplicationId],
		[CandidateId],
		[VacancyId],
		[ApplicationStatusTypeId],
		[WithdrawnOrDeclinedReasonId],
		[UnsuccessfulReasonId],
		[OutcomeReasonOther],
		[NextActionId],
		[NextActionOther],
		[AllocatedTo],
		[CVAttachmentId],
		[BeingSupportedBy],
		[LockedForSupportUntil],
		[WithdrawalAcknowledged]
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
    @c11,
    @c12,
    @c13,
    @c14	) 
end