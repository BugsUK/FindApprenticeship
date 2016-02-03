create procedure [sp_MSins_dboVacancyOwnerRelationship]
    @c1 int,
    @c2 int,
    @c3 int,
    @c4 bit,
    @c5 bit,
    @c6 int,
    @c7 varchar(4000),
    @c8 nvarchar(max),
    @c9 nvarchar(256),
    @c10 int,
    @c11 bit
as
begin  
	insert into [dbo].[VacancyOwnerRelationship](
		[VacancyOwnerRelationshipId],
		[EmployerId],
		[ProviderSiteID],
		[ContractHolderIsEmployer],
		[ManagerIsEmployer],
		[StatusTypeId],
		[Notes],
		[EmployerDescription],
		[EmployerWebsite],
		[EmployerLogoAttachmentId],
		[NationWideAllowed]
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
    @c11	) 
end