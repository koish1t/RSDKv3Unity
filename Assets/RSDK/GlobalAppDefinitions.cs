using System;
using System.Reflection;

namespace Retro_Engine
{

    public static class GlobalAppDefinitions
    {
      public const int RETRO_EN = 0;
      public const int RETRO_FR = 1;
      public const int RETRO_IT = 2;
      public const int RETRO_DE = 3;
      public const int RETRO_ES = 4;
      public const int RETRO_JP = 5;
      public const int DEFAULTSCREEN = 0;
      public const int MAINGAME = 1;
      public const int RESETGAME = 2;
      public const int EXITGAME = 3;
      public const int SCRIPTERROR = 4;
      public const int ENTER_HIRESMODE = 5;
      public const int EXIT_HIRESMODE = 6;
      public const int PAUSE_ENGINE = 7;
      public const int PAUSE_WAIT = 8;
      public const int VIDEO_WAIT = 9;
      public const int RETRO_WIN = 0;
      public const int RETRO_OSX = 1;
      public const int RETRO_XBOX_360 = 2;
      public const int RETRO_PS3 = 3;
      public const int RETRO_iOS = 4;
      public const int RETRO_ANDROID = 5;
      public const int RETRO_WP7 = 6;
      public const int MAX_PLAYERS = 2;
      public const int FACING_LEFT = 1;
      public const int FACING_RIGHT = 0;
      public const int GAME_FULL = 0;
      public const int GAME_TRIAL = 1;
      public const int OBJECT_BORDER_Y1 = 256 /*0x0100*/;
      public const int OBJECT_BORDER_Y2 = 496;
      public const double Pi = 3.141592654;
      public static char[] gameWindowText = "Retro-Engine".ToCharArray();
      public static char[] gameVersion;
      public static char[] gameDescriptionText = new char[256 /*0x0100*/];
      public static char[] gamePlatform = "Mobile".ToCharArray();
      public static char[] gameRenderType = "HW_Rendering".ToCharArray();
      public static char[] gameHapticsSetting = "No_Haptics".ToCharArray();
      public static byte gameMode = 1;
      public static byte gameLanguage = 0;
      public static int gameMessage = 0;
      public static byte gameOnlineActive = 1;
      public static byte gameHapticsEnabled = 0;
      public static byte frameCounter = 0;
      public static int frameSkipTimer = -1;
      public static int frameSkipSetting = 0;
      public static int gameSFXVolume = 100;
      public static int gameBGMVolume = 100;
      public static byte gameTrialMode = 0;
      public static int gamePlatformID = 6;
      public static bool HQ3DFloorEnabled = false;
      public static int SCREEN_XSIZE = 320;
      public static int SCREEN_CENTER;
      public static int SCREEN_SCROLL_LEFT;
      public static int SCREEN_SCROLL_RIGHT;
      public static int OBJECT_BORDER_X1;
      public static int OBJECT_BORDER_X2;
      public static int[] SinValue256 = new int[256 /*0x0100*/];
      public static int[] CosValue256 = new int[256 /*0x0100*/];
      public static int[] SinValue512 = new int[512 /*0x0200*/];
      public static int[] CosValue512 = new int[512 /*0x0200*/];
      public static int[] SinValueM7 = new int[512 /*0x0200*/];
      public static int[] CosValueM7 = new int[512 /*0x0200*/];
      public static byte[,] ATanValue256 = new byte[256 /*0x0100*/, 256 /*0x0100*/];

      static GlobalAppDefinitions()
      {
        string[] strArray = Assembly.GetExecutingAssembly().FullName.Split(',')[1].Split('=');
        strArray[1] = strArray[1].Remove(6);
        GlobalAppDefinitions.gameVersion = strArray[1].ToCharArray();
      }

