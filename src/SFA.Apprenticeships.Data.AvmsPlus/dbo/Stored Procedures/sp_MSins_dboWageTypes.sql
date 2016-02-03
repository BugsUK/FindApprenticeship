create procedure [sp_MSins_dboWageTypes]
    @c1 int,
    @c2 nvarchar(20)
as
begin  
	insert into [dbo].[WageTypes](
		[WageTypeID],
		[WageTypeName]
	) values (
    @c1,
    @c2	) 
end