
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 12/07/2017 13:06:19
-- Generated from EDMX file: C:\Data\ShadowCanyonStudio\TriviaTraverse\TriviaTraverseApi\Models\TriviaTraverseModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [TriviaTraverse171207];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CampaignSection_CampaignCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CampaignSections] DROP CONSTRAINT [FK_CampaignSection_CampaignCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerCampaign_Player]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlayerCampaigns] DROP CONSTRAINT [FK_PlayerCampaign_Player];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerCampaignCategoryQueue_CampaignCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlayerCampaignCategoryQueues] DROP CONSTRAINT [FK_PlayerCampaignCategoryQueue_CampaignCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerCampaignCategoryQueue_PlayerCampaign]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlayerCampaignCategoryQueues] DROP CONSTRAINT [FK_PlayerCampaignCategoryQueue_PlayerCampaign];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerCampaignSectionQuestion_CampaignSection]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlayerCampaignSectionQuestions] DROP CONSTRAINT [FK_PlayerCampaignSectionQuestion_CampaignSection];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerCampaignSectionQuestion_PlayerCampaign]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlayerCampaignSectionQuestions] DROP CONSTRAINT [FK_PlayerCampaignSectionQuestion_PlayerCampaign];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerCampaignSectionQuestion_Question]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlayerCampaignSectionQuestions] DROP CONSTRAINT [FK_PlayerCampaignSectionQuestion_Question];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerCampaignStageCategory_CampaignStage]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlayerCampaignStageCategories] DROP CONSTRAINT [FK_PlayerCampaignStageCategory_CampaignStage];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerCampaignStageCategory_CampaignStageCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlayerCampaignStageCategories] DROP CONSTRAINT [FK_PlayerCampaignStageCategory_CampaignStageCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerCampaignStageCategory_PlayerCampaign]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlayerCampaignStageCategories] DROP CONSTRAINT [FK_PlayerCampaignStageCategory_PlayerCampaign];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerQuestionResult_Player]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlayerQuestionResults] DROP CONSTRAINT [FK_PlayerQuestionResult_Player];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerQuestionResult_Question]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlayerQuestionResults] DROP CONSTRAINT [FK_PlayerQuestionResult_Question];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerTutorialMessageStatus_Player]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlayerTutorialMessageStatuses] DROP CONSTRAINT [FK_PlayerTutorialMessageStatus_Player];
GO
IF OBJECT_ID(N'[dbo].[FK_Question_CampaignSection]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Questions] DROP CONSTRAINT [FK_Question_CampaignSection];
GO
IF OBJECT_ID(N'[dbo].[FK_Question_Category]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Questions] DROP CONSTRAINT [FK_Question_Category];
GO
IF OBJECT_ID(N'[dbo].[FK_Question_QuestionType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Questions] DROP CONSTRAINT [FK_Question_QuestionType];
GO
IF OBJECT_ID(N'[dbo].[FK_VGame_GameType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VGames] DROP CONSTRAINT [FK_VGame_GameType];
GO
IF OBJECT_ID(N'[dbo].[FK_VGameCategory_Category]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VGameCategories] DROP CONSTRAINT [FK_VGameCategory_Category];
GO
IF OBJECT_ID(N'[dbo].[FK_VGameCategory_VGame]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VGameCategories] DROP CONSTRAINT [FK_VGameCategory_VGame];
GO
IF OBJECT_ID(N'[dbo].[FK_VGameCategorySectionQuestion_Question]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VGameCategorySectionQuestions] DROP CONSTRAINT [FK_VGameCategorySectionQuestion_Question];
GO
IF OBJECT_ID(N'[dbo].[FK_VGameCategorySectionQuestion_VGameCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VGameCategorySectionQuestions] DROP CONSTRAINT [FK_VGameCategorySectionQuestion_VGameCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_VGamePlayer_Player]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VGamePlayers] DROP CONSTRAINT [FK_VGamePlayer_Player];
GO
IF OBJECT_ID(N'[dbo].[FK_VGamePlayer_VGame]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VGamePlayers] DROP CONSTRAINT [FK_VGamePlayer_VGame];
GO
IF OBJECT_ID(N'[dbo].[FK_VGamePlayerCategory_Player]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VGamePlayerCategories] DROP CONSTRAINT [FK_VGamePlayerCategory_Player];
GO
IF OBJECT_ID(N'[dbo].[FK_VGamePlayerCategory_VGameCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VGamePlayerCategories] DROP CONSTRAINT [FK_VGamePlayerCategory_VGameCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_VGamePlayerSectionQuestion_Player]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VGamePlayerSectionQuestions] DROP CONSTRAINT [FK_VGamePlayerSectionQuestion_Player];
GO
IF OBJECT_ID(N'[dbo].[FK_VGamePlayerSectionQuestion_Question]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VGamePlayerSectionQuestions] DROP CONSTRAINT [FK_VGamePlayerSectionQuestion_Question];
GO
IF OBJECT_ID(N'[dbo].[FK_VGamePlayerSectionQuestion_VGame]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VGamePlayerSectionQuestions] DROP CONSTRAINT [FK_VGamePlayerSectionQuestion_VGame];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[CampaignCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CampaignCategories];
GO
IF OBJECT_ID(N'[dbo].[CampaignSections]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CampaignSections];
GO
IF OBJECT_ID(N'[dbo].[CampaignStages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CampaignStages];
GO
IF OBJECT_ID(N'[dbo].[Categories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categories];
GO
IF OBJECT_ID(N'[dbo].[GameTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GameTypes];
GO
IF OBJECT_ID(N'[dbo].[PlayerCampaignCategoryQueues]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PlayerCampaignCategoryQueues];
GO
IF OBJECT_ID(N'[dbo].[PlayerCampaigns]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PlayerCampaigns];
GO
IF OBJECT_ID(N'[dbo].[PlayerCampaignSectionQuestions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PlayerCampaignSectionQuestions];
GO
IF OBJECT_ID(N'[dbo].[PlayerCampaignStageCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PlayerCampaignStageCategories];
GO
IF OBJECT_ID(N'[dbo].[PlayerQuestionResults]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PlayerQuestionResults];
GO
IF OBJECT_ID(N'[dbo].[Players]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Players];
GO
IF OBJECT_ID(N'[dbo].[PlayerTutorialMessageStatuses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PlayerTutorialMessageStatuses];
GO
IF OBJECT_ID(N'[dbo].[Questions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Questions];
GO
IF OBJECT_ID(N'[dbo].[QuestionTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QuestionTypes];
GO
IF OBJECT_ID(N'[dbo].[VGameCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VGameCategories];
GO
IF OBJECT_ID(N'[dbo].[VGameCategorySectionQuestions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VGameCategorySectionQuestions];
GO
IF OBJECT_ID(N'[dbo].[VGameNames]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VGameNames];
GO
IF OBJECT_ID(N'[dbo].[VGamePlayerCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VGamePlayerCategories];
GO
IF OBJECT_ID(N'[dbo].[VGamePlayers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VGamePlayers];
GO
IF OBJECT_ID(N'[dbo].[VGamePlayerSectionQuestions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VGamePlayerSectionQuestions];
GO
IF OBJECT_ID(N'[dbo].[VGames]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VGames];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'CampaignCategories'
CREATE TABLE [dbo].[CampaignCategory] (
    [CampaignCategoryId] int IDENTITY(1,1) NOT NULL,
    [CategoryName] nvarchar(50)  NOT NULL,
    [IsTutorial] bit  NOT NULL,
    [QueueLevel] int  NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NOT NULL,
    [Deleted] bit  NOT NULL
);
GO

-- Creating table 'CampaignSections'
CREATE TABLE [dbo].[CampaignSection] (
    [CampaignSectionId] int IDENTITY(1,1) NOT NULL,
    [CampaignCategoryId] int  NOT NULL,
    [SectionName] nvarchar(50)  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NOT NULL,
    [Deleted] bit  NOT NULL
);
GO

-- Creating table 'CampaignStages'
CREATE TABLE [dbo].[CampaignStage] (
    [CampaignStageId] int IDENTITY(1,1) NOT NULL,
    [StageLevel] int  NOT NULL,
    [StarsRequired] int  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NOT NULL,
    [Deleted] bit  NOT NULL
);
GO

-- Creating table 'Categories'
CREATE TABLE [dbo].[Category] (
    [CategoryId] int IDENTITY(1,1) NOT NULL,
    [CategoryName] nvarchar(50)  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NOT NULL,
    [Deleted] bit  NOT NULL
);
GO

-- Creating table 'GameTypes'
CREATE TABLE [dbo].[GameType] (
    [GameTypeId] int IDENTITY(1,1) NOT NULL,
    [Description] varchar(50)  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NOT NULL,
    [Deleted] bit  NOT NULL
);
GO

-- Creating table 'PlayerCampaignCategoryQueues'
CREATE TABLE [dbo].[PlayerCampaignCategoryQueue] (
    [PlayerCampaignCategoryQueueId] int IDENTITY(1,1) NOT NULL,
    [PlayerCampaignId] int  NOT NULL,
    [CampaignCategoryId] int  NOT NULL,
    [IsUsed] bit  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdateAt] datetime  NOT NULL,
    [Deleted] bit  NOT NULL
);
GO

-- Creating table 'PlayerCampaigns'
CREATE TABLE [dbo].[PlayerCampaign] (
    [PlayerCampaignId] int IDENTITY(1,1) NOT NULL,
    [PlayerId] int  NOT NULL
);
GO

-- Creating table 'PlayerCampaignSectionQuestions'
CREATE TABLE [dbo].[PlayerCampaignSectionQuestion] (
    [PlayerCampaignSectionQuestionId] int IDENTITY(1,1) NOT NULL,
    [PlayerCampaignId] int  NOT NULL,
    [CampaignSectionId] int  NOT NULL,
    [QuestionId] int  NOT NULL,
    [SectionQuestionOrder] int  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NOT NULL,
    [Deleted] bit  NOT NULL
);
GO

-- Creating table 'PlayerCampaignStageCategories'
CREATE TABLE [dbo].[PlayerCampaignStageCategory] (
    [PlayerCampaignStageCategoryId] int IDENTITY(1,1) NOT NULL,
    [PlayerCampaignId] int  NOT NULL,
    [CampaignCategoryId] int  NOT NULL,
    [CampaignStageId] int  NOT NULL
);
GO

-- Creating table 'PlayerQuestionResults'
CREATE TABLE [dbo].[PlayerQuestionResult] (
    [PlayerQuestionResultId] int IDENTITY(1,1) NOT NULL,
    [PlayerId] int  NOT NULL,
    [QuestionId] int  NOT NULL,
    [PlayerAnswerText] nchar(55)  NULL,
    [IsCorrect] bit  NULL,
    [PointsRewarded] int  NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NOT NULL,
    [Deleted] bit  NOT NULL
);
GO

-- Creating table 'Players'
CREATE TABLE [dbo].[Player] (
    [PlayerId] int IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(100)  NOT NULL,
    [EmailAddr] nvarchar(55)  NOT NULL,
    [Password] nvarchar(50)  NOT NULL,
    [FbLogin] bit  NOT NULL,
    [PlayerLevel] int  NOT NULL,
    [CurrentSteps] int  NOT NULL,
    [StepBank] int  NOT NULL,
    [LastStepUpdate] datetime  NOT NULL,
    [Coins] int  NOT NULL,
    [Stars] int  NOT NULL,
    [Points] int  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NOT NULL,
    [Deleted] bit  NOT NULL
);
GO

-- Creating table 'PlayerTutorialMessageStatuses'
CREATE TABLE [dbo].[PlayerTutorialMessageStatus] (
    [PlayerTutorialMessageStatusId] int IDENTITY(1,1) NOT NULL,
    [PlayerId] int  NOT NULL,
    [QuestionStatusOne] bit  NOT NULL,
    [QuestionStatusTwo] bit  NOT NULL,
    [CampaignSectionNewlyComlplete] bit  NOT NULL,
    [CampaignStageNewlyUnlocked] bit  NOT NULL,
    [CampaignStageBonus] bit  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NOT NULL,
    [Deleted] bit  NOT NULL
);
GO

-- Creating table 'Questions'
CREATE TABLE [dbo].[Question] (
    [QuestionId] int IDENTITY(1,1) NOT NULL,
    [CategoryId] int  NOT NULL,
    [CampaignSectionId] int  NULL,
    [CampaignSectionQuestionOrder] int  NULL,
    [QuestionLevel] int  NOT NULL,
    [QuestionTypeId] int  NOT NULL,
    [Text] nvarchar(255)  NULL,
    [HasImage] bit  NULL,
    [ImageUrl] nvarchar(255)  NULL,
    [AnswerCorrect] nvarchar(55)  NOT NULL,
    [AnswerWrong1] nvarchar(55)  NOT NULL,
    [AnswerWrong2] nvarchar(55)  NOT NULL,
    [AnswerWrong3] nvarchar(55)  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NOT NULL,
    [Deleted] bit  NOT NULL
);
GO

-- Creating table 'QuestionTypes'
CREATE TABLE [dbo].[QuestionType] (
    [QuestionTypeId] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(55)  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NOT NULL,
    [Deleted] bit  NOT NULL
);
GO

-- Creating table 'VGameCategories'
CREATE TABLE [dbo].[VGameCategory] (
    [VGameCategoryId] int IDENTITY(1,1) NOT NULL,
    [VGameId] int  NOT NULL,
    [CategoryId] int  NOT NULL,
    [Stage] int  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NOT NULL,
    [Deleted] bit  NOT NULL
);
GO

-- Creating table 'VGameCategorySectionQuestions'
CREATE TABLE [dbo].[VGameCategorySectionQuestion] (
    [VGameCategorySectionQuestionId] int IDENTITY(1,1) NOT NULL,
    [VGameCategoryId] int  NOT NULL,
    [QuestionId] int  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NOT NULL,
    [Deleted] bit  NOT NULL
);
GO

-- Creating table 'VGameNames'
CREATE TABLE [dbo].[VGameName] (
    [VGameNameId] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NOT NULL
);
GO

-- Creating table 'VGamePlayerCategories'
CREATE TABLE [dbo].[VGamePlayerCategory] (
    [VGamePlayerCategoryId] int IDENTITY(1,1) NOT NULL,
    [VGameCategoryId] int  NOT NULL,
    [PlayerId] int  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NOT NULL,
    [Deleted] bit  NOT NULL
);
GO

-- Creating table 'VGamePlayers'
CREATE TABLE [dbo].[VGamePlayer] (
    [VGamePlayerId] int IDENTITY(1,1) NOT NULL,
    [VGameId] int  NOT NULL,
    [PlayerId] int  NOT NULL,
    [GameSteps] int  NOT NULL,
    [LastStepUpdate] datetime  NOT NULL,
    [Score] int  NOT NULL,
    [Stage] int  NOT NULL,
    [QuestionsAnswered] int  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NOT NULL,
    [Deleted] bit  NOT NULL
);
GO

-- Creating table 'VGamePlayerSectionQuestions'
CREATE TABLE [dbo].[VGamePlayerSectionQuestion] (
    [VGamePlayerSectionQuestionId] int IDENTITY(1,1) NOT NULL,
    [VGameId] int  NOT NULL,
    [PlayerId] int  NOT NULL,
    [QuestionId] int  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdateAt] datetime  NOT NULL,
    [Deleted] bit  NOT NULL
);
GO

-- Creating table 'VGames'
CREATE TABLE [dbo].[VGame] (
    [VGameId] int IDENTITY(1,1) NOT NULL,
    [GameName] varchar(50)  NOT NULL,
    [GameTypeId] int  NOT NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NOT NULL,
    [PlayerMax] int  NOT NULL,
    [StepCap] int  NOT NULL,
    [IsPrivate] bit  NOT NULL,
    [GameLength] int  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [UpdatedAt] datetime  NOT NULL,
    [Deleted] bit  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [CampaignCategoryId] in table 'CampaignCategories'
ALTER TABLE [dbo].[CampaignCategory]
ADD CONSTRAINT [PK_CampaignCategory]
    PRIMARY KEY CLUSTERED ([CampaignCategoryId] ASC);
GO

-- Creating primary key on [CampaignSectionId] in table 'CampaignSections'
ALTER TABLE [dbo].[CampaignSection]
ADD CONSTRAINT [PK_CampaignSection]
    PRIMARY KEY CLUSTERED ([CampaignSectionId] ASC);
GO

-- Creating primary key on [CampaignStageId] in table 'CampaignStages'
ALTER TABLE [dbo].[CampaignStage]
ADD CONSTRAINT [PK_CampaignStage]
    PRIMARY KEY CLUSTERED ([CampaignStageId] ASC);
GO

-- Creating primary key on [CategoryId] in table 'Categories'
ALTER TABLE [dbo].[Category]
ADD CONSTRAINT [PK_Category]
    PRIMARY KEY CLUSTERED ([CategoryId] ASC);
GO

-- Creating primary key on [GameTypeId] in table 'GameTypes'
ALTER TABLE [dbo].[GameType]
ADD CONSTRAINT [PK_GameType]
    PRIMARY KEY CLUSTERED ([GameTypeId] ASC);
GO

-- Creating primary key on [PlayerCampaignCategoryQueueId] in table 'PlayerCampaignCategoryQueues'
ALTER TABLE [dbo].[PlayerCampaignCategoryQueue]
ADD CONSTRAINT [PK_PlayerCampaignCategoryQueue]
    PRIMARY KEY CLUSTERED ([PlayerCampaignCategoryQueueId] ASC);
GO

-- Creating primary key on [PlayerCampaignId] in table 'PlayerCampaigns'
ALTER TABLE [dbo].[PlayerCampaign]
ADD CONSTRAINT [PK_PlayerCampaign]
    PRIMARY KEY CLUSTERED ([PlayerCampaignId] ASC);
GO

-- Creating primary key on [PlayerCampaignSectionQuestionId] in table 'PlayerCampaignSectionQuestions'
ALTER TABLE [dbo].[PlayerCampaignSectionQuestion]
ADD CONSTRAINT [PK_PlayerCampaignSectionQuestion]
    PRIMARY KEY CLUSTERED ([PlayerCampaignSectionQuestionId] ASC);
GO

-- Creating primary key on [PlayerCampaignStageCategoryId] in table 'PlayerCampaignStageCategories'
ALTER TABLE [dbo].[PlayerCampaignStageCategory]
ADD CONSTRAINT [PK_PlayerCampaignStageCategory]
    PRIMARY KEY CLUSTERED ([PlayerCampaignStageCategoryId] ASC);
GO

-- Creating primary key on [PlayerQuestionResultId] in table 'PlayerQuestionResults'
ALTER TABLE [dbo].[PlayerQuestionResult]
ADD CONSTRAINT [PK_PlayerQuestionResult]
    PRIMARY KEY CLUSTERED ([PlayerQuestionResultId] ASC);
GO

-- Creating primary key on [PlayerId] in table 'Players'
ALTER TABLE [dbo].[Player]
ADD CONSTRAINT [PK_Player]
    PRIMARY KEY CLUSTERED ([PlayerId] ASC);
GO

-- Creating primary key on [PlayerTutorialMessageStatusId] in table 'PlayerTutorialMessageStatuses'
ALTER TABLE [dbo].[PlayerTutorialMessageStatus]
ADD CONSTRAINT [PK_PlayerTutorialMessageStatus]
    PRIMARY KEY CLUSTERED ([PlayerTutorialMessageStatusId] ASC);
GO

-- Creating primary key on [QuestionId] in table 'Questions'
ALTER TABLE [dbo].[Question]
ADD CONSTRAINT [PK_Question]
    PRIMARY KEY CLUSTERED ([QuestionId] ASC);
GO

-- Creating primary key on [QuestionTypeId] in table 'QuestionTypes'
ALTER TABLE [dbo].[QuestionType]
ADD CONSTRAINT [PK_QuestionType]
    PRIMARY KEY CLUSTERED ([QuestionTypeId] ASC);
GO

-- Creating primary key on [VGameCategoryId] in table 'VGameCategories'
ALTER TABLE [dbo].[VGameCategory]
ADD CONSTRAINT [PK_VGameCategory]
    PRIMARY KEY CLUSTERED ([VGameCategoryId] ASC);
GO

-- Creating primary key on [VGameCategorySectionQuestionId] in table 'VGameCategorySectionQuestions'
ALTER TABLE [dbo].[VGameCategorySectionQuestion]
ADD CONSTRAINT [PK_VGameCategorySectionQuestion]
    PRIMARY KEY CLUSTERED ([VGameCategorySectionQuestionId] ASC);
GO

-- Creating primary key on [VGameNameId] in table 'VGameNames'
ALTER TABLE [dbo].[VGameName]
ADD CONSTRAINT [PK_VGameName]
    PRIMARY KEY CLUSTERED ([VGameNameId] ASC);
GO

-- Creating primary key on [VGamePlayerCategoryId] in table 'VGamePlayerCategories'
ALTER TABLE [dbo].[VGamePlayerCategory]
ADD CONSTRAINT [PK_VGamePlayerCategory]
    PRIMARY KEY CLUSTERED ([VGamePlayerCategoryId] ASC);
GO

-- Creating primary key on [VGamePlayerId] in table 'VGamePlayers'
ALTER TABLE [dbo].[VGamePlayer]
ADD CONSTRAINT [PK_VGamePlayer]
    PRIMARY KEY CLUSTERED ([VGamePlayerId] ASC);
GO

-- Creating primary key on [VGamePlayerSectionQuestionId] in table 'VGamePlayerSectionQuestions'
ALTER TABLE [dbo].[VGamePlayerSectionQuestion]
ADD CONSTRAINT [PK_VGamePlayerSectionQuestion]
    PRIMARY KEY CLUSTERED ([VGamePlayerSectionQuestionId] ASC);
GO

-- Creating primary key on [VGameId] in table 'VGames'
ALTER TABLE [dbo].[VGame]
ADD CONSTRAINT [PK_VGame]
    PRIMARY KEY CLUSTERED ([VGameId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CampaignCategoryId] in table 'CampaignSections'
ALTER TABLE [dbo].[CampaignSection]
ADD CONSTRAINT [FK_CampaignSection_CampaignCategory]
    FOREIGN KEY ([CampaignCategoryId])
    REFERENCES [dbo].[CampaignCategory]
        ([CampaignCategoryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CampaignSection_CampaignCategory'
CREATE INDEX [IX_FK_CampaignSection_CampaignCategory]
ON [dbo].[CampaignSection]
    ([CampaignCategoryId]);
GO

-- Creating foreign key on [CampaignCategoryId] in table 'PlayerCampaignCategoryQueues'
ALTER TABLE [dbo].[PlayerCampaignCategoryQueue]
ADD CONSTRAINT [FK_PlayerCampaignCategoryQueue_CampaignCategory]
    FOREIGN KEY ([CampaignCategoryId])
    REFERENCES [dbo].[CampaignCategory]
        ([CampaignCategoryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PlayerCampaignCategoryQueue_CampaignCategory'
CREATE INDEX [IX_FK_PlayerCampaignCategoryQueue_CampaignCategory]
ON [dbo].[PlayerCampaignCategoryQueue]
    ([CampaignCategoryId]);
GO

-- Creating foreign key on [CampaignCategoryId] in table 'PlayerCampaignStageCategories'
ALTER TABLE [dbo].[PlayerCampaignStageCategory]
ADD CONSTRAINT [FK_PlayerCampaignStageCategory_CampaignStageCategory]
    FOREIGN KEY ([CampaignCategoryId])
    REFERENCES [dbo].[CampaignCategory]
        ([CampaignCategoryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PlayerCampaignStageCategory_CampaignStageCategory'
CREATE INDEX [IX_FK_PlayerCampaignStageCategory_CampaignStageCategory]
ON [dbo].[PlayerCampaignStageCategory]
    ([CampaignCategoryId]);
GO

-- Creating foreign key on [CampaignSectionId] in table 'PlayerCampaignSectionQuestions'
ALTER TABLE [dbo].[PlayerCampaignSectionQuestion]
ADD CONSTRAINT [FK_PlayerCampaignSectionQuestion_CampaignSection]
    FOREIGN KEY ([CampaignSectionId])
    REFERENCES [dbo].[CampaignSection]
        ([CampaignSectionId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PlayerCampaignSectionQuestion_CampaignSection'
CREATE INDEX [IX_FK_PlayerCampaignSectionQuestion_CampaignSection]
ON [dbo].[PlayerCampaignSectionQuestion]
    ([CampaignSectionId]);
GO

-- Creating foreign key on [CampaignSectionId] in table 'Questions'
ALTER TABLE [dbo].[Question]
ADD CONSTRAINT [FK_Question_CampaignSection]
    FOREIGN KEY ([CampaignSectionId])
    REFERENCES [dbo].[CampaignSection]
        ([CampaignSectionId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Question_CampaignSection'
CREATE INDEX [IX_FK_Question_CampaignSection]
ON [dbo].[Question]
    ([CampaignSectionId]);
GO

-- Creating foreign key on [CampaignStageId] in table 'PlayerCampaignStageCategories'
ALTER TABLE [dbo].[PlayerCampaignStageCategory]
ADD CONSTRAINT [FK_PlayerCampaignStageCategory_CampaignStage]
    FOREIGN KEY ([CampaignStageId])
    REFERENCES [dbo].[CampaignStage]
        ([CampaignStageId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PlayerCampaignStageCategory_CampaignStage'
CREATE INDEX [IX_FK_PlayerCampaignStageCategory_CampaignStage]
ON [dbo].[PlayerCampaignStageCategory]
    ([CampaignStageId]);
GO

-- Creating foreign key on [CategoryId] in table 'Questions'
ALTER TABLE [dbo].[Question]
ADD CONSTRAINT [FK_Question_Category]
    FOREIGN KEY ([CategoryId])
    REFERENCES [dbo].[Category]
        ([CategoryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Question_Category'
CREATE INDEX [IX_FK_Question_Category]
ON [dbo].[Question]
    ([CategoryId]);
GO

-- Creating foreign key on [CategoryId] in table 'VGameCategories'
ALTER TABLE [dbo].[VGameCategory]
ADD CONSTRAINT [FK_VGameCategory_Category]
    FOREIGN KEY ([CategoryId])
    REFERENCES [dbo].[Category]
        ([CategoryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VGameCategory_Category'
CREATE INDEX [IX_FK_VGameCategory_Category]
ON [dbo].[VGameCategory]
    ([CategoryId]);
GO

-- Creating foreign key on [GameTypeId] in table 'VGames'
ALTER TABLE [dbo].[VGame]
ADD CONSTRAINT [FK_VGame_GameType]
    FOREIGN KEY ([GameTypeId])
    REFERENCES [dbo].[GameType]
        ([GameTypeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VGame_GameType'
CREATE INDEX [IX_FK_VGame_GameType]
ON [dbo].[VGame]
    ([GameTypeId]);
GO

-- Creating foreign key on [PlayerCampaignId] in table 'PlayerCampaignCategoryQueues'
ALTER TABLE [dbo].[PlayerCampaignCategoryQueue]
ADD CONSTRAINT [FK_PlayerCampaignCategoryQueue_PlayerCampaign]
    FOREIGN KEY ([PlayerCampaignId])
    REFERENCES [dbo].[PlayerCampaign]
        ([PlayerCampaignId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PlayerCampaignCategoryQueue_PlayerCampaign'
CREATE INDEX [IX_FK_PlayerCampaignCategoryQueue_PlayerCampaign]
ON [dbo].[PlayerCampaignCategoryQueue]
    ([PlayerCampaignId]);
GO

-- Creating foreign key on [PlayerId] in table 'PlayerCampaigns'
ALTER TABLE [dbo].[PlayerCampaign]
ADD CONSTRAINT [FK_PlayerCampaign_Player]
    FOREIGN KEY ([PlayerId])
    REFERENCES [dbo].[Player]
        ([PlayerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PlayerCampaign_Player'
CREATE INDEX [IX_FK_PlayerCampaign_Player]
ON [dbo].[PlayerCampaign]
    ([PlayerId]);
GO

-- Creating foreign key on [PlayerCampaignId] in table 'PlayerCampaignSectionQuestions'
ALTER TABLE [dbo].[PlayerCampaignSectionQuestion]
ADD CONSTRAINT [FK_PlayerCampaignSectionQuestion_PlayerCampaign]
    FOREIGN KEY ([PlayerCampaignId])
    REFERENCES [dbo].[PlayerCampaign]
        ([PlayerCampaignId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PlayerCampaignSectionQuestion_PlayerCampaign'
CREATE INDEX [IX_FK_PlayerCampaignSectionQuestion_PlayerCampaign]
ON [dbo].[PlayerCampaignSectionQuestion]
    ([PlayerCampaignId]);
GO

-- Creating foreign key on [PlayerCampaignId] in table 'PlayerCampaignStageCategories'
ALTER TABLE [dbo].[PlayerCampaignStageCategory]
ADD CONSTRAINT [FK_PlayerCampaignStageCategory_PlayerCampaign]
    FOREIGN KEY ([PlayerCampaignId])
    REFERENCES [dbo].[PlayerCampaign]
        ([PlayerCampaignId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PlayerCampaignStageCategory_PlayerCampaign'
CREATE INDEX [IX_FK_PlayerCampaignStageCategory_PlayerCampaign]
ON [dbo].[PlayerCampaignStageCategory]
    ([PlayerCampaignId]);
GO

-- Creating foreign key on [QuestionId] in table 'PlayerCampaignSectionQuestions'
ALTER TABLE [dbo].[PlayerCampaignSectionQuestion]
ADD CONSTRAINT [FK_PlayerCampaignSectionQuestion_Question]
    FOREIGN KEY ([QuestionId])
    REFERENCES [dbo].[Question]
        ([QuestionId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PlayerCampaignSectionQuestion_Question'
CREATE INDEX [IX_FK_PlayerCampaignSectionQuestion_Question]
ON [dbo].[PlayerCampaignSectionQuestion]
    ([QuestionId]);
GO

-- Creating foreign key on [PlayerId] in table 'PlayerQuestionResults'
ALTER TABLE [dbo].[PlayerQuestionResult]
ADD CONSTRAINT [FK_PlayerQuestionResult_Player]
    FOREIGN KEY ([PlayerId])
    REFERENCES [dbo].[Player]
        ([PlayerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PlayerQuestionResult_Player'
CREATE INDEX [IX_FK_PlayerQuestionResult_Player]
ON [dbo].[PlayerQuestionResult]
    ([PlayerId]);
GO

-- Creating foreign key on [QuestionId] in table 'PlayerQuestionResults'
ALTER TABLE [dbo].[PlayerQuestionResult]
ADD CONSTRAINT [FK_PlayerQuestionResult_Question]
    FOREIGN KEY ([QuestionId])
    REFERENCES [dbo].[Question]
        ([QuestionId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PlayerQuestionResult_Question'
CREATE INDEX [IX_FK_PlayerQuestionResult_Question]
ON [dbo].[PlayerQuestionResult]
    ([QuestionId]);
GO

-- Creating foreign key on [PlayerId] in table 'PlayerTutorialMessageStatuses'
ALTER TABLE [dbo].[PlayerTutorialMessageStatus]
ADD CONSTRAINT [FK_PlayerTutorialMessageStatus_Player]
    FOREIGN KEY ([PlayerId])
    REFERENCES [dbo].[Player]
        ([PlayerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PlayerTutorialMessageStatus_Player'
CREATE INDEX [IX_FK_PlayerTutorialMessageStatus_Player]
ON [dbo].[PlayerTutorialMessageStatus]
    ([PlayerId]);
GO

-- Creating foreign key on [PlayerId] in table 'VGamePlayers'
ALTER TABLE [dbo].[VGamePlayer]
ADD CONSTRAINT [FK_VGamePlayer_Player]
    FOREIGN KEY ([PlayerId])
    REFERENCES [dbo].[Player]
        ([PlayerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VGamePlayer_Player'
CREATE INDEX [IX_FK_VGamePlayer_Player]
ON [dbo].[VGamePlayer]
    ([PlayerId]);
GO

-- Creating foreign key on [PlayerId] in table 'VGamePlayerCategories'
ALTER TABLE [dbo].[VGamePlayerCategory]
ADD CONSTRAINT [FK_VGamePlayerCategory_Player]
    FOREIGN KEY ([PlayerId])
    REFERENCES [dbo].[Player]
        ([PlayerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VGamePlayerCategory_Player'
CREATE INDEX [IX_FK_VGamePlayerCategory_Player]
ON [dbo].[VGamePlayerCategory]
    ([PlayerId]);
GO

-- Creating foreign key on [PlayerId] in table 'VGamePlayerSectionQuestions'
ALTER TABLE [dbo].[VGamePlayerSectionQuestion]
ADD CONSTRAINT [FK_VGamePlayerSectionQuestion_Player]
    FOREIGN KEY ([PlayerId])
    REFERENCES [dbo].[Player]
        ([PlayerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VGamePlayerSectionQuestion_Player'
CREATE INDEX [IX_FK_VGamePlayerSectionQuestion_Player]
ON [dbo].[VGamePlayerSectionQuestion]
    ([PlayerId]);
GO

-- Creating foreign key on [QuestionTypeId] in table 'Questions'
ALTER TABLE [dbo].[Question]
ADD CONSTRAINT [FK_Question_QuestionType]
    FOREIGN KEY ([QuestionTypeId])
    REFERENCES [dbo].[QuestionType]
        ([QuestionTypeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Question_QuestionType'
CREATE INDEX [IX_FK_Question_QuestionType]
ON [dbo].[Question]
    ([QuestionTypeId]);
GO

-- Creating foreign key on [QuestionId] in table 'VGameCategorySectionQuestions'
ALTER TABLE [dbo].[VGameCategorySectionQuestion]
ADD CONSTRAINT [FK_VGameCategorySectionQuestion_Question]
    FOREIGN KEY ([QuestionId])
    REFERENCES [dbo].[Question]
        ([QuestionId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VGameCategorySectionQuestion_Question'
CREATE INDEX [IX_FK_VGameCategorySectionQuestion_Question]
ON [dbo].[VGameCategorySectionQuestion]
    ([QuestionId]);
GO

-- Creating foreign key on [QuestionId] in table 'VGamePlayerSectionQuestions'
ALTER TABLE [dbo].[VGamePlayerSectionQuestion]
ADD CONSTRAINT [FK_VGamePlayerSectionQuestion_Question]
    FOREIGN KEY ([QuestionId])
    REFERENCES [dbo].[Question]
        ([QuestionId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VGamePlayerSectionQuestion_Question'
CREATE INDEX [IX_FK_VGamePlayerSectionQuestion_Question]
ON [dbo].[VGamePlayerSectionQuestion]
    ([QuestionId]);
GO

-- Creating foreign key on [VGameId] in table 'VGameCategories'
ALTER TABLE [dbo].[VGameCategory]
ADD CONSTRAINT [FK_VGameCategory_VGame]
    FOREIGN KEY ([VGameId])
    REFERENCES [dbo].[VGame]
        ([VGameId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VGameCategory_VGame'
CREATE INDEX [IX_FK_VGameCategory_VGame]
ON [dbo].[VGameCategory]
    ([VGameId]);
GO

-- Creating foreign key on [VGameCategoryId] in table 'VGameCategorySectionQuestions'
ALTER TABLE [dbo].[VGameCategorySectionQuestion]
ADD CONSTRAINT [FK_VGameCategorySectionQuestion_VGameCategory]
    FOREIGN KEY ([VGameCategoryId])
    REFERENCES [dbo].[VGameCategory]
        ([VGameCategoryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VGameCategorySectionQuestion_VGameCategory'
CREATE INDEX [IX_FK_VGameCategorySectionQuestion_VGameCategory]
ON [dbo].[VGameCategorySectionQuestion]
    ([VGameCategoryId]);
GO

-- Creating foreign key on [VGameCategoryId] in table 'VGamePlayerCategories'
ALTER TABLE [dbo].[VGamePlayerCategory]
ADD CONSTRAINT [FK_VGamePlayerCategory_VGameCategory]
    FOREIGN KEY ([VGameCategoryId])
    REFERENCES [dbo].[VGameCategory]
        ([VGameCategoryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VGamePlayerCategory_VGameCategory'
CREATE INDEX [IX_FK_VGamePlayerCategory_VGameCategory]
ON [dbo].[VGamePlayerCategory]
    ([VGameCategoryId]);
GO

-- Creating foreign key on [VGameId] in table 'VGamePlayers'
ALTER TABLE [dbo].[VGamePlayer]
ADD CONSTRAINT [FK_VGamePlayer_VGame]
    FOREIGN KEY ([VGameId])
    REFERENCES [dbo].[VGame]
        ([VGameId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VGamePlayer_VGame'
CREATE INDEX [IX_FK_VGamePlayer_VGame]
ON [dbo].[VGamePlayer]
    ([VGameId]);
GO

-- Creating foreign key on [VGameId] in table 'VGamePlayerSectionQuestions'
ALTER TABLE [dbo].[VGamePlayerSectionQuestion]
ADD CONSTRAINT [FK_VGamePlayerSectionQuestion_VGame]
    FOREIGN KEY ([VGameId])
    REFERENCES [dbo].[VGame]
        ([VGameId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VGamePlayerSectionQuestion_VGame'
CREATE INDEX [IX_FK_VGamePlayerSectionQuestion_VGame]
ON [dbo].[VGamePlayerSectionQuestion]
    ([VGameId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------