create procedure [sp_MSins_dboSectorSuccessRates]
    @c1 int,
    @c2 int,
    @c3 smallint,
    @c4 bit
as
begin  
	insert into [dbo].[SectorSuccessRates](
		[ProviderID],
		[SectorID],
		[PassRate],
		[New]
	) values (
    @c1,
    @c2,
    @c3,
    @c4	) 
end