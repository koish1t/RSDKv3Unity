using UnityEngine;

namespace Retro_Engine
{

    public static class StageSystem
    {
      public const int ACTLAYOUT = 0;
      public const int LOADSTAGE = 0;
      public const int PLAYSTAGE = 1;
      public const int STAGEPAUSED = 2;
      public static InputResult gKeyDown = new InputResult();
      public static InputResult gKeyPress = new InputResult();
      public static byte stageMode;
      public static byte pauseEnabled;
      public static int stageListPosition;
      public static Mappings128x128 tile128x128 = new Mappings128x128();
      public static LayoutMap[] stageLayouts = new LayoutMap[9];
      public static byte tLayerMidPoint;
      public static byte[] activeTileLayers = new byte[4];
      public static CollisionMask16x16[] tileCollisions = new CollisionMask16x16[2];
      public static LineScrollParallax hParallax = new LineScrollParallax();
      public static LineScrollParallax vParallax = new LineScrollParallax();
      public static int lastXSize;
      public static int lastYSize;
      public static int[] bgDeformationData0 = new int[576];
      public static int[] bgDeformationData1 = new int[576];
      public static int[] bgDeformationData2 = new int[576];
      public static int[] bgDeformationData3 = new int[576];
      public static int xBoundary1 = 0;
      public static int xBoundary2;
      public static int yBoundary1 = 0;
      public static int yBoundary2;
      public static int newXBoundary1 = 0;
      public static int newXBoundary2;
      public static int newYBoundary1 = 0;
      public static int newYBoundary2;
      public static byte cameraEnabled;
      public static sbyte cameraTarget;
      public static byte cameraShift = 0;
      public static byte cameraStyle = 0;
      public static int cameraAdjustY;
      public static int xScrollOffset = 0;
      public static int yScrollOffset = 0;
      public static int yScrollA = 0;
      public static int yScrollB = 240 /*0xF0*/;
      public static int xScrollA = 0;
      public static int xScrollB = 320;
      public static int xScrollMove = 0;
      public static int yScrollMove = 0;
      public static int screenShakeX = 0;
      public static int screenShakeY = 0;
      public static int waterLevel;
      public static char[] titleCardText = new char[24];
      public static char titleCardWord2;
      public static TextMenu[] gameMenu = new TextMenu[2];
      public static byte timeEnabled;
      public static byte milliSeconds;
      public static byte seconds;
      public static byte minutes;
      public static byte debugMode = 0;

      static StageSystem()
      {
        for (int index = 0; index < StageSystem.stageLayouts.Length; ++index)
          StageSystem.stageLayouts[index] = new LayoutMap();
        for (int index = 0; index < StageSystem.tileCollisions.Length; ++index)
          StageSystem.tileCollisions[index] = new CollisionMask16x16();
        for (int index = 0; index < StageSystem.gameMenu.Length; ++index)
          StageSystem.gameMenu[index] = new TextMenu();
      }

      public static void ProcessStage()
      {
        switch (StageSystem.stageMode)
        {
          case 0:
            AudioPlayback.StopMusic();
            GraphicsSystem.fadeMode = (byte) 0;
            GraphicsSystem.paletteMode = (byte) 0;
            GraphicsSystem.SetActivePalette((byte) 0, 0, 256 /*0x0100*/);
            StageSystem.cameraEnabled = (byte) 1;
            StageSystem.cameraTarget = (sbyte) -1;
            StageSystem.cameraAdjustY = 0;
            StageSystem.xScrollOffset = 0;
            StageSystem.yScrollOffset = 0;
            StageSystem.yScrollA = 0;
            StageSystem.yScrollB = 240 /*0xF0*/;
            StageSystem.xScrollA = 0;
            StageSystem.xScrollB = 320;
            StageSystem.xScrollMove = 0;
            StageSystem.yScrollMove = 0;
            StageSystem.screenShakeX = 0;
            StageSystem.screenShakeY = 0;
            Scene3D.numVertices = 0;
            Scene3D.numFaces = 0;
            for (int index = 0; index < 2; ++index)
            {
              PlayerSystem.playerList[index].xPos = 0;
              PlayerSystem.playerList[index].yPos = 0;
              PlayerSystem.playerList[index].xVelocity = 0;
              PlayerSystem.playerList[index].yVelocity = 0;
              PlayerSystem.playerList[index].angle = 0;
              PlayerSystem.playerList[index].visible = (byte) 1;
              PlayerSystem.playerList[index].collisionPlane = (byte) 0;
              PlayerSystem.playerList[index].collisionMode = (byte) 0;
              PlayerSystem.playerList[index].gravity = (byte) 1;
              PlayerSystem.playerList[index].speed = 0;
              PlayerSystem.playerList[index].tileCollisions = (byte) 1;
              PlayerSystem.playerList[index].objectInteraction = (byte) 1;
              PlayerSystem.playerList[index].value[0] = 0;
              PlayerSystem.playerList[index].value[1] = 0;
              PlayerSystem.playerList[index].value[2] = 0;
              PlayerSystem.playerList[index].value[3] = 0;
              PlayerSystem.playerList[index].value[4] = 0;
              PlayerSystem.playerList[index].value[5] = 0;
              PlayerSystem.playerList[index].value[6] = 0;
              PlayerSystem.playerList[index].value[7] = 0;
            }
            StageSystem.pauseEnabled = (byte) 0;
            StageSystem.timeEnabled = (byte) 0;
            StageSystem.milliSeconds = (byte) 0;
            StageSystem.seconds = (byte) 0;
            StageSystem.minutes = (byte) 0;
            GlobalAppDefinitions.frameCounter = (byte) 0;
            StageSystem.resetBackgroundSettings();
            StageSystem.LoadStageFiles();
            GraphicsSystem.texBufferMode = (byte) 0;
            for (int index = 0; index < 9; ++index)
            {
              if (StageSystem.stageLayouts[index].type == (byte) 4)
                GraphicsSystem.texBufferMode = (byte) 1;
            }
            for (int index = 0; index < (int) StageSystem.hParallax.numEntries; ++index)
            {
              if (StageSystem.hParallax.deformationEnabled[index] == (byte) 1)
                GraphicsSystem.texBufferMode = (byte) 1;
            }
            if (GraphicsSystem.tileGfx[204802] > (byte) 0)
              GraphicsSystem.texBufferMode = (byte) 0;
            if (GraphicsSystem.texBufferMode == (byte) 0)
            {
              for (int index = 0; index < 4096 /*0x1000*/; index += 4)
              {
                GraphicsSystem.tileUVArray[index] = (float) ((index >> 2 & 31 /*0x1F*/) * 16 /*0x10*/) * 0.0009765625f;
                GraphicsSystem.tileUVArray[index + 1] = (float) ((index >> 2 >> 5) * 16 /*0x10*/) * 0.0009765625f;
                GraphicsSystem.tileUVArray[index + 2] = GraphicsSystem.tileUVArray[index] + 1f / 64f;
                GraphicsSystem.tileUVArray[index + 3] = GraphicsSystem.tileUVArray[index + 1] + 1f / 64f;
              }
            }
            else
            {
              for (int index = 0; index < 4096 /*0x1000*/; index += 4)
              {
                GraphicsSystem.tileUVArray[index] = (float) ((index >> 2) % 28 * 18 + 1) * 0.0009765625f;
                GraphicsSystem.tileUVArray[index + 1] = (float) ((index >> 2) / 28 * 18 + 1) * 0.0009765625f;
                GraphicsSystem.tileUVArray[index + 2] = GraphicsSystem.tileUVArray[index] + 1f / 64f;
                GraphicsSystem.tileUVArray[index + 3] = GraphicsSystem.tileUVArray[index + 1] + 1f / 64f;
              }
              GraphicsSystem.tileUVArray[4092] = 0.475585938f;
              GraphicsSystem.tileUVArray[4093] = 0.475585938f;
              GraphicsSystem.tileUVArray[4094] = 0.491210938f;
              GraphicsSystem.tileUVArray[4095 /*0x0FFF*/] = 0.491210938f;
            }
            RenderDevice.UpdateHardwareTextures();
            StageSystem.stageMode = (byte) 1;
            GraphicsSystem.gfxIndexSize = (ushort) 0;
            GraphicsSystem.gfxVertexSize = (ushort) 0;
            GraphicsSystem.gfxIndexSizeOpaque = (ushort) 0;
            GraphicsSystem.gfxVertexSizeOpaque = (ushort) 0;
            StageSystem.stageMode = (byte) 1;
            break;
          case 1:
            if (GraphicsSystem.fadeMode > (byte) 0)
              --GraphicsSystem.fadeMode;
            if (GraphicsSystem.paletteMode > (byte) 0)
            {
              GraphicsSystem.paletteMode = (byte) 0;
              GraphicsSystem.texPaletteNum = 0;
            }
            StageSystem.lastXSize = -1;
            StageSystem.lastYSize = -1;
            InputSystem.CheckKeyDown(StageSystem.gKeyDown, byte.MaxValue);
            InputSystem.CheckKeyPress(StageSystem.gKeyPress, byte.MaxValue);
            if (StageSystem.pauseEnabled == (byte) 1 && StageSystem.gKeyPress.start == (byte) 1)
            {
              StageSystem.stageMode = (byte) 2;
              AudioPlayback.PauseSound();
            }
            if (StageSystem.timeEnabled == (byte) 1)
            {
              ++GlobalAppDefinitions.frameCounter;
              if (GlobalAppDefinitions.frameCounter == (byte) 60)
              {
                GlobalAppDefinitions.frameCounter = (byte) 0;
                ++StageSystem.seconds;
                if (StageSystem.seconds > (byte) 59)
                {
                  StageSystem.seconds = (byte) 0;
                  ++StageSystem.minutes;
                  if (StageSystem.minutes > (byte) 59)
                    StageSystem.minutes = (byte) 0;
                }
              }
              StageSystem.milliSeconds = (byte) ((int) GlobalAppDefinitions.frameCounter * 100 / 60);
            }
            ObjectSystem.ProcessObjects();
            if (StageSystem.cameraTarget > (sbyte) -1)
            {
              if (StageSystem.cameraEnabled == (byte) 1)
              {
                switch (StageSystem.cameraStyle)
                {
                  case 0:
                    PlayerSystem.SetPlayerScreenPosition(PlayerSystem.playerList[(int) StageSystem.cameraTarget]);
                    break;
                  case 1:
                    PlayerSystem.SetPlayerScreenPositionCDStyle(PlayerSystem.playerList[(int) StageSystem.cameraTarget]);
                    break;
                  case 2:
                    PlayerSystem.SetPlayerScreenPositionCDStyle(PlayerSystem.playerList[(int) StageSystem.cameraTarget]);
                    break;
                  case 3:
                    PlayerSystem.SetPlayerScreenPositionCDStyle(PlayerSystem.playerList[(int) StageSystem.cameraTarget]);
                    break;
                  case 4:
                    PlayerSystem.SetPlayerHLockedScreenPosition(PlayerSystem.playerList[(int) StageSystem.cameraTarget]);
                    break;
                }
              }
              else
                PlayerSystem.SetPlayerLockedScreenPosition(PlayerSystem.playerList[(int) StageSystem.cameraTarget]);
            }
            StageSystem.DrawStageGfx();
            if (GraphicsSystem.fadeMode > (byte) 0)
              GraphicsSystem.DrawRectangle(0, 0, GlobalAppDefinitions.SCREEN_XSIZE, 240 /*0xF0*/, (int) GraphicsSystem.fadeR, (int) GraphicsSystem.fadeG, (int) GraphicsSystem.fadeB, (int) GraphicsSystem.fadeA);
            if (StageSystem.stageMode != (byte) 2)
              break;
            GlobalAppDefinitions.gameMode = (byte) 8;
            break;
          case 2:
            if (GraphicsSystem.fadeMode > (byte) 0)
              --GraphicsSystem.fadeMode;
            if (GraphicsSystem.paletteMode > (byte) 0)
            {
              GraphicsSystem.paletteMode = (byte) 0;
              GraphicsSystem.texPaletteNum = 0;
            }
            StageSystem.lastXSize = -1;
            StageSystem.lastYSize = -1;
            InputSystem.CheckKeyDown(StageSystem.gKeyDown, byte.MaxValue);
            InputSystem.CheckKeyPress(StageSystem.gKeyPress, byte.MaxValue);
            GraphicsSystem.gfxIndexSize = (ushort) 0;
            GraphicsSystem.gfxVertexSize = (ushort) 0;
            GraphicsSystem.gfxIndexSizeOpaque = (ushort) 0;
            GraphicsSystem.gfxVertexSizeOpaque = (ushort) 0;
            ObjectSystem.ProcessPausedObjects();
            ObjectSystem.DrawObjectList(0);
            ObjectSystem.DrawObjectList(1);
            ObjectSystem.DrawObjectList(2);
            ObjectSystem.DrawObjectList(3);
            ObjectSystem.DrawObjectList(4);
            ObjectSystem.DrawObjectList(5);
            ObjectSystem.DrawObjectList(6);
            if (StageSystem.pauseEnabled != (byte) 1 || StageSystem.gKeyPress.start != (byte) 1)
              break;
            StageSystem.stageMode = (byte) 1;
            AudioPlayback.resumeSound();
            break;
        }
      }

