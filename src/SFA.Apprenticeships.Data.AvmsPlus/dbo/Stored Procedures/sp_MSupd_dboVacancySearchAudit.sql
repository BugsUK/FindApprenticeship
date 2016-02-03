create procedure [sp_MSupd_dboVacancySearchAudit]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 nvarchar(200) = NULL,
		@c4 int = NULL,
		@c5 nvarchar(4000) = NULL,
		@c6 int = NULL,
		@c7 int = NULL,
		@c8 nvarchar(8) = NULL,
		@c9 int = NULL,
		@c10 int = NULL,
		@c11 int = NULL,
		@c12 int = NULL,
		@c13 int = NULL,
		@c14 int = NULL,
		@c15 int = NULL,
		@c16 int = NULL,
		@c17 nvarchar(100) = NULL,
		@c18 nvarchar(50) = NULL,
		@c19 int = NULL,
		@c20 int = NULL,
		@c21 datetime = NULL,
		@c22 int = NULL,
		@pkc1 int = NULL,
		@bitmap binary(3)
as
begin  
update [dbo].[VacancySearchAudit] set
		[SearchType] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [SearchType] end,
		[SearchTerm] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [SearchTerm] end,
		[ApprenticeshipOccupation] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [ApprenticeshipOccupation] end,
		[ApprenticeFrameworks] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [ApprenticeFrameworks] end,
		[LocationId] = case substring(@bitmap,1,1) & 32 when 32 then @c6 else [LocationId] end,
		[VacancyPostedSince] = case substring(@bitmap,1,1) & 64 when 64 then @c7 else [VacancyPostedSince] end,
		[PostCode] = case substring(@bitmap,1,1) & 128 when 128 then @c8 else [PostCode] end,
		[DistanceFromInMiles] = case substring(@bitmap,2,1) & 1 when 1 then @c9 else [DistanceFromInMiles] end,
		[DistanceFromInMeters] = case substring(@bitmap,2,1) & 2 when 2 then @c10 else [DistanceFromInMeters] end,
		[Easting] = case substring(@bitmap,2,1) & 4 when 4 then @c11 else [Easting] end,
		[Northing] = case substring(@bitmap,2,1) & 8 when 8 then @c12 else [Northing] end,
		[WeeklyWagesFrom] = case substring(@bitmap,2,1) & 16 when 16 then @c13 else [WeeklyWagesFrom] end,
		[WeeklyWagesTo] = case substring(@bitmap,2,1) & 32 when 32 then @c14 else [WeeklyWagesTo] end,
		[PageNo] = case substring(@bitmap,2,1) & 64 when 64 then @c15 else [PageNo] end,
		[PageSize] = case substring(@bitmap,2,1) & 128 when 128 then @c16 else [PageSize] end,
		[SortByField] = case substring(@bitmap,3,1) & 1 when 1 then @c17 else [SortByField] end,
		[SortOrder] = case substring(@bitmap,3,1) & 2 when 2 then @c18 else [SortOrder] end,
		[TotalVacancies] = case substring(@bitmap,3,1) & 4 when 4 then @c19 else [TotalVacancies] end,
		[TotalPositions] = case substring(@bitmap,3,1) & 8 when 8 then @c20 else [TotalPositions] end,
		[SearchDate] = case substring(@bitmap,3,1) & 16 when 16 then @c21 else [SearchDate] end,
		[ApprenticeshipTypeId] = case substring(@bitmap,3,1) & 32 when 32 then @c22 else [ApprenticeshipTypeId] end
where [VacancySearchAuditId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end