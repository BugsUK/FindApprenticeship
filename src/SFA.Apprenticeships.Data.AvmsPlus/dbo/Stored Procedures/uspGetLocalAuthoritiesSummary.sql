create procedure [dbo].[uspGetLocalAuthoritiesSummary]
as

select 
	 la.LocalAuthorityId
	,la.CodeName
	,la.FullName
	,c.FullName as 'County'	
from LocalAuthority la
left outer join County c on c.CountyId = la.CountyId
where 0 != la.LocalAuthorityId