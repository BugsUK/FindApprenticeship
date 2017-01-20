SET IDENTITY_INSERT [dbo].[VacancyReferralCommentsFieldType] ON
GO

MERGE INTO [dbo].[VacancyReferralCommentsFieldType] AS Target 
USING (VALUES 
	(1, 'TIT', 'TIT', 'Title'),
	(2, 'SDE', 'SDE', 'Short Description'),
	(3, 'FDE', 'FDE', 'Full Description'),
	(4, 'WWK', 'WWK', 'Working Week'),
	(5, 'WWG', 'WWG', 'Weekly Wage'),
	(6, 'FUT', 'FUT', 'Future Prospects'),
	(7, 'CFS', 'CFS', 'Contact Name for NAS Support'),
	(8, 'EAN', 'EAN', 'Employer''s Anonymous Name'),
	(9, 'EDE', 'EDE', 'Employer''s Description'),
	(10, 'EWB', 'EWB', 'Employer''s Website'),
	(11, 'APO', 'APO', 'Apprenticeship Occupation'),
	(12, 'APF', 'AFO', 'Apprenticeship Framework'),
	(13, 'VTP', 'VTP', 'Vacancy Type'),
	(14, 'TRP', 'TRP', 'Training To Be Provided'),
	(15, 'EAD', 'EAD', 'Expected Apprenticeship Duration'),
	(16, 'SKL', 'SKL', 'Skills Required'),
	(17, 'QUA', 'QUA', 'Qualification Criteria'),
	(18, 'PER', 'PER', 'Personal Qualities'),
	(19, 'REA', 'REA', 'Reality Check'),
	(20, 'IOI', 'IOI', 'Important Other Information'),
	(21, 'QU1', 'QU1', 'Question 1'),
	(22, 'QU2', 'QU2', 'Question 2'),
	(23, 'CLD', 'CLD', 'Closing Date'),
	(24, 'ISF', 'ISF', 'Interviews Start From'),
	(25, 'PSD', 'PSD', 'Possible Start Date'),
	(26, 'EAI', 'EAI', 'Employer''s Application Instructions'),
	(27, 'AWA', 'AWA', 'Application Website Address'),
	(28, 'DRA', 'DRA', 'Display Recruitment Agency Name'),
	(29, 'NPO', 'NPO', 'Number of Positions'),
	(30, 'OAI', 'OAI', 'Offline Application Instructions'),
	(31, 'OAU', 'OAU', 'Offline Application Url'),
	(32, 'SID', 'SID', 'Standard Id'),
	(33, 'ALE', 'ALE', 'Apprenticeship Level'),
	(34, 'CDE', 'CDE', 'Contact Details'),
	(35, 'LAD', 'LAD', 'Location Addresses'),
	(36, 'ALI', 'ALI', 'Additional Location Information'),
	(37, 'AED', 'AED', 'Anonymous Employer Description'),
	(38, 'AER', 'AER', 'Anonymous Employer Reason'),
	(39, 'AAE', 'AAE', 'Anonymous About the Employer'),
	(40, 'OIN', 'OIN', 'Other Information')
) 

AS Source (VacancyReferralCommentsFieldTypeId, CodeName, ShortName, FullName) 
ON Target.VacancyReferralCommentsFieldTypeId = Source.VacancyReferralCommentsFieldTypeId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (VacancyReferralCommentsFieldTypeId, CodeName, ShortName, FullName) 
VALUES (VacancyReferralCommentsFieldTypeId, CodeName, ShortName, FullName) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[VacancyReferralCommentsFieldType] OFF
GO
