
  DELETE [PlayerQuestionResult];

  DELETE [VGamePlayerSectionQuestion];
  DELETE [VGamePlayerCategory];
  DELETE [VGameCategorySectionQuestion];
  Delete [VGameCategory];
  DELETE [VGamePlayer];
  DELETE [VGame];

  DELETE [PlayerCampaignCategoryQueue];
  DELETE [PlayerCampaignStageCategory];
  DELETE [PlayerCampaignSectionQuestion];
  DELETE [PlayerCampaign];
  DELETE [PlayerTutorialMessageStatus];

DELETE [Player] where [IsBot] = 0;

INSERT INTO [dbo].[Player]
           ([UserName],[IsBot],[EmailAddr],[Password],[FbLogin],[PlayerLevel]
           ,[CurrentSteps],[StepBank],[LastStepUpdate],[Coins],[Stars],[Points]
           ,[CreatedAt],[UpdatedAt],[Deleted])
     VALUES
           ('BotOne',1,'botOne@shadowcanyonestudios.com','dw45sehs5h',0,1
           ,0,0,GetDate(),0,0,0
           ,GetDate(),GetDate(),0),
           ('BotTwo',1,'BotTwo@shadowcanyonestudios.com','dw45sehs5h',0,1
           ,0,0,GetDate(),0,0,0
           ,GetDate(),GetDate(),0),
           ('BotThree',1,'BotThree@shadowcanyonestudios.com','dw45sehs5h',0,1
           ,0,0,GetDate(),0,0,0
           ,GetDate(),GetDate(),0),
           ('BotFour',1,'BotFour@shadowcanyonestudios.com','dw45sehs5h',0,1
           ,0,0,GetDate(),0,0,0
           ,GetDate(),GetDate(),0),
           ('BotFive',1,'BotFive@shadowcanyonestudios.com','dw45sehs5h',0,1
           ,0,0,GetDate(),0,0,0
           ,GetDate(),GetDate(),0),
           ('BotSix',1,'BotSix@shadowcanyonestudios.com','dw45sehs5h',0,1
           ,0,0,GetDate(),0,0,0
           ,GetDate(),GetDate(),0),
           ('BotSeven',1,'BotSeven@shadowcanyonestudios.com','dw45sehs5h',0,1
           ,0,0,GetDate(),0,0,0
           ,GetDate(),GetDate(),0),
           ('BotEight',1,'BotEight@shadowcanyonestudios.com','dw45sehs5h',0,1
           ,0,0,GetDate(),0,0,0
           ,GetDate(),GetDate(),0),
           ('BotNine',1,'BotNine@shadowcanyonestudios.com','dw45sehs5h',0,1
           ,0,0,GetDate(),0,0,0
           ,GetDate(),GetDate(),0),
           ('BotTen',1,'BotTen@shadowcanyonestudios.com','dw45sehs5h',0,1
           ,0,0,GetDate(),0,0,0
           ,GetDate(),GetDate(),0)
GO



SELECT * FROM [Player];
  Select * FROM [VGamePlayer];
  select * from [VGamePlayerCategory];
SELECT * from [BotInfo];

  Select * from [PlayerTutorialMessageStatus];

  select * from [PlayerCampaign] where [playerid] = 44;
  SELECT * FROM [PlayerCampaignStageCategory] where [playercampaignid] = 44;
  select * from [Campaignstage];

  select * from [PlayerQuestionResult] where [playerid] = 179 and [PLayerQuestionResultId] > 681;

  select * from [playerCampaignCategoryQueue];
  select * from [campaignSection];

  select * from [PlayerCampaignSectionQuestion] where playercampaignid = 5;

  select * from [VGame] where [vgameid] = 70;
  Select * FROM [VGamePlayer] where [vgameid] = 70;
  select * from [VGamePlayerCategory] where [playerid] = 72;
  select * from [VGameCategory];

  select * from [Category];
  select * from [question] where [campaignsectionid] = 56;

  delete [VGamePlayer] where [vgameplayerid] = 38;

  Update [VGamePlayer] SET [GameSteps] = 0, [Score] = 0, [QuestionsAnswered] = 0;
  UPDATE [BotInfo] set [StageQuestionProgress] = 0, [QuestionProgress]= '1,2,2,3,3,4,5';
