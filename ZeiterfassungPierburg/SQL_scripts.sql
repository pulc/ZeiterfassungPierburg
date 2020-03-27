create database	zeiterfassung


USE [zeiterfassung]
GO
/****** Object:  Table [dbo].[Fertigungsteil]    Script Date: 27-Mar-20 12:01:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fertigungsteil](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ZeichenNr] [nvarchar](20) NULL,
	[teZEIT] [decimal](7, 1) NULL,
	[Bezeichnung] [nvarchar](50) NULL,
	[AnzahlMA] [int] NULL,
	[istAktiv] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Mitarbeiter]    Script Date: 27-Mar-20 12:01:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mitarbeiter](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Kostenstelle] [int] NULL,
	[Personalnummer] [int] NULL,
	[Nachname] [nvarchar](30) NULL,
	[Vorname] [nvarchar](30) NULL,
	[Persongruppe] [int] NULL,
	[Beschäftigungsart] [nvarchar](15) NULL,
	[istAktiv] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MitarbeiterInSchicht]    Script Date: 27-Mar-20 12:01:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MitarbeiterInSchicht](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SchichtInfoID] [int] NULL,
	[MitarbeiterID] [int] NULL,
	[ProduktionsanlageID] [int] NULL,
	[FertigungsteilID] [int] NULL,
	[Stück] [int] NULL,
	[DirStunden] [decimal](3, 1) NULL,
	[InDirStunden] [decimal](3, 1) NULL,
	[IstInSAPEingetragen] [bit] NULL,
	[ErstelltAm] [datetime] NULL,
	[Bemerkung] [nvarchar](100) NULL,
	[Auswertung] [decimal](5, 2) NULL,
	[EingetragenVon] [nvarchar](40) NULL,
	[AnlageInSchicht] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Produktionsanlage]    Script Date: 27-Mar-20 12:01:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Produktionsanlage](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Bezeichner] [nvarchar](25) NULL,
	[Kostenstelle] [int] NULL,
	[SAPAPNr] [int] NULL,
	[IstEineMaschine] [bit] NULL,
	[istAktiv] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Schichtinfo]    Script Date: 27-Mar-20 12:01:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Schichtinfo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Art] [tinyint] NULL,
	[Datum] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TeileInProduktionsanlage]    Script Date: 27-Mar-20 12:01:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeileInProduktionsanlage](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProduktionsanlageID] [int] NULL,
	[FertigungsteilID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Zugriffsrechte]    Script Date: 27-Mar-20 12:01:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Zugriffsrechte](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Benutzername] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL,
	[Zugriffsebene] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Fertigungsteil] ADD  CONSTRAINT [defaultFKFertigungsteil]  DEFAULT ('true') FOR [istAktiv]
GO
ALTER TABLE [dbo].[Mitarbeiter] ADD  DEFAULT ('true') FOR [istAktiv]
GO
ALTER TABLE [dbo].[MitarbeiterInSchicht] ADD  DEFAULT ('false') FOR [IstInSAPEingetragen]
GO
ALTER TABLE [dbo].[Produktionsanlage] ADD  CONSTRAINT [f_default]  DEFAULT ('true') FOR [istAktiv]
GO
ALTER TABLE [dbo].[MitarbeiterInSchicht]  WITH CHECK ADD  CONSTRAINT [FK_FertigungsteilMitarbeiterSchicht] FOREIGN KEY([FertigungsteilID])
REFERENCES [dbo].[Fertigungsteil] ([ID])
GO
ALTER TABLE [dbo].[MitarbeiterInSchicht] CHECK CONSTRAINT [FK_FertigungsteilMitarbeiterSchicht]
GO
ALTER TABLE [dbo].[MitarbeiterInSchicht]  WITH CHECK ADD  CONSTRAINT [FK_MitarbeiterMitarbeiterSchicht] FOREIGN KEY([MitarbeiterID])
REFERENCES [dbo].[Mitarbeiter] ([ID])
GO
ALTER TABLE [dbo].[MitarbeiterInSchicht] CHECK CONSTRAINT [FK_MitarbeiterMitarbeiterSchicht]
GO
ALTER TABLE [dbo].[MitarbeiterInSchicht]  WITH CHECK ADD  CONSTRAINT [FK_ProduktionsanlageMitarbeiterSchicht] FOREIGN KEY([ProduktionsanlageID])
REFERENCES [dbo].[Produktionsanlage] ([ID])
GO
ALTER TABLE [dbo].[MitarbeiterInSchicht] CHECK CONSTRAINT [FK_ProduktionsanlageMitarbeiterSchicht]
GO
ALTER TABLE [dbo].[MitarbeiterInSchicht]  WITH CHECK ADD  CONSTRAINT [FK_SchichtinfoMitarbeiterSchicht] FOREIGN KEY([SchichtInfoID])
REFERENCES [dbo].[Schichtinfo] ([ID])
GO
ALTER TABLE [dbo].[MitarbeiterInSchicht] CHECK CONSTRAINT [FK_SchichtinfoMitarbeiterSchicht]
GO


USE [zeiterfassung]
GO

INSERT INTO [dbo].[Zugriffsrechte]
           ([Benutzername]
           ,[Password]
           ,[Zugriffsebene])
     VALUES
           ('a','a',1)
GO


