
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 12/18/2024 05:57:49
-- Generated from EDMX file: C:\Users\axel_\OneDrive\Escritorio\Labyrinth\Labyrinth\DataAccess\Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Labyrinth];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_FriendList_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FriendList] DROP CONSTRAINT [FK_FriendList_User];
GO
IF OBJECT_ID(N'[dbo].[FK_FriendList_User1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FriendList] DROP CONSTRAINT [FK_FriendList_User1];
GO
IF OBJECT_ID(N'[dbo].[FK_FriendRequest_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FriendRequest] DROP CONSTRAINT [FK_FriendRequest_User];
GO
IF OBJECT_ID(N'[dbo].[FK_FriendRequest_User1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FriendRequest] DROP CONSTRAINT [FK_FriendRequest_User1];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerSkins_Skin]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlayerSkins] DROP CONSTRAINT [FK_PlayerSkins_Skin];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerSkins_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlayerSkins] DROP CONSTRAINT [FK_PlayerSkins_User];
GO
IF OBJECT_ID(N'[dbo].[FK_Preferences_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Preferences] DROP CONSTRAINT [FK_Preferences_User];
GO
IF OBJECT_ID(N'[dbo].[FK_Stats_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Stats] DROP CONSTRAINT [FK_Stats_User];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FriendList]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FriendList];
GO
IF OBJECT_ID(N'[dbo].[FriendRequest]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FriendRequest];
GO
IF OBJECT_ID(N'[dbo].[PlayerSkins]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PlayerSkins];
GO
IF OBJECT_ID(N'[dbo].[Preferences]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Preferences];
GO
IF OBJECT_ID(N'[dbo].[Skin]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Skin];
GO
IF OBJECT_ID(N'[dbo].[Stats]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Stats];
GO
IF OBJECT_ID(N'[dbo].[User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[User];
GO
IF OBJECT_ID(N'[dbo].[VerificationCode]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VerificationCode];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'FriendList'
CREATE TABLE [dbo].[FriendList] (
    [idUserOne] int  NOT NULL,
    [idUserTwo] int  NOT NULL,
    [idFriendList] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'FriendRequest'
CREATE TABLE [dbo].[FriendRequest] (
    [idFriendRequest] int IDENTITY(1,1) NOT NULL,
    [idRequester] int  NOT NULL,
    [idRequested] int  NOT NULL,
    [status] varchar(12)  NOT NULL
);
GO

-- Creating table 'PlayerSkins'
CREATE TABLE [dbo].[PlayerSkins] (
    [idPlayerSkins] int IDENTITY(1,1) NOT NULL,
    [idUser] int  NOT NULL,
    [idSkin] int  NOT NULL
);
GO

-- Creating table 'Preferences'
CREATE TABLE [dbo].[Preferences] (
    [idPreferences] int IDENTITY(1,1) NOT NULL,
    [lastPickedSkin] int  NOT NULL,
    [idUser] int  NOT NULL,
    [lastPickedGameboard] int  NULL
);
GO

-- Creating table 'Skin'
CREATE TABLE [dbo].[Skin] (
    [idSkin] int IDENTITY(1,1) NOT NULL,
    [name] varchar(50)  NOT NULL,
    [lock] bit  NOT NULL,
    [gamesWonToUnlock] int  NULL
);
GO

-- Creating table 'Stats'
CREATE TABLE [dbo].[Stats] (
    [idStats] int IDENTITY(1,1) NOT NULL,
    [gamesWon] int  NULL,
    [gamesPlayed] int  NULL,
    [idUser] int  NOT NULL
);
GO

-- Creating table 'VerificationCode'
CREATE TABLE [dbo].[VerificationCode] (
    [email] varchar(50)  NOT NULL,
    [code] varchar(12)  NOT NULL,
    [idVerificationCode] int  NOT NULL
);
GO

-- Creating table 'User'
CREATE TABLE [dbo].[User] (
    [idUser] int IDENTITY(1,1) NOT NULL,
    [email] varchar(50)  NOT NULL,
    [userName] varchar(50)  NOT NULL,
    [profilePicture] varchar(120)  NULL,
    [password] varchar(64)  NOT NULL,
    [countryCode] varchar(2)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [idFriendList] in table 'FriendList'
ALTER TABLE [dbo].[FriendList]
ADD CONSTRAINT [PK_FriendList]
    PRIMARY KEY CLUSTERED ([idFriendList] ASC);
GO

-- Creating primary key on [idFriendRequest] in table 'FriendRequest'
ALTER TABLE [dbo].[FriendRequest]
ADD CONSTRAINT [PK_FriendRequest]
    PRIMARY KEY CLUSTERED ([idFriendRequest] ASC);
GO

-- Creating primary key on [idPlayerSkins] in table 'PlayerSkins'
ALTER TABLE [dbo].[PlayerSkins]
ADD CONSTRAINT [PK_PlayerSkins]
    PRIMARY KEY CLUSTERED ([idPlayerSkins] ASC);
GO

-- Creating primary key on [idPreferences] in table 'Preferences'
ALTER TABLE [dbo].[Preferences]
ADD CONSTRAINT [PK_Preferences]
    PRIMARY KEY CLUSTERED ([idPreferences] ASC);
GO

-- Creating primary key on [idSkin] in table 'Skin'
ALTER TABLE [dbo].[Skin]
ADD CONSTRAINT [PK_Skin]
    PRIMARY KEY CLUSTERED ([idSkin] ASC);
GO

-- Creating primary key on [idStats] in table 'Stats'
ALTER TABLE [dbo].[Stats]
ADD CONSTRAINT [PK_Stats]
    PRIMARY KEY CLUSTERED ([idStats] ASC);
GO

-- Creating primary key on [idVerificationCode] in table 'VerificationCode'
ALTER TABLE [dbo].[VerificationCode]
ADD CONSTRAINT [PK_VerificationCode]
    PRIMARY KEY CLUSTERED ([idVerificationCode] ASC);
GO

-- Creating primary key on [idUser] in table 'User'
ALTER TABLE [dbo].[User]
ADD CONSTRAINT [PK_User]
    PRIMARY KEY CLUSTERED ([idUser] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [idSkin] in table 'PlayerSkins'
ALTER TABLE [dbo].[PlayerSkins]
ADD CONSTRAINT [FK_PlayerSkins_Skin]
    FOREIGN KEY ([idSkin])
    REFERENCES [dbo].[Skin]
        ([idSkin])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PlayerSkins_Skin'
CREATE INDEX [IX_FK_PlayerSkins_Skin]
ON [dbo].[PlayerSkins]
    ([idSkin]);
GO

-- Creating foreign key on [idUserOne] in table 'FriendList'
ALTER TABLE [dbo].[FriendList]
ADD CONSTRAINT [FK_FriendList_User]
    FOREIGN KEY ([idUserOne])
    REFERENCES [dbo].[User]
        ([idUser])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FriendList_User'
CREATE INDEX [IX_FK_FriendList_User]
ON [dbo].[FriendList]
    ([idUserOne]);
GO

-- Creating foreign key on [idUserTwo] in table 'FriendList'
ALTER TABLE [dbo].[FriendList]
ADD CONSTRAINT [FK_FriendList_User1]
    FOREIGN KEY ([idUserTwo])
    REFERENCES [dbo].[User]
        ([idUser])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FriendList_User1'
CREATE INDEX [IX_FK_FriendList_User1]
ON [dbo].[FriendList]
    ([idUserTwo]);
GO

-- Creating foreign key on [idRequester] in table 'FriendRequest'
ALTER TABLE [dbo].[FriendRequest]
ADD CONSTRAINT [FK_FriendRequest_User]
    FOREIGN KEY ([idRequester])
    REFERENCES [dbo].[User]
        ([idUser])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FriendRequest_User'
CREATE INDEX [IX_FK_FriendRequest_User]
ON [dbo].[FriendRequest]
    ([idRequester]);
GO

-- Creating foreign key on [idRequested] in table 'FriendRequest'
ALTER TABLE [dbo].[FriendRequest]
ADD CONSTRAINT [FK_FriendRequest_User1]
    FOREIGN KEY ([idRequested])
    REFERENCES [dbo].[User]
        ([idUser])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FriendRequest_User1'
CREATE INDEX [IX_FK_FriendRequest_User1]
ON [dbo].[FriendRequest]
    ([idRequested]);
GO

-- Creating foreign key on [idUser] in table 'PlayerSkins'
ALTER TABLE [dbo].[PlayerSkins]
ADD CONSTRAINT [FK_PlayerSkins_User]
    FOREIGN KEY ([idUser])
    REFERENCES [dbo].[User]
        ([idUser])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PlayerSkins_User'
CREATE INDEX [IX_FK_PlayerSkins_User]
ON [dbo].[PlayerSkins]
    ([idUser]);
GO

-- Creating foreign key on [idUser] in table 'Preferences'
ALTER TABLE [dbo].[Preferences]
ADD CONSTRAINT [FK_Preferences_User]
    FOREIGN KEY ([idUser])
    REFERENCES [dbo].[User]
        ([idUser])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Preferences_User'
CREATE INDEX [IX_FK_Preferences_User]
ON [dbo].[Preferences]
    ([idUser]);
GO

-- Creating foreign key on [idUser] in table 'Stats'
ALTER TABLE [dbo].[Stats]
ADD CONSTRAINT [FK_Stats_User]
    FOREIGN KEY ([idUser])
    REFERENCES [dbo].[User]
        ([idUser])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Stats_User'
CREATE INDEX [IX_FK_Stats_User]
ON [dbo].[Stats]
    ([idUser]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------