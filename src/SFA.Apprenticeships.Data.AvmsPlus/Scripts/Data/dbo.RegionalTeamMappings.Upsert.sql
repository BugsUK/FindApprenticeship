MERGE INTO [dbo].[RegionalTeamMappings] AS Target 
USING (VALUES 
	('DH', 1),
	('DL', 1),
	('HG', 1),
	('NE', 1),
	('SR', 1),
	('TS', 1),
	('YO', 1),

	('BB', 2),
	('BL', 2),
	('CA', 2),
	('CW', 2),
	('FY', 2),
	('L', 2),
	('LA', 2),
	('M', 2),
	('OL', 2),
	('PR', 2),
	('SK', 2),
	('WA', 2),
	('WN', 2),
	('CH', 2),

	('BD', 3),
	('DN', 3),
	('HD', 3),
	('HU', 3),
	('HX', 3),
	('LN', 3),
	('LS', 3),
	('S', 3),
	('WF', 3),

	('DE', 4),
	('LE', 4),
	('NG', 4),
	('NN', 4),

	('B', 5),
	('CV', 5),
	('DY', 5),
	('HR', 5),
	('ST', 5),
	('SY', 5),
	('TF', 5),
	('WR', 5),
	('WS', 5),
	('WV', 5),

	('AL', 6),
	('CB', 6),
	('CM', 6),
	('CO', 6),
	('IG', 6),
	('IP', 6),
	('LU', 6),
	('NR', 6),
	('PE', 6),
	('RM', 6),
	('SG', 6),
	('SS', 6),
	('WD', 6),

	('BN', 7),
	('BR', 7),
	('CR', 7),
	('CT', 7),
	('DA', 7),
	('GU', 7),
	('EN', 7),
	('HA', 7),
	('HP', 7),
	('KT', 7),
	('ME', 7),
	('MK', 7),
	('OX', 7),
	('RG', 7),
	('RH', 7),
	('SL', 7),
	('SM', 7),
	('SO', 7),
	('TN', 7),
	('TW', 7),
	('UB', 7),
	('E', 7),
	('EC', 7),
	('N', 7),
	('NW', 7),
	('SE', 7),
	('SW', 7),
	('W', 7),
	('WC', 7),

	('BA', 8),
	('BH', 8),
	('BS', 8),
	('DT', 8),
	('EX', 8),
	('GL', 8),
	('PL', 8),
	('PO', 8),
	('SN', 8),
	('SP', 8),
	('TA', 8),
	('TQ', 8),
	('TR', 8)
) 

AS Source (PostcodeStart, RegionalTeam_Id) 
ON Target.PostcodeStart = Source.PostcodeStart
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET PostcodeStart = Source.PostcodeStart, RegionalTeam_Id = Source.RegionalTeam_Id
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (PostcodeStart, RegionalTeam_Id) 
VALUES (PostcodeStart, RegionalTeam_Id) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;
