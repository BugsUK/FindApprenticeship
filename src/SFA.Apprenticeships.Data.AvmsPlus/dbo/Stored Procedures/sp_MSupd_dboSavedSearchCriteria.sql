create procedure [sp_MSupd_dboSavedSearchCriteria]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 nvarchar(50) = NULL,
		@c4 int = NULL,
		@c5 int = NULL,
		@c6 nvarchar(8) = NULL,
		@c7 decimal(13,10) = NULL,
		@c8 decimal(13,10) = NULL,
		@c9 int = NULL,
		@c10 int = NULL,
		@c11 smallint = NULL,
		@c12 smallint = NULL,
		@c13 smallint = NULL,
		@c14 int = NULL,
		@c15 nvarchar(255) = NULL,
		@c16 nvarchar(255) = NULL,
		@c17 nvarchar(100) = NULL,
		@c18 datetime = NULL,
		@c19 bit = NULL,
		@c20 bit = NULL,
		@c21 int = NULL,
		@c22 int = NULL,
		@c23 int = NULL,
		@pkc1 int = NULL,
		@bitmap binary(3)
as
begin  
update [dbo].[SavedSearchCriteria] set
		[CandidateId] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [CandidateId] end,
		[Name] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [Name] end,
		[SearchType] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [SearchType] end,
		[CountyId] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [CountyId] end,
		[Postcode] = case substring(@bitmap,1,1) & 32 when 32 then @c6 else [Postcode] end,
		[Longitude] = case substring(@bitmap,1,1) & 64 when 64 then @c7 else [Longitude] end,
		[Latitude] = case substring(@bitmap,1,1) & 128 when 128 then @c8 else [Latitude] end,
		[GeocodeEasting] = case substring(@bitmap,2,1) & 1 when 1 then @c9 else [GeocodeEasting] end,
		[GeocodeNorthing] = case substring(@bitmap,2,1) & 2 when 2 then @c10 else [GeocodeNorthing] end,
		[DistanceFromPostcode] = case substring(@bitmap,2,1) & 4 when 4 then @c11 else [DistanceFromPostcode] end,
		[MinWages] = case substring(@bitmap,2,1) & 8 when 8 then @c12 else [MinWages] end,
		[MaxWages] = case substring(@bitmap,2,1) & 16 when 16 then @c13 else [MaxWages] end,
		[VacancyReferenceNumber] = case substring(@bitmap,2,1) & 32 when 32 then @c14 else [VacancyReferenceNumber] end,
		[Employer] = case substring(@bitmap,2,1) & 64 when 64 then @c15 else [Employer] end,
		[TrainingProvider] = case substring(@bitmap,2,1) & 128 when 128 then @c16 else [TrainingProvider] end,
		[Keywords] = case substring(@bitmap,3,1) & 1 when 1 then @c17 else [Keywords] end,
		[DateSearched] = case substring(@bitmap,3,1) & 2 when 2 then @c18 else [DateSearched] end,
		[BackgroundSearch] = case substring(@bitmap,3,1) & 4 when 4 then @c19 else [BackgroundSearch] end,
		[AlertSent] = case substring(@bitmap,3,1) & 8 when 8 then @c20 else [AlertSent] end,
		[CountBackgroundMatches] = case substring(@bitmap,3,1) & 16 when 16 then @c21 else [CountBackgroundMatches] end,
		[VacancyPostedSince] = case substring(@bitmap,3,1) & 32 when 32 then @c22 else [VacancyPostedSince] end,
		[ApprenticeshipTypeId] = case substring(@bitmap,3,1) & 64 when 64 then @c23 else [ApprenticeshipTypeId] end
where [SavedSearchCriteriaId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end