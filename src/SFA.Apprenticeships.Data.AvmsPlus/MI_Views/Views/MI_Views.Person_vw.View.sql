CREATE VIEW [MI_Views].[Person_vw]
AS
     SELECT [PersonId],
            [Title],
            [OtherTitle],
            '' AS [FirstName],
            '' AS [MiddleNames],
            '' AS [Surname],
            '' AS [LandlineNumber],
            '' AS [MobileNumber],
            '' AS [Email],
            [PersonTypeId]
     FROM [Person]
     WHERE [PersonTypeId] = 1
     UNION ALL
     SELECT [PersonId],
            [Title],
            [OtherTitle],
            [FirstName],
            [MiddleNames],
            [Surname],
            [LandlineNumber],
            [MobileNumber],
            [Email],
            [PersonTypeId]
     FROM [Person]
     WHERE [PersonTypeId] <> 1;