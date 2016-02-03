create procedure [sp_MSupd_dboSystemParameters]
		@c1 int = NULL,
		@c2 nvarchar(100) = NULL,
		@c3 nvarchar(100) = NULL,
		@c4 nvarchar(300) = NULL,
		@c5 bit = NULL,
		@c6 int = NULL,
		@c7 int = NULL,
		@c8 nvarchar(600) = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[SystemParameters] set
		[ParameterName] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [ParameterName] end,
		[ParameterType] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [ParameterType] end,
		[ParameterValue] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [ParameterValue] end,
		[Editable] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [Editable] end,
		[LowerLimit] = case substring(@bitmap,1,1) & 32 when 32 then @c6 else [LowerLimit] end,
		[UpperLimit] = case substring(@bitmap,1,1) & 64 when 64 then @c7 else [UpperLimit] end,
		[Description] = case substring(@bitmap,1,1) & 128 when 128 then @c8 else [Description] end
where [SystemParametersId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end