/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [PlayerId]
      ,[UserName]
      ,[EmailAddr]
      ,[Password]
      ,[PlayerLevel]
      ,[TutorialInfoLevel]
      ,[CurrentSteps]
      ,[StepBank]
      ,[Coins]
      ,[Stars]
      ,[Points]
      ,[CreatedAt]
      ,[UpdatedAt]
      ,[Deleted]
  FROM [dbo].[Player]

  DELETE [PlayerQuestionResult]
  DELETE [PlayerCampaignCategoryQueue];
  DELETE [SectionCategoryQuestionPlayerResult];
  DELETE [PlayerCampaignStageCategory];
  DELETE [PLayerCampaign];
DELETE [Player];


  select * from [PlayerCampaign] where [playerid] = 70;
  SELECT * FROM [PlayerCampaignStageCategory] where [playercampaignid] = 67;
  select * from [Campaignstage];

  select * from [SectionCategoryQuestionPlayerResult];
  select * from [PlayerQuestionResult];

