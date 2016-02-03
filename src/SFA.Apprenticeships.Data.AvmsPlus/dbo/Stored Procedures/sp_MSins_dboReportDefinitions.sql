create procedure [sp_MSins_dboReportDefinitions]
    @c1 int,
    @c2 nvarchar(100),
    @c3 nvarchar(100),
    @c4 nvarchar(100),
    @c5 nvarchar(100),
    @c6 nvarchar(max),
    @c7 nvarchar(100),
    @c8 nvarchar(100),
    @c9 nvarchar(100),
    @c10 nvarchar(255)
as
begin  
	insert into [dbo].[ReportDefinitions](
		[RoleTypeID],
		[DisplayName],
		[HTMLVersion],
		[CSVVersion],
		[SummaryVersion],
		[Description],
		[GeographicSectionName],
		[DateSectionName],
		[ApplicationSectionName],
		[Flags]
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
    @c10	) 
end