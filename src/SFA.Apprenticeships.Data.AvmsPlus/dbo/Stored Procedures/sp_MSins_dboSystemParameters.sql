create procedure [sp_MSins_dboSystemParameters]
    @c1 int,
    @c2 nvarchar(100),
    @c3 nvarchar(100),
    @c4 nvarchar(300),
    @c5 bit,
    @c6 int,
    @c7 int,
    @c8 nvarchar(600)
as
begin  
	insert into [dbo].[SystemParameters](
		[SystemParametersId],
		[ParameterName],
		[ParameterType],
		[ParameterValue],
		[Editable],
		[LowerLimit],
		[UpperLimit],
		[Description]
	) values (
    @c1,
    @c2,
    @c3,
    @c4,
    @c5,
    @c6,
    @c7,
    @c8	) 
end