      public static void LoadStageFiles()
      {
        FileData fData = new FileData();
        byte[] byteP = new byte[3];
        char[] charP = new char[64 /*0x40*/];
        int scriptNum = 1;
        AudioPlayback.StopAllSFX();
        if (!FileIO.CheckCurrentStageFolder(StageSystem.stageListPosition))
        {
          AudioPlayback.releaseStageSFX();
          GraphicsSystem.LoadPalette("MasterPalette.act".ToCharArray(), 0, 0, 0, 256 /*0x0100*/);
          ObjectSystem.ClearScriptData();
          for (int index = 16 /*0x10*/; index > 0; --index)
            GraphicsSystem.removeGraphicsFile("".ToCharArray(), index - 1);
          if (FileIO.LoadStageFile("StageConfig.bin".ToCharArray(), StageSystem.stageListPosition, fData))
          {
            byteP[0] = FileIO.ReadByte();
            FileIO.CloseFile();
          }
          if (byteP[0] == (byte) 1 && FileIO.LoadFile("Data/Game/GameConfig.bin".ToCharArray(), fData))
          {
            byteP[0] = FileIO.ReadByte();
            for (int index = 0; index < (int) byteP[0]; ++index)
              byteP[1] = FileIO.ReadByte();
            byteP[0] = FileIO.ReadByte();
            for (int index = 0; index < (int) byteP[0]; ++index)
              byteP[1] = FileIO.ReadByte();
            byteP[0] = FileIO.ReadByte();
            for (int index = 0; index < (int) byteP[0]; ++index)
              byteP[1] = FileIO.ReadByte();
            byteP[0] = FileIO.ReadByte();
            for (int index = 0; index < (int) byteP[0]; ++index)
            {
              byteP[1] = FileIO.ReadByte();
              FileIO.ReadCharArray(ref charP, (int) byteP[1]);
              charP[(int) byteP[1]] = char.MinValue;
              ObjectSystem.SetObjectTypeName(charP, scriptNum + index);
            }
            if (FileIO.useByteCode)
            {
              FileIO.GetFileInfo(fData);
              FileIO.CloseFile();
              ObjectSystem.LoadByteCodeFile(4, scriptNum);
              scriptNum += (int) byteP[0];
              FileIO.SetFileInfo(fData);
            }
            FileIO.CloseFile();
          }
          if (FileIO.LoadStageFile("StageConfig.bin".ToCharArray(), StageSystem.stageListPosition, fData))
          {
            byteP[0] = FileIO.ReadByte();
            for (int entryPos = 96 /*0x60*/; entryPos < 128 /*0x80*/; ++entryPos)
            {
              FileIO.ReadByteArray(ref byteP, 3);
              GraphicsSystem.SetPaletteEntry((byte) entryPos, byteP[0], byteP[1], byteP[2]);
            }
            byteP[0] = FileIO.ReadByte();
            for (int index = 0; index < (int) byteP[0]; ++index)
            {
              byteP[1] = FileIO.ReadByte();
              FileIO.ReadCharArray(ref charP, (int) byteP[1]);
              charP[(int) byteP[1]] = char.MinValue;
              ObjectSystem.SetObjectTypeName(charP, index + scriptNum);
            }
            if (FileIO.useByteCode)
            {
              for (int index = 0; index < (int) byteP[0]; ++index)
              {
                byteP[1] = FileIO.ReadByte();
                FileIO.ReadCharArray(ref charP, (int) byteP[1]);
                charP[(int) byteP[1]] = char.MinValue;
              }
              FileIO.GetFileInfo(fData);
              FileIO.CloseFile();
              ObjectSystem.LoadByteCodeFile((int) FileIO.activeStageList, scriptNum);
              FileIO.SetFileInfo(fData);
            }
            byteP[0] = FileIO.ReadByte();
            AudioPlayback.numStageSFX = (int) byteP[0];
            for (int index = 0; index < (int) byteP[0]; ++index)
            {
              byteP[1] = FileIO.ReadByte();
              FileIO.ReadCharArray(ref charP, (int) byteP[1]);
              charP[(int) byteP[1]] = char.MinValue;
              FileIO.GetFileInfo(fData);
              FileIO.CloseFile();
              AudioPlayback.LoadSfx(charP, index + AudioPlayback.numGlobalSFX);
              FileIO.SetFileInfo(fData);
            }
            FileIO.CloseFile();
          }
          GraphicsSystem.LoadStageGIFFile(StageSystem.stageListPosition);
          StageSystem.LoadStageCollisions();
          StageSystem.LoadStageBackground();
        }
        StageSystem.Load128x128Mappings();
        for (int trackNo = 0; trackNo < 16 /*0x10*/; ++trackNo)
          AudioPlayback.SetMusicTrack("".ToCharArray(), trackNo, (byte) 0, 0U);
        for (int index = 0; index < 1184; ++index)
        {
          ObjectSystem.objectEntityList[index].type = (byte) 0;
          ObjectSystem.objectEntityList[index].direction = (byte) 0;
          ObjectSystem.objectEntityList[index].animation = (byte) 0;
          ObjectSystem.objectEntityList[index].prevAnimation = (byte) 0;
          ObjectSystem.objectEntityList[index].animationSpeed = 0;
          ObjectSystem.objectEntityList[index].animationTimer = 0;
          ObjectSystem.objectEntityList[index].frame = (byte) 0;
          ObjectSystem.objectEntityList[index].priority = (byte) 0;
          ObjectSystem.objectEntityList[index].direction = (byte) 0;
          ObjectSystem.objectEntityList[index].rotation = 0;
          ObjectSystem.objectEntityList[index].state = (byte) 0;
          ObjectSystem.objectEntityList[index].propertyValue = (byte) 0;
          ObjectSystem.objectEntityList[index].xPos = 0;
          ObjectSystem.objectEntityList[index].yPos = 0;
          ObjectSystem.objectEntityList[index].drawOrder = (byte) 3;
          ObjectSystem.objectEntityList[index].scale = 512 /*0x0200*/;
          ObjectSystem.objectEntityList[index].inkEffect = (byte) 0;
          ObjectSystem.objectEntityList[index].value[0] = 0;
          ObjectSystem.objectEntityList[index].value[1] = 0;
          ObjectSystem.objectEntityList[index].value[2] = 0;
          ObjectSystem.objectEntityList[index].value[3] = 0;
          ObjectSystem.objectEntityList[index].value[4] = 0;
          ObjectSystem.objectEntityList[index].value[5] = 0;
          ObjectSystem.objectEntityList[index].value[6] = 0;
          ObjectSystem.objectEntityList[index].value[7] = 0;
        }
        StageSystem.LoadActLayout();
        ObjectSystem.ProcessStartupScripts();
        StageSystem.xScrollA = (PlayerSystem.playerList[0].xPos >> 16 /*0x10*/) - 160 /*0xA0*/;
        StageSystem.xScrollB = StageSystem.xScrollA + 320;
        StageSystem.yScrollA = (PlayerSystem.playerList[0].yPos >> 16 /*0x10*/) - 104;
        StageSystem.yScrollB = StageSystem.yScrollA + 240 /*0xF0*/;
      }

      public static void Load128x128Mappings()
      {
        FileData fData = new FileData();
        int index = 0;
        byte[] byteP = new byte[2];
        if (!FileIO.LoadStageFile("128x128Tiles.bin".ToCharArray(), StageSystem.stageListPosition, fData))
          return;
        for (; index < 32768 /*0x8000*/; ++index)
        {
          FileIO.ReadByteArray(ref byteP, 2);
          byteP[0] = (byte) ((uint) byteP[0] - (uint) ((int) byteP[0] >> 6 << 6));
          StageSystem.tile128x128.visualPlane[index] = (byte) ((uint) byteP[0] >> 4);
          byteP[0] = (byte) ((uint) byteP[0] - (uint) ((int) byteP[0] >> 4 << 4));
          StageSystem.tile128x128.direction[index] = (byte) ((uint) byteP[0] >> 2);
          byteP[0] = (byte) ((uint) byteP[0] - (uint) ((int) byteP[0] >> 2 << 2));
          StageSystem.tile128x128.tile16x16[index] = (ushort) (((uint) byteP[0] << 8) + (uint) byteP[1]);
          StageSystem.tile128x128.gfxDataPos[index] = (int) StageSystem.tile128x128.tile16x16[index] << 2;
          byteP[0] = FileIO.ReadByte();
          StageSystem.tile128x128.collisionFlag[0, index] = (byte) ((uint) byteP[0] >> 4);
          StageSystem.tile128x128.collisionFlag[1, index] = (byte) ((uint) byteP[0] - (uint) ((int) byteP[0] >> 4 << 4));
        }
        FileIO.CloseFile();
      }

