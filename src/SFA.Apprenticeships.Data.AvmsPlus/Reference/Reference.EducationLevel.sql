CREATE TABLE [Reference].[EducationLevel] (
    [EducationLevelId]		  INT           NOT NULL IDENTITY (17, 1),
    [CodeName]                NVARCHAR (3)  NOT NULL,
    [ShortName]               NVARCHAR (10) NOT NULL,
    [FullName]                NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_EducationLevel] PRIMARY KEY CLUSTERED ([EducationLevelId] ASC),
	CONSTRAINT [uq_idx_EducationLevel] UNIQUE NONCLUSTERED ([FullName] ASC)
);

