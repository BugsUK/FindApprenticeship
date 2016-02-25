CREATE PROCEDURE [dbo].[uspGetAllDivisions]

AS

SELECT [DivisionID]
      ,[DivisionCode]
      ,[DivisionShortName]
      ,[DivisionFullName]
      ,[DivisionTypeId]
      ,[DivisionGroupPurposeId]
  FROM [vwDivisions]