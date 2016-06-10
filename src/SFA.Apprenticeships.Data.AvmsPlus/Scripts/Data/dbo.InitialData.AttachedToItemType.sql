SET identity_insert AttachedtoItemType ON
MERGE INTO dbo.AttachedtoItemType as TARGET 
USING 
(VALUES
('1','APP','APP','Application'),
('2','CAN','CAN','Candidate'),
('3','EMP','EMP','Employer'),
('4','TP','TP','Training Provider'),
('5','VAC','VAC','Vacancy'),
('6','SP','SP','System Parameter'),
('7','UR','UR','Unsuccessful Reasons'),
('8','WDR','WDR','Withdrawn/Declined Reasons'),
('9','LR','LR','Lsc Region'),
('10','TAC','TAC','TermsAndConditions'),
('11','EXS','EXS','External Systems')
)
AS SOURCE (AttachedtoItemTypeId, CodeName, ShortName, FullName) 
ON TARGET.AttachedtoItemTypeId = SOURCE.AttachedtoItemTypeId
WHEN MATCHED THEN
	UPDATE SET TARGET.CodeName = SOURCE.Codename, 
	TARGET.ShortName = SOURCE.ShortName, 
	TARGET.FullName = SOURCE.FullName
WHEN NOT MATCHED BY TARGET THEN
	INSERT (AttachedtoItemTypeId, CodeName, ShortName, FullName) 
	Values (SOURCE.AttachedtoItemTypeId,SOURCE.Codename,SOURCE.ShortName, SOURCE.FullName)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;
set identity_insert AttachedtoItemType OFF

