create function [dbo].[fnReportGenerateVacancyRefNo] (@vacancyRefNo int)
returns nvarchar( 12 )
as
begin
 
declare @vacancyNumberLength int = 9;
declare @vacancyRefString nvarchar( 10 ) = convert( nvarchar(10), @vacancyRefNo ); 

return 'VAC' + replicate( '0', @vacancyNumberLength - len( @vacancyRefString ) ) + @vacancyRefString;

end