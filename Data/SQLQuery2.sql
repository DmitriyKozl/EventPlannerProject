CREATE TABLE [dbo].[Events] (
    [Id]           INT  IDENTITY (1, 1) NOT NULL,
    [EventCode]    VARCHAR (100)  NOT NULL,
    [DateTimeStart]DATETIME       NOT NULL,
    [StartTijd]    Time           NOT NULL,
    [DateTimeEind] DATETIME       NOT NULL,
    [EindTijd]     Time           NOT NULL,
    [EventTitel]   VARCHAR (1000) NULL,
    [Prijs]        INT            NULL,
    [Beschrijving] VARCHAR (1000) NULL,

    PRIMARY KEY CLUSTERED ([Id] ASC)
)

