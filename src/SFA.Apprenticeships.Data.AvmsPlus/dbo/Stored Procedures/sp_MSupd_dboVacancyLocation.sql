create procedure [sp_MSupd_dboVacancyLocation]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 smallint = NULL,
		@c4 nvarchar(50) = NULL,
		@c5 nvarchar(50) = NULL,
		@c6 nvarchar(50) = NULL,
		@c7 nvarchar(50) = NULL,
		@c8 nvarchar(50) = NULL,
		@c9 nvarchar(40) = NULL,
		@c10 int = NULL,
		@c11 nvarchar(8) = NULL,
		@c12 int = NULL,
		@c13 int = NULL,
		@c14 int = NULL,
		@c15 decimal(13,10) = NULL,
		@c16 decimal(13,10) = NULL,
		@c17 nvarchar(256) = NULL,
		@pkc1 int = NULL,
		@bitmap binary(3)
as
begin  
update [dbo].[VacancyLocation] set
		[VacancyId] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [VacancyId] end,
		[NumberofPositions] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [NumberofPositions] end,
		[AddressLine1] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [AddressLine1] end,
		[AddressLine2] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [AddressLine2] end,
		[AddressLine3] = case substring(@bitmap,1,1) & 32 when 32 then @c6 else [AddressLine3] end,
		[AddressLine4] = case substring(@bitmap,1,1) & 64 when 64 then @c7 else [AddressLine4] end,
		[AddressLine5] = case substring(@bitmap,1,1) & 128 when 128 then @c8 else [AddressLine5] end,
		[Town] = case substring(@bitmap,2,1) & 1 when 1 then @c9 else [Town] end,
		[CountyId] = case substring(@bitmap,2,1) & 2 when 2 then @c10 else [CountyId] end,
		[PostCode] = case substring(@bitmap,2,1) & 4 when 4 then @c11 else [PostCode] end,
		[LocalAuthorityId] = case substring(@bitmap,2,1) & 8 when 8 then @c12 else [LocalAuthorityId] end,
		[GeocodeEasting] = case substring(@bitmap,2,1) & 16 when 16 then @c13 else [GeocodeEasting] end,
		[GeocodeNorthing] = case substring(@bitmap,2,1) & 32 when 32 then @c14 else [GeocodeNorthing] end,
		[Longitude] = case substring(@bitmap,2,1) & 64 when 64 then @c15 else [Longitude] end,
		[Latitude] = case substring(@bitmap,2,1) & 128 when 128 then @c16 else [Latitude] end,
		[EmployersWebsite] = case substring(@bitmap,3,1) & 1 when 1 then @c17 else [EmployersWebsite] end
where [VacancyLocationId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end