create procedure [sp_MSupd_dboSectorSuccessRates]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 smallint = NULL,
		@c4 bit = NULL,
		@pkc1 int = NULL,
		@pkc2 int = NULL,
		@bitmap binary(1)
as
begin  
if (substring(@bitmap,1,1) & 1 = 1) or
 (substring(@bitmap,1,1) & 2 = 2)
begin 
update [dbo].[SectorSuccessRates] set
		[ProviderID] = case substring(@bitmap,1,1) & 1 when 1 then @c1 else [ProviderID] end,
		[SectorID] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [SectorID] end,
		[PassRate] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [PassRate] end,
		[New] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [New] end
where [ProviderID] = @pkc1
  and [SectorID] = @pkc2
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end  
else
begin 
update [dbo].[SectorSuccessRates] set
		[PassRate] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [PassRate] end,
		[New] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [New] end
where [ProviderID] = @pkc1
  and [SectorID] = @pkc2
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end 
end