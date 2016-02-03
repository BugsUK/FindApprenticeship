create procedure [sp_MSins_dboReportAgeRanges]
    @c1 int,
    @c2 nvarchar(10),
    @c3 int,
    @c4 int
as
begin  
	insert into [dbo].[ReportAgeRanges](
		[ReportAgeRangeID],
		[ReportAgeRangeLabel],
		[MinYears],
		[MaxYears]
	) values (
    @c1,
    @c2,
    @c3,
    @c4	) 
end