      public static void LoadStageCollisions()
      {
        FileData fData = new FileData();
        int num1 = 0;
        if (!FileIO.LoadStageFile("CollisionMasks.bin".ToCharArray(), StageSystem.stageListPosition, fData))
          return;
        for (int index1 = 0; index1 < 1024 /*0x0400*/; ++index1)
        {
          for (int index2 = 0; index2 < 2; ++index2)
          {
            byte num2 = FileIO.ReadByte();
            int num3 = (int) num2 >> 4;
            StageSystem.tileCollisions[index2].flags[index1] = (byte) ((uint) num2 & 15U);
            byte num4 = FileIO.ReadByte();
            StageSystem.tileCollisions[index2].angle[index1] = (uint) num4;
            byte num5 = FileIO.ReadByte();
            StageSystem.tileCollisions[index2].angle[index1] += (uint) num5 << 8;
            byte num6 = FileIO.ReadByte();
            StageSystem.tileCollisions[index2].angle[index1] += (uint) num6 << 16 /*0x10*/;
            byte num7 = FileIO.ReadByte();
            StageSystem.tileCollisions[index2].angle[index1] += (uint) num7 << 24;
            if (num3 == 0)
            {
              for (int index3 = 0; index3 < 16 /*0x10*/; index3 += 2)
              {
                byte num8 = FileIO.ReadByte();
                StageSystem.tileCollisions[index2].floorMask[num1 + index3] = (sbyte) ((int) num8 >> 4);
                StageSystem.tileCollisions[index2].floorMask[num1 + index3 + 1] = (sbyte) ((int) num8 & 15);
              }
              byte num9 = FileIO.ReadByte();
              byte num10 = 1;
              for (int index4 = 0; index4 < 8; ++index4)
              {
                if (((int) num9 & (int) num10) < 1)
                {
                  StageSystem.tileCollisions[index2].floorMask[num1 + index4 + 8] = (sbyte) 64 /*0x40*/;
                  StageSystem.tileCollisions[index2].roofMask[num1 + index4 + 8] = (sbyte) -64 /*0xC0*/;
                }
                else
                  StageSystem.tileCollisions[index2].roofMask[num1 + index4 + 8] = (sbyte) 15;
                num10 <<= 1;
              }
              byte num11 = FileIO.ReadByte();
              byte num12 = 1;
              for (int index5 = 0; index5 < 8; ++index5)
              {
                if (((int) num11 & (int) num12) < 1)
                {
                  StageSystem.tileCollisions[index2].floorMask[num1 + index5] = (sbyte) 64 /*0x40*/;
                  StageSystem.tileCollisions[index2].roofMask[num1 + index5] = (sbyte) -64 /*0xC0*/;
                }
                else
                  StageSystem.tileCollisions[index2].roofMask[num1 + index5] = (sbyte) 15;
                num12 <<= 1;
              }
              for (byte index6 = 0; index6 < (byte) 16 /*0x10*/; ++index6)
              {
                int num13 = 0;
                while (num13 > -1)
                {
                  if (num13 == 16 /*0x10*/)
                  {
                    StageSystem.tileCollisions[index2].leftWallMask[num1 + (int) index6] = (sbyte) 64 /*0x40*/;
                    num13 = -1;
                  }
                  else if ((int) index6 >= (int) StageSystem.tileCollisions[index2].floorMask[num1 + num13])
                  {
                    StageSystem.tileCollisions[index2].leftWallMask[num1 + (int) index6] = (sbyte) num13;
                    num13 = -1;
                  }
                  else
                    ++num13;
                }
              }
              for (byte index7 = 0; index7 < (byte) 16 /*0x10*/; ++index7)
              {
                int num14 = 15;
                while (num14 < 16 /*0x10*/)
                {
                  if (num14 == -1)
                  {
                    StageSystem.tileCollisions[index2].rightWallMask[num1 + (int) index7] = (sbyte) -64 /*0xC0*/;
                    num14 = 16 /*0x10*/;
                  }
                  else if ((int) index7 >= (int) StageSystem.tileCollisions[index2].floorMask[num1 + num14])
                  {
                    StageSystem.tileCollisions[index2].rightWallMask[num1 + (int) index7] = (sbyte) num14;
                    num14 = 16 /*0x10*/;
                  }
                  else
                    --num14;
                }
              }
            }
            else
            {
              for (int index8 = 0; index8 < 16 /*0x10*/; index8 += 2)
              {
                byte num15 = FileIO.ReadByte();
                StageSystem.tileCollisions[index2].roofMask[num1 + index8] = (sbyte) ((int) num15 >> 4);
                StageSystem.tileCollisions[index2].roofMask[num1 + index8 + 1] = (sbyte) ((int) num15 & 15);
              }
              byte num16 = FileIO.ReadByte();
              byte num17 = 1;
              for (int index9 = 0; index9 < 8; ++index9)
              {
                if (((int) num16 & (int) num17) < 1)
                {
                  StageSystem.tileCollisions[index2].floorMask[num1 + index9 + 8] = (sbyte) 64 /*0x40*/;
                  StageSystem.tileCollisions[index2].roofMask[num1 + index9 + 8] = (sbyte) -64 /*0xC0*/;
                }
                else
                  StageSystem.tileCollisions[index2].floorMask[num1 + index9 + 8] = (sbyte) 0;
                num17 <<= 1;
              }
              byte num18 = FileIO.ReadByte();
              byte num19 = 1;
              for (int index10 = 0; index10 < 8; ++index10)
              {
                if (((int) num18 & (int) num19) < 1)
                {
                  StageSystem.tileCollisions[index2].floorMask[num1 + index10] = (sbyte) 64 /*0x40*/;
                  StageSystem.tileCollisions[index2].roofMask[num1 + index10] = (sbyte) -64 /*0xC0*/;
                }
                else
                  StageSystem.tileCollisions[index2].floorMask[num1 + index10] = (sbyte) 0;
                num19 <<= 1;
              }
              for (byte index11 = 0; index11 < (byte) 16 /*0x10*/; ++index11)
              {
                int num20 = 0;
                while (num20 > -1)
                {
                  if (num20 == 16 /*0x10*/)
                  {
                    StageSystem.tileCollisions[index2].leftWallMask[num1 + (int) index11] = (sbyte) 64 /*0x40*/;
                    num20 = -1;
                  }
                  else if ((int) index11 <= (int) StageSystem.tileCollisions[index2].roofMask[num1 + num20])
                  {
                    StageSystem.tileCollisions[index2].leftWallMask[num1 + (int) index11] = (sbyte) num20;
                    num20 = -1;
                  }
                  else
                    ++num20;
                }
              }
              for (byte index12 = 0; index12 < (byte) 16 /*0x10*/; ++index12)
              {
                int num21 = 15;
                while (num21 < 16 /*0x10*/)
                {
                  if (num21 == -1)
                  {
                    StageSystem.tileCollisions[index2].rightWallMask[num1 + (int) index12] = (sbyte) -64 /*0xC0*/;
                    num21 = 16 /*0x10*/;
                  }
                  else if ((int) index12 <= (int) StageSystem.tileCollisions[index2].roofMask[num1 + num21])
                  {
                    StageSystem.tileCollisions[index2].rightWallMask[num1 + (int) index12] = (sbyte) num21;
                    num21 = 16 /*0x10*/;
                  }
                  else
                    --num21;
                }
              }
            }
          }
          num1 += 16 /*0x10*/;
        }
        FileIO.CloseFile();
      }

      public static void LoadActLayout()
      {
        FileData fData = new FileData();
        if (!FileIO.LoadActFile(".bin".ToCharArray(), StageSystem.stageListPosition, fData))
          return;
        byte num1 = FileIO.ReadByte();
        int num2 = (int) num1;
        StageSystem.titleCardWord2 = (char) num1;
        int index1;
        for (index1 = 0; index1 < num2; ++index1)
        {
          StageSystem.titleCardText[index1] = (char) FileIO.ReadByte();
          if (StageSystem.titleCardText[index1] == '-')
            StageSystem.titleCardWord2 = (char) (index1 + 1);
        }
        StageSystem.titleCardText[index1] = char.MinValue;
        for (int index2 = 0; index2 < 4; ++index2)
        {
          byte num3 = FileIO.ReadByte();
          StageSystem.activeTileLayers[index2] = num3;
        }
        StageSystem.tLayerMidPoint = FileIO.ReadByte();
        StageSystem.stageLayouts[0].xSize = FileIO.ReadByte();
        StageSystem.stageLayouts[0].ySize = FileIO.ReadByte();
        StageSystem.xBoundary1 = 0;
        StageSystem.newXBoundary1 = 0;
        StageSystem.yBoundary1 = 0;
        StageSystem.newYBoundary1 = 0;
        StageSystem.xBoundary2 = (int) StageSystem.stageLayouts[0].xSize << 7;
        StageSystem.yBoundary2 = (int) StageSystem.stageLayouts[0].ySize << 7;
        StageSystem.waterLevel = StageSystem.yBoundary2 + 128 /*0x80*/;
        StageSystem.newXBoundary2 = StageSystem.xBoundary2;
        StageSystem.newYBoundary2 = StageSystem.yBoundary2;
        for (int index3 = 0; index3 < 65536 /*0x010000*/; ++index3)
          StageSystem.stageLayouts[0].tileMap[index3] = (ushort) 0;
        for (int index4 = 0; index4 < (int) StageSystem.stageLayouts[0].ySize; ++index4)
        {
          for (int index5 = 0; index5 < (int) StageSystem.stageLayouts[0].xSize; ++index5)
          {
            byte num4 = FileIO.ReadByte();
            StageSystem.stageLayouts[0].tileMap[(index4 << 8) + index5] = (ushort) ((uint) num4 << 8);
            byte num5 = FileIO.ReadByte();
            StageSystem.stageLayouts[0].tileMap[(index4 << 8) + index5] += (ushort) num5;
          }
        }
        int num6 = (int) FileIO.ReadByte();
        for (int index6 = 0; index6 < num6; ++index6)
        {
          for (int index7 = (int) FileIO.ReadByte(); index7 > 0; --index7)
            FileIO.ReadByte();
        }
        int num7 = ((int) FileIO.ReadByte() << 8) + (int) FileIO.ReadByte();
        int index8 = 32 /*0x20*/;
        for (int index9 = 0; index9 < num7; ++index9)
        {
          byte num8 = FileIO.ReadByte();
          ObjectSystem.objectEntityList[index8].type = num8;
          byte num9 = FileIO.ReadByte();
          ObjectSystem.objectEntityList[index8].propertyValue = num9;
          byte num10 = FileIO.ReadByte();
          ObjectSystem.objectEntityList[index8].xPos = (int) num10 << 8;
          byte num11 = FileIO.ReadByte();
          ObjectSystem.objectEntityList[index8].xPos += (int) num11;
          ObjectSystem.objectEntityList[index8].xPos <<= 16 /*0x10*/;
          byte num12 = FileIO.ReadByte();
          ObjectSystem.objectEntityList[index8].yPos = (int) num12 << 8;
          byte num13 = FileIO.ReadByte();
          ObjectSystem.objectEntityList[index8].yPos += (int) num13;
          ObjectSystem.objectEntityList[index8].yPos <<= 16 /*0x10*/;
          ++index8;
        }
        StageSystem.stageLayouts[0].type = (byte) 1;
        FileIO.CloseFile();
      }

