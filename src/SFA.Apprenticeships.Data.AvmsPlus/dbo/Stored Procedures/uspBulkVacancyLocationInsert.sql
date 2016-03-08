CREATE PROCEDURE [dbo].[uspBulkVacancyLocationInsert]
	@VacancyId int,
	@NumberOfPositions int,
	@AddressLine1 nvarchar(50),
	@AddressLine2 nvarchar(50),
	@AddressLine3 nvarchar(50),
	@AddressLine4 nvarchar(50),
	@AddressLine5 nvarchar(50),
	@Town nvarchar(40),
	@CountyName nvarchar(150),
	@PostCode nvarchar(8),
	@localAuthority nvarchar(8),
	@GeocodeEasting decimal(22, 11),  
	@GeocodeNorthing decimal(22, 11),  
	@Longitude decimal(22, 11),  
	@Latitude decimal(22, 11), 
	@EmployersWebsite nvarchar(256),
	@errorCode int output
		            
as
            
begin 

set nocount on

select @errorCode = 0

declare @localAuthorityId int
if (@localAuthority is null)
	select @localAuthorityId = null
else
begin
	select @localAuthorityId = LocalAuthorityId from LocalAuthority where CodeName = @localAuthority
	if (@localAuthorityId is null)
		select @errorCode = -10063 --Invalid local authority
end

declare @CountyId int
select @CountyId = CountyId from dbo.County where FullName = @CountyName
if @CountyId is null
	select @errorCode = -10038 --MLCountyInvalid

if (0 = @errorCode)
begin			
	insert into [dbo].[VacancyLocation]	(
			VacancyId,
			NumberOfPositions,
			AddressLine1,
			AddressLine2,
			AddressLine3,
			AddressLine4,
			AddressLine5,
			Town,
			CountyId,
			Postcode,
			LocalAuthorityId,
			GeocodeEasting,
			GeocodeNorthing,
			Longitude,
			Latitude,
			EmployersWebsite
		)
				
	values (
			@VacancyId,
			@NumberOfPositions,
			@AddressLine1,
			@AddressLine2,
			@AddressLine3,
			@AddressLine4,
			@AddressLine5,
			@Town,
			@CountyId,
			@PostCode,
			@LocalAuthorityId,
			@GeocodeEasting,  
			@GeocodeNorthing,  
			@Longitude,  
			@Latitude, 
			@EmployersWebsite
		)			
end
		
set nocount off
            
end