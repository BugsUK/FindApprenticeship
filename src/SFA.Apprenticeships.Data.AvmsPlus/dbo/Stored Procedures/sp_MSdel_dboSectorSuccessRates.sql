create procedure [sp_MSdel_dboSectorSuccessRates]
		@pkc1 int,
		@pkc2 int
as
begin  
	delete [dbo].[SectorSuccessRates]
where [ProviderID] = @pkc1
  and [SectorID] = @pkc2
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end