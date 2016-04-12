SET IDENTITY_INSERT [dbo].[ApplicationUnsuccessfulReasonType] ON
GO

MERGE INTO [dbo].[ApplicationUnsuccessfulReasonType] AS Target 
USING (VALUES 
(0, N'', N'', N'Please Select...', 0, N'n/a', N'Please Select...', 1),
(1, N'CIP', N'CIP', N'You’re not eligible for an apprenticeship', 6, N'Thank you for your application. Unfortunately you&rsquo;ve been unsuccessful because you&rsquo;re not eligible for an apprenticeship. Please contact the&nbsp;training provider for further information.<br />
<br />
For careers advice and support visit the <a href="https://nationalcareersservice.direct.gov.uk/pages/home.aspx" target="_blank">National Careers Service</a>.<br />', N'', 0),
(2, N'CIO', N'CIO', N'You haven’t met the requirements', 2, N'Thank you for your application. Unfortunately you&rsquo;ve been unsuccessful because you didn&rsquo;t meet the requirements for the apprenticeship. Please&nbsp;contact the training provider for further information.<br />
<br />
For careers advice and support visit the <a href="https://nationalcareersservice.direct.gov.uk/pages/home.aspx" target="_blank">National Careers Service</a>.', N'', 0),
(3, N'SER', N'SER', N'You met the employer’s/provider''s requirements but have been unsuccessful', 1, N'Thank you for your application. Unfortunately, despite meeting the employer''s/training provider''s requirements for the apprenticeship, you&rsquo;ve been unsuccessful. Please contact them for further information.<br />
<br />
For careers advice and support visit the <a href="https://nationalcareersservice.direct.gov.uk/pages/home.aspx" target="_blank">National Careers Service</a>', N'', 0),
(4, N'UUC', N'UUC', N'You didn’t attend the interview', 3, N'Thank you for your application. You&rsquo;ve been unsuccessful because you didn&rsquo;t attend the interview. Please contact the training provider for further&nbsp;information.<br />
<br />
For careers advice and support visit the <a href="https://nationalcareersservice.direct.gov.uk/pages/home.aspx" target="_blank">National Careers Service</a>.', N'', 0),
(5, N'VAF', N'VAF', N'The apprenticeship has been withdrawn', 1, N'Thank you for your application. Unfortunately the apprenticeship has been withdrawn. Please contact the training provider for further information.<br />
<br />
For careers advice and support visit the <a href="https://nationalcareersservice.direct.gov.uk/pages/home.aspx" target="_blank">National Careers Service</a>.<br />', N'', 0),
(6, N'CNS', N'CNS', N'You''ve been unsuccessful - other', 6, N'Thank you for your application. Unfortunately you&rsquo;ve been unsuccessful on this occasion. Please contact the training provider for further information.<br />
<br />
For careers advice and support visit the <a href="https://nationalcareersservice.direct.gov.uk/pages/home.aspx" target="_blank">National Careers Service</a>.<br />', N'', 0),
(7, N'VJD', N'VJD', N'Not suitable for vacancy due to journey / distance involved', 1, N'Thank you for your application. Unfortunately on this occasion you have not been successful with your application. For further information please refer to the Learning Provider or Employer offering the opportunity.', NULL, NULL),
(8, N'FIA', N'FIA', N'Failed initial assessment test', 	1, N'Thank you for your application. Unfortunately on this occasion you have not been successful with your application. For further information please refer to the Learning Provider or Employer offering the opportunity.', NULL, NULL),
(9, N'EWV', N'EWV', N'Employer withdrew vacancy', 0, N'Thank you for your application. On this occasion the employer has decided not to proceed with this vacancy. For further information please refer to the Learning Provider or Employer offering the opportunity.', NULL, NULL),
(10, N'AWC', N'AWC', N'Accepted an alternative apprenticeship position', 0, N'Thank you for your application. I understand that you are no longer interested in progressing at this point.', N'Accepted an alternative apprenticeship position', 1),
(11, N'MSV', N'MSV', N'Have found other employment', 0, N'Thank you for your application. I understand that you are no longer interested in progressing at this point.', N'Have found other employment', 1),
(12, N'CNI', N'CNI', N'Taken up other learning or education', 0, N'Thank you for your application. I understand that you are no longer interested in progressing at this point.', N'Taken up other learning or education', 1),
(13, N'COE', N'COE', N'Other reason for Withdrawing Application', 0, N'Thank you for your application. I understand that you are no longer interested in progressing at this point.', N'Other reason for Withdrawing Application', 1),
(14, N'OTH', N'OTH', N'Other', 3, N'Thank you for your application. Unfortunately on this occasion you have not been successful with your application. For further information please refer to the Learning Provider or Employer offering the opportunity.', NULL, NULL),
(15, N'15', N'15', N'You’re not eligible for a traineeship', 6, N'Thank you for your application. Unfortunately you&rsquo;ve been unsuccessful because you&rsquo;re not eligible for a traineeship. Please contact the training&nbsp;provider for further information.<br />
<br />
For careers advice and support visit the <a href="https://nationalcareersservice.direct.gov.uk/pages/home.aspx" target="_blank">National Careers Service</a>.', N'', 0),
(16, N'16', N'16', N'The training provider couldn’t contact you', 1, N'Thank you for your application. Unfortunately it couldn&rsquo;t be processed because the training provider couldn&rsquo;t contact you. Please check your&nbsp;contact details are correct.<br />
<br />
For careers advice and support visit the <a href="https://nationalcareersservice.direct.gov.uk/pages/home.aspx" target="_blank">National Careers Service</a>.', N'', 0),
(17, N'17', N'17', N'Offered the position but turned it down', 0, N'You have been made unsuccessful as you have declined the offer made to you', NULL, 0)
) 
AS Source (ApplicationUnsuccessfulReasonTypeId, CodeName, ShortName, FullName, ReferralPoints, CandidateDisplayText, CandidateFullName, Withdrawn) 
ON Target.ApplicationUnsuccessfulReasonTypeId = Source.ApplicationUnsuccessfulReasonTypeId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName, ReferralPoints = Source.ReferralPoints, CandidateDisplayText = Source.CandidateDisplayText, CandidateFullName = Source.CandidateFullName, Withdrawn = Source.Withdrawn
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (ApplicationUnsuccessfulReasonTypeId, CodeName, ShortName, FullName, ReferralPoints, CandidateDisplayText, CandidateFullName, Withdrawn) 
VALUES (ApplicationUnsuccessfulReasonTypeId, CodeName, ShortName, FullName, ReferralPoints, CandidateDisplayText, CandidateFullName, Withdrawn) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[ApplicationUnsuccessfulReasonType] OFF
GO