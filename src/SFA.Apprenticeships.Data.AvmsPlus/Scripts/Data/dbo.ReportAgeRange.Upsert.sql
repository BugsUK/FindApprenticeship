MERGE INTO [dbo].[ReportAgeRanges] AS Target 
USING (VALUES 
	(1, N'Up to 16', 0, 15),
	(2, N'16 - 18', 16, 18),
	(3, N'Up to 20', 0, 19),
	(4, N'19 - 24', 19, 24),
	(5, N'25 +', 25, 250)
) 
AS Source (ReportAgeRangeID, ReportAgeRangeLabel, MinYears, MaxYears) 
ON Target.ReportAgeRangeID = Source.ReportAgeRangeID 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET ReportAgeRangeLabel = Source.ReportAgeRangeLabel, MinYears = Source.MinYears, MaxYears = Source.MaxYears
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (ReportAgeRangeID, ReportAgeRangeLabel, MinYears, MaxYears) 
VALUES (ReportAgeRangeID, ReportAgeRangeLabel, MinYears, MaxYears) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;