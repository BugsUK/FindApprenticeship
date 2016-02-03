create procedure [sp_MSins_dboUniqueKeyRegister]
    @c1 int,
    @c2 nchar(2),
    @c3 nvarchar(30),
    @c4 datetime
as
begin  
	insert into [dbo].[UniqueKeyRegister](
		[UniqueKeyRegisterId],
		[KeyType],
		[KeyValue],
		[DateTimeStamp]
	) values (
    @c1,
    @c2,
    @c3,
    @c4	) 
end