      public static void CalculateTrigAngles()
      {
        for (int index = 0; index < 512 /*0x0200*/; ++index)
        {
          GlobalAppDefinitions.SinValueM7[index] = (int) (Math.Sin((double) index / 256.0 * 3.141592654) * 4096.0);
          GlobalAppDefinitions.CosValueM7[index] = (int) (Math.Cos((double) index / 256.0 * 3.141592654) * 4096.0);
        }
        GlobalAppDefinitions.SinValueM7[0] = 0;
        GlobalAppDefinitions.CosValueM7[0] = 4096 /*0x1000*/;
        GlobalAppDefinitions.SinValueM7[128 /*0x80*/] = 4096 /*0x1000*/;
        GlobalAppDefinitions.CosValueM7[128 /*0x80*/] = 0;
        GlobalAppDefinitions.SinValueM7[256 /*0x0100*/] = 0;
        GlobalAppDefinitions.CosValueM7[256 /*0x0100*/] = -4096;
        GlobalAppDefinitions.SinValueM7[384] = -4096;
        GlobalAppDefinitions.CosValueM7[384] = 0;
        for (int index = 0; index < 512 /*0x0200*/; ++index)
        {
          GlobalAppDefinitions.SinValue512[index] = (int) (Math.Sin((double) index / 256.0 * 3.141592654) * 512.0);
          GlobalAppDefinitions.CosValue512[index] = (int) (Math.Cos((double) index / 256.0 * 3.141592654) * 512.0);
        }
        GlobalAppDefinitions.SinValue512[0] = 0;
        GlobalAppDefinitions.CosValue512[0] = 512 /*0x0200*/;
        GlobalAppDefinitions.SinValue512[128 /*0x80*/] = 512 /*0x0200*/;
        GlobalAppDefinitions.CosValue512[128 /*0x80*/] = 0;
        GlobalAppDefinitions.SinValue512[256 /*0x0100*/] = 0;
        GlobalAppDefinitions.CosValue512[256 /*0x0100*/] = -512;
        GlobalAppDefinitions.SinValue512[384] = -512;
        GlobalAppDefinitions.CosValue512[384] = 0;
        for (int index = 0; index < 256 /*0x0100*/; ++index)
        {
          GlobalAppDefinitions.SinValue256[index] = GlobalAppDefinitions.SinValue512[index << 1] >> 1;
          GlobalAppDefinitions.CosValue256[index] = GlobalAppDefinitions.CosValue512[index << 1] >> 1;
        }
        for (int y = 0; y < 256 /*0x0100*/; ++y)
        {
          for (int x = 0; x < 256 /*0x0100*/; ++x)
            GlobalAppDefinitions.ATanValue256[x, y] = (byte) (Math.Atan2((double) y, (double) x) * 40.74366542620519);
        }
      }

      public static byte ArcTanLookup(int x, int y)
      {
        int index1 = x >= 0 ? x : -x;
        int index2 = y >= 0 ? y : -y;
        if (index1 > index2)
        {
          while (index1 > (int) byte.MaxValue)
          {
            index1 >>= 4;
            index2 >>= 4;
          }
        }
        else
        {
          for (; index2 > (int) byte.MaxValue; index2 >>= 4)
            index1 >>= 4;
        }
        return x <= 0 ? (y <= 0 ? (byte) (128U /*0x80*/ + (uint) GlobalAppDefinitions.ATanValue256[index1, index2]) : (byte) (128U /*0x80*/ - (uint) GlobalAppDefinitions.ATanValue256[index1, index2])) : (y <= 0 ? (byte) (256U /*0x0100*/ - (uint) GlobalAppDefinitions.ATanValue256[index1, index2]) : GlobalAppDefinitions.ATanValue256[index1, index2]);
      }

