CREATE VIEW [MI_Views].[WageTypes_Vw]
AS
     SELECT WageTypeID,
            FullName AS WageTypeName
     FROM dbo.WageType;