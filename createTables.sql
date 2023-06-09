CREATE TABLE [dbo].[Dagplan] (
    [DagplanID]  INT  IDENTITY (1, 1) NOT NULL,
    [Datum]      DATE NOT NULL,
    [BezoekerID] INT  NOT NULL,
    PRIMARY KEY CLUSTERED ([DagplanID] ASC)
);

CREATE TABLE [dbo].[DagplanEvenement] (
    [DagplanID] INT      NOT NULL,
    [EventsId]  INT      NOT NULL,
    [Tijdstip]  TIME (7) NOT NULL
);


CREATE TABLE [dbo].[Events] (
    [EventsId]      INT            IDENTITY (1, 1) NOT NULL,
    [EventCode]     VARCHAR (100)  NOT NULL,
    [DateTimeStart] DATETIME       NOT NULL,
    [StartTijd]     TIME (7)       NOT NULL,
    [DateTimeEind]  DATETIME       NOT NULL,
    [EindTijd]      TIME (7)       NOT NULL,
    [EventTitel]    VARCHAR (1000) NULL,
    [Prijs]         DECIMAL (18)   NULL,
    [Beschrijving]  VARCHAR (1000) NULL,
    PRIMARY KEY CLUSTERED ([EventsId] ASC)
);

CREATE TABLE [dbo].[Gebruiker] (
    [GebruikerID] INT          IDENTITY (1, 1) NOT NULL,
    [Naam]        VARCHAR (50) NOT NULL,
    [Voornaam]    VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([GebruikerID] ASC)
);