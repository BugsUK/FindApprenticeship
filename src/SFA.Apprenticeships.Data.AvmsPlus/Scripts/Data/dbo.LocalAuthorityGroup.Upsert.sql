MERGE INTO [dbo].[LocalAuthorityGroup] AS Target 
USING (VALUES 
	(0, N'NUL', N'NUL', N'Unspecified', 4, 3, NULL),
	(1, N'EM', N'EM', N'East Midlands', 2, 1, 2002),
	(2, N'EE', N'EE', N'Central Eastern', 2, 1, 2002),
	(3, N'LON', N'LON', N'London', 2, 1, 2004),
	(4, N'NE', N'NE', N'North East', 2, 1, 2001),
	(5, N'M', N'M', N'NW (GMCW)', 2, 1, 2001),
	(6, N'L', N'L', N'NW (LCRCL)', 2, 1, 2001),
	(7, N'SE', N'SE', N'South East', 2, 1, 2004),
	(8, N'RG', N'RG', N'Thames Valley', 2, 1, 2003),
	(9, N'RH', N'RH', N'South Central', 2, 1, 2003),
	(10, N'SW', N'SW', N'South West', 2, 1, 2003),
	(11, N'WM', N'WM', N'West Midlands', 2, 1, 2002),
	(12, N'YH', N'YH', N'Yorks and Humber', 2, 1, 2001),
	(13, N'NAC', N'NACS', N'Helpdesk', 2, 1, 2005),
	(1001, N'EM', N'EM', N'East Midlands', 4, 3, 2001),
	(1002, N'EE', N'EE', N'East of England', 4, 3, 2001),
	(1003, N'LON', N'LON', N'London', 4, 3, 2001),
	(1004, N'NE', N'NE', N'North East', 4, 3, NULL),
	(1005, N'NW', N'NW', N'North West', 4, 3, NULL),
	(1006, N'SE', N'SE', N'South East', 4, 3, NULL),
	(1007, N'SW', N'SW', N'South West', 4, 3, NULL),
	(1008, N'WM', N'WM', N'West Midlands', 4, 3, NULL),
	(1009, N'YH', N'YH', N'Yorkshire and The Humber', 4, 3, NULL),
	(2001, N'N', N'N', N'North', 1, 1, NULL),
	(2002, N'C', N'C', N'Central', 1, 1, NULL),
	(2003, N'S', N'S', N'South', 1, 1, NULL),
	(2004, N'LSE', N'LSE', N'London and South East', 1, 1, NULL),
	(2005, N'NAT', N'NAT', N'National', 1, 1, NULL),
	(3001, N'BS', N'Bristol', N'Bristol', 3, 2, NULL),
	(3002, N'TS', N'Middlesbrough', N'Middlesbrough', 3, 2, NULL),
	(3003, N'TV', N'Tees Valley', N'Tees Valley', 3, 2, NULL),
	(3004, N'BP', N'Blackpool', N'Blackpool', 3, 2, NULL),
	(3005, N'BF', N'Bradford', N'Bradford', 3, 2, NULL),
	(3006, N'DC', N'Doncaster', N'Doncaster', 3, 2, NULL),
	(3007, N'LS', N'Leeds', N'Leeds', 3, 2, NULL),
	(3008, N'MW', N'Medway', N'Medway', 3, 2, NULL),
	(3009, N'MK', N'Milton Keynes', N'Milton Keynes', 3, 2, NULL),
	(3010, N'PB', N'Peterborough', N'Peterborough', 3, 2, NULL),
	(3011, N'PM', N'Plymouth', N'Plymouth', 3, 2, NULL),
	(3012, N'RO', N'Rotherham', N'Rotherham', 3, 2, NULL),
	(3013, N'SF', N'Sheffield', N'Sheffield', 3, 2, NULL),
	(3014, N'SD', N'Swindon', N'Swindon', 3, 2, NULL),
	(3015, N'TF', N'Telford', N'Telford', 3, 2, NULL),
	(3016, N'TR', N'Thurrock', N'Thurrock', 3, 2, NULL),
	(3017, N'TB', N'Torbay', N'Torbay', 3, 2, NULL),
	(3018, N'WF', N'Wakefield', N'Wakefield', 3, 2, NULL),
	(3019, N'YK', N'York', N'York', 3, 2, NULL)
) 
AS Source (LocalAuthorityGroupID, CodeName, ShortName, FullName, LocalAuthorityGroupTypeID, LocalAuthorityGroupPurposeID, ParentLocalAuthorityGroupID) 
ON Target.LocalAuthorityGroupID = Source.LocalAuthorityGroupID 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName,
	ShortName = Source.ShortName, 
	FullName = Source.FullName, 
	LocalAuthorityGroupTypeID = Source.LocalAuthorityGroupTypeID, 
	LocalAuthorityGroupPurposeID = Source.LocalAuthorityGroupPurposeID, 
	ParentLocalAuthorityGroupID = Source.ParentLocalAuthorityGroupID
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (LocalAuthorityGroupID, CodeName, ShortName, FullName, LocalAuthorityGroupTypeID, LocalAuthorityGroupPurposeID, ParentLocalAuthorityGroupID) 
VALUES (LocalAuthorityGroupID, CodeName, ShortName, FullName, LocalAuthorityGroupTypeID, LocalAuthorityGroupPurposeID, ParentLocalAuthorityGroupID) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;