create procedure [sp_MSins_dboProvider]
    @c1 int,
    @c2 int,
    @c3 int,
    @c4 nvarchar(255),
    @c5 nvarchar(255),
    @c6 bit,
    @c7 datetime,
    @c8 datetime,
    @c9 int,
    @c10 bit,
    @c11 int
as
begin  
	insert into [dbo].[Provider](
		[ProviderID],
		[UPIN],
		[UKPRN],
		[FullName],
		[TradingName],
		[IsContracted],
		[ContractedFrom],
		[ContractedTo],
		[ProviderStatusTypeID],
		[IsNASProvider],
		[OriginalUPIN]
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