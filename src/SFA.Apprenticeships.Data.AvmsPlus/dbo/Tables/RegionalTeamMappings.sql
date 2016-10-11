CREATE TABLE [dbo].[RegionalTeamMappings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PostcodeStart] [varchar](6) NOT NULL,
	[RegionalTeam_Id] [int] NOT NULL,
 CONSTRAINT [PK_RegionalTeamMappings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[RegionalTeamMappings]  WITH CHECK ADD  CONSTRAINT [FK_RegionalTeamMappings_RegionalTeams] FOREIGN KEY([RegionalTeam_Id])
REFERENCES [dbo].[RegionalTeams] ([Id])
GO