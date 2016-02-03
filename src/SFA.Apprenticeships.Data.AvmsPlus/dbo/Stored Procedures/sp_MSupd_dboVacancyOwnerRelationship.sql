create procedure [sp_MSupd_dboVacancyOwnerRelationship]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 int = NULL,
		@c4 bit = NULL,
		@c5 bit = NULL,
		@c6 int = NULL,
		@c7 varchar(4000) = NULL,
		@c8 nvarchar(max) = NULL,
		@c9 nvarchar(256) = NULL,
		@c10 int = NULL,
		@c11 bit = NULL,
		@pkc1 int = NULL,
		@bitmap binary(2)
as
begin  
update [dbo].[VacancyOwnerRelationship] set
		[EmployerId] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [EmployerId] end,
		[ProviderSiteID] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [ProviderSiteID] end,
		[ContractHolderIsEmployer] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [ContractHolderIsEmployer] end,
		[ManagerIsEmployer] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [ManagerIsEmployer] end,
		[StatusTypeId] = case substring(@bitmap,1,1) & 32 when 32 then @c6 else [StatusTypeId] end,
		[Notes] = case substring(@bitmap,1,1) & 64 when 64 then @c7 else [Notes] end,
		[EmployerDescription] = case substring(@bitmap,1,1) & 128 when 128 then @c8 else [EmployerDescription] end,
		[EmployerWebsite] = case substring(@bitmap,2,1) & 1 when 1 then @c9 else [EmployerWebsite] end,
		[EmployerLogoAttachmentId] = case substring(@bitmap,2,1) & 2 when 2 then @c10 else [EmployerLogoAttachmentId] end,
		[NationWideAllowed] = case substring(@bitmap,2,1) & 4 when 4 then @c11 else [NationWideAllowed] end
where [VacancyOwnerRelationshipId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end