      public static void LoadStageBackground()
      {
        FileData fData = new FileData();
        byte[] numArray1 = new byte[3];
        byte[] numArray2 = new byte[2];
        for (int index = 0; index < 9; ++index)
        {
          StageSystem.stageLayouts[index].type = (byte) 0;
          StageSystem.stageLayouts[index].deformationPos = 0;
          StageSystem.stageLayouts[index].deformationPosW = 0;
        }
        for (int index = 0; index < 256 /*0x0100*/; ++index)
        {
          StageSystem.hParallax.scrollPosition[index] = 0;
          StageSystem.vParallax.scrollPosition[index] = 0;
        }
        for (int index = 0; index < 32768 /*0x8000*/; ++index)
          StageSystem.stageLayouts[0].lineScrollRef[index] = (byte) 0;
        if (!FileIO.LoadStageFile("Backgrounds.bin".ToCharArray(), StageSystem.stageListPosition, fData))
          return;
        byte num1 = FileIO.ReadByte();
        byte num2 = FileIO.ReadByte();
        StageSystem.hParallax.numEntries = num2;
        for (int index = 0; index < (int) StageSystem.hParallax.numEntries; ++index)
        {
          byte num3 = FileIO.ReadByte();
          StageSystem.hParallax.parallaxFactor[index] = (int) num3 << 8;
          byte num4 = FileIO.ReadByte();
          StageSystem.hParallax.parallaxFactor[index] += (int) num4;
          byte num5 = FileIO.ReadByte();
          StageSystem.hParallax.scrollSpeed[index] = (int) num5 << 10;
          StageSystem.hParallax.scrollPosition[index] = 0;
          byte num6 = FileIO.ReadByte();
          StageSystem.hParallax.deformationEnabled[index] = num6;
        }
        byte num7 = FileIO.ReadByte();
        StageSystem.vParallax.numEntries = num7;
        for (int index = 0; index < (int) StageSystem.vParallax.numEntries; ++index)
        {
          byte num8 = FileIO.ReadByte();
          StageSystem.vParallax.parallaxFactor[index] = (int) num8 << 8;
          byte num9 = FileIO.ReadByte();
          StageSystem.vParallax.parallaxFactor[index] += (int) num9;
          byte num10 = FileIO.ReadByte();
          StageSystem.vParallax.scrollSpeed[index] = (int) num10 << 10;
          StageSystem.vParallax.scrollPosition[index] = 0;
          byte num11 = FileIO.ReadByte();
          StageSystem.vParallax.deformationEnabled[index] = num11;
        }
        for (int index1 = 1; index1 < (int) num1 + 1; ++index1)
        {
          byte num12 = FileIO.ReadByte();
          StageSystem.stageLayouts[index1].xSize = num12;
          byte num13 = FileIO.ReadByte();
          StageSystem.stageLayouts[index1].ySize = num13;
          byte num14 = FileIO.ReadByte();
          StageSystem.stageLayouts[index1].type = num14;
          byte num15 = FileIO.ReadByte();
          StageSystem.stageLayouts[index1].parallaxFactor = (int) num15 << 8;
          byte num16 = FileIO.ReadByte();
          StageSystem.stageLayouts[index1].parallaxFactor += (int) num16;
          byte num17 = FileIO.ReadByte();
          StageSystem.stageLayouts[index1].scrollSpeed = (int) num17 << 10;
          StageSystem.stageLayouts[index1].scrollPosition = 0;
          for (int index2 = 0; index2 < 65536 /*0x010000*/; ++index2)
            StageSystem.stageLayouts[index1].tileMap[index2] = (ushort) 0;
          for (int index3 = 0; index3 < 32768 /*0x8000*/; ++index3)
            StageSystem.stageLayouts[index1].lineScrollRef[index3] = (byte) 0;
          int index4 = 0;
          int num18 = 0;
          while (num18 < 1)
          {
            numArray1[0] = FileIO.ReadByte();
            if (numArray1[0] == byte.MaxValue)
            {
              numArray1[1] = FileIO.ReadByte();
              if (numArray1[1] == byte.MaxValue)
              {
                num18 = 1;
              }
              else
              {
                numArray1[2] = FileIO.ReadByte();
                numArray2[0] = numArray1[1];
                numArray2[1] = (byte) ((uint) numArray1[2] - 1U);
                for (int index5 = 0; index5 < (int) numArray2[1]; ++index5)
                {
                  StageSystem.stageLayouts[index1].lineScrollRef[index4] = numArray2[0];
                  ++index4;
                }
              }
            }
            else
            {
              StageSystem.stageLayouts[index1].lineScrollRef[index4] = numArray1[0];
              ++index4;
            }
          }
          for (int index6 = 0; index6 < (int) StageSystem.stageLayouts[index1].ySize; ++index6)
          {
            for (int index7 = 0; index7 < (int) StageSystem.stageLayouts[index1].xSize; ++index7)
            {
              byte num19 = FileIO.ReadByte();
              StageSystem.stageLayouts[index1].tileMap[(index6 << 8) + index7] = (ushort) ((uint) num19 << 8);
              byte num20 = FileIO.ReadByte();
              StageSystem.stageLayouts[index1].tileMap[(index6 << 8) + index7] += (ushort) num20;
            }
          }
        }
        FileIO.CloseFile();
      }

      public static void ResetBackgroundSettings()
      {
        for (int index = 0; index < 9; ++index)
        {
          StageSystem.stageLayouts[index].deformationPos = 0;
          StageSystem.stageLayouts[index].deformationPosW = 0;
          StageSystem.stageLayouts[index].scrollPosition = 0;
        }
        for (int index = 0; index < 256 /*0x0100*/; ++index)
        {
          StageSystem.hParallax.scrollPosition[index] = 0;
          StageSystem.vParallax.scrollPosition[index] = 0;
        }
        for (int index = 0; index < 576; ++index)
        {
          StageSystem.bgDeformationData0[index] = 0;
          StageSystem.bgDeformationData1[index] = 0;
          StageSystem.bgDeformationData2[index] = 0;
          StageSystem.bgDeformationData3[index] = 0;
        }
      }

      public static void SetLayerDeformation(
        int selectedDef,
        int waveLength,
        int waveWidth,
        int wType,
        int yPos,
        int wSize)
      {
        int index1 = 0;
        switch (selectedDef)
        {
          case 0:
            switch (wType)
            {
              case 0:
                for (int index2 = 0; index2 < 131072 /*0x020000*/; index2 += 512 /*0x0200*/)
                {
                  StageSystem.bgDeformationData0[index1] = GlobalAppDefinitions.SinValue512[index2 / waveLength & 511 /*0x01FF*/] * waveWidth >> 5;
                  ++index1;
                }
                break;
              case 1:
                int index3 = index1 + yPos;
                for (int index4 = 0; index4 < wSize; ++index4)
                {
                  StageSystem.bgDeformationData0[index3] = GlobalAppDefinitions.SinValue512[(index4 << 9) / waveLength & 511 /*0x01FF*/] * waveWidth >> 5;
                  ++index3;
                }
                break;
            }
            for (int index5 = 256 /*0x0100*/; index5 < 576; ++index5)
              StageSystem.bgDeformationData0[index5] = StageSystem.bgDeformationData0[index5 - 256 /*0x0100*/];
            break;
          case 1:
            switch (wType)
            {
              case 0:
                for (int index6 = 0; index6 < 131072 /*0x020000*/; index6 += 512 /*0x0200*/)
                {
                  StageSystem.bgDeformationData1[index1] = GlobalAppDefinitions.SinValue512[index6 / waveLength & 511 /*0x01FF*/] * waveWidth >> 5;
                  ++index1;
                }
                break;
              case 1:
                int index7 = index1 + yPos;
                for (int index8 = 0; index8 < wSize; ++index8)
                {
                  StageSystem.bgDeformationData1[index7] = GlobalAppDefinitions.SinValue512[(index8 << 9) / waveLength & 511 /*0x01FF*/] * waveWidth >> 5;
                  ++index7;
                }
                break;
            }
            for (int index9 = 256 /*0x0100*/; index9 < 576; ++index9)
              StageSystem.bgDeformationData1[index9] = StageSystem.bgDeformationData1[index9 - 256 /*0x0100*/];
            break;
          case 2:
            switch (wType)
            {
              case 0:
                for (int index10 = 0; index10 < 131072 /*0x020000*/; index10 += 512 /*0x0200*/)
                {
                  StageSystem.bgDeformationData2[index1] = GlobalAppDefinitions.SinValue512[index10 / waveLength & 511 /*0x01FF*/] * waveWidth >> 5;
                  ++index1;
                }
                break;
              case 1:
                int index11 = index1 + yPos;
                for (int index12 = 0; index12 < wSize; ++index12)
                {
                  StageSystem.bgDeformationData2[index11] = GlobalAppDefinitions.SinValue512[(index12 << 9) / waveLength & 511 /*0x01FF*/] * waveWidth >> 5;
                  ++index11;
                }
                break;
            }
            for (int index13 = 256 /*0x0100*/; index13 < 576; ++index13)
              StageSystem.bgDeformationData2[index13] = StageSystem.bgDeformationData2[index13 - 256 /*0x0100*/];
            break;
          case 3:
            switch (wType)
            {
              case 0:
                for (int index14 = 0; index14 < 131072 /*0x020000*/; index14 += 512 /*0x0200*/)
                {
                  StageSystem.bgDeformationData3[index1] = GlobalAppDefinitions.SinValue512[index14 / waveLength & 511 /*0x01FF*/] * waveWidth >> 5;
                  ++index1;
                }
                break;
              case 1:
                int index15 = index1 + yPos;
                for (int index16 = 0; index16 < wSize; ++index16)
                {
                  StageSystem.bgDeformationData3[index15] = GlobalAppDefinitions.SinValue512[(index16 << 9) / waveLength & 511 /*0x01FF*/] * waveWidth >> 5;
                  ++index15;
                }
                break;
            }
            for (int index17 = 256 /*0x0100*/; index17 < 576; ++index17)
              StageSystem.bgDeformationData3[index17] = StageSystem.bgDeformationData3[index17 - 256 /*0x0100*/];
            break;
        }
      }

