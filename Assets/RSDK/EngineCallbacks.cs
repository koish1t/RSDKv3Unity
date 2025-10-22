using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Retro_Engine
{

    public static class EngineCallbacks
    {
      public const int DISPLAY_LOGOS = 0;
      public const int TITLE_SCREEN_PRESS_START = 1;
      public const int ENTER_TIMEATTACK_NOTIFY = 2;
      public const int EXIT_TIMEATTACK_NOTIFY = 3;
      public const int FINISH_GAME_NOTIFY = 4;
      public const int RETURN_TO_ARCADE_SELECTED = 5;
      public const int RESTART_GAME_SELECTED = 6;
      public const int EXIT_GAME_SELECTED = 7;
      public const int UNLOCK_FULL_GAME_SELECTED = 8;
      public const int TERMS_SELECTED = 9;
      public const int PRIVACY_SELECTED = 10;
      public const int TRIAL_ENDED = 11;
      public const int SETTINGS_SELECTED = 12;
      public const int FULL_VERSION_ONLY = 14;
      public static Sonic_CD.Game gameRef;
      public static string[] restartTitle = new string[6]
      {
        "Restart",
        "Recommencer",
        "Ricominciare",
        "Neustart",
        "Reiniciar",
        "Restart"
      };
      public static string[] exitTitle = new string[6]
      {
        "Exit",
        "Quitter",
        "Abbandonare",
        "Verlassen",
        "Abandonar",
        "Exit"
      };
      public static string[] restartMessage = new string[6]
      {
        "Are you sure you want to restart? Any unsaved progress will be lost.",
        "Veux-tu vraiment recommencer la partie? Toute progression non sauvegardée sera perdue.",
        "Sei sicuro di voler ricominciare la partita? I progressi non salvati andranno perduti.",
        "Möchtest du das rennen wirklich verlassen? Ungespeicherter Fortschritt geht verloren.",
        "¿Seguro que quieres reiniciar? Todo progreso no guardado se perderá.",
        "リスタートしますか？@セーブされていないデータが@きえるかのうせいがあります。"
      };
      public static string[] exitMessage = new string[6]
      {
        "Are you sure you want to exit? Any unsaved progress will be lost.",
        "Veux-tu vraiment quitter la partie? Toute progression non sauvegardée sera perdue.",
        "Sei sicuro di voler abbandonare la partita? I progressi non salvati andranno perduti.",
        "Möchtest du das spiel wirklich verlassen? Ungespeicherter Fortschritt geht verloren.",
        "¿Seguro que quieres abandonar? Todo progreso no guardado se perderá.",
        "ゲームを終了しますか？@セーブされていないデータが@きえるかのうせいがあります。"
      };
      public static string[] upsellTitle = new string[6]
      {
        "Buy Full Game",
        "Acheter le jeu complet",
        "Acquista gioco completo",
        "Spiele-Vollversion kaufen",
        "Comprar juego completo",
        "完全版の購入"
      };
      public static string[] upsellMessage = new string[6]
      {
        "This feature is not available in the trial version. Buy Full Game?",
        "Cette option n’est pas disponible avec la version d’essai. Acheter le jeu complet ?",
        "Questa opzione non è disponibile nella versione di prova. Acquistare il gioco completo?",
        "Diese Funktion ist in der Demo-Version nicht verfügbar. Spiele-Vollversion kaufen?",
        "Esta función no está disponible en la prueba. ¿Comprar juego completo?",
        "完全版をアンロック この機能は、@完全版でのみご利用いただけます。@完全版を購入しますか？"
      };
      public static string[] updateTitle = new string[6]
      {
        "Title Update Available",
        "Mise à jour du titre disponible",
        "È disponibile un aggiornamento del gioco",
        "Titel-Aktualisierung verfügbar",
        "Actualización del título disponible",
        "アップデートが可能です"
      };
      public static string[] updateMessage = new string[6]
      {
        "An update is available! This update is required to connect to Xbox LIVE. Update now?",
        "Une mise à jour est disponible ! Elle est nécessaire pour se connecter à Xbox LIVE. Mettre à jour maintenant ?",
        "È disponibile un aggiornamento! Quest'aggiornamento è necessario per connettersi a Xbox LIVE. Aggiornare ora?",
        "Eine Aktualisierung ist verfügbar! Diese Aktualisierung wird benötigt, um eine Verbindung zu Xbox LIVE herzustellen. Jetzt aktualisieren?",
        "¡Hay una actualización disponible! Esta actualización es necesaria para conectar con Xbox LIVE. ¿Actualizar ahora?",
        "アップデートが可能です。このアップデートはXbox LIVEへの接続が必要です。アップデートを行いますか？"
      };
      public static string[] liveErrorMessage = new string[6]
      {
        "Unable to connect to Xbox LIVE at this time. Please check your connection.",
        "Connexion au Xbox LIVE impossible. Vérifiez votre connexion.",
        "Impossibile connettersi a Xbox LIVE. Verifica la connessione.",
        "Es konnte momentan keine Verbindung zu Xbox LIVE hergestellt werden. Bitte überprüfen Sie Ihre Verbindung.",
        "No se puede conectar con Xbox LIVE. Comprueba tu conexión.",
        "Xbox LIVE に接続できませんでした。ネットワーク接続を確認してください。"
      };
      public static string[] yesMessage = new string[6]
      {
        "Yes",
        "Oui",
        "Sí",
        "Ya",
        "Sí",
        "Yes"
      };
      public static string[] noMessage = new string[6]
      {
        "No",
        "Non",
        "No",
        "Nein",
        "No",
        "No"
      };
      public static string[] achievementText = new string[6]
      {
        "Achievements ",
        "Succès ",
        "Obiettivi ",
        "Erfolge ",
        "Logros ",
        "Achievements "
      };
      public static string[] gamerscoreText = new string[6]
      {
        "Gamerscore ",
        "Score ",
        "Punteggio ",
        "Punkte ",
        "Puntuación ",
        "Gamerscore "
      };
      private static int prevMessage = 0;
      private static bool engineInit = false;
      private static int waitValue;

      static EngineCallbacks()
      {
        EngineCallbacks.restartMessage[5] = EngineCallbacks.restartMessage[5].Replace("@", " " + Environment.NewLine);
        EngineCallbacks.exitMessage[5] = EngineCallbacks.exitMessage[5].Replace("@", " " + Environment.NewLine);
        EngineCallbacks.upsellMessage[5] = EngineCallbacks.upsellMessage[5].Replace("@", " " + Environment.NewLine);
      }

      public static void PlayVideoFile(char[] fileName)
      {
        int startIndex = FileIO.StringLength(ref fileName);
        string uriString = $"Content/Video/{new string(fileName).Remove(startIndex)}.wmv";
        AudioPlayback.StopMusic();
        Thread.Sleep(1000);
        Application.OpenURL(uriString);
        EngineCallbacks.waitValue = 0;
        GlobalAppDefinitions.gameMode = (byte) 9;
      }

      public static void OnlineSetAchievement(int achievementID, int achievementDone)
      {
        if (achievementDone <= 99 || GlobalAppDefinitions.gameOnlineActive != (byte) 1 || GlobalAppDefinitions.gameTrialMode != (byte) 0)
          return;
        switch (achievementID)
        {
          case 0:
            EngineCallbacks.gameRef.AwardAchievement("88 Miles Per Hour");
            break;
          case 1:
            EngineCallbacks.gameRef.AwardAchievement("Just One Hug is Enough");
            break;
          case 2:
            EngineCallbacks.gameRef.AwardAchievement("Paradise Found");
            break;
          case 3:
            EngineCallbacks.gameRef.AwardAchievement("Take the High Road");
            break;
          case 4:
            EngineCallbacks.gameRef.AwardAchievement("King of the Rings");
            break;
          case 5:
            EngineCallbacks.gameRef.AwardAchievement("Statue Saviour");
            break;
          case 6:
            EngineCallbacks.gameRef.AwardAchievement("Heavy Metal");
            break;
          case 7:
            EngineCallbacks.gameRef.AwardAchievement("All Stages Clear");
            break;
          case 8:
            EngineCallbacks.gameRef.AwardAchievement("Treasure Hunter");
            break;
          case 9:
            EngineCallbacks.gameRef.AwardAchievement("Dr Eggman Got Served");
            break;
          case 10:
            EngineCallbacks.gameRef.AwardAchievement("Just In Time");
            break;
          case 11:
            EngineCallbacks.gameRef.AwardAchievement("Saviour of the Planet");
            break;
        }
      }

      public static void OnlineSetLeaderboard(int leaderboardID, int result)
      {
        EngineCallbacks.gameRef.SetLeaderboard(leaderboardID, result);
        switch (leaderboardID)
        {
          default:
            break;
        }
      }

      public static void OnlineLoadAchievementsMenu()
      {
        int num = 0;
        for (int index = 0; index < 12; ++index)
          num += EngineCallbacks.gameRef.achievementEarned[index];
        TextSystem.SetupTextMenu(StageSystem.gameMenu[0], 0);
        TextSystem.SetupTextMenu(StageSystem.gameMenu[1], 0);
        switch (GlobalAppDefinitions.gameLanguage)
        {
          case 1:
            string str1 = $"{EngineCallbacks.achievementText[1]}    ({num.ToString()}/12)";
            TextSystem.AddTextMenuEntryMapped(StageSystem.gameMenu[0], str1.ToCharArray());
            string str2 = $"{EngineCallbacks.gamerscoreText[1]}    ({EngineCallbacks.gameRef.earnedGamerScore.ToString()}/200)";
            TextSystem.AddTextMenuEntryMapped(StageSystem.gameMenu[0], str2.ToCharArray());
            break;
          case 2:
            string str3 = $"{EngineCallbacks.achievementText[2]}    ({num.ToString()}/12)";
            TextSystem.AddTextMenuEntryMapped(StageSystem.gameMenu[0], str3.ToCharArray());
            string str4 = $"{EngineCallbacks.gamerscoreText[2]}    ({EngineCallbacks.gameRef.earnedGamerScore.ToString()}/200)";
            TextSystem.AddTextMenuEntryMapped(StageSystem.gameMenu[0], str4.ToCharArray());
            break;
          case 3:
            string str5 = $"{EngineCallbacks.achievementText[3]}    ({num.ToString()}/12)";
            TextSystem.AddTextMenuEntryMapped(StageSystem.gameMenu[0], str5.ToCharArray());
            string str6 = $"{EngineCallbacks.gamerscoreText[3]}    ({EngineCallbacks.gameRef.earnedGamerScore.ToString()}/200)";
            TextSystem.AddTextMenuEntryMapped(StageSystem.gameMenu[0], str6.ToCharArray());
            break;
          case 4:
            string str7 = $"{EngineCallbacks.achievementText[4]}    ({num.ToString()}/12)";
            TextSystem.AddTextMenuEntryMapped(StageSystem.gameMenu[0], str7.ToCharArray());
            string str8 = $"{EngineCallbacks.gamerscoreText[4]}    ({EngineCallbacks.gameRef.earnedGamerScore.ToString()}/200)";
            TextSystem.AddTextMenuEntryMapped(StageSystem.gameMenu[0], str8.ToCharArray());
            break;
          case 5:
            string str9 = $"{EngineCallbacks.achievementText[5]}    ({num.ToString()}/12)";
            TextSystem.AddTextMenuEntryMapped(StageSystem.gameMenu[0], str9.ToCharArray());
            string str10 = $"{EngineCallbacks.gamerscoreText[5]}    ({EngineCallbacks.gameRef.earnedGamerScore.ToString()}/200)";
            TextSystem.AddTextMenuEntryMapped(StageSystem.gameMenu[0], str10.ToCharArray());
            break;
          default:
            string str11 = $"{EngineCallbacks.achievementText[0]}    ({num.ToString()}/12)";
            TextSystem.AddTextMenuEntryMapped(StageSystem.gameMenu[0], str11.ToCharArray());
            string str12 = $"{EngineCallbacks.gamerscoreText[0]}    ({EngineCallbacks.gameRef.earnedGamerScore.ToString()}/200)";
            TextSystem.AddTextMenuEntryMapped(StageSystem.gameMenu[0], str12.ToCharArray());
            break;
        }
        for (int index = 0; index < 12; ++index)
        {
          ObjectSystem.objectEntityList[34 + index].value[1] = EngineCallbacks.gameRef.achievementEarned[index];
          ObjectSystem.objectEntityList[34 + index].frame = (byte) EngineCallbacks.gameRef.achievementID[index];
          string str13 = EngineCallbacks.gameRef.achievementName[index] == null ? $"Achievement Name    ({EngineCallbacks.gameRef.achievementGamerScore[index].ToString()} G)" : $"{EngineCallbacks.gameRef.achievementName[index]}    ({EngineCallbacks.gameRef.achievementGamerScore[index].ToString()} G)";
          TextSystem.AddTextMenuEntryMapped(StageSystem.gameMenu[1], str13.ToCharArray());
          string str14 = EngineCallbacks.gameRef.achievementDesc[index] == null ? "Achievement Description" : EngineCallbacks.gameRef.achievementDesc[index];
          TextSystem.AddTextMenuEntryMapped(StageSystem.gameMenu[1], str14.ToCharArray());
        }
      }

      public static void OnlineLoadLeaderboardsMenu()
      {
        EngineCallbacks.gameRef.LoadLeaderboardEntries();
      }

      public static void RetroEngineCallback(int callbackID)
      {
        switch (callbackID)
        {
          case 6:
            GlobalAppDefinitions.gameMode = (byte) 1;
            GlobalAppDefinitions.gameMessage = 3;
            break;
          case 7:
            if (FileIO.activeStageList == (byte) 0)
            {
              GlobalAppDefinitions.gameMode = (byte) 1;
              GlobalAppDefinitions.gameMessage = 3;
              break;
            }
            GlobalAppDefinitions.gameMode = (byte) 1;
            GlobalAppDefinitions.gameMessage = 3;
            break;
          case 8:
            break;
          case 9:
            Application.OpenURL("http://www.sega.com/legal/terms_mobile.php");
            break;
          case 10:
            Application.OpenURL("http://www.sega.com/legal/privacy_mobile.php");
            break;
          case 14:
            GlobalAppDefinitions.gameMode = (byte) 1;
            GlobalAppDefinitions.gameMessage = 3;
            break;
        }
      }

      public static void UpsellScreen(IAsyncResult ar)
      {
        GlobalAppDefinitions.gameMode = (byte) 1;
        GlobalAppDefinitions.gameMessage = 3;
        if (GlobalAppDefinitions.gameTrialMode != (byte) 1)
          return;
        GlobalAppDefinitions.gameTrialMode = (byte) 0;
        GlobalAppDefinitions.gameMode = (byte) 2;
      }

      public static void ConfirmationScreen(IAsyncResult ar)
      {
        GlobalAppDefinitions.gameMode = (byte) 1;
        GlobalAppDefinitions.gameMessage = 3;
      }

      public static void ExitConfirmation(IAsyncResult ar)
      {
        Application.Quit();
      }

      public static void LiveErrorMessage(IAsyncResult ar)
      {
        GlobalAppDefinitions.gameMode = (byte) 1;
      }

      public static void ShowLiveUpdateMessage()
      {
        EngineCallbacks.gameRef.displayTitleUpdateMessage = false;
      }

      public static void UpdateMessage(IAsyncResult ar)
      {
        GlobalAppDefinitions.gameMode = (byte) 1;
        GlobalAppDefinitions.gameMessage = 3;
        EngineCallbacks.gameRef.displayTitleUpdateMessage = false;
      }

      public static void StartupRetroEngine()
      {
        if (!EngineCallbacks.engineInit)
        {
          GlobalAppDefinitions.CalculateTrigAngles();
          GraphicsSystem.GenerateBlendLookupTable();
          if (FileIO.CheckRSDKFile())
            GlobalAppDefinitions.LoadGameConfig("Data/Game/GameConfig.bin".ToCharArray());
          AudioPlayback.InitAudioPlayback();
          StageSystem.InitFirstStage();
          ObjectSystem.ClearScriptData();
          EngineCallbacks.engineInit = true;
        }
        else
          RenderDevice.UpdateHardwareTextures();
      }

      public static void ProcessMainLoop()
      {
        switch (GlobalAppDefinitions.gameMode)
        {
          case 0:
            GraphicsSystem.gfxIndexSize = (ushort) 0;
            GraphicsSystem.gfxVertexSize = (ushort) 0;
            GraphicsSystem.gfxIndexSizeOpaque = (ushort) 0;
            GraphicsSystem.gfxVertexSizeOpaque = (ushort) 0;
            StageSystem.ProcessStageSelectMenu();
            break;
          case 1:
            GraphicsSystem.gfxIndexSize = (ushort) 0;
            GraphicsSystem.gfxVertexSize = (ushort) 0;
            GraphicsSystem.gfxIndexSizeOpaque = (ushort) 0;
            GraphicsSystem.gfxVertexSizeOpaque = (ushort) 0;
            GraphicsSystem.vertexSize3D = (ushort) 0;
            GraphicsSystem.indexSize3D = (ushort) 0;
            GraphicsSystem.render3DEnabled = false;
            StageSystem.ProcessStage();
            if (EngineCallbacks.prevMessage == GlobalAppDefinitions.gameMessage)
            {
              GlobalAppDefinitions.gameMessage = 0;
              EngineCallbacks.prevMessage = 0;
              break;
            }
            EngineCallbacks.prevMessage = GlobalAppDefinitions.gameMessage;
            break;
          case 2:
            GlobalAppDefinitions.LoadGameConfig("Data/Game/GameConfig.bin".ToCharArray());
            StageSystem.InitFirstStage();
            FileIO.ResetCurrentStageFolder();
            break;
          case 4:
            GlobalAppDefinitions.LoadGameConfig("Data/Game/GameConfig.bin".ToCharArray());
            StageSystem.InitErrorMessage();
            FileIO.ResetCurrentStageFolder();
            break;
          case 5:
            GlobalAppDefinitions.gameMode = (byte) 1;
            RenderDevice.highResMode = 1;
            break;
          case 6:
            GlobalAppDefinitions.gameMode = (byte) 1;
            RenderDevice.highResMode = 0;
            break;
          case 8:
            if (EngineCallbacks.waitValue < 8)
            {
              ++EngineCallbacks.waitValue;
              break;
            }
            GlobalAppDefinitions.gameMode = (byte) 1;
            break;
          case 9:
            if (EngineCallbacks.waitValue < 60)
            {
              ++EngineCallbacks.waitValue;
              break;
            }
            GlobalAppDefinitions.gameMode = (byte) 1;
            break;
        }
      }
    }
}