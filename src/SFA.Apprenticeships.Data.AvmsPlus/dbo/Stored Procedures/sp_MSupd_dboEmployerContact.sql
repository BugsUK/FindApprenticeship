create procedure [sp_MSupd_dboEmployerContact]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 nvarchar(50) = NULL,
		@c4 nvarchar(50) = NULL,
		@c5 nvarchar(50) = NULL,
		@c6 nvarchar(50) = NULL,
		@c7 nvarchar(50) = NULL,
		@c8 nvarchar(50) = NULL,
		@c9 int = NULL,
		@c10 nvarchar(8) = NULL,
		@c11 int = NULL,
		@c12 nvarchar(50) = NULL,
		@c13 nvarchar(50) = NULL,
		@c14 nvarchar(16) = NULL,
		@c15 nvarchar(16) = NULL,
		@c16 int = NULL,
		@c17 nvarchar(50) = NULL,
		@pkc1 int = NULL,
		@bitmap binary(3)
as
begin  
update [dbo].[EmployerContact] set
		[PersonId] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [PersonId] end,
		[AddressLine1] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [AddressLine1] end,
		[AddressLine2] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [AddressLine2] end,
		[AddressLine3] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [AddressLine3] end,
		[AddressLine4] = case substring(@bitmap,1,1) & 32 when 32 then @c6 else [AddressLine4] end,
		[AddressLine5] = case substring(@bitmap,1,1) & 64 when 64 then @c7 else [AddressLine5] end,
		[Town] = case substring(@bitmap,1,1) & 128 when 128 then @c8 else [Town] end,
		[CountyId] = case substring(@bitmap,2,1) & 1 when 1 then @c9 else [CountyId] end,
		[PostCode] = case substring(@bitmap,2,1) & 2 when 2 then @c10 else [PostCode] end,
		[LocalAuthorityId] = case substring(@bitmap,2,1) & 4 when 4 then @c11 else [LocalAuthorityId] end,
		[JobTitle] = case substring(@bitmap,2,1) & 8 when 8 then @c12 else [JobTitle] end,
		[Department] = case substring(@bitmap,2,1) & 16 when 16 then @c13 else [Department] end,
		[FaxNumber] = case substring(@bitmap,2,1) & 32 when 32 then @c14 else [FaxNumber] end,
		[AlternatePhoneNumber] = case substring(@bitmap,2,1) & 64 when 64 then @c15 else [AlternatePhoneNumber] end,
		[ContactPreferenceTypeId] = case substring(@bitmap,2,1) & 128 when 128 then @c16 else [ContactPreferenceTypeId] end,
		[Availability] = case substring(@bitmap,3,1) & 1 when 1 then @c17 else [Availability] end
where [EmployerContactId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end