      public static void DrawStageGfx()
      {
        GraphicsSystem.gfxVertexSize = (ushort) 0;
        GraphicsSystem.gfxIndexSize = (ushort) 0;
        GraphicsSystem.waterDrawPos = StageSystem.waterLevel - StageSystem.yScrollOffset;
        if (GraphicsSystem.waterDrawPos < -16)
          GraphicsSystem.waterDrawPos = -16;
        if (GraphicsSystem.waterDrawPos >= 240 /*0xF0*/)
          GraphicsSystem.waterDrawPos = 256 /*0x0100*/;
        ObjectSystem.DrawObjectList(0);
        if (StageSystem.activeTileLayers[0] < (byte) 9)
        {
          switch (StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[0]].type)
          {
            case 1:
              StageSystem.DrawHLineScrollLayer8((byte) 0);
              break;
            case 3:
              StageSystem.Draw3DFloorLayer((byte) 0);
              break;
            case 4:
              StageSystem.Draw3DFloorLayer((byte) 0);
              break;
          }
        }
        GraphicsSystem.gfxIndexSizeOpaque = GraphicsSystem.gfxIndexSize;
        GraphicsSystem.gfxVertexSizeOpaque = GraphicsSystem.gfxVertexSize;
        ObjectSystem.DrawObjectList(1);
        if (StageSystem.activeTileLayers[1] < (byte) 9)
        {
          switch (StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[1]].type)
          {
            case 1:
              StageSystem.DrawHLineScrollLayer8((byte) 1);
              break;
            case 3:
              StageSystem.Draw3DFloorLayer((byte) 1);
              break;
            case 4:
              StageSystem.Draw3DFloorLayer((byte) 1);
              break;
          }
        }
        ObjectSystem.DrawObjectList(2);
        if (StageSystem.activeTileLayers[2] < (byte) 9)
        {
          switch (StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[2]].type)
          {
            case 1:
              StageSystem.DrawHLineScrollLayer8((byte) 2);
              break;
            case 3:
              StageSystem.Draw3DFloorLayer((byte) 2);
              break;
            case 4:
              StageSystem.Draw3DFloorLayer((byte) 2);
              break;
          }
        }
        ObjectSystem.DrawObjectList(3);
        ObjectSystem.DrawObjectList(4);
        if (StageSystem.activeTileLayers[3] < (byte) 9)
        {
          switch (StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[3]].type)
          {
            case 1:
              StageSystem.DrawHLineScrollLayer8((byte) 3);
              break;
            case 3:
              StageSystem.Draw3DFloorLayer((byte) 3);
              break;
            case 4:
              StageSystem.Draw3DFloorLayer((byte) 3);
              break;
          }
        }
        ObjectSystem.DrawObjectList(5);
        ObjectSystem.DrawObjectList(6);
      }

      public static void DrawHLineScrollLayer8(byte layerNum)
      {
        int num1 = 0;
        int[] gfxDataPos = StageSystem.tile128x128.gfxDataPos;
        byte[] direction = StageSystem.tile128x128.direction;
        byte[] visualPlane = StageSystem.tile128x128.visualPlane;
        int num2 = (int) StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].xSize;
        int num3 = (int) StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].ySize;
        int num4 = (GlobalAppDefinitions.SCREEN_XSIZE >> 4) + 3;
        byte num5 = (int) layerNum < (int) StageSystem.tLayerMidPoint ? (byte) 0 : (byte) 1;
        ushort[] tileMap;
        byte[] lineScrollRef;
        int num6;
        int num7;
        int[] numArray1;
        int[] numArray2;
        int num8;
        if (StageSystem.activeTileLayers[(int) layerNum] == (byte) 0)
        {
          tileMap = StageSystem.stageLayouts[0].tileMap;
          StageSystem.lastXSize = num2;
          int yScrollOffset = StageSystem.yScrollOffset;
          lineScrollRef = StageSystem.stageLayouts[0].lineScrollRef;
          StageSystem.hParallax.linePos[0] = StageSystem.xScrollOffset;
          num6 = StageSystem.stageLayouts[0].deformationPos + yScrollOffset & (int) byte.MaxValue;
          num7 = StageSystem.stageLayouts[0].deformationPosW + yScrollOffset & (int) byte.MaxValue;
          numArray1 = StageSystem.bgDeformationData0;
          numArray2 = StageSystem.bgDeformationData1;
          num8 = yScrollOffset % (num3 << 7);
        }
        else
        {
          tileMap = StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].tileMap;
          int num9 = StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].parallaxFactor * StageSystem.yScrollOffset >> 8;
          int num10 = num3 << 7;
          StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].scrollPosition += StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].scrollSpeed;
          if (StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].scrollPosition > num10 << 16 /*0x10*/)
            StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].scrollPosition -= num10 << 16 /*0x10*/;
          num8 = (num9 + (StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].scrollPosition >> 16 /*0x10*/)) % num10;
          num3 = num10 >> 7;
          lineScrollRef = StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].lineScrollRef;
          num6 = StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].deformationPos + num8 & (int) byte.MaxValue;
          num7 = StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].deformationPosW + num8 & (int) byte.MaxValue;
          numArray1 = StageSystem.bgDeformationData2;
          numArray2 = StageSystem.bgDeformationData3;
        }
        switch (StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].type)
        {
          case 1:
            if (StageSystem.lastXSize != num2)
            {
              int num11 = num2 << 7;
              for (int index = 0; index < (int) StageSystem.hParallax.numEntries; ++index)
              {
                StageSystem.hParallax.linePos[index] = StageSystem.hParallax.parallaxFactor[index] * StageSystem.xScrollOffset >> 8;
                StageSystem.hParallax.scrollPosition[index] += StageSystem.hParallax.scrollSpeed[index];
                if (StageSystem.hParallax.scrollPosition[index] > num11 << 16 /*0x10*/)
                  StageSystem.hParallax.scrollPosition[index] -= num11 << 16 /*0x10*/;
                StageSystem.hParallax.linePos[index] += StageSystem.hParallax.scrollPosition[index] >> 16 /*0x10*/;
                StageSystem.hParallax.linePos[index] %= num11;
              }
              num2 = num11 >> 7;
            }
            StageSystem.lastXSize = num2;
            break;
        }
        if (num8 < 0)
          num8 += num3 << 7;
        int num12 = num8 >> 4 << 4;
        int index1 = num1 + num12;
        int index2 = num6 + (num12 - num8);
        int index3 = num7 + (num12 - num8);
        if (index2 < 0)
          index2 += 256 /*0x0100*/;
        if (index3 < 0)
          index3 += 256 /*0x0100*/;
        int num13 = -(num8 & 15);
        int num14 = num8 >> 7;
        int num15 = (num8 & (int) sbyte.MaxValue) >> 4;
        int num16 = num13 != 0 ? 272 : 256 /*0x0100*/;
        GraphicsSystem.waterDrawPos <<= 4;
        int num17 = num13 << 4;
        for (int index4 = num16; index4 > 0; index4 -= 16 /*0x10*/)
        {
          int num18 = StageSystem.hParallax.linePos[(int) lineScrollRef[index1]] - 16 /*0x10*/;
          int index5 = index1 + 8;
          bool flag;
          if (num18 == StageSystem.hParallax.linePos[(int) lineScrollRef[index5]] - 16 /*0x10*/)
          {
            if (StageSystem.hParallax.deformationEnabled[(int) lineScrollRef[index5]] == (byte) 1)
            {
              int num19 = num17 < GraphicsSystem.waterDrawPos ? numArray1[index2] : numArray2[index3];
              int index6 = index2 + 8;
              int index7 = index3 + 8;
              int num20 = num17 + 64 /*0x40*/ <= GraphicsSystem.waterDrawPos ? numArray1[index6] : numArray2[index7];
              flag = num19 != num20;
              index2 = index6 - 8;
              index3 = index7 - 8;
            }
            else
              flag = false;
          }
          else
            flag = true;
          int index8 = index5 - 8;
          if (flag)
          {
            int num21 = num2 << 7;
            if (num18 < 0)
              num18 += num21;
            if (num18 >= num21)
              num18 -= num21;
            int num22 = num18 >> 7;
            int num23 = (num18 & (int) sbyte.MaxValue) >> 4;
            int num24 = -((num18 & 15) << 4) - 256 /*0x0100*/;
            int num25 = num24;
            int index9;
            int index10;
            if (StageSystem.hParallax.deformationEnabled[(int) lineScrollRef[index8]] == (byte) 1)
            {
              if (num17 >= GraphicsSystem.waterDrawPos)
                num24 -= numArray2[index3];
              else
                num24 -= numArray1[index2];
              index9 = index2 + 8;
              index10 = index3 + 8;
              if (num17 + 64 /*0x40*/ > GraphicsSystem.waterDrawPos)
                num25 -= numArray2[index10];
              else
                num25 -= numArray1[index9];
            }
            else
            {
              index9 = index2 + 8;
              index10 = index3 + 8;
            }
            int index11 = index8 + 8;
            int index12 = (num22 <= -1 || num14 <= -1 ? 0 : (int) tileMap[num22 + (num14 << 8)] << 6) + (num23 + (num15 << 3));
            for (int index13 = num4; index13 > 0; --index13)
            {
              if ((int) visualPlane[index12] == (int) num5 && gfxDataPos[index12] > 0)
              {
                int num26 = 0;
                switch (direction[index12])
                {
                  case 0:
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) num24;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) num17;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num26];
                    int num27 = num26 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num27];
                    int num28 = num27 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (num24 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) num17;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num28];
                    int num29 = num28 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) num25;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (num17 + 128 /*0x80*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num29] - 1f / 128f;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (num25 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxIndexSize += (ushort) 2;
                    break;
                  case 1:
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (num24 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) num17;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num26];
                    int num30 = num26 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num30];
                    int num31 = num30 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) num24;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) num17;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num31];
                    int num32 = num31 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (num25 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (num17 + 128 /*0x80*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num32] - 1f / 128f;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) num25;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxIndexSize += (ushort) 2;
                    break;
                  case 2:
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) num25;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (num17 + 128 /*0x80*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num26];
                    int num33 = num26 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num33] + 1f / 128f;
                    int num34 = num33 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (num25 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (num17 + 128 /*0x80*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num34];
                    int num35 = num34 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) num24;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) num17;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num35];
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (num24 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxIndexSize += (ushort) 2;
                    break;
                  case 3:
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (num25 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (num17 + 128 /*0x80*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num26];
                    int num36 = num26 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num36] + 1f / 128f;
                    int num37 = num36 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) num25;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (num17 + 128 /*0x80*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num37];
                    int num38 = num37 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (num24 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) num17;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index12] + num38];
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) num24;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxIndexSize += (ushort) 2;
                    break;
                }
              }
              num24 += 256 /*0x0100*/;
              num25 += 256 /*0x0100*/;
              ++num23;
              if (num23 > 7)
              {
                ++num22;
                if (num22 == num2)
                  num22 = 0;
                num23 = 0;
                index12 = ((int) tileMap[num22 + (num14 << 8)] << 6) + (num23 + (num15 << 3));
              }
              else
                ++index12;
            }
            int num39 = num17 + 128 /*0x80*/;
            int num40 = StageSystem.hParallax.linePos[(int) lineScrollRef[index11]] - 16 /*0x10*/;
            int num41 = num2 << 7;
            if (num40 < 0)
              num40 += num41;
            if (num40 >= num41)
              num40 -= num41;
            int num42 = num40 >> 7;
            int num43 = (num40 & (int) sbyte.MaxValue) >> 4;
            int num44 = -((num40 & 15) << 4) - 256 /*0x0100*/;
            int num45 = num44;
            if (StageSystem.hParallax.deformationEnabled[(int) lineScrollRef[index11]] == (byte) 1)
            {
              if (num39 >= GraphicsSystem.waterDrawPos)
                num44 -= numArray2[index10];
              else
                num44 -= numArray1[index9];
              index2 = index9 + 8;
              index3 = index10 + 8;
              if (num39 + 64 /*0x40*/ > GraphicsSystem.waterDrawPos)
                num45 -= numArray2[index3];
              else
                num45 -= numArray1[index2];
            }
            else
            {
              index2 = index9 + 8;
              index3 = index10 + 8;
            }
            index1 = index11 + 8;
            int index14 = (num42 <= -1 || num14 <= -1 ? 0 : (int) tileMap[num42 + (num14 << 8)] << 6) + (num43 + (num15 << 3));
            for (int index15 = num4; index15 > 0; --index15)
            {
              if ((int) visualPlane[index14] == (int) num5 && gfxDataPos[index14] > 0)
              {
                int num46 = 0;
                switch (direction[index14])
                {
                  case 0:
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) num44;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) num39;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index14] + num46];
                    int num47 = num46 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index14] + num47] + 1f / 128f;
                    int num48 = num47 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (num44 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) num39;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index14] + num48];
                    int num49 = num48 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) num45;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (num39 + 128 /*0x80*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index14] + num49];
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (num45 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxIndexSize += (ushort) 2;
                    break;
                  case 1:
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (num44 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) num39;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index14] + num46];
                    int num50 = num46 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index14] + num50] + 1f / 128f;
                    int num51 = num50 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) num44;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) num39;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index14] + num51];
                    int num52 = num51 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (num45 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (num39 + 128 /*0x80*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index14] + num52];
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) num45;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxIndexSize += (ushort) 2;
                    break;
                  case 2:
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) num45;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (num39 + 128 /*0x80*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index14] + num46];
                    int num53 = num46 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index14] + num53];
                    int num54 = num53 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (num45 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (num39 + 128 /*0x80*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index14] + num54];
                    int num55 = num54 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) num44;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) num39;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index14] + num55] - 1f / 128f;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (num44 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxIndexSize += (ushort) 2;
                    break;
                  case 3:
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (num45 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (num39 + 128 /*0x80*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index14] + num46];
                    int num56 = num46 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index14] + num56];
                    int num57 = num56 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) num45;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (num39 + 128 /*0x80*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index14] + num57];
                    int num58 = num57 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (num44 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) num39;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index14] + num58] - 1f / 128f;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) num44;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxIndexSize += (ushort) 2;
                    break;
                }
              }
              num44 += 256 /*0x0100*/;
              num45 += 256 /*0x0100*/;
              ++num43;
              if (num43 > 7)
              {
                ++num42;
                if (num42 == num2)
                  num42 = 0;
                num43 = 0;
                index14 = ((int) tileMap[num42 + (num14 << 8)] << 6) + (num43 + (num15 << 3));
              }
              else
                ++index14;
            }
            num17 = num39 + 128 /*0x80*/;
          }
          else
          {
            int num59 = num2 << 7;
            if (num18 < 0)
              num18 += num59;
            if (num18 >= num59)
              num18 -= num59;
            int num60 = num18 >> 7;
            int num61 = (num18 & (int) sbyte.MaxValue) >> 4;
            int num62 = -((num18 & 15) << 4) - 256 /*0x0100*/;
            int num63 = num62;
            if (StageSystem.hParallax.deformationEnabled[(int) lineScrollRef[index8]] == (byte) 1)
            {
              if (num17 >= GraphicsSystem.waterDrawPos)
                num62 -= numArray2[index3];
              else
                num62 -= numArray1[index2];
              index2 += 16 /*0x10*/;
              index3 += 16 /*0x10*/;
              if (num17 + 128 /*0x80*/ > GraphicsSystem.waterDrawPos)
                num63 -= numArray2[index3];
              else
                num63 -= numArray1[index2];
            }
            else
            {
              index2 += 16 /*0x10*/;
              index3 += 16 /*0x10*/;
            }
            index1 = index8 + 16 /*0x10*/;
            int index16 = (num60 <= -1 || num14 <= -1 ? 0 : (int) tileMap[num60 + (num14 << 8)] << 6) + (num61 + (num15 << 3));
            for (int index17 = num4; index17 > 0; --index17)
            {
              if ((int) visualPlane[index16] == (int) num5 && gfxDataPos[index16] > 0)
              {
                int num64 = 0;
                switch (direction[index16])
                {
                  case 0:
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) num62;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) num17;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index16] + num64];
                    int num65 = num64 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index16] + num65];
                    int num66 = num65 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (num62 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) num17;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index16] + num66];
                    int num67 = num66 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) num63;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (num17 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index16] + num67];
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (num63 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxIndexSize += (ushort) 2;
                    break;
                  case 1:
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (num62 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) num17;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index16] + num64];
                    int num68 = num64 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index16] + num68];
                    int num69 = num68 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) num62;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) num17;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index16] + num69];
                    int num70 = num69 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (num63 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (num17 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index16] + num70];
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) num63;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxIndexSize += (ushort) 2;
                    break;
                  case 2:
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) num63;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (num17 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index16] + num64];
                    int num71 = num64 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index16] + num71];
                    int num72 = num71 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (num63 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (num17 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index16] + num72];
                    int num73 = num72 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) num62;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) num17;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index16] + num73];
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (num62 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxIndexSize += (ushort) 2;
                    break;
                  case 3:
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (num63 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (num17 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index16] + num64];
                    int num74 = num64 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index16] + num74];
                    int num75 = num74 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) num63;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (num17 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index16] + num75];
                    int num76 = num75 + 1;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (num62 + 256 /*0x0100*/);
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) num17;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index16] + num76];
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) num62;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
                    GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
                    ++GraphicsSystem.gfxVertexSize;
                    GraphicsSystem.gfxIndexSize += (ushort) 2;
                    break;
                }
              }
              num62 += 256 /*0x0100*/;
              num63 += 256 /*0x0100*/;
              ++num61;
              if (num61 > 7)
              {
                ++num60;
                if (num60 == num2)
                  num60 = 0;
                num61 = 0;
                index16 = ((int) tileMap[num60 + (num14 << 8)] << 6) + (num61 + (num15 << 3));
              }
              else
                ++index16;
            }
            num17 += 256 /*0x0100*/;
          }
          ++num15;
          if (num15 > 7)
          {
            ++num14;
            if (num14 == num3)
            {
              num14 = 0;
              index1 -= num3 << 7;
            }
            num15 = 0;
          }
        }
        GraphicsSystem.waterDrawPos >>= 4;
      }

      public static void Draw3DFloorLayer(byte layerNum)
      {
        int[] gfxDataPos = StageSystem.tile128x128.gfxDataPos;
        byte[] direction = StageSystem.tile128x128.direction;
        int num1 = (int) StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].xSize << 7;
        int num2 = (int) StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].ySize << 7;
        ushort[] tileMap = StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].tileMap;
        GraphicsSystem.vertexSize3D = (ushort) 0;
        GraphicsSystem.indexSize3D = (ushort) 0;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = 0.0f;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = 0.0f;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = 0.5f;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = 0.0f;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
        ++GraphicsSystem.vertexSize3D;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = 4096f;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = 0.0f;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = 1f;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = 0.0f;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
        ++GraphicsSystem.vertexSize3D;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = 0.0f;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = 4096f;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = 0.5f;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = 0.5f;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
        ++GraphicsSystem.vertexSize3D;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = 4096f;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = 4096f;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = 1f;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = 0.5f;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
        GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
        ++GraphicsSystem.vertexSize3D;
        GraphicsSystem.indexSize3D += (ushort) 2;
        if (!GlobalAppDefinitions.HQ3DFloorEnabled)
        {
          int num3 = (StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].xPos >> 16 /*0x10*/) - 160 /*0xA0*/ + GlobalAppDefinitions.SinValue512[StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].angle] / 3 >> 4 << 4;
          int num4 = (StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].zPos >> 16 /*0x10*/) - 160 /*0xA0*/ + GlobalAppDefinitions.CosValue512[StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].angle] / 3 >> 4 << 4;
          for (int index1 = 20; index1 > 0; --index1)
          {
            for (int index2 = 20; index2 > 0; --index2)
            {
              if (num3 > -1 && num3 < num1 && num4 > -1 && num4 < num2)
              {
                int num5 = num3 >> 7;
                int num6 = num4 >> 7;
                int num7 = (num3 & (int) sbyte.MaxValue) >> 4;
                int num8 = (num4 & (int) sbyte.MaxValue) >> 4;
                int index3 = ((int) tileMap[num5 + (num6 << 8)] << 6) + (num7 + (num8 << 3));
                if (gfxDataPos[index3] > 0)
                {
                  int num9 = 0;
                  switch (direction[index3])
                  {
                    case 0:
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = (float) num3;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = (float) num4;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num9];
                      int num10 = num9 + 1;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num10];
                      int num11 = num10 + 1;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = (float) (num3 + 16 /*0x10*/);
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.z;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num11];
                      int num12 = num11 + 1;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.y;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = (float) (num4 + 16 /*0x10*/);
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num12];
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.z;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.y;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.indexSize3D += (ushort) 2;
                      break;
                    case 1:
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = (float) (num3 + 16 /*0x10*/);
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = (float) num4;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num9];
                      int num13 = num9 + 1;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num13];
                      int num14 = num13 + 1;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = (float) num3;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.z;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num14];
                      int num15 = num14 + 1;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.y;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = (float) (num4 + 16 /*0x10*/);
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num15];
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.z;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.y;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.indexSize3D += (ushort) 2;
                      break;
                    case 2:
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = (float) num3;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = (float) (num4 + 16 /*0x10*/);
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num9];
                      int num16 = num9 + 1;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num16];
                      int num17 = num16 + 1;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = (float) (num3 + 16 /*0x10*/);
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.z;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num17];
                      int num18 = num17 + 1;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.y;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = (float) num4;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num18];
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.z;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.y;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.indexSize3D += (ushort) 2;
                      break;
                    case 3:
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = (float) (num3 + 16 /*0x10*/);
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = (float) (num4 + 16 /*0x10*/);
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num9];
                      int num19 = num9 + 1;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num19];
                      int num20 = num19 + 1;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = (float) num3;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.z;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num20];
                      int num21 = num20 + 1;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.y;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = (float) num4;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index3] + num21];
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.z;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.y;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.indexSize3D += (ushort) 2;
                      break;
                  }
                }
              }
              num3 += 16 /*0x10*/;
            }
            num3 -= 320;
            num4 += 16 /*0x10*/;
          }
        }
        else
        {
          int num22 = (StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].xPos >> 16 /*0x10*/) - 256 /*0x0100*/ + (GlobalAppDefinitions.SinValue512[StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].angle] >> 1) >> 4 << 4;
          int num23 = (StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].zPos >> 16 /*0x10*/) - 256 /*0x0100*/ + (GlobalAppDefinitions.CosValue512[StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].angle] >> 1) >> 4 << 4;
          for (int index4 = 32 /*0x20*/; index4 > 0; --index4)
          {
            for (int index5 = 32 /*0x20*/; index5 > 0; --index5)
            {
              if (num22 > -1 && num22 < num1 && num23 > -1 && num23 < num2)
              {
                int num24 = num22 >> 7;
                int num25 = num23 >> 7;
                int num26 = (num22 & (int) sbyte.MaxValue) >> 4;
                int num27 = (num23 & (int) sbyte.MaxValue) >> 4;
                int index6 = ((int) tileMap[num24 + (num25 << 8)] << 6) + (num26 + (num27 << 3));
                if (gfxDataPos[index6] > 0)
                {
                  int num28 = 0;
                  switch (direction[index6])
                  {
                    case 0:
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = (float) num22;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = (float) num23;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num28];
                      int num29 = num28 + 1;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num29];
                      int num30 = num29 + 1;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = (float) (num22 + 16 /*0x10*/);
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.z;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num30];
                      int num31 = num30 + 1;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.y;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = (float) (num23 + 16 /*0x10*/);
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num31];
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.z;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.y;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.indexSize3D += (ushort) 2;
                      break;
                    case 1:
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = (float) (num22 + 16 /*0x10*/);
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = (float) num23;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num28];
                      int num32 = num28 + 1;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num32];
                      int num33 = num32 + 1;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = (float) num22;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.z;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num33];
                      int num34 = num33 + 1;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.y;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = (float) (num23 + 16 /*0x10*/);
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num34];
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.z;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.y;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.indexSize3D += (ushort) 2;
                      break;
                    case 2:
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = (float) num22;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = (float) (num23 + 16 /*0x10*/);
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num28];
                      int num35 = num28 + 1;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num35];
                      int num36 = num35 + 1;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = (float) (num22 + 16 /*0x10*/);
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.z;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num36];
                      int num37 = num36 + 1;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.y;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = (float) num23;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num37];
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.z;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.y;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.indexSize3D += (ushort) 2;
                      break;
                    case 3:
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = (float) (num22 + 16 /*0x10*/);
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = (float) (num23 + 16 /*0x10*/);
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num28];
                      int num38 = num28 + 1;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num38];
                      int num39 = num38 + 1;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = (float) num22;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.z;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num39];
                      int num40 = num39 + 1;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.y;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = (float) num23;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.tileUVArray[gfxDataPos[index6] + num40];
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].position.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.y = 0.0f;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].position.z = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].position.z;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.x = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 2].texCoord.x;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].texCoord.y = GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D - 1].texCoord.y;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.r = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.g = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.b = byte.MaxValue;
                      GraphicsSystem.polyList3D[(int) GraphicsSystem.vertexSize3D].color.a = byte.MaxValue;
                      ++GraphicsSystem.vertexSize3D;
                      GraphicsSystem.indexSize3D += (ushort) 2;
                      break;
                  }
                }
              }
              num22 += 16 /*0x10*/;
            }
            num22 -= 512 /*0x0200*/;
            num23 += 16 /*0x10*/;
          }
        }
        GraphicsSystem.floor3DPos.x = (float) (StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].xPos >> 8) * (-1f / 256f);
        GraphicsSystem.floor3DPos.y = (float) (StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].yPos >> 8) * (1f / 256f);
        GraphicsSystem.floor3DPos.z = (float) (StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].zPos >> 8) * (-1f / 256f);
        GraphicsSystem.floor3DAngle = (float) ((double) StageSystem.stageLayouts[(int) StageSystem.activeTileLayers[(int) layerNum]].angle / 512.0 * -360.0);
        GraphicsSystem.render3DEnabled = true;
      }

      public static void InitFirstStage()
      {
        StageSystem.xScrollOffset = 0;
        StageSystem.yScrollOffset = 0;
        AudioPlayback.StopMusic();
        AudioPlayback.StopAllSFX();
        AudioPlayback.releaseStageSFX();
        GraphicsSystem.fadeMode = (byte) 0;
        PlayerSystem.playerMenuNum = (byte) 0;
        GraphicsSystem.ClearGraphicsData();
        AnimationSystem.ClearAnimationData();
        GraphicsSystem.LoadPalette("MasterPalette.act".ToCharArray(), 0, 0, 0, 256 /*0x0100*/);
        FileIO.activeStageList = (byte) 0;
        StageSystem.stageMode = (byte) 0;
        GlobalAppDefinitions.gameMode = (byte) 1;
        StageSystem.stageListPosition = 0;
      }

      public static void InitStageSelectMenu()
      {
        StageSystem.xScrollOffset = 0;
        StageSystem.yScrollOffset = 0;
        AudioPlayback.StopMusic();
        AudioPlayback.StopAllSFX();
        AudioPlayback.releaseStageSFX();
        GraphicsSystem.fadeMode = (byte) 0;
        PlayerSystem.playerMenuNum = (byte) 0;
        GlobalAppDefinitions.gameMode = (byte) 0;
        GraphicsSystem.ClearGraphicsData();
        AnimationSystem.ClearAnimationData();
        GraphicsSystem.LoadPalette("MasterPalette.act".ToCharArray(), 0, 0, 0, 256 /*0x0100*/);
        TextSystem.textMenuSurfaceNo = 0;
        GraphicsSystem.LoadGIFFile("Data/Game/SystemText.gif".ToCharArray(), 0);
        StageSystem.stageMode = (byte) 0;
        TextSystem.SetupTextMenu(StageSystem.gameMenu[0], 0);
        TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], "RETRO ENGINE DEV MENU".ToCharArray());
        TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
        TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], "SONIC CD Version".ToCharArray());
        TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], GlobalAppDefinitions.gameVersion);
        TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
        TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
        TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
        TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], "PLAY GAME".ToCharArray());
        TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
        TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], "STAGE SELECT".ToCharArray());
        StageSystem.gameMenu[0].alignment = (byte) 2;
        StageSystem.gameMenu[0].numSelections = (byte) 2;
        StageSystem.gameMenu[0].selection1 = 0;
        StageSystem.gameMenu[0].selection2 = 7;
        StageSystem.gameMenu[1].numVisibleRows = (ushort) 0;
        StageSystem.gameMenu[1].visibleRowOffset = (ushort) 0;
        RenderDevice.UpdateHardwareTextures();
      }

      public static void InitErrorMessage()
      {
        StageSystem.xScrollOffset = 0;
        StageSystem.yScrollOffset = 0;
        AudioPlayback.StopMusic();
        AudioPlayback.StopAllSFX();
        AudioPlayback.releaseStageSFX();
        GraphicsSystem.fadeMode = (byte) 0;
        PlayerSystem.playerMenuNum = (byte) 0;
        GlobalAppDefinitions.gameMode = (byte) 0;
        GraphicsSystem.ClearGraphicsData();
        AnimationSystem.ClearAnimationData();
        GraphicsSystem.LoadPalette("MasterPalette.act".ToCharArray(), 0, 0, 0, 256 /*0x0100*/);
        TextSystem.textMenuSurfaceNo = 0;
        GraphicsSystem.LoadGIFFile("Data/Game/SystemText.gif".ToCharArray(), 0);
        StageSystem.gameMenu[0].alignment = (byte) 2;
        StageSystem.gameMenu[0].numSelections = (byte) 1;
        StageSystem.gameMenu[1].numVisibleRows = (ushort) 0;
        StageSystem.gameMenu[1].visibleRowOffset = (ushort) 0;
        RenderDevice.UpdateHardwareTextures();
        StageSystem.stageMode = (byte) 4;
      }

      public static void ProcessStageSelectMenu()
      {
        GraphicsSystem.gfxVertexSize = (ushort) 0;
        GraphicsSystem.gfxIndexSize = (ushort) 0;
        GraphicsSystem.ClearScreen((byte) 240 /*0xF0*/);
        InputSystem.MenuKeyDown(StageSystem.gKeyDown, (byte) 131);
        GraphicsSystem.DrawSprite(32 /*0x20*/, 66, 16 /*0x10*/, 16 /*0x10*/, 78, 240 /*0xF0*/, 0);
        GraphicsSystem.DrawSprite(32 /*0x20*/, 178, 16 /*0x10*/, 16 /*0x10*/, 95, 240 /*0xF0*/, 0);
        GraphicsSystem.DrawSprite(GlobalAppDefinitions.SCREEN_XSIZE - 32 /*0x20*/, 208 /*0xD0*/, 16 /*0x10*/, 16 /*0x10*/, 112 /*0x70*/, 240 /*0xF0*/, 0);
        StageSystem.gKeyPress.start = (byte) 0;
        StageSystem.gKeyPress.up = (byte) 0;
        StageSystem.gKeyPress.down = (byte) 0;
        if (StageSystem.gKeyDown.touches > 0)
        {
          if (StageSystem.gKeyDown.touchX[0] < 120)
          {
            if (StageSystem.gKeyDown.touchY[0] < 120)
            {
              if (StageSystem.gKeyDown.up == (byte) 0)
                StageSystem.gKeyPress.up = (byte) 1;
              StageSystem.gKeyDown.up = (byte) 1;
            }
            else
            {
              if (StageSystem.gKeyDown.down == (byte) 0)
                StageSystem.gKeyPress.down = (byte) 1;
              StageSystem.gKeyDown.down = (byte) 1;
            }
          }
          if (StageSystem.gKeyDown.touchX[0] > 200)
          {
            if (StageSystem.gKeyDown.start == (byte) 0)
              StageSystem.gKeyPress.start = (byte) 1;
            StageSystem.gKeyDown.start = (byte) 1;
          }
        }
        else
        {
          StageSystem.gKeyDown.start = (byte) 0;
          StageSystem.gKeyDown.up = (byte) 0;
          StageSystem.gKeyDown.down = (byte) 0;
        }
        switch (StageSystem.stageMode)
        {
          case 0:
            if (StageSystem.gKeyPress.down == (byte) 1)
              StageSystem.gameMenu[0].selection2 += 2;
            if (StageSystem.gKeyPress.up == (byte) 1)
              StageSystem.gameMenu[0].selection2 -= 2;
            if (StageSystem.gameMenu[0].selection2 > 9)
              StageSystem.gameMenu[0].selection2 = 7;
            if (StageSystem.gameMenu[0].selection2 < 7)
              StageSystem.gameMenu[0].selection2 = 9;
            TextSystem.DrawTextMenu(StageSystem.gameMenu[0], GlobalAppDefinitions.SCREEN_CENTER, 72);
            if (StageSystem.gKeyPress.start == (byte) 1)
            {
              if (StageSystem.gameMenu[0].selection2 == 7)
              {
                StageSystem.stageMode = (byte) 0;
                GlobalAppDefinitions.gameMode = (byte) 1;
                FileIO.activeStageList = (byte) 0;
                StageSystem.stageListPosition = 0;
                break;
              }
              TextSystem.SetupTextMenu(StageSystem.gameMenu[0], 0);
              TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], "CHOOSE A PLAYER".ToCharArray());
              TextSystem.SetupTextMenu(StageSystem.gameMenu[1], 0);
              TextSystem.LoadConfigListText(StageSystem.gameMenu[1], 0);
              StageSystem.gameMenu[1].alignment = (byte) 0;
              StageSystem.gameMenu[1].numSelections = (byte) 1;
              StageSystem.gameMenu[1].selection1 = 0;
              StageSystem.stageMode = (byte) 1;
              break;
            }
            break;
          case 1:
            if (StageSystem.gKeyPress.down == (byte) 1)
              ++StageSystem.gameMenu[1].selection1;
            if (StageSystem.gKeyPress.up == (byte) 1)
              --StageSystem.gameMenu[1].selection1;
            if (StageSystem.gameMenu[1].selection1 == (int) StageSystem.gameMenu[1].numRows)
              StageSystem.gameMenu[1].selection1 = 0;
            if (StageSystem.gameMenu[1].selection1 < 0)
              StageSystem.gameMenu[1].selection1 = (int) StageSystem.gameMenu[1].numRows - 1;
            TextSystem.DrawTextMenu(StageSystem.gameMenu[0], GlobalAppDefinitions.SCREEN_CENTER - 4, 72);
            TextSystem.DrawTextMenu(StageSystem.gameMenu[1], GlobalAppDefinitions.SCREEN_CENTER - 40, 96 /*0x60*/);
            if (StageSystem.gKeyPress.start == (byte) 1)
            {
              PlayerSystem.playerMenuNum = (byte) StageSystem.gameMenu[1].selection1;
              TextSystem.SetupTextMenu(StageSystem.gameMenu[0], 0);
              TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], "SELECT A STAGE LIST".ToCharArray());
              TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
              TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
              TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], "   PRESENTATION".ToCharArray());
              TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
              TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], "   REGULAR".ToCharArray());
              TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
              TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], "   SPECIAL".ToCharArray());
              TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
              TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], "   BONUS".ToCharArray());
              StageSystem.gameMenu[0].alignment = (byte) 0;
              StageSystem.gameMenu[0].selection2 = 3;
              StageSystem.stageMode = (byte) 2;
              break;
            }
            break;
          case 2:
            if (StageSystem.gKeyPress.down == (byte) 1)
              StageSystem.gameMenu[0].selection2 += 2;
            if (StageSystem.gKeyPress.up == (byte) 1)
              StageSystem.gameMenu[0].selection2 -= 2;
            if (StageSystem.gameMenu[0].selection2 > 9)
              StageSystem.gameMenu[0].selection2 = 3;
            if (StageSystem.gameMenu[0].selection2 < 3)
              StageSystem.gameMenu[0].selection2 = 9;
            TextSystem.DrawTextMenu(StageSystem.gameMenu[0], GlobalAppDefinitions.SCREEN_CENTER - 80 /*0x50*/, 72);
            int num = 0;
            switch (StageSystem.gameMenu[0].selection2)
            {
              case 3:
                if (FileIO.noPresentationStages > (byte) 0)
                  num = 1;
                FileIO.activeStageList = (byte) 0;
                break;
              case 5:
                if (FileIO.noZoneStages > (byte) 0)
                  num = 1;
                FileIO.activeStageList = (byte) 1;
                break;
              case 7:
                if (FileIO.noSpecialStages > (byte) 0)
                  num = 1;
                FileIO.activeStageList = (byte) 3;
                break;
              case 9:
                if (FileIO.noBonusStages > (byte) 0)
                  num = 1;
                FileIO.activeStageList = (byte) 2;
                break;
            }
            if (StageSystem.gKeyPress.start == (byte) 1 && num == 1)
            {
              TextSystem.SetupTextMenu(StageSystem.gameMenu[0], 0);
              TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], "SELECT A STAGE".ToCharArray());
              TextSystem.SetupTextMenu(StageSystem.gameMenu[1], 0);
              TextSystem.LoadConfigListText(StageSystem.gameMenu[1], 1 + (StageSystem.gameMenu[0].selection2 - 3 >> 1));
              StageSystem.gameMenu[1].alignment = (byte) 1;
              StageSystem.gameMenu[1].numSelections = (byte) 3;
              StageSystem.gameMenu[1].selection1 = 0;
              if (StageSystem.gameMenu[1].numRows > (ushort) 18)
                StageSystem.gameMenu[1].numVisibleRows = (ushort) 18;
              StageSystem.gameMenu[0].alignment = (byte) 2;
              StageSystem.gameMenu[0].numSelections = (byte) 1;
              StageSystem.gameMenu[1].timer = (sbyte) 0;
              StageSystem.stageMode = (byte) 3;
              break;
            }
            break;
          case 3:
            if (StageSystem.gKeyDown.down == (byte) 1)
            {
              ++StageSystem.gameMenu[1].timer;
              if (StageSystem.gameMenu[1].timer > (sbyte) 4)
              {
                StageSystem.gameMenu[1].timer = (sbyte) 0;
                StageSystem.gKeyPress.down = (byte) 1;
              }
            }
            else if (StageSystem.gKeyDown.up == (byte) 1)
            {
              --StageSystem.gameMenu[1].timer;
              if (StageSystem.gameMenu[1].timer < (sbyte) -4)
              {
                StageSystem.gameMenu[1].timer = (sbyte) 0;
                StageSystem.gKeyPress.up = (byte) 1;
              }
            }
            else
              StageSystem.gameMenu[1].timer = (sbyte) 0;
            if (StageSystem.gKeyPress.down == (byte) 1)
            {
              ++StageSystem.gameMenu[1].selection1;
              if (StageSystem.gameMenu[1].selection1 - (int) StageSystem.gameMenu[1].visibleRowOffset >= (int) StageSystem.gameMenu[1].numVisibleRows)
                ++StageSystem.gameMenu[1].visibleRowOffset;
            }
            if (StageSystem.gKeyPress.up == (byte) 1)
            {
              --StageSystem.gameMenu[1].selection1;
              if (StageSystem.gameMenu[1].selection1 - (int) StageSystem.gameMenu[1].visibleRowOffset < 0)
                --StageSystem.gameMenu[1].visibleRowOffset;
            }
            if (StageSystem.gameMenu[1].selection1 == (int) StageSystem.gameMenu[1].numRows)
            {
              StageSystem.gameMenu[1].selection1 = 0;
              StageSystem.gameMenu[1].visibleRowOffset = (ushort) 0;
            }
            if (StageSystem.gameMenu[1].selection1 < 0)
            {
              StageSystem.gameMenu[1].selection1 = (int) StageSystem.gameMenu[1].numRows - 1;
              StageSystem.gameMenu[1].visibleRowOffset = (ushort) ((uint) StageSystem.gameMenu[1].numRows - (uint) StageSystem.gameMenu[1].numVisibleRows);
            }
            TextSystem.DrawTextMenu(StageSystem.gameMenu[0], GlobalAppDefinitions.SCREEN_CENTER - 4, 40);
            TextSystem.DrawTextMenu(StageSystem.gameMenu[1], GlobalAppDefinitions.SCREEN_CENTER + 100, 64 /*0x40*/);
            if (StageSystem.gKeyPress.start == (byte) 1)
            {
              StageSystem.debugMode = StageSystem.gKeyDown.touches <= 1 ? (byte) 0 : (byte) 1;
              StageSystem.stageMode = (byte) 0;
              GlobalAppDefinitions.gameMode = (byte) 1;
              StageSystem.stageListPosition = StageSystem.gameMenu[1].selection1;
              break;
            }
            break;
          case 4:
            TextSystem.DrawTextMenu(StageSystem.gameMenu[0], GlobalAppDefinitions.SCREEN_CENTER, 72);
            if (StageSystem.gKeyPress.start == (byte) 1)
            {
              StageSystem.stageMode = (byte) 0;
              TextSystem.SetupTextMenu(StageSystem.gameMenu[0], 0);
              TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], "RETRO ENGINE DEV MENU".ToCharArray());
              TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
              TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
              TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
              TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
              TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
              TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
              TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], "PLAY GAME".ToCharArray());
              TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
              TextSystem.addTextMenuEntry(StageSystem.gameMenu[0], "STAGE SELECT".ToCharArray());
              StageSystem.gameMenu[0].alignment = (byte) 2;
              StageSystem.gameMenu[0].numSelections = (byte) 2;
              StageSystem.gameMenu[0].selection1 = 0;
              StageSystem.gameMenu[0].selection2 = 7;
              StageSystem.gameMenu[1].numVisibleRows = (ushort) 0;
              StageSystem.gameMenu[1].visibleRowOffset = (ushort) 0;
              break;
            }
            break;
        }
        GraphicsSystem.gfxIndexSizeOpaque = GraphicsSystem.gfxIndexSize;
        GraphicsSystem.gfxVertexSizeOpaque = GraphicsSystem.gfxVertexSize;
      }

      public static void resetBackgroundSettings()
      {
      }
    }
}