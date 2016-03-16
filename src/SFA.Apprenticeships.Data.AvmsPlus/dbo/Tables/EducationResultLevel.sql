CREATE TABLE [dbo].[EducationResultLevel] (
    [EducationResultLevelId] INT            IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [CodeName]               NVARCHAR (5)   NOT NULL,
    [ShortName]              NVARCHAR (10)  NOT NULL,
    [FullName]               NVARCHAR (150) NOT NULL,
    CONSTRAINT [PK_EducationResultLevel] PRIMARY KEY CLUSTERED ([EducationResultLevelId] ASC),
    CONSTRAINT [uq_idx_EducationResultLevel] UNIQUE NONCLUSTERED ([FullName] ASC)
);

