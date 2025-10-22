using Retro_Engine;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Sonic_CD
{

    public class Game : MonoBehaviour
    {
      public int earnedGamerScore;
      public int maxGamerScore;
      public string[] achievementName = new string[12];
      public string[] achievementDesc = new string[12];
      public int[] achievementEarned = new int[12];
      public int[] achievementGamerScore = new int[12];
      public int[] achievementID = new int[12];
      public bool displayTitleUpdateMessage;

      void Start()
      {
        GlobalAppDefinitions.gameOnlineActive = 0;
        Initialize();
      }

      protected void Initialize()
      {
        Screen.SetResolution(1280, 720, false);
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;
      }

      protected void InitializeAchievements()
          {
            this.maxGamerScore = 0;
            this.earnedGamerScore = 0;
        
        string[] achievementKeys = {
          "88 Miles Per Hour",
          "Just One Hug is Enough", 
          "Paradise Found",
          "Take the High Road",
          "King of the Rings",
          "Statue Saviour",
          "Heavy Metal",
          "All Stages Clear",
          "Treasure Hunter",
          "Dr Eggman Got Served",
          "Just In Time",
          "Saviour of the Planet"
        };
        
        string[] achievementNames = {
          "88 Miles Per Hour",
          "Just One Hug is Enough",
          "Paradise Found", 
          "Take the High Road",
          "King of the Rings",
          "Statue Saviour",
          "Heavy Metal",
          "All Stages Clear",
          "Treasure Hunter",
          "Dr Eggman Got Served",
          "Just In Time",
          "Saviour of the Planet"
        };
        
        string[] achievementDescriptions = {
          "Reach 88 miles per hour",
          "Get just one hug",
          "Find paradise",
          "Take the high road",
          "Become king of the rings",
          "Save the statue",
          "Heavy metal achievement",
          "Clear all stages",
          "Find all treasures",
          "Defeat Dr. Eggman",
          "Just in time",
          "Save the planet"
        };
        
        int[] achievementScores = { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 };
        
        for (int i = 0; i < 12; i++)
        {
          this.achievementName[i] = achievementNames[i];
          this.achievementDesc[i] = achievementDescriptions[i];
          this.achievementGamerScore[i] = achievementScores[i];
          this.achievementID[i] = i;
          this.achievementEarned[i] = 0;
          this.maxGamerScore += achievementScores[i];
        }
      }

      public void AwardAchievement(string achievementKey)
      {
        for (int i = 0; i < 12; i++)
        {
          if (this.achievementName[i] == achievementKey && this.achievementEarned[i] == 0)
          {
            this.achievementEarned[i] = 1;
            this.earnedGamerScore += this.achievementGamerScore[i];
            Debug.Log($"Achievement unlocked: {achievementKey}");
              break;
            }
          }
      }

      public void LoadLeaderboardEntries()
      {
          GlobalAppDefinitions.gameMode = (byte) 7;
        ReadLeaderboardEntries();
        GlobalAppDefinitions.gameMode = (byte) 1;
      }

      protected void ReadLeaderboardEntries()
      {
        int globalVariable = ObjectSystem.globalVariables[114];
        TextSystem.SetupTextMenu(StageSystem.gameMenu[0], 0);
        
        if (globalVariable == 0)
        {
          for (int index = 0; index < 10; ++index)
          {
            int score = PlayerPrefs.GetInt($"BestScore_{index}", 0);
            string playerName = PlayerPrefs.GetString($"BestScorePlayer_{index}", "Player");
            string str = $"{(index + 1).ToString() + ".",4}{playerName,-15}{" ",1}{score.ToString(),8}";
            TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], str.ToCharArray());
          }
        }
        else
        {
          for (int index = 0; index < 10; ++index)
          {
            int timeValue = PlayerPrefs.GetInt($"BestTime_{index}", 0);
            string playerName = PlayerPrefs.GetString($"BestTimePlayer_{index}", "Player");
            int minutes = timeValue / 6000;
            int seconds = timeValue / 100 % 60;
            int centiseconds = timeValue % 100;
            string str = $"{(index + 1).ToString() + ".",4}{playerName,-15}{"  ",2}{minutes.ToString(),1}{"'",1}{seconds.ToString(),2}{"\"",1}{centiseconds.ToString(),2}";
            TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], str.ToCharArray());
          }
        }
        
        for (int count = 10; count < 100; ++count)
        {
          string str = $"{(count + 1).ToString() + ".",4}{"---------------",-15}";
          TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], str.ToCharArray());
        }
      }

      public void SetLeaderboard(int leaderboardID, int result)
      {
        if (leaderboardID == 0)
        {
          UpdateScoreLeaderboard(result);
        }
        else
        {
          UpdateTimeLeaderboard(result);
        }
      }

      private void UpdateScoreLeaderboard(int newScore)
      {
        int[] scores = new int[10];
        string[] players = new string[10];
        
        for (int i = 0; i < 10; i++)
        {
          scores[i] = PlayerPrefs.GetInt($"BestScore_{i}", 0);
          players[i] = PlayerPrefs.GetString($"BestScorePlayer_{i}", "Player");
        }
        
        for (int i = 0; i < 10; i++)
        {
          if (newScore > scores[i])
          {
            for (int j = 9; j > i; j--)
            {
              scores[j] = scores[j - 1];
              players[j] = players[j - 1];
            }
            scores[i] = newScore;
            players[i] = "Player";
            
            for (int k = 0; k < 10; k++)
            {
              PlayerPrefs.SetInt($"BestScore_{k}", scores[k]);
              PlayerPrefs.SetString($"BestScorePlayer_{k}", players[k]);
            }
            PlayerPrefs.Save();
            break;
          }
        }
      }

      private void UpdateTimeLeaderboard(int newTime)
      {
        int[] times = new int[10];
        string[] players = new string[10];
        
        for (int i = 0; i < 10; i++)
        {
          times[i] = PlayerPrefs.GetInt($"BestTime_{i}", int.MaxValue);
          players[i] = PlayerPrefs.GetString($"BestTimePlayer_{i}", "Player");
        }
        
        for (int i = 0; i < 10; i++)
        {
          if (newTime < times[i])
          {
            for (int j = 9; j > i; j--)
            {
              times[j] = times[j - 1];
              players[j] = players[j - 1];
            }
            times[i] = newTime;
            players[i] = "Player";
            
            for (int k = 0; k < 10; k++)
            {
              PlayerPrefs.SetInt($"BestTime_{k}", times[k]);
              PlayerPrefs.SetString($"BestTimePlayer_{k}", players[k]);
            }
            PlayerPrefs.Save();
            break;
          }
        }
      }

      void OnApplicationFocus(bool hasFocus)
      {
        if (hasFocus)
      {
        if (StageSystem.stageMode == (byte) 2)
        {
          if (GlobalAppDefinitions.gameMode == (byte) 7)
          {
            GlobalAppDefinitions.gameMode = (byte) 1;
            GlobalAppDefinitions.gameMessage = 4;
          }
        }
        else
        {
          if (GlobalAppDefinitions.gameMode == (byte) 7)
            GlobalAppDefinitions.gameMode = (byte) 1;
          GlobalAppDefinitions.gameMessage = 2;
          AudioPlayback.ResumeSound();
        }
        }
        else
        {
          GlobalAppDefinitions.gameMessage = 2;
          if (StageSystem.stageMode != (byte) 2 && GlobalAppDefinitions.gameMode != (byte) 7)
            AudioPlayback.PauseSound();
          GlobalAppDefinitions.gameMode = (byte) 7;
        }
      }

      void OnApplicationPause(bool pauseStatus)
      {
        if (pauseStatus)
      {
        GlobalAppDefinitions.gameMessage = 2;
        if (StageSystem.stageMode != (byte) 2 && GlobalAppDefinitions.gameMode != (byte) 7)
          AudioPlayback.PauseSound();
        GlobalAppDefinitions.gameMode = (byte) 7;
        }
        else
        {
          if (StageSystem.stageMode == (byte) 2)
          {
            if (GlobalAppDefinitions.gameMode == (byte) 7)
            {
              GlobalAppDefinitions.gameMode = (byte) 1;
              GlobalAppDefinitions.gameMessage = 4;
            }
          }
          else
          {
            if (GlobalAppDefinitions.gameMode == (byte) 7)
              GlobalAppDefinitions.gameMode = (byte) 1;
            GlobalAppDefinitions.gameMessage = 2;
            AudioPlayback.ResumeSound();
          }
        }
      }

      void Awake()
      {
        GlobalAppDefinitions.CalculateTrigAngles();
        
        RenderDevice.InitRenderDevice();
        RenderDevice.SetScreenDimensions(800, 480);
        
        EngineCallbacks.StartupRetroEngine();
        EngineCallbacks.gameRef = this;
        AudioPlayback.gameRef = this;
        
        switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
        {
          case "fr":
            GlobalAppDefinitions.gameLanguage = (byte) 1;
            break;
          case "it":
            GlobalAppDefinitions.gameLanguage = (byte) 2;
            break;
          case "de":
            GlobalAppDefinitions.gameLanguage = (byte) 3;
            break;
          case "es":
            GlobalAppDefinitions.gameLanguage = (byte) 4;
            break;
          case "ja":
            GlobalAppDefinitions.gameLanguage = (byte) 5;
            break;
          default:
            GlobalAppDefinitions.gameLanguage = (byte) 0;
            break;
        }
        
        InitializeAchievements();
        
        GlobalAppDefinitions.gameTrialMode = (byte) 0;
      }

      void OnDestroy()  {}

      void Update()
      {
        int pointerID = 0;
        InputSystem.CheckKeyboardInput();
        
        InputSystem.ClearTouchData();
        if (Input.touchCount > 0)
        {
          for (int i = 0; i < Input.touchCount && i < 10; i++)
          {
            Touch touch = Input.GetTouch(i);
            switch (touch.phase)
            {
              case TouchPhase.Began:
              case TouchPhase.Moved:
                InputSystem.AddTouch(touch.position.x, touch.position.y, pointerID);
              break;
          }
          ++pointerID;
        }
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
          if (FileIO.activeStageList == (byte) 0)
          {
            switch (StageSystem.stageListPosition)
            {
              case 4:
              case 5:
                InputSystem.touchData.start = (byte) 1;
                break;
              default:
                InputSystem.touchData.buttonB = (byte) 1;
                break;
            }
          }
          else if (StageSystem.stageMode == (byte) 2)
          {
            if (ObjectSystem.objectEntityList[9].state == (byte) 3 && GlobalAppDefinitions.gameMode == (byte) 1)
            {
              ObjectSystem.objectEntityList[9].state = (byte) 4;
              ObjectSystem.objectEntityList[9].value[0] = 0;
              ObjectSystem.objectEntityList[9].value[1] = 0;
              ObjectSystem.objectEntityList[9].alpha = (byte) 248;
              AudioPlayback.PlaySfx(27, (byte) 0);
            }
          }
          else
            GlobalAppDefinitions.gameMessage = 2;
        }
        
        if (StageSystem.stageMode != (byte) 2)
          EngineCallbacks.ProcessMainLoop();
      }

      void LateUpdate()
      {
        if (this.displayTitleUpdateMessage)
          EngineCallbacks.ShowLiveUpdateMessage();
        if (StageSystem.stageMode == (byte) 2)
          EngineCallbacks.ProcessMainLoop();
        if (RenderDevice.highResMode == 0)
          RenderDevice.FlipScreen();
        else
          RenderDevice.FlipScreenHRes();
      }
    }
}