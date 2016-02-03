create procedure [sp_MSins_dboCAFFieldsFieldType]
    @c1 smallint,
    @c2 nvarchar(3),
    @c3 nvarchar(100),
    @c4 nvarchar(200)
as
begin  
	insert into [dbo].[CAFFieldsFieldType](
		[CAFFieldsFieldTypeId],
		[CodeName],
		[ShortName],
		[FullName]
	) values (
    @c1,
    @c2,
    @c3,
    @c4	) 
end