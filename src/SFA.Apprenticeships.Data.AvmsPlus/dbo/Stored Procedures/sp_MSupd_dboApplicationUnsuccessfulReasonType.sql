create procedure [sp_MSupd_dboApplicationUnsuccessfulReasonType]
		@c1 int = NULL,
		@c2 nvarchar(3) = NULL,
		@c3 nvarchar(10) = NULL,
		@c4 nvarchar(100) = NULL,
		@c5 int = NULL,
		@c6 nvarchar(900) = NULL,
		@c7 nvarchar(100) = NULL,
		@c8 bit = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[ApplicationUnsuccessfulReasonType] set
		[CodeName] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [CodeName] end,
		[ShortName] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [ShortName] end,
		[FullName] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [FullName] end,
		[ReferralPoints] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [ReferralPoints] end,
		[CandidateDisplayText] = case substring(@bitmap,1,1) & 32 when 32 then @c6 else [CandidateDisplayText] end,
		[CandidateFullName] = case substring(@bitmap,1,1) & 64 when 64 then @c7 else [CandidateFullName] end,
		[Withdrawn] = case substring(@bitmap,1,1) & 128 when 128 then @c8 else [Withdrawn] end
where [ApplicationUnsuccessfulReasonTypeId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end