      public static void LoadGameConfig(char[] filePath)
      {
        FileData fData = new FileData();
        char[] strB = new char[32 /*0x20*/];
        if (!FileIO.LoadFile(filePath, fData))
          return;
        byte numBytes1 = FileIO.ReadByte();
        FileIO.ReadCharArray(ref GlobalAppDefinitions.gameWindowText, (int) numBytes1);
        GlobalAppDefinitions.gameWindowText[(int) numBytes1] = char.MinValue;
        byte num1 = FileIO.ReadByte();
        byte num2;
        for (int index = 0; index < (int) num1; ++index)
          num2 = FileIO.ReadByte();
        byte numBytes2 = FileIO.ReadByte();
        FileIO.ReadCharArray(ref GlobalAppDefinitions.gameDescriptionText, (int) numBytes2);
        GlobalAppDefinitions.gameDescriptionText[(int) numBytes2] = char.MinValue;
        byte num3 = FileIO.ReadByte();
        for (int index1 = 0; index1 < (int) num3; ++index1)
        {
          byte num4 = FileIO.ReadByte();
          for (int index2 = 0; index2 < (int) num4; ++index2)
            num2 = FileIO.ReadByte();
        }
        for (int index3 = 0; index3 < (int) num3; ++index3)
        {
          byte num5 = FileIO.ReadByte();
          for (int index4 = 0; index4 < (int) num5; ++index4)
            num2 = FileIO.ReadByte();
        }
        byte num6 = FileIO.ReadByte();
        ObjectSystem.NO_GLOBALVARIABLES = (byte) 0;
        for (int strPos = 0; strPos < (int) num6; ++strPos)
        {
          ++ObjectSystem.NO_GLOBALVARIABLES;
          byte num7 = FileIO.ReadByte();
          int index;
          for (index = 0; index < (int) num7; ++index)
          {
            byte num8 = FileIO.ReadByte();
            strB[index] = (char) num8;
          }
          strB[index] = char.MinValue;
          FileIO.StrCopy2D(ref ObjectSystem.globalVariableNames, ref strB, strPos);
          byte num9 = FileIO.ReadByte();
          ObjectSystem.globalVariables[strPos] = (int) num9 << 24;
          byte num10 = FileIO.ReadByte();
          ObjectSystem.globalVariables[strPos] += (int) num10 << 16 /*0x10*/;
          byte num11 = FileIO.ReadByte();
          ObjectSystem.globalVariables[strPos] += (int) num11 << 8;
          byte num12 = FileIO.ReadByte();
          ObjectSystem.globalVariables[strPos] += (int) num12;
        }
        byte num13 = FileIO.ReadByte();
        for (int index5 = 0; index5 < (int) num13; ++index5)
        {
          byte num14 = FileIO.ReadByte();
          int index6;
          for (index6 = 0; index6 < (int) num14; ++index6)
          {
            byte num15 = FileIO.ReadByte();
            strB[index6] = (char) num15;
          }
          strB[index6] = char.MinValue;
        }
        byte num16 = FileIO.ReadByte();
        for (int index7 = 0; index7 < (int) num16; ++index7)
        {
          byte num17 = FileIO.ReadByte();
          for (int index8 = 0; index8 < (int) num17; ++index8)
            num2 = FileIO.ReadByte();
        }
        FileIO.noPresentationStages = FileIO.ReadByte();
        byte num18;
        for (int index9 = 0; index9 < (int) FileIO.noPresentationStages; ++index9)
        {
          byte num19 = FileIO.ReadByte();
          int index10;
          for (index10 = 0; index10 < (int) num19; ++index10)
          {
            byte num20 = FileIO.ReadByte();
            FileIO.pStageList[index9].stageFolderName[index10] = (char) num20;
          }
          FileIO.pStageList[index9].stageFolderName[index10] = char.MinValue;
          byte num21 = FileIO.ReadByte();
          int index11;
          for (index11 = 0; index11 < (int) num21; ++index11)
          {
            byte num22 = FileIO.ReadByte();
            FileIO.pStageList[index9].actNumber[index11] = (char) num22;
          }
          FileIO.pStageList[index9].actNumber[index11] = char.MinValue;
          byte num23 = FileIO.ReadByte();
          for (int index12 = 0; index12 < (int) num23; ++index12)
            num2 = FileIO.ReadByte();
          num18 = FileIO.ReadByte();
        }
        FileIO.noZoneStages = FileIO.ReadByte();
        for (int index13 = 0; index13 < (int) FileIO.noZoneStages; ++index13)
        {
          byte num24 = FileIO.ReadByte();
          int index14;
          for (index14 = 0; index14 < (int) num24; ++index14)
          {
            byte num25 = FileIO.ReadByte();
            FileIO.zStageList[index13].stageFolderName[index14] = (char) num25;
          }
          FileIO.zStageList[index13].stageFolderName[index14] = char.MinValue;
          byte num26 = FileIO.ReadByte();
          int index15;
          for (index15 = 0; index15 < (int) num26; ++index15)
          {
            byte num27 = FileIO.ReadByte();
            FileIO.zStageList[index13].actNumber[index15] = (char) num27;
          }
          FileIO.zStageList[index13].actNumber[index15] = char.MinValue;
          byte num28 = FileIO.ReadByte();
          for (int index16 = 0; index16 < (int) num28; ++index16)
            num2 = FileIO.ReadByte();
          num18 = FileIO.ReadByte();
        }
        FileIO.noSpecialStages = FileIO.ReadByte();
        for (int index17 = 0; index17 < (int) FileIO.noSpecialStages; ++index17)
        {
          byte num29 = FileIO.ReadByte();
          int index18;
          for (index18 = 0; index18 < (int) num29; ++index18)
          {
            byte num30 = FileIO.ReadByte();
            FileIO.sStageList[index17].stageFolderName[index18] = (char) num30;
          }
          FileIO.sStageList[index17].stageFolderName[index18] = char.MinValue;
          byte num31 = FileIO.ReadByte();
          int index19;
          for (index19 = 0; index19 < (int) num31; ++index19)
          {
            byte num32 = FileIO.ReadByte();
            FileIO.sStageList[index17].actNumber[index19] = (char) num32;
          }
          FileIO.sStageList[index17].actNumber[index19] = char.MinValue;
          byte num33 = FileIO.ReadByte();
          for (int index20 = 0; index20 < (int) num33; ++index20)
            num2 = FileIO.ReadByte();
          num18 = FileIO.ReadByte();
        }
        FileIO.noBonusStages = FileIO.ReadByte();
        for (int index21 = 0; index21 < (int) FileIO.noBonusStages; ++index21)
        {
          byte num34 = FileIO.ReadByte();
          int index22;
          for (index22 = 0; index22 < (int) num34; ++index22)
          {
            byte num35 = FileIO.ReadByte();
            FileIO.bStageList[index21].stageFolderName[index22] = (char) num35;
          }
          FileIO.bStageList[index21].stageFolderName[index22] = char.MinValue;
          byte num36 = FileIO.ReadByte();
          int index23;
          for (index23 = 0; index23 < (int) num36; ++index23)
          {
            byte num37 = FileIO.ReadByte();
            FileIO.bStageList[index21].actNumber[index23] = (char) num37;
          }
          FileIO.bStageList[index21].actNumber[index23] = char.MinValue;
          byte num38 = FileIO.ReadByte();
          for (int index24 = 0; index24 < (int) num38; ++index24)
            num2 = FileIO.ReadByte();
          num18 = FileIO.ReadByte();
        }
        FileIO.CloseFile();
      }
    }
}