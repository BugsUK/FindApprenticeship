create procedure [sp_MSins_dboEducationResultLevel]
    @c1 int,
    @c2 nvarchar(5),
    @c3 nvarchar(10),
    @c4 nvarchar(150)
as
begin  
	insert into [dbo].[EducationResultLevel](
		[EducationResultLevelId],
		[CodeName],
		[ShortName],
		[FullName]
	) values (
    @c1,
    @c2,
    @c3,
    @c4	) 
end