create procedure [sp_MSupd_dboProvider]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 int = NULL,
		@c4 nvarchar(255) = NULL,
		@c5 nvarchar(255) = NULL,
		@c6 bit = NULL,
		@c7 datetime = NULL,
		@c8 datetime = NULL,
		@c9 int = NULL,
		@c10 bit = NULL,
		@c11 int = NULL,
		@pkc1 int = NULL,
		@bitmap binary(2)
as
begin  
update [dbo].[Provider] set
		[UPIN] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [UPIN] end,
		[UKPRN] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [UKPRN] end,
		[FullName] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [FullName] end,
		[TradingName] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [TradingName] end,
		[IsContracted] = case substring(@bitmap,1,1) & 32 when 32 then @c6 else [IsContracted] end,
		[ContractedFrom] = case substring(@bitmap,1,1) & 64 when 64 then @c7 else [ContractedFrom] end,
		[ContractedTo] = case substring(@bitmap,1,1) & 128 when 128 then @c8 else [ContractedTo] end,
		[ProviderStatusTypeID] = case substring(@bitmap,2,1) & 1 when 1 then @c9 else [ProviderStatusTypeID] end,
		[IsNASProvider] = case substring(@bitmap,2,1) & 2 when 2 then @c10 else [IsNASProvider] end,
		[OriginalUPIN] = case substring(@bitmap,2,1) & 4 when 4 then @c11 else [OriginalUPIN] end
where [ProviderID] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end