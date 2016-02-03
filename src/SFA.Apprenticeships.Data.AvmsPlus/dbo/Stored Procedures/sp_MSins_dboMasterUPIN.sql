create procedure [sp_MSins_dboMasterUPIN]
    @c1 int,
    @c2 int,
    @c3 int
as
begin  
	insert into [dbo].[MasterUPIN](
		[MasterUPINId],
		[UKPRN],
		[UPIN]
	) values (
    @c1,
    @c2,
    @c3	) 
end