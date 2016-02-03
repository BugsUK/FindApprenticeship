create procedure [sp_MSins_dboSearchAuditRecord]
    @c1 int,
    @c2 int,
    @c3 datetime,
    @c4 nvarchar(500),
    @c5 datetime,
    @c6 int
as
begin  
	insert into [dbo].[SearchAuditRecord](
		[SearchAuditRecordId],
		[CandidateId],
		[RunDate],
		[SearchCriteria],
		[RunTime],
		[RecordCount]
	) values (
    @c1,
    @c2,
    @c3,
    @c4,
    @c5,
    @c6	) 
end