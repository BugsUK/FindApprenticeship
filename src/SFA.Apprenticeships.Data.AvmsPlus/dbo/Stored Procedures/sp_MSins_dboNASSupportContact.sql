create procedure [sp_MSins_dboNASSupportContact]
    @c1 int,
    @c2 int,
    @c3 nvarchar(100)
as
begin  
	insert into [dbo].[NASSupportContact](
		[NASSupportContactId],
		[ManagingAreaID],
		[EmailAddress]
	) values (
    @c1,
    @c2,
    @c3	) 
end