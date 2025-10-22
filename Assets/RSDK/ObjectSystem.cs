// Decompiled with JetBrains decompiler
// Type: Retro_Engine.ObjectSystem
// Assembly: Sonic CD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D35AF46A-1892-4F52-B201-E664C9200079
// Assembly location: C:\Users\koishi\Documents\REProjects\SCD-WP7-REDO\Data\Sonic CD.dll

using System;

namespace Retro_Engine
{

    public static class ObjectSystem
    {
      public const int SUB_MAIN = 0;
      public const int SUB_PLAYER = 1;
      public const int SUB_DRAW = 2;
      public const int SUB_STARTUP = 3;
      public const int NUM_ARITHMETIC_TOKENS = 13;
      public const int NUM_EVALUATION_TOKENS = 6;
      public const int NUM_OPCODES = 135;
      public const int NUM_VARIABLE_NAMES = 229;
      public const int NUM_CONSTANTS = 31 /*0x1F*/;
      public const int SCRIPT_DATA_SIZE = 262144 /*0x040000*/;
      public const int JUMP_TABLE_SIZE = 16384 /*0x4000*/;
      private static char[,] functionNames = new char[512 /*0x0200*/, 32 /*0x20*/];
      private static char[,] typeNames = new char[256 /*0x0100*/, 32 /*0x20*/];
      public static int[] scriptData = new int[262144 /*0x040000*/];
      public static int scriptDataPos = 0;
      public static int scriptDataOffset;
      public static int scriptLineNumber;
      public static int[] jumpTableData = new int[16384 /*0x4000*/];
      public static int jumpTableDataPos = 0;
      public static int jumpTableOffset;
      public static int[] jumpTableStack = new int[1024 /*0x0400*/];
      public static int jumpTableStackPos = 0;
      public static int NUM_FUNCTIONS;
      public static int[] functionStack = new int[1024 /*0x0400*/];
      public static int functionStackPos = 0;
      public static SpriteFrame[] scriptFrames = new SpriteFrame[4096 /*0x1000*/];
      public static int scriptFramesNo = 0;
      public static byte NO_GLOBALVARIABLES;
      public static char[,] globalVariableNames = new char[256 /*0x0100*/, 32 /*0x20*/];
      public static int[] globalVariables = new int[256 /*0x0100*/];
      public static int objectLoop;
      public static ScriptEngine scriptEng = new ScriptEngine();
      public static char[] scriptText = new char[256 /*0x0100*/];
      public static ObjectScript[] objectScriptList = new ObjectScript[256 /*0x0100*/];
      public static FunctionScript[] functionScriptList = new FunctionScript[512 /*0x0200*/];
      public static ObjectEntity[] objectEntityList = new ObjectEntity[1184];
      public static ObjectDrawList[] objectDrawOrderList = new ObjectDrawList[7];
      public static int playerNum;
      public static CollisionSensor[] cSensor = new CollisionSensor[6];
      private static Random rand = new Random();
      private static sbyte[] scriptOpcodeSizes = new sbyte[135]
      {
        (sbyte) 0,
        (sbyte) 2,
        (sbyte) 2,
        (sbyte) 2,
        (sbyte) 1,
        (sbyte) 1,
        (sbyte) 2,
        (sbyte) 2,
        (sbyte) 2,
        (sbyte) 2,
        (sbyte) 2,
        (sbyte) 2,
        (sbyte) 2,
        (sbyte) 2,
        (sbyte) 1,
        (sbyte) 2,
        (sbyte) 2,
        (sbyte) 2,
        (sbyte) 2,
        (sbyte) 3,
        (sbyte) 3,
        (sbyte) 3,
        (sbyte) 3,
        (sbyte) 3,
        (sbyte) 3,
        (sbyte) 0,
        (sbyte) 0,
        (sbyte) 3,
        (sbyte) 3,
        (sbyte) 3,
        (sbyte) 3,
        (sbyte) 3,
        (sbyte) 3,
        (sbyte) 0,
        (sbyte) 2,
        (sbyte) 0,
        (sbyte) 0,
        (sbyte) 2,
        (sbyte) 2,
        (sbyte) 2,
        (sbyte) 2,
        (sbyte) 2,
        (sbyte) 5,
        (sbyte) 5,
        (sbyte) 3,
        (sbyte) 4,
        (sbyte) 7,
        (sbyte) 1,
        (sbyte) 1,
        (sbyte) 1,
        (sbyte) 3,
        (sbyte) 3,
        (sbyte) 4,
        (sbyte) 7,
        (sbyte) 7,
        (sbyte) 3,
        (sbyte) 6,
        (sbyte) 6,
        (sbyte) 5,
        (sbyte) 3,
        (sbyte) 4,
        (sbyte) 3,
        (sbyte) 7,
        (sbyte) 2,
        (sbyte) 1,
        (sbyte) 4,
        (sbyte) 4,
        (sbyte) 1,
        (sbyte) 4,
        (sbyte) 3,
        (sbyte) 4,
        (sbyte) 0,
        (sbyte) 8,
        (sbyte) 5,
        (sbyte) 5,
        (sbyte) 4,
        (sbyte) 2,
        (sbyte) 0,
        (sbyte) 0,
        (sbyte) 0,
        (sbyte) 0,
        (sbyte) 0,
        (sbyte) 3,
        (sbyte) 1,
        (sbyte) 0,
        (sbyte) 2,
        (sbyte) 1,
        (sbyte) 3,
        (sbyte) 4,
        (sbyte) 4,
        (sbyte) 1,
        (sbyte) 0,
        (sbyte) 2,
        (sbyte) 1,
        (sbyte) 1,
        (sbyte) 0,
        (sbyte) 1,
        (sbyte) 2,
        (sbyte) 4,
        (sbyte) 4,
        (sbyte) 2,
        (sbyte) 2,
        (sbyte) 2,
        (sbyte) 4,
        (sbyte) 3,
        (sbyte) 1,
        (sbyte) 0,
        (sbyte) 6,
        (sbyte) 4,
        (sbyte) 4,
        (sbyte) 4,
        (sbyte) 3,
        (sbyte) 3,
        (sbyte) 0,
        (sbyte) 0,
        (sbyte) 1,
        (sbyte) 2,
        (sbyte) 3,
        (sbyte) 3,
        (sbyte) 4,
        (sbyte) 2,
        (sbyte) 4,
        (sbyte) 2,
        (sbyte) 0,
        (sbyte) 0,
        (sbyte) 1,
        (sbyte) 3,
        (sbyte) 7,
        (sbyte) 5,
        (sbyte) 2,
        (sbyte) 2,
        (sbyte) 2,
        (sbyte) 1,
        (sbyte) 1,
        (sbyte) 4
      };

      static ObjectSystem()
      {
        for (int index = 0; index < ObjectSystem.scriptFrames.Length; ++index)
          ObjectSystem.scriptFrames[index] = new SpriteFrame();
        for (int index = 0; index < ObjectSystem.objectScriptList.Length; ++index)
          ObjectSystem.objectScriptList[index] = new ObjectScript();
        for (int index = 0; index < ObjectSystem.functionScriptList.Length; ++index)
          ObjectSystem.functionScriptList[index] = new FunctionScript();
        for (int index = 0; index < ObjectSystem.objectEntityList.Length; ++index)
          ObjectSystem.objectEntityList[index] = new ObjectEntity();
        for (int index = 0; index < ObjectSystem.objectDrawOrderList.Length; ++index)
          ObjectSystem.objectDrawOrderList[index] = new ObjectDrawList();
        for (int index = 0; index < ObjectSystem.cSensor.Length; ++index)
          ObjectSystem.cSensor[index] = new CollisionSensor();
      }

      public static void ClearScriptData()
      {
        char[] charArray = "BlankObject".ToCharArray();
        for (int index = 0; index < 262144 /*0x040000*/; ++index)
          ObjectSystem.scriptData[index] = 0;
        for (int index = 0; index < 16384 /*0x4000*/; ++index)
          ObjectSystem.jumpTableData[index] = 0;
        ObjectSystem.scriptDataPos = 0;
        ObjectSystem.jumpTableDataPos = 0;
        ObjectSystem.scriptFramesNo = 0;
        ObjectSystem.NUM_FUNCTIONS = 0;
        AnimationSystem.ClearAnimationData();
        for (int index = 0; index < 2; ++index)
        {
          PlayerSystem.playerList[index].animationFile = AnimationSystem.GetDefaultAnimationRef();
          PlayerSystem.playerList[index].objectPtr = ObjectSystem.objectEntityList[0];
        }
        for (int index = 0; index < 256 /*0x0100*/; ++index)
        {
          ObjectSystem.objectScriptList[index].mainScript = 262143 /*0x03FFFF*/;
          ObjectSystem.objectScriptList[index].mainJumpTable = 16383 /*0x3FFF*/;
          ObjectSystem.objectScriptList[index].playerScript = 262143 /*0x03FFFF*/;
          ObjectSystem.objectScriptList[index].playerJumpTable = 16383 /*0x3FFF*/;
          ObjectSystem.objectScriptList[index].drawScript = 262143 /*0x03FFFF*/;
          ObjectSystem.objectScriptList[index].drawJumpTable = 16383 /*0x3FFF*/;
          ObjectSystem.objectScriptList[index].startupScript = 262143 /*0x03FFFF*/;
          ObjectSystem.objectScriptList[index].startupJumpTable = 16383 /*0x3FFF*/;
          ObjectSystem.objectScriptList[index].frameListOffset = 0;
          ObjectSystem.objectScriptList[index].numFrames = 0;
          ObjectSystem.objectScriptList[index].surfaceNum = (byte) 0;
          ObjectSystem.objectScriptList[index].animationFile = AnimationSystem.GetDefaultAnimationRef();
          ObjectSystem.functionScriptList[index].mainScript = 262143 /*0x03FFFF*/;
          ObjectSystem.functionScriptList[index].mainJumpTable = 16383 /*0x3FFF*/;
          ObjectSystem.typeNames[index, 0] = char.MinValue;
        }
        ObjectSystem.SetObjectTypeName(charArray, 0);
      }

      public static void SetObjectTypeName(char[] typeName, int scriptNum)
      {
        int index1 = 0;
        int index2 = 0;
        while (index1 < typeName.Length)
        {
          if (typeName[index1] != char.MinValue)
          {
            if (typeName[index1] != ' ')
            {
              ObjectSystem.typeNames[scriptNum, index2] = typeName[index1];
              ++index2;
            }
            ++index1;
          }
          else
            index1 = typeName.Length;
        }
        if (index2 >= ObjectSystem.typeNames.GetLength(1))
          return;
        ObjectSystem.typeNames[scriptNum, index2] = char.MinValue;
      }

      public static void LoadByteCodeFile(int fileType, int scriptNum)
      {
        FileData fData = new FileData();
        char[] charArray1 = "Data/Scripts/ByteCode/".ToCharArray();
        char[] charArray2 = ".bin".ToCharArray();
        char[] charArray3 = "GlobalCode.bin".ToCharArray();
        FileIO.StrCopy(ref ObjectSystem.scriptText, ref charArray1);
        switch (fileType)
        {
          case 0:
            FileIO.StrAdd(ref ObjectSystem.scriptText, ref FileIO.pStageList[StageSystem.stageListPosition].stageFolderName);
            FileIO.StrAdd(ref ObjectSystem.scriptText, ref charArray2);
            break;
          case 1:
            FileIO.StrAdd(ref ObjectSystem.scriptText, ref FileIO.zStageList[StageSystem.stageListPosition].stageFolderName);
            FileIO.StrAdd(ref ObjectSystem.scriptText, ref charArray2);
            break;
          case 2:
            FileIO.StrAdd(ref ObjectSystem.scriptText, ref FileIO.bStageList[StageSystem.stageListPosition].stageFolderName);
            FileIO.StrAdd(ref ObjectSystem.scriptText, ref charArray2);
            break;
          case 3:
            FileIO.StrAdd(ref ObjectSystem.scriptText, ref FileIO.sStageList[StageSystem.stageListPosition].stageFolderName);
            FileIO.StrAdd(ref ObjectSystem.scriptText, ref charArray2);
            break;
          case 4:
            FileIO.StrAdd(ref ObjectSystem.scriptText, ref charArray3);
            break;
        }
        if (!FileIO.LoadFile(ObjectSystem.scriptText, fData))
          return;
        int scriptDataPos = ObjectSystem.scriptDataPos;
        int num1 = (int) FileIO.ReadByte() + ((int) FileIO.ReadByte() << 8) + ((int) FileIO.ReadByte() << 16 /*0x10*/) + ((int) FileIO.ReadByte() << 24);
        while (num1 > 0)
        {
          byte num2 = FileIO.ReadByte();
          byte num3 = (byte) ((uint) num2 & (uint) sbyte.MaxValue);
          if (num2 < (byte) 128 /*0x80*/)
          {
            for (; num3 > (byte) 0; --num3)
            {
              byte num4 = FileIO.ReadByte();
              ObjectSystem.scriptData[scriptDataPos] = (int) num4;
              ++scriptDataPos;
              ++ObjectSystem.scriptDataPos;
              --num1;
            }
          }
          else
          {
            for (; num3 > (byte) 0; --num3)
            {
              int num5 = (int) FileIO.ReadByte() + ((int) FileIO.ReadByte() << 8) + ((int) FileIO.ReadByte() << 16 /*0x10*/) + ((int) FileIO.ReadByte() << 24);
              ObjectSystem.scriptData[scriptDataPos] = num5;
              ++scriptDataPos;
              ++ObjectSystem.scriptDataPos;
              --num1;
            }
          }
        }
        int jumpTableDataPos = ObjectSystem.jumpTableDataPos;
        int num6 = (int) FileIO.ReadByte() + ((int) FileIO.ReadByte() << 8) + ((int) FileIO.ReadByte() << 16 /*0x10*/) + ((int) FileIO.ReadByte() << 24);
        while (num6 > 0)
        {
          byte num7 = FileIO.ReadByte();
          byte num8 = (byte) ((uint) num7 & (uint) sbyte.MaxValue);
          if (num7 < (byte) 128 /*0x80*/)
          {
            for (; num8 > (byte) 0; --num8)
            {
              byte num9 = FileIO.ReadByte();
              ObjectSystem.jumpTableData[jumpTableDataPos] = (int) num9;
              ++jumpTableDataPos;
              ++ObjectSystem.jumpTableDataPos;
              --num6;
            }
          }
          else
          {
            for (; num8 > (byte) 0; --num8)
            {
              int num10 = (int) FileIO.ReadByte() + ((int) FileIO.ReadByte() << 8) + ((int) FileIO.ReadByte() << 16 /*0x10*/) + ((int) FileIO.ReadByte() << 24);
              ObjectSystem.jumpTableData[jumpTableDataPos] = num10;
              ++jumpTableDataPos;
              ++ObjectSystem.jumpTableDataPos;
              --num6;
            }
          }
        }
        int num11 = (int) FileIO.ReadByte() + ((int) FileIO.ReadByte() << 8);
        int index1 = scriptNum;
        for (int index2 = num11; index2 > 0; --index2)
        {
          int num12 = (int) FileIO.ReadByte() + ((int) FileIO.ReadByte() << 8) + ((int) FileIO.ReadByte() << 16 /*0x10*/) + ((int) FileIO.ReadByte() << 24);
          ObjectSystem.objectScriptList[index1].mainScript = num12;
          int num13 = (int) FileIO.ReadByte() + ((int) FileIO.ReadByte() << 8) + ((int) FileIO.ReadByte() << 16 /*0x10*/) + ((int) FileIO.ReadByte() << 24);
          ObjectSystem.objectScriptList[index1].playerScript = num13;
          int num14 = (int) FileIO.ReadByte() + ((int) FileIO.ReadByte() << 8) + ((int) FileIO.ReadByte() << 16 /*0x10*/) + ((int) FileIO.ReadByte() << 24);
          ObjectSystem.objectScriptList[index1].drawScript = num14;
          int num15 = (int) FileIO.ReadByte() + ((int) FileIO.ReadByte() << 8) + ((int) FileIO.ReadByte() << 16 /*0x10*/) + ((int) FileIO.ReadByte() << 24);
          ObjectSystem.objectScriptList[index1].startupScript = num15;
          ++index1;
        }
        int index3 = scriptNum;
        for (int index4 = num11; index4 > 0; --index4)
        {
          int num16 = (int) FileIO.ReadByte() + ((int) FileIO.ReadByte() << 8) + ((int) FileIO.ReadByte() << 16 /*0x10*/) + ((int) FileIO.ReadByte() << 24);
          ObjectSystem.objectScriptList[index3].mainJumpTable = num16;
          int num17 = (int) FileIO.ReadByte() + ((int) FileIO.ReadByte() << 8) + ((int) FileIO.ReadByte() << 16 /*0x10*/) + ((int) FileIO.ReadByte() << 24);
          ObjectSystem.objectScriptList[index3].playerJumpTable = num17;
          int num18 = (int) FileIO.ReadByte() + ((int) FileIO.ReadByte() << 8) + ((int) FileIO.ReadByte() << 16 /*0x10*/) + ((int) FileIO.ReadByte() << 24);
          ObjectSystem.objectScriptList[index3].drawJumpTable = num18;
          int num19 = (int) FileIO.ReadByte() + ((int) FileIO.ReadByte() << 8) + ((int) FileIO.ReadByte() << 16 /*0x10*/) + ((int) FileIO.ReadByte() << 24);
          ObjectSystem.objectScriptList[index3].startupJumpTable = num19;
          ++index3;
        }
        int num20 = (int) FileIO.ReadByte() + ((int) FileIO.ReadByte() << 8);
        int index5 = 0;
        for (int index6 = num20; index6 > 0; --index6)
        {
          int num21 = (int) FileIO.ReadByte() + ((int) FileIO.ReadByte() << 8) + ((int) FileIO.ReadByte() << 16 /*0x10*/) + ((int) FileIO.ReadByte() << 24);
          ObjectSystem.functionScriptList[index5].mainScript = num21;
          ++index5;
        }
        int index7 = 0;
        for (int index8 = num20; index8 > 0; --index8)
        {
          int num22 = (int) FileIO.ReadByte() + ((int) FileIO.ReadByte() << 8) + ((int) FileIO.ReadByte() << 16 /*0x10*/) + ((int) FileIO.ReadByte() << 24);
          ObjectSystem.functionScriptList[index7].mainJumpTable = num22;
          ++index7;
        }
        FileIO.CloseFile();
      }

      public static void ProcessScript(int scriptCodePtr, int jumpTablePtr, int scriptSub)
      {
        bool flag = false;
        int index1 = 0;
        int num1 = scriptCodePtr;
        ObjectSystem.jumpTableStackPos = 0;
        ObjectSystem.functionStackPos = 0;
        while (!flag)
        {
          int index2 = ObjectSystem.scriptData[scriptCodePtr];
          ++scriptCodePtr;
          int num2 = 0;
          sbyte num3 = ObjectSystem.scriptOpcodeSizes[index2];
          for (int index3 = 0; index3 < (int) num3; ++index3)
          {
            switch (ObjectSystem.scriptData[scriptCodePtr])
            {
              case 1:
                ++scriptCodePtr;
                int num4 = num2 + 1;
                switch (ObjectSystem.scriptData[scriptCodePtr])
                {
                  case 0:
                    index1 = ObjectSystem.objectLoop;
                    break;
                  case 1:
                    ++scriptCodePtr;
                    if (ObjectSystem.scriptData[scriptCodePtr] == 1)
                    {
                      ++scriptCodePtr;
                      index1 = ObjectSystem.scriptEng.arrayPosition[ObjectSystem.scriptData[scriptCodePtr]];
                    }
                    else
                    {
                      ++scriptCodePtr;
                      index1 = ObjectSystem.scriptData[scriptCodePtr];
                    }
                    num4 += 2;
                    break;
                  case 2:
                    ++scriptCodePtr;
                    if (ObjectSystem.scriptData[scriptCodePtr] == 1)
                    {
                      ++scriptCodePtr;
                      index1 = ObjectSystem.objectLoop + ObjectSystem.scriptEng.arrayPosition[ObjectSystem.scriptData[scriptCodePtr]];
                    }
                    else
                    {
                      ++scriptCodePtr;
                      index1 = ObjectSystem.objectLoop + ObjectSystem.scriptData[scriptCodePtr];
                    }
                    num4 += 2;
                    break;
                  case 3:
                    ++scriptCodePtr;
                    if (ObjectSystem.scriptData[scriptCodePtr] == 1)
                    {
                      ++scriptCodePtr;
                      index1 = ObjectSystem.objectLoop - ObjectSystem.scriptEng.arrayPosition[ObjectSystem.scriptData[scriptCodePtr]];
                    }
                    else
                    {
                      ++scriptCodePtr;
                      index1 = ObjectSystem.objectLoop - ObjectSystem.scriptData[scriptCodePtr];
                    }
                    num4 += 2;
                    break;
                }
                ++scriptCodePtr;
                int num5 = num4 + 1;
                switch (ObjectSystem.scriptData[scriptCodePtr])
                {
                  case 0:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.scriptEng.tempValue[0];
                    break;
                  case 1:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.scriptEng.tempValue[1];
                    break;
                  case 2:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.scriptEng.tempValue[2];
                    break;
                  case 3:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.scriptEng.tempValue[3];
                    break;
                  case 4:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.scriptEng.tempValue[4];
                    break;
                  case 5:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.scriptEng.tempValue[5];
                    break;
                  case 6:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.scriptEng.tempValue[6];
                    break;
                  case 7:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.scriptEng.tempValue[7];
                    break;
                  case 8:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.scriptEng.checkResult;
                    break;
                  case 9:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.scriptEng.arrayPosition[0];
                    break;
                  case 10:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.scriptEng.arrayPosition[1];
                    break;
                  case 11:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.globalVariables[index1];
                    break;
                  case 12:
                    ObjectSystem.scriptEng.operands[index3] = index1;
                    break;
                  case 13:
                    ObjectSystem.scriptEng.operands[index3] = (int) ObjectSystem.objectEntityList[index1].type;
                    break;
                  case 14:
                    ObjectSystem.scriptEng.operands[index3] = (int) ObjectSystem.objectEntityList[index1].propertyValue;
                    break;
                  case 15:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.objectEntityList[index1].xPos;
                    break;
                  case 16 /*0x10*/:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.objectEntityList[index1].yPos;
                    break;
                  case 17:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.objectEntityList[index1].xPos >> 16 /*0x10*/;
                    break;
                  case 18:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.objectEntityList[index1].yPos >> 16 /*0x10*/;
                    break;
                  case 19:
                    ObjectSystem.scriptEng.operands[index3] = (int) ObjectSystem.objectEntityList[index1].state;
                    break;
                  case 20:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.objectEntityList[index1].rotation;
                    break;
                  case 21:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.objectEntityList[index1].scale;
                    break;
                  case 22:
                    ObjectSystem.scriptEng.operands[index3] = (int) ObjectSystem.objectEntityList[index1].priority;
                    break;
                  case 23:
                    ObjectSystem.scriptEng.operands[index3] = (int) ObjectSystem.objectEntityList[index1].drawOrder;
                    break;
                  case 24:
                    ObjectSystem.scriptEng.operands[index3] = (int) ObjectSystem.objectEntityList[index1].direction;
                    break;
                  case 25:
                    ObjectSystem.scriptEng.operands[index3] = (int) ObjectSystem.objectEntityList[index1].inkEffect;
                    break;
                  case 26:
                    ObjectSystem.scriptEng.operands[index3] = (int) ObjectSystem.objectEntityList[index1].alpha;
                    break;
                  case 27:
                    ObjectSystem.scriptEng.operands[index3] = (int) ObjectSystem.objectEntityList[index1].frame;
                    break;
                  case 28:
                    ObjectSystem.scriptEng.operands[index3] = (int) ObjectSystem.objectEntityList[index1].animation;
                    break;
                  case 29:
                    ObjectSystem.scriptEng.operands[index3] = (int) ObjectSystem.objectEntityList[index1].prevAnimation;
                    break;
                  case 30:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.objectEntityList[index1].animationSpeed;
                    break;
                  case 31 /*0x1F*/:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.objectEntityList[index1].animationTimer;
                    break;
                  case 32 /*0x20*/:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.objectEntityList[index1].value[0];
                    break;
                  case 33:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.objectEntityList[index1].value[1];
                    break;
                  case 34:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.objectEntityList[index1].value[2];
                    break;
                  case 35:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.objectEntityList[index1].value[3];
                    break;
                  case 36:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.objectEntityList[index1].value[4];
                    break;
                  case 37:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.objectEntityList[index1].value[5];
                    break;
                  case 38:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.objectEntityList[index1].value[6];
                    break;
                  case 39:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.objectEntityList[index1].value[7];
                    break;
                  case 40:
                    ObjectSystem.scriptEng.sRegister = ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16 /*0x10*/;
                    if (ObjectSystem.scriptEng.sRegister > StageSystem.xScrollOffset - GlobalAppDefinitions.OBJECT_BORDER_X1 && ObjectSystem.scriptEng.sRegister < StageSystem.xScrollOffset + GlobalAppDefinitions.OBJECT_BORDER_X2)
                    {
                      ObjectSystem.scriptEng.sRegister = ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos >> 16 /*0x10*/;
                      ObjectSystem.scriptEng.operands[index3] = ObjectSystem.scriptEng.sRegister <= StageSystem.yScrollOffset - 256 /*0x0100*/ || ObjectSystem.scriptEng.sRegister >= StageSystem.yScrollOffset + 496 ? 1 : 0;
                      break;
                    }
                    ObjectSystem.scriptEng.operands[index3] = 1;
                    break;
                  case 41:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.state;
                    break;
                  case 42:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].controlMode;
                    break;
                  case 43:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].controlLock;
                    break;
                  case 44:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].collisionMode;
                    break;
                  case 45:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].collisionPlane;
                    break;
                  case 46:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].xPos;
                    break;
                  case 47:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].yPos;
                    break;
                  case 48 /*0x30*/:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].xPos >> 16 /*0x10*/;
                    break;
                  case 49:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].yPos >> 16 /*0x10*/;
                    break;
                  case 50:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].screenXPos;
                    break;
                  case 51:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].screenYPos;
                    break;
                  case 52:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].speed;
                    break;
                  case 53:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].xVelocity;
                    break;
                  case 54:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].yVelocity;
                    break;
                  case 55:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].gravity;
                    break;
                  case 56:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].angle;
                    break;
                  case 57:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].skidding;
                    break;
                  case 58:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].pushing;
                    break;
                  case 59:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].trackScroll;
                    break;
                  case 60:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].up;
                    break;
                  case 61:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].down;
                    break;
                  case 62:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].left;
                    break;
                  case 63 /*0x3F*/:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].right;
                    break;
                  case 64 /*0x40*/:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].jumpPress;
                    break;
                  case 65:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].jumpHold;
                    break;
                  case 66:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].followPlayer1;
                    break;
                  case 67:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].lookPos;
                    break;
                  case 68:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].water;
                    break;
                  case 69:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.topSpeed;
                    break;
                  case 70:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.acceleration;
                    break;
                  case 71:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.deceleration;
                    break;
                  case 72:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.airAcceleration;
                    break;
                  case 73:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.airDeceleration;
                    break;
                  case 74:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.gravity;
                    break;
                  case 75:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.jumpStrength;
                    break;
                  case 76:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.jumpCap;
                    break;
                  case 77:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.rollingAcceleration;
                    break;
                  case 78:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.rollingDeceleration;
                    break;
                  case 79:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].objectNum;
                    break;
                  case 80 /*0x50*/:
                    ObjectSystem.scriptEng.operands[index3] = (int) AnimationSystem.collisionBoxList[PlayerSystem.playerList[ObjectSystem.playerNum].animationFile.cbListOffset + (int) AnimationSystem.animationFrames[AnimationSystem.animationList[PlayerSystem.playerList[ObjectSystem.playerNum].animationFile.aniListOffset + (int) PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.animation].frameListOffset + (int) PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.frame].collisionBox].left[0];
                    break;
                  case 81:
                    ObjectSystem.scriptEng.operands[index3] = (int) AnimationSystem.collisionBoxList[PlayerSystem.playerList[ObjectSystem.playerNum].animationFile.cbListOffset + (int) AnimationSystem.animationFrames[AnimationSystem.animationList[PlayerSystem.playerList[ObjectSystem.playerNum].animationFile.aniListOffset + (int) PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.animation].frameListOffset + (int) PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.frame].collisionBox].top[0];
                    break;
                  case 82:
                    ObjectSystem.scriptEng.operands[index3] = (int) AnimationSystem.collisionBoxList[PlayerSystem.playerList[ObjectSystem.playerNum].animationFile.cbListOffset + (int) AnimationSystem.animationFrames[AnimationSystem.animationList[PlayerSystem.playerList[ObjectSystem.playerNum].animationFile.aniListOffset + (int) PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.animation].frameListOffset + (int) PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.frame].collisionBox].right[0];
                    break;
                  case 83:
                    ObjectSystem.scriptEng.operands[index3] = (int) AnimationSystem.collisionBoxList[PlayerSystem.playerList[ObjectSystem.playerNum].animationFile.cbListOffset + (int) AnimationSystem.animationFrames[AnimationSystem.animationList[PlayerSystem.playerList[ObjectSystem.playerNum].animationFile.aniListOffset + (int) PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.animation].frameListOffset + (int) PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.frame].collisionBox].bottom[0];
                    break;
                  case 84:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].flailing[index1];
                    break;
                  case 85:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].timer;
                    break;
                  case 86:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].tileCollisions;
                    break;
                  case 87:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].objectInteraction;
                    break;
                  case 88:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].visible;
                    break;
                  case 89:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.rotation;
                    break;
                  case 90:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.scale;
                    break;
                  case 91:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.priority;
                    break;
                  case 92:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.drawOrder;
                    break;
                  case 93:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.direction;
                    break;
                  case 94:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.inkEffect;
                    break;
                  case 95:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.alpha;
                    break;
                  case 96 /*0x60*/:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.frame;
                    break;
                  case 97:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.animation;
                    break;
                  case 98:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.prevAnimation;
                    break;
                  case 99:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.animationSpeed;
                    break;
                  case 100:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.animationTimer;
                    break;
                  case 101:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[0];
                    break;
                  case 102:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[1];
                    break;
                  case 103:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[2];
                    break;
                  case 104:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[3];
                    break;
                  case 105:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[4];
                    break;
                  case 106:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[5];
                    break;
                  case 107:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[6];
                    break;
                  case 108:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[7];
                    break;
                  case 109:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].value[0];
                    break;
                  case 110:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].value[1];
                    break;
                  case 111:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].value[2];
                    break;
                  case 112 /*0x70*/:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].value[3];
                    break;
                  case 113:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].value[4];
                    break;
                  case 114:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].value[5];
                    break;
                  case 115:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].value[6];
                    break;
                  case 116:
                    ObjectSystem.scriptEng.operands[index3] = PlayerSystem.playerList[ObjectSystem.playerNum].value[7];
                    break;
                  case 117:
                    ObjectSystem.scriptEng.sRegister = PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.xPos >> 16 /*0x10*/;
                    if (ObjectSystem.scriptEng.sRegister > StageSystem.xScrollOffset - GlobalAppDefinitions.OBJECT_BORDER_X1 && ObjectSystem.scriptEng.sRegister < StageSystem.xScrollOffset + GlobalAppDefinitions.OBJECT_BORDER_X2)
                    {
                      ObjectSystem.scriptEng.sRegister = PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.yPos >> 16 /*0x10*/;
                      ObjectSystem.scriptEng.operands[index3] = ObjectSystem.scriptEng.sRegister <= StageSystem.yScrollOffset - 256 /*0x0100*/ || ObjectSystem.scriptEng.sRegister >= StageSystem.yScrollOffset + 496 ? 1 : 0;
                      break;
                    }
                    ObjectSystem.scriptEng.operands[index3] = 1;
                    break;
                  case 118:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.stageMode;
                    break;
                  case 119:
                    ObjectSystem.scriptEng.operands[index3] = (int) FileIO.activeStageList;
                    break;
                  case 120:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.stageListPosition;
                    break;
                  case 121:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.timeEnabled;
                    break;
                  case 122:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.milliSeconds;
                    break;
                  case 123:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.seconds;
                    break;
                  case 124:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.minutes;
                    break;
                  case 125:
                    ObjectSystem.scriptEng.operands[index3] = FileIO.actNumber;
                    break;
                  case 126:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.pauseEnabled;
                    break;
                  case (int) sbyte.MaxValue:
                    switch (FileIO.activeStageList)
                    {
                      case 0:
                        ObjectSystem.scriptEng.operands[index3] = (int) FileIO.noPresentationStages;
                        break;
                      case 1:
                        ObjectSystem.scriptEng.operands[index3] = (int) FileIO.noZoneStages;
                        break;
                      case 2:
                        ObjectSystem.scriptEng.operands[index3] = (int) FileIO.noBonusStages;
                        break;
                      case 3:
                        ObjectSystem.scriptEng.operands[index3] = (int) FileIO.noSpecialStages;
                        break;
                    }
                    break;
                  case 128 /*0x80*/:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.newXBoundary1;
                    break;
                  case 129:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.newXBoundary2;
                    break;
                  case 130:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.newYBoundary1;
                    break;
                  case 131:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.newYBoundary2;
                    break;
                  case 132:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.xBoundary1;
                    break;
                  case 133:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.xBoundary2;
                    break;
                  case 134:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.yBoundary1;
                    break;
                  case 135:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.yBoundary2;
                    break;
                  case 136:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.bgDeformationData0[index1];
                    break;
                  case 137:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.bgDeformationData1[index1];
                    break;
                  case 138:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.bgDeformationData2[index1];
                    break;
                  case 139:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.bgDeformationData3[index1];
                    break;
                  case 140:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.waterLevel;
                    break;
                  case 141:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.activeTileLayers[index1];
                    break;
                  case 142:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.tLayerMidPoint;
                    break;
                  case 143:
                    ObjectSystem.scriptEng.operands[index3] = (int) PlayerSystem.playerMenuNum;
                    break;
                  case 144 /*0x90*/:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.playerNum;
                    break;
                  case 145:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.cameraEnabled;
                    break;
                  case 146:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.cameraTarget;
                    break;
                  case 147:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.cameraStyle;
                    break;
                  case 148:
                    ObjectSystem.scriptEng.operands[index3] = ObjectSystem.objectDrawOrderList[index1].listSize;
                    break;
                  case 149:
                    ObjectSystem.scriptEng.operands[index3] = GlobalAppDefinitions.SCREEN_CENTER;
                    break;
                  case 150:
                    ObjectSystem.scriptEng.operands[index3] = 120;
                    break;
                  case 151:
                    ObjectSystem.scriptEng.operands[index3] = GlobalAppDefinitions.SCREEN_XSIZE;
                    break;
                  case 152:
                    ObjectSystem.scriptEng.operands[index3] = 240 /*0xF0*/;
                    break;
                  case 153:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.xScrollOffset;
                    break;
                  case 154:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.yScrollOffset;
                    break;
                  case 155:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.screenShakeX;
                    break;
                  case 156:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.screenShakeY;
                    break;
                  case 157:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.cameraAdjustY;
                    break;
                  case 158:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.gKeyDown.touchDown[index1];
                    break;
                  case 159:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.gKeyDown.touchX[index1];
                    break;
                  case 160 /*0xA0*/:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.gKeyDown.touchY[index1];
                    break;
                  case 161:
                    ObjectSystem.scriptEng.operands[index3] = AudioPlayback.musicVolume;
                    break;
                  case 162:
                    ObjectSystem.scriptEng.operands[index3] = AudioPlayback.currentMusicTrack;
                    break;
                  case 163:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.gKeyDown.up;
                    break;
                  case 164:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.gKeyDown.down;
                    break;
                  case 165:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.gKeyDown.left;
                    break;
                  case 166:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.gKeyDown.right;
                    break;
                  case 167:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.gKeyDown.buttonA;
                    break;
                  case 168:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.gKeyDown.buttonB;
                    break;
                  case 169:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.gKeyDown.buttonC;
                    break;
                  case 170:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.gKeyDown.start;
                    break;
                  case 171:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.gKeyPress.up;
                    break;
                  case 172:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.gKeyPress.down;
                    break;
                  case 173:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.gKeyPress.left;
                    break;
                  case 174:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.gKeyPress.right;
                    break;
                  case 175:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.gKeyPress.buttonA;
                    break;
                  case 176 /*0xB0*/:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.gKeyPress.buttonB;
                    break;
                  case 177:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.gKeyPress.buttonC;
                    break;
                  case 178:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.gKeyPress.start;
                    break;
                  case 179:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.gameMenu[0].selection1;
                    break;
                  case 180:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.gameMenu[1].selection1;
                    break;
                  case 181:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.stageLayouts[index1].xSize;
                    break;
                  case 182:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.stageLayouts[index1].ySize;
                    break;
                  case 183:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.stageLayouts[index1].type;
                    break;
                  case 184:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.stageLayouts[index1].angle;
                    break;
                  case 185:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.stageLayouts[index1].xPos;
                    break;
                  case 186:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.stageLayouts[index1].yPos;
                    break;
                  case 187:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.stageLayouts[index1].zPos;
                    break;
                  case 188:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.stageLayouts[index1].parallaxFactor;
                    break;
                  case 189:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.stageLayouts[index1].scrollSpeed;
                    break;
                  case 190:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.stageLayouts[index1].scrollPosition;
                    break;
                  case 191:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.stageLayouts[index1].deformationPos;
                    break;
                  case 192 /*0xC0*/:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.stageLayouts[index1].deformationPosW;
                    break;
                  case 193:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.hParallax.parallaxFactor[index1];
                    break;
                  case 194:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.hParallax.scrollSpeed[index1];
                    break;
                  case 195:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.hParallax.scrollPosition[index1];
                    break;
                  case 196:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.vParallax.parallaxFactor[index1];
                    break;
                  case 197:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.vParallax.scrollSpeed[index1];
                    break;
                  case 198:
                    ObjectSystem.scriptEng.operands[index3] = StageSystem.vParallax.scrollPosition[index1];
                    break;
                  case 199:
                    ObjectSystem.scriptEng.operands[index3] = Scene3D.numVertices;
                    break;
                  case 200:
                    ObjectSystem.scriptEng.operands[index3] = Scene3D.numFaces;
                    break;
                  case 201:
                    ObjectSystem.scriptEng.operands[index3] = Scene3D.vertexBuffer[index1].x;
                    break;
                  case 202:
                    ObjectSystem.scriptEng.operands[index3] = Scene3D.vertexBuffer[index1].y;
                    break;
                  case 203:
                    ObjectSystem.scriptEng.operands[index3] = Scene3D.vertexBuffer[index1].z;
                    break;
                  case 204:
                    ObjectSystem.scriptEng.operands[index3] = Scene3D.vertexBuffer[index1].u;
                    break;
                  case 205:
                    ObjectSystem.scriptEng.operands[index3] = Scene3D.vertexBuffer[index1].v;
                    break;
                  case 206:
                    ObjectSystem.scriptEng.operands[index3] = Scene3D.indexBuffer[index1].a;
                    break;
                  case 207:
                    ObjectSystem.scriptEng.operands[index3] = Scene3D.indexBuffer[index1].b;
                    break;
                  case 208 /*0xD0*/:
                    ObjectSystem.scriptEng.operands[index3] = Scene3D.indexBuffer[index1].c;
                    break;
                  case 209:
                    ObjectSystem.scriptEng.operands[index3] = Scene3D.indexBuffer[index1].d;
                    break;
                  case 210:
                    ObjectSystem.scriptEng.operands[index3] = (int) Scene3D.indexBuffer[index1].flag;
                    break;
                  case 211:
                    ObjectSystem.scriptEng.operands[index3] = Scene3D.indexBuffer[index1].color;
                    break;
                  case 212:
                    ObjectSystem.scriptEng.operands[index3] = Scene3D.projectionX;
                    break;
                  case 213:
                    ObjectSystem.scriptEng.operands[index3] = Scene3D.projectionY;
                    break;
                  case 214:
                    ObjectSystem.scriptEng.operands[index3] = (int) GlobalAppDefinitions.gameMode;
                    break;
                  case 215:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.debugMode;
                    break;
                  case 216:
                    ObjectSystem.scriptEng.operands[index3] = GlobalAppDefinitions.gameMessage;
                    break;
                  case 217:
                    ObjectSystem.scriptEng.operands[index3] = FileIO.saveRAM[index1];
                    break;
                  case 218:
                    ObjectSystem.scriptEng.operands[index3] = (int) GlobalAppDefinitions.gameLanguage;
                    break;
                  case 219:
                    ObjectSystem.scriptEng.operands[index3] = (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum;
                    break;
                  case 220:
                    ObjectSystem.scriptEng.operands[index3] = (int) GlobalAppDefinitions.gameOnlineActive;
                    break;
                  case 221:
                    ObjectSystem.scriptEng.operands[index3] = GlobalAppDefinitions.frameSkipTimer;
                    break;
                  case 222:
                    ObjectSystem.scriptEng.operands[index3] = GlobalAppDefinitions.frameSkipSetting;
                    break;
                  case 223:
                    ObjectSystem.scriptEng.operands[index3] = GlobalAppDefinitions.gameSFXVolume;
                    break;
                  case 224 /*0xE0*/:
                    ObjectSystem.scriptEng.operands[index3] = GlobalAppDefinitions.gameBGMVolume;
                    break;
                  case 225:
                    ObjectSystem.scriptEng.operands[index3] = GlobalAppDefinitions.gamePlatformID;
                    break;
                  case 226:
                    ObjectSystem.scriptEng.operands[index3] = (int) GlobalAppDefinitions.gameTrialMode;
                    break;
                  case 227:
                    ObjectSystem.scriptEng.operands[index3] = (int) StageSystem.gKeyPress.start;
                    break;
                  case 228:
                    ObjectSystem.scriptEng.operands[index3] = (int) GlobalAppDefinitions.gameHapticsEnabled;
                    break;
                }
                ++scriptCodePtr;
                num2 = num5 + 1;
                break;
              case 2:
                ++scriptCodePtr;
                ObjectSystem.scriptEng.operands[index3] = ObjectSystem.scriptData[scriptCodePtr];
                ++scriptCodePtr;
                num2 += 2;
                break;
              case 3:
                ++scriptCodePtr;
                int num6 = num2 + 1;
                int index4 = 0;
                index1 = 0;
                ObjectSystem.scriptEng.sRegister = ObjectSystem.scriptData[scriptCodePtr];
                ObjectSystem.scriptText[ObjectSystem.scriptEng.sRegister] = char.MinValue;
                for (; index4 < ObjectSystem.scriptEng.sRegister; ++index4)
                {
                  switch (index1)
                  {
                    case 0:
                      ++scriptCodePtr;
                      ++num6;
                      ObjectSystem.scriptText[index4] = (char) (ObjectSystem.scriptData[scriptCodePtr] >> 24);
                      ++index1;
                      break;
                    case 1:
                      ObjectSystem.scriptText[index4] = (char) ((ObjectSystem.scriptData[scriptCodePtr] & 16777215 /*0xFFFFFF*/) >> 16 /*0x10*/);
                      ++index1;
                      break;
                    case 2:
                      ObjectSystem.scriptText[index4] = (char) ((ObjectSystem.scriptData[scriptCodePtr] & (int) ushort.MaxValue) >> 8);
                      ++index1;
                      break;
                    case 3:
                      ObjectSystem.scriptText[index4] = (char) (ObjectSystem.scriptData[scriptCodePtr] & (int) byte.MaxValue);
                      index1 = 0;
                      break;
                  }
                }
                if (index1 == 0)
                {
                  scriptCodePtr += 2;
                  num2 = num6 + 2;
                  break;
                }
                ++scriptCodePtr;
                num2 = num6 + 1;
                break;
            }
          }
          switch (index2)
          {
            case 0:
              flag = true;
              break;
            case 1:
              ObjectSystem.scriptEng.operands[0] = ObjectSystem.scriptEng.operands[1];
              break;
            case 2:
              ObjectSystem.scriptEng.operands[0] += ObjectSystem.scriptEng.operands[1];
              break;
            case 3:
              ObjectSystem.scriptEng.operands[0] -= ObjectSystem.scriptEng.operands[1];
              break;
            case 4:
              ++ObjectSystem.scriptEng.operands[0];
              break;
            case 5:
              --ObjectSystem.scriptEng.operands[0];
              break;
            case 6:
              ObjectSystem.scriptEng.operands[0] *= ObjectSystem.scriptEng.operands[1];
              break;
            case 7:
              ObjectSystem.scriptEng.operands[0] /= ObjectSystem.scriptEng.operands[1];
              break;
            case 8:
              ObjectSystem.scriptEng.operands[0] >>= ObjectSystem.scriptEng.operands[1];
              break;
            case 9:
              ObjectSystem.scriptEng.operands[0] <<= ObjectSystem.scriptEng.operands[1];
              break;
            case 10:
              ObjectSystem.scriptEng.operands[0] &= ObjectSystem.scriptEng.operands[1];
              break;
            case 11:
              ObjectSystem.scriptEng.operands[0] |= ObjectSystem.scriptEng.operands[1];
              break;
            case 12:
              ObjectSystem.scriptEng.operands[0] ^= ObjectSystem.scriptEng.operands[1];
              break;
            case 13:
              ObjectSystem.scriptEng.operands[0] %= ObjectSystem.scriptEng.operands[1];
              break;
            case 14:
              ObjectSystem.scriptEng.operands[0] = -ObjectSystem.scriptEng.operands[0];
              break;
            case 15:
              ObjectSystem.scriptEng.checkResult = ObjectSystem.scriptEng.operands[0] != ObjectSystem.scriptEng.operands[1] ? 0 : 1;
              num3 = (sbyte) 0;
              break;
            case 16 /*0x10*/:
              ObjectSystem.scriptEng.checkResult = ObjectSystem.scriptEng.operands[0] <= ObjectSystem.scriptEng.operands[1] ? 0 : 1;
              num3 = (sbyte) 0;
              break;
            case 17:
              ObjectSystem.scriptEng.checkResult = ObjectSystem.scriptEng.operands[0] >= ObjectSystem.scriptEng.operands[1] ? 0 : 1;
              num3 = (sbyte) 0;
              break;
            case 18:
              ObjectSystem.scriptEng.checkResult = ObjectSystem.scriptEng.operands[0] == ObjectSystem.scriptEng.operands[1] ? 0 : 1;
              num3 = (sbyte) 0;
              break;
            case 19:
              if (ObjectSystem.scriptEng.operands[1] == ObjectSystem.scriptEng.operands[2])
              {
                ++ObjectSystem.jumpTableStackPos;
                ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
              }
              else
              {
                scriptCodePtr = num1;
                scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0]];
                ++ObjectSystem.jumpTableStackPos;
                ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
              }
              num3 = (sbyte) 0;
              break;
            case 20:
              if (ObjectSystem.scriptEng.operands[1] > ObjectSystem.scriptEng.operands[2])
              {
                ++ObjectSystem.jumpTableStackPos;
                ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
              }
              else
              {
                scriptCodePtr = num1;
                scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0]];
                ++ObjectSystem.jumpTableStackPos;
                ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
              }
              num3 = (sbyte) 0;
              break;
            case 21:
              if (ObjectSystem.scriptEng.operands[1] >= ObjectSystem.scriptEng.operands[2])
              {
                ++ObjectSystem.jumpTableStackPos;
                ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
              }
              else
              {
                scriptCodePtr = num1;
                scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0]];
                ++ObjectSystem.jumpTableStackPos;
                ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
              }
              num3 = (sbyte) 0;
              break;
            case 22:
              if (ObjectSystem.scriptEng.operands[1] < ObjectSystem.scriptEng.operands[2])
              {
                ++ObjectSystem.jumpTableStackPos;
                ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
              }
              else
              {
                scriptCodePtr = num1;
                scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0]];
                ++ObjectSystem.jumpTableStackPos;
                ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
              }
              num3 = (sbyte) 0;
              break;
            case 23:
              if (ObjectSystem.scriptEng.operands[1] <= ObjectSystem.scriptEng.operands[2])
              {
                ++ObjectSystem.jumpTableStackPos;
                ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
              }
              else
              {
                scriptCodePtr = num1;
                scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0]];
                ++ObjectSystem.jumpTableStackPos;
                ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
              }
              num3 = (sbyte) 0;
              break;
            case 24:
              if (ObjectSystem.scriptEng.operands[1] != ObjectSystem.scriptEng.operands[2])
              {
                ++ObjectSystem.jumpTableStackPos;
                ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
              }
              else
              {
                scriptCodePtr = num1;
                scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0]];
                ++ObjectSystem.jumpTableStackPos;
                ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
              }
              num3 = (sbyte) 0;
              break;
            case 25:
              num3 = (sbyte) 0;
              scriptCodePtr = num1;
              scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] + 1];
              --ObjectSystem.jumpTableStackPos;
              break;
            case 26:
              num3 = (sbyte) 0;
              --ObjectSystem.jumpTableStackPos;
              break;
            case 27:
              if (ObjectSystem.scriptEng.operands[1] == ObjectSystem.scriptEng.operands[2])
              {
                ++ObjectSystem.jumpTableStackPos;
                ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
              }
              else
              {
                scriptCodePtr = num1;
                scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0] + 1];
              }
              num3 = (sbyte) 0;
              break;
            case 28:
              if (ObjectSystem.scriptEng.operands[1] > ObjectSystem.scriptEng.operands[2])
              {
                ++ObjectSystem.jumpTableStackPos;
                ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
              }
              else
              {
                scriptCodePtr = num1;
                scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0] + 1];
              }
              num3 = (sbyte) 0;
              break;
            case 29:
              if (ObjectSystem.scriptEng.operands[1] >= ObjectSystem.scriptEng.operands[2])
              {
                ++ObjectSystem.jumpTableStackPos;
                ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
              }
              else
              {
                scriptCodePtr = num1;
                scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0] + 1];
              }
              num3 = (sbyte) 0;
              break;
            case 30:
              if (ObjectSystem.scriptEng.operands[1] < ObjectSystem.scriptEng.operands[2])
              {
                ++ObjectSystem.jumpTableStackPos;
                ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
              }
              else
              {
                scriptCodePtr = num1;
                scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0] + 1];
              }
              num3 = (sbyte) 0;
              break;
            case 31 /*0x1F*/:
              if (ObjectSystem.scriptEng.operands[1] <= ObjectSystem.scriptEng.operands[2])
              {
                ++ObjectSystem.jumpTableStackPos;
                ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
              }
              else
              {
                scriptCodePtr = num1;
                scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0] + 1];
              }
              num3 = (sbyte) 0;
              break;
            case 32 /*0x20*/:
              if (ObjectSystem.scriptEng.operands[1] != ObjectSystem.scriptEng.operands[2])
              {
                ++ObjectSystem.jumpTableStackPos;
                ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
              }
              else
              {
                scriptCodePtr = num1;
                scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0] + 1];
              }
              num3 = (sbyte) 0;
              break;
            case 33:
              num3 = (sbyte) 0;
              scriptCodePtr = num1;
              scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos]];
              --ObjectSystem.jumpTableStackPos;
              break;
            case 34:
              ++ObjectSystem.jumpTableStackPos;
              ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
              if (ObjectSystem.scriptEng.operands[1] >= ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0]] && ObjectSystem.scriptEng.operands[1] <= ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0] + 1])
              {
                scriptCodePtr = num1;
                scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0] + 4 + (ObjectSystem.scriptEng.operands[1] - ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0]])];
              }
              else
              {
                scriptCodePtr = num1;
                scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0] + 2];
              }
              num3 = (sbyte) 0;
              break;
            case 35:
              num3 = (sbyte) 0;
              scriptCodePtr = num1;
              scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] + 3];
              --ObjectSystem.jumpTableStackPos;
              break;
            case 36:
              num3 = (sbyte) 0;
              --ObjectSystem.jumpTableStackPos;
              break;
            case 37:
              ObjectSystem.scriptEng.operands[0] = ObjectSystem.rand.Next(0, ObjectSystem.scriptEng.operands[1]);
              break;
            case 38:
              ObjectSystem.scriptEng.sRegister = ObjectSystem.scriptEng.operands[1];
              if (ObjectSystem.scriptEng.sRegister < 0)
                ObjectSystem.scriptEng.sRegister = 512 /*0x0200*/ - ObjectSystem.scriptEng.sRegister;
              ObjectSystem.scriptEng.sRegister &= 511 /*0x01FF*/;
              ObjectSystem.scriptEng.operands[0] = GlobalAppDefinitions.SinValue512[ObjectSystem.scriptEng.sRegister];
              break;
            case 39:
              ObjectSystem.scriptEng.sRegister = ObjectSystem.scriptEng.operands[1];
              if (ObjectSystem.scriptEng.sRegister < 0)
                ObjectSystem.scriptEng.sRegister = 512 /*0x0200*/ - ObjectSystem.scriptEng.sRegister;
              ObjectSystem.scriptEng.sRegister &= 511 /*0x01FF*/;
              ObjectSystem.scriptEng.operands[0] = GlobalAppDefinitions.CosValue512[ObjectSystem.scriptEng.sRegister];
              break;
            case 40:
              ObjectSystem.scriptEng.sRegister = ObjectSystem.scriptEng.operands[1];
              if (ObjectSystem.scriptEng.sRegister < 0)
                ObjectSystem.scriptEng.sRegister = 256 /*0x0100*/ - ObjectSystem.scriptEng.sRegister;
              ObjectSystem.scriptEng.sRegister &= (int) byte.MaxValue;
              ObjectSystem.scriptEng.operands[0] = GlobalAppDefinitions.SinValue256[ObjectSystem.scriptEng.sRegister];
              break;
            case 41:
              ObjectSystem.scriptEng.sRegister = ObjectSystem.scriptEng.operands[1];
              if (ObjectSystem.scriptEng.sRegister < 0)
                ObjectSystem.scriptEng.sRegister = 256 /*0x0100*/ - ObjectSystem.scriptEng.sRegister;
              ObjectSystem.scriptEng.sRegister &= (int) byte.MaxValue;
              ObjectSystem.scriptEng.operands[0] = GlobalAppDefinitions.CosValue256[ObjectSystem.scriptEng.sRegister];
              break;
            case 42:
              ObjectSystem.scriptEng.sRegister = ObjectSystem.scriptEng.operands[1];
              if (ObjectSystem.scriptEng.sRegister < 0)
                ObjectSystem.scriptEng.sRegister = 512 /*0x0200*/ - ObjectSystem.scriptEng.sRegister;
              ObjectSystem.scriptEng.sRegister &= 511 /*0x01FF*/;
              ObjectSystem.scriptEng.operands[0] = (GlobalAppDefinitions.SinValue512[ObjectSystem.scriptEng.sRegister] >> ObjectSystem.scriptEng.operands[2]) + ObjectSystem.scriptEng.operands[3] - ObjectSystem.scriptEng.operands[4];
              break;
            case 43:
              ObjectSystem.scriptEng.sRegister = ObjectSystem.scriptEng.operands[1];
              if (ObjectSystem.scriptEng.sRegister < 0)
                ObjectSystem.scriptEng.sRegister = 512 /*0x0200*/ - ObjectSystem.scriptEng.sRegister;
              ObjectSystem.scriptEng.sRegister &= 511 /*0x01FF*/;
              ObjectSystem.scriptEng.operands[0] = (GlobalAppDefinitions.CosValue512[ObjectSystem.scriptEng.sRegister] >> ObjectSystem.scriptEng.operands[2]) + ObjectSystem.scriptEng.operands[3] - ObjectSystem.scriptEng.operands[4];
              break;
            case 44:
              ObjectSystem.scriptEng.operands[0] = (int) GlobalAppDefinitions.ArcTanLookup(ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2]);
              break;
            case 45:
              ObjectSystem.scriptEng.operands[0] = ObjectSystem.scriptEng.operands[1] * ObjectSystem.scriptEng.operands[3] + ObjectSystem.scriptEng.operands[2] * (256 /*0x0100*/ - ObjectSystem.scriptEng.operands[3]) >> 8;
              break;
            case 46:
              ObjectSystem.scriptEng.operands[0] = (ObjectSystem.scriptEng.operands[2] * ObjectSystem.scriptEng.operands[6] >> 8) + (ObjectSystem.scriptEng.operands[3] * (256 /*0x0100*/ - ObjectSystem.scriptEng.operands[6]) >> 8);
              ObjectSystem.scriptEng.operands[1] = (ObjectSystem.scriptEng.operands[4] * ObjectSystem.scriptEng.operands[6] >> 8) + (ObjectSystem.scriptEng.operands[5] * (256 /*0x0100*/ - ObjectSystem.scriptEng.operands[6]) >> 8);
              break;
            case 47:
              num3 = (sbyte) 0;
              ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum = GraphicsSystem.AddGraphicsFile(ObjectSystem.scriptText);
              break;
            case 48 /*0x30*/:
              num3 = (sbyte) 0;
              GraphicsSystem.RemoveGraphicsFile(ObjectSystem.scriptText, -1);
              break;
            case 49:
              num3 = (sbyte) 0;
              GraphicsSystem.DrawSprite((ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16 /*0x10*/) - StageSystem.xScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos >> 16 /*0x10*/) - StageSystem.yScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
              break;
            case 50:
              num3 = (sbyte) 0;
              GraphicsSystem.DrawSprite((ObjectSystem.scriptEng.operands[1] >> 16 /*0x10*/) - StageSystem.xScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, (ObjectSystem.scriptEng.operands[2] >> 16 /*0x10*/) - StageSystem.yScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
              break;
            case 51:
              num3 = (sbyte) 0;
              GraphicsSystem.DrawSprite(ObjectSystem.scriptEng.operands[1] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
              break;
            case 52:
              num3 = (sbyte) 0;
              GraphicsSystem.DrawTintRectangle(ObjectSystem.scriptEng.operands[0], ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
              break;
            case 53:
              num3 = (sbyte) 0;
              ObjectSystem.scriptEng.operands[7] = 10;
              if (ObjectSystem.scriptEng.operands[6] == 0)
              {
                ObjectSystem.scriptEng.operands[8] = ObjectSystem.scriptEng.operands[3] != 0 ? ObjectSystem.scriptEng.operands[3] * 10 : 10;
                while (ObjectSystem.scriptEng.operands[4] > 0)
                {
                  if (ObjectSystem.scriptEng.operands[8] >= ObjectSystem.scriptEng.operands[7])
                  {
                    ObjectSystem.scriptEng.sRegister = (ObjectSystem.scriptEng.operands[3] - ObjectSystem.scriptEng.operands[3] / ObjectSystem.scriptEng.operands[7] * ObjectSystem.scriptEng.operands[7]) / (ObjectSystem.scriptEng.operands[7] / 10);
                    ObjectSystem.scriptEng.sRegister += ObjectSystem.scriptEng.operands[0];
                    GraphicsSystem.DrawSprite(ObjectSystem.scriptEng.operands[1] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.sRegister].xPivot, ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.sRegister].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.sRegister].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.sRegister].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.sRegister].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.sRegister].top, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                  }
                  ObjectSystem.scriptEng.operands[1] -= ObjectSystem.scriptEng.operands[5];
                  ObjectSystem.scriptEng.operands[7] *= 10;
                  --ObjectSystem.scriptEng.operands[4];
                }
                break;
              }
              while (ObjectSystem.scriptEng.operands[4] > 0)
              {
                ObjectSystem.scriptEng.sRegister = (ObjectSystem.scriptEng.operands[3] - ObjectSystem.scriptEng.operands[3] / ObjectSystem.scriptEng.operands[7] * ObjectSystem.scriptEng.operands[7]) / (ObjectSystem.scriptEng.operands[7] / 10);
                ObjectSystem.scriptEng.sRegister += ObjectSystem.scriptEng.operands[0];
                GraphicsSystem.DrawSprite(ObjectSystem.scriptEng.operands[1] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.sRegister].xPivot, ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.sRegister].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.sRegister].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.sRegister].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.sRegister].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.sRegister].top, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                ObjectSystem.scriptEng.operands[1] -= ObjectSystem.scriptEng.operands[5];
                ObjectSystem.scriptEng.operands[7] *= 10;
                --ObjectSystem.scriptEng.operands[4];
              }
              break;
            case 54:
              num3 = (sbyte) 0;
              switch (ObjectSystem.scriptEng.operands[3])
              {
                case 1:
                  ObjectSystem.scriptEng.sRegister = 0;
                  if (ObjectSystem.scriptEng.operands[4] == 1 && StageSystem.titleCardText[ObjectSystem.scriptEng.sRegister] != char.MinValue)
                  {
                    ObjectSystem.scriptEng.operands[7] = (int) StageSystem.titleCardText[ObjectSystem.scriptEng.sRegister];
                    if (ObjectSystem.scriptEng.operands[7] == 32 /*0x20*/)
                      ObjectSystem.scriptEng.operands[7] = 0;
                    if (ObjectSystem.scriptEng.operands[7] == 45)
                      ObjectSystem.scriptEng.operands[7] = 0;
                    if (ObjectSystem.scriptEng.operands[7] > 47 && ObjectSystem.scriptEng.operands[7] < 58)
                      ObjectSystem.scriptEng.operands[7] -= 22;
                    if (ObjectSystem.scriptEng.operands[7] > 57 && ObjectSystem.scriptEng.operands[7] < 102)
                      ObjectSystem.scriptEng.operands[7] -= 65;
                    if (ObjectSystem.scriptEng.operands[7] > -1)
                    {
                      ObjectSystem.scriptEng.operands[7] += ObjectSystem.scriptEng.operands[0];
                      GraphicsSystem.DrawSprite(ObjectSystem.scriptEng.operands[1] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].xPivot, ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].top, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                      ObjectSystem.scriptEng.operands[1] += ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].xSize + ObjectSystem.scriptEng.operands[6];
                    }
                    else
                      ObjectSystem.scriptEng.operands[1] += ObjectSystem.scriptEng.operands[5] + ObjectSystem.scriptEng.operands[6];
                    ObjectSystem.scriptEng.operands[0] += 26;
                    ++ObjectSystem.scriptEng.sRegister;
                  }
                  for (; StageSystem.titleCardText[ObjectSystem.scriptEng.sRegister] != char.MinValue && StageSystem.titleCardText[ObjectSystem.scriptEng.sRegister] != '-'; ++ObjectSystem.scriptEng.sRegister)
                  {
                    ObjectSystem.scriptEng.operands[7] = (int) StageSystem.titleCardText[ObjectSystem.scriptEng.sRegister];
                    if (ObjectSystem.scriptEng.operands[7] == 32 /*0x20*/)
                      ObjectSystem.scriptEng.operands[7] = 0;
                    if (ObjectSystem.scriptEng.operands[7] == 45)
                      ObjectSystem.scriptEng.operands[7] = 0;
                    if (ObjectSystem.scriptEng.operands[7] > 47 && ObjectSystem.scriptEng.operands[7] < 58)
                      ObjectSystem.scriptEng.operands[7] -= 22;
                    if (ObjectSystem.scriptEng.operands[7] > 57 && ObjectSystem.scriptEng.operands[7] < 102)
                      ObjectSystem.scriptEng.operands[7] -= 65;
                    if (ObjectSystem.scriptEng.operands[7] > -1)
                    {
                      ObjectSystem.scriptEng.operands[7] += ObjectSystem.scriptEng.operands[0];
                      GraphicsSystem.DrawSprite(ObjectSystem.scriptEng.operands[1] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].xPivot, ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].top, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                      ObjectSystem.scriptEng.operands[1] += ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].xSize + ObjectSystem.scriptEng.operands[6];
                    }
                    else
                      ObjectSystem.scriptEng.operands[1] += ObjectSystem.scriptEng.operands[5] + ObjectSystem.scriptEng.operands[6];
                  }
                  break;
                case 2:
                  ObjectSystem.scriptEng.sRegister = (int) StageSystem.titleCardWord2;
                  if (ObjectSystem.scriptEng.operands[4] == 1 && StageSystem.titleCardText[ObjectSystem.scriptEng.sRegister] != char.MinValue)
                  {
                    ObjectSystem.scriptEng.operands[7] = (int) StageSystem.titleCardText[ObjectSystem.scriptEng.sRegister];
                    if (ObjectSystem.scriptEng.operands[7] == 32 /*0x20*/)
                      ObjectSystem.scriptEng.operands[7] = 0;
                    if (ObjectSystem.scriptEng.operands[7] == 45)
                      ObjectSystem.scriptEng.operands[7] = 0;
                    if (ObjectSystem.scriptEng.operands[7] > 47 && ObjectSystem.scriptEng.operands[7] < 58)
                      ObjectSystem.scriptEng.operands[7] -= 22;
                    if (ObjectSystem.scriptEng.operands[7] > 57 && ObjectSystem.scriptEng.operands[7] < 102)
                      ObjectSystem.scriptEng.operands[7] -= 65;
                    if (ObjectSystem.scriptEng.operands[7] > -1)
                    {
                      ObjectSystem.scriptEng.operands[7] += ObjectSystem.scriptEng.operands[0];
                      GraphicsSystem.DrawSprite(ObjectSystem.scriptEng.operands[1] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].xPivot, ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].top, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                      ObjectSystem.scriptEng.operands[1] += ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].xSize + ObjectSystem.scriptEng.operands[6];
                    }
                    else
                      ObjectSystem.scriptEng.operands[1] += ObjectSystem.scriptEng.operands[5] + ObjectSystem.scriptEng.operands[6];
                    ObjectSystem.scriptEng.operands[0] += 26;
                    ++ObjectSystem.scriptEng.sRegister;
                  }
                  for (; StageSystem.titleCardText[ObjectSystem.scriptEng.sRegister] != char.MinValue; ++ObjectSystem.scriptEng.sRegister)
                  {
                    ObjectSystem.scriptEng.operands[7] = (int) StageSystem.titleCardText[ObjectSystem.scriptEng.sRegister];
                    if (ObjectSystem.scriptEng.operands[7] == 32 /*0x20*/)
                      ObjectSystem.scriptEng.operands[7] = 0;
                    if (ObjectSystem.scriptEng.operands[7] == 45)
                      ObjectSystem.scriptEng.operands[7] = 0;
                    if (ObjectSystem.scriptEng.operands[7] > 47 && ObjectSystem.scriptEng.operands[7] < 58)
                      ObjectSystem.scriptEng.operands[7] -= 22;
                    if (ObjectSystem.scriptEng.operands[7] > 57 && ObjectSystem.scriptEng.operands[7] < 102)
                      ObjectSystem.scriptEng.operands[7] -= 65;
                    if (ObjectSystem.scriptEng.operands[7] > -1)
                    {
                      ObjectSystem.scriptEng.operands[7] += ObjectSystem.scriptEng.operands[0];
                      GraphicsSystem.DrawSprite(ObjectSystem.scriptEng.operands[1] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].xPivot, ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].top, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                      ObjectSystem.scriptEng.operands[1] += ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].xSize + ObjectSystem.scriptEng.operands[6];
                    }
                    else
                      ObjectSystem.scriptEng.operands[1] += ObjectSystem.scriptEng.operands[5] + ObjectSystem.scriptEng.operands[6];
                  }
                  break;
              }
              break;
            case 55:
              num3 = (sbyte) 0;
              TextSystem.textMenuSurfaceNo = (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum;
              TextSystem.DrawTextMenu(StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]], ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2]);
              break;
            case 56:
              num3 = (sbyte) 0;
              if (scriptSub == 3 && ObjectSystem.scriptFramesNo < 4096 /*0x1000*/)
              {
                ObjectSystem.scriptFrames[ObjectSystem.scriptFramesNo].xPivot = ObjectSystem.scriptEng.operands[0];
                ObjectSystem.scriptFrames[ObjectSystem.scriptFramesNo].yPivot = ObjectSystem.scriptEng.operands[1];
                ObjectSystem.scriptFrames[ObjectSystem.scriptFramesNo].xSize = ObjectSystem.scriptEng.operands[2];
                ObjectSystem.scriptFrames[ObjectSystem.scriptFramesNo].ySize = ObjectSystem.scriptEng.operands[3];
                ObjectSystem.scriptFrames[ObjectSystem.scriptFramesNo].left = ObjectSystem.scriptEng.operands[4];
                ObjectSystem.scriptFrames[ObjectSystem.scriptFramesNo].top = ObjectSystem.scriptEng.operands[5];
                ++ObjectSystem.scriptFramesNo;
                break;
              }
              break;
            case 57:
              num3 = (sbyte) 0;
              break;
            case 58:
              num3 = (sbyte) 0;
              GraphicsSystem.LoadPalette(ObjectSystem.scriptText, ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3], ObjectSystem.scriptEng.operands[4]);
              break;
            case 59:
              num3 = (sbyte) 0;
              GraphicsSystem.RotatePalette((byte) ObjectSystem.scriptEng.operands[0], (byte) ObjectSystem.scriptEng.operands[1], (byte) ObjectSystem.scriptEng.operands[2]);
              break;
            case 60:
              num3 = (sbyte) 0;
              GraphicsSystem.SetFade((byte) ObjectSystem.scriptEng.operands[0], (byte) ObjectSystem.scriptEng.operands[1], (byte) ObjectSystem.scriptEng.operands[2], (ushort) ObjectSystem.scriptEng.operands[3]);
              break;
            case 61:
              num3 = (sbyte) 0;
              GraphicsSystem.SetActivePalette((byte) ObjectSystem.scriptEng.operands[0], ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2]);
              break;
            case 62:
              GraphicsSystem.SetLimitedFade((byte) ObjectSystem.scriptEng.operands[0], (byte) ObjectSystem.scriptEng.operands[1], (byte) ObjectSystem.scriptEng.operands[2], (byte) ObjectSystem.scriptEng.operands[3], (ushort) ObjectSystem.scriptEng.operands[4], ObjectSystem.scriptEng.operands[5], ObjectSystem.scriptEng.operands[6]);
              break;
            case 63 /*0x3F*/:
              num3 = (sbyte) 0;
              GraphicsSystem.CopyPalette((byte) ObjectSystem.scriptEng.operands[0], (byte) ObjectSystem.scriptEng.operands[1]);
              break;
            case 64 /*0x40*/:
              num3 = (sbyte) 0;
              GraphicsSystem.ClearScreen((byte) ObjectSystem.scriptEng.operands[0]);
              break;
            case 65:
              num3 = (sbyte) 0;
              switch (ObjectSystem.scriptEng.operands[1])
              {
                case 0:
                  GraphicsSystem.DrawScaledSprite(ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, (ObjectSystem.scriptEng.operands[2] >> 16 /*0x10*/) - StageSystem.xScrollOffset, (ObjectSystem.scriptEng.operands[3] >> 16 /*0x10*/) - StageSystem.yScrollOffset, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].scale, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].scale, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                  break;
                case 1:
                  GraphicsSystem.DrawRotatedSprite(ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, (ObjectSystem.scriptEng.operands[2] >> 16 /*0x10*/) - StageSystem.xScrollOffset, (ObjectSystem.scriptEng.operands[3] >> 16 /*0x10*/) - StageSystem.yScrollOffset, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].rotation, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                  break;
                case 2:
                  GraphicsSystem.DrawRotoZoomSprite(ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, (ObjectSystem.scriptEng.operands[2] >> 16 /*0x10*/) - StageSystem.xScrollOffset, (ObjectSystem.scriptEng.operands[3] >> 16 /*0x10*/) - StageSystem.yScrollOffset, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].rotation, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].scale, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                  break;
                case 3:
                  switch (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].inkEffect)
                  {
                    case 0:
                      GraphicsSystem.DrawSprite((ObjectSystem.scriptEng.operands[2] >> 16 /*0x10*/) - StageSystem.xScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, (ObjectSystem.scriptEng.operands[3] >> 16 /*0x10*/) - StageSystem.yScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                      break;
                    case 1:
                      GraphicsSystem.DrawBlendedSprite((ObjectSystem.scriptEng.operands[2] >> 16 /*0x10*/) - StageSystem.xScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, (ObjectSystem.scriptEng.operands[3] >> 16 /*0x10*/) - StageSystem.yScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                      break;
                    case 2:
                      GraphicsSystem.DrawAlphaBlendedSprite((ObjectSystem.scriptEng.operands[2] >> 16 /*0x10*/) - StageSystem.xScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, (ObjectSystem.scriptEng.operands[3] >> 16 /*0x10*/) - StageSystem.yScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].alpha, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                      break;
                    case 3:
                      GraphicsSystem.DrawAdditiveBlendedSprite((ObjectSystem.scriptEng.operands[2] >> 16 /*0x10*/) - StageSystem.xScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, (ObjectSystem.scriptEng.operands[3] >> 16 /*0x10*/) - StageSystem.yScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].alpha, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                      break;
                    case 4:
                      GraphicsSystem.DrawSubtractiveBlendedSprite((ObjectSystem.scriptEng.operands[2] >> 16 /*0x10*/) - StageSystem.xScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, (ObjectSystem.scriptEng.operands[3] >> 16 /*0x10*/) - StageSystem.yScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].alpha, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                      break;
                  }
                  break;
                case 4:
                  if (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].inkEffect != (byte) 2)
                  {
                    GraphicsSystem.DrawScaledSprite(ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, (ObjectSystem.scriptEng.operands[2] >> 16 /*0x10*/) - StageSystem.xScrollOffset, (ObjectSystem.scriptEng.operands[3] >> 16 /*0x10*/) - StageSystem.yScrollOffset, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].scale, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].scale, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                    break;
                  }
                  GraphicsSystem.DrawScaledTintMask(ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, (ObjectSystem.scriptEng.operands[2] >> 16 /*0x10*/) - StageSystem.xScrollOffset, (ObjectSystem.scriptEng.operands[3] >> 16 /*0x10*/) - StageSystem.yScrollOffset, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].scale, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].scale, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                  break;
                case 5:
                  switch (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction)
                  {
                    case 0:
                      GraphicsSystem.DrawSpriteFlipped((ObjectSystem.scriptEng.operands[2] >> 16 /*0x10*/) - StageSystem.xScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, (ObjectSystem.scriptEng.operands[3] >> 16 /*0x10*/) - StageSystem.yScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                      break;
                    case 1:
                      GraphicsSystem.DrawSpriteFlipped((ObjectSystem.scriptEng.operands[2] >> 16 /*0x10*/) - StageSystem.xScrollOffset - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, (ObjectSystem.scriptEng.operands[3] >> 16 /*0x10*/) - StageSystem.yScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                      break;
                    case 2:
                      GraphicsSystem.DrawSpriteFlipped((ObjectSystem.scriptEng.operands[2] >> 16 /*0x10*/) - StageSystem.xScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, (ObjectSystem.scriptEng.operands[3] >> 16 /*0x10*/) - StageSystem.yScrollOffset - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                      break;
                    case 3:
                      GraphicsSystem.DrawSpriteFlipped((ObjectSystem.scriptEng.operands[2] >> 16 /*0x10*/) - StageSystem.xScrollOffset - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, (ObjectSystem.scriptEng.operands[3] >> 16 /*0x10*/) - StageSystem.yScrollOffset - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                      break;
                  }
                  break;
              }
              break;
            case 66:
              num3 = (sbyte) 0;
              switch (ObjectSystem.scriptEng.operands[1])
              {
                case 0:
                  GraphicsSystem.DrawScaledSprite(ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3], -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].scale, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].scale, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                  break;
                case 1:
                  GraphicsSystem.DrawRotatedSprite(ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3], -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].rotation, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                  break;
                case 2:
                  GraphicsSystem.DrawRotoZoomSprite(ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3], -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].rotation, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].scale, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                  break;
                case 3:
                  switch (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].inkEffect)
                  {
                    case 0:
                      GraphicsSystem.DrawSprite(ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, ObjectSystem.scriptEng.operands[3] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                      break;
                    case 1:
                      GraphicsSystem.DrawBlendedSprite(ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, ObjectSystem.scriptEng.operands[3] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                      break;
                    case 2:
                      GraphicsSystem.DrawAlphaBlendedSprite(ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, ObjectSystem.scriptEng.operands[3] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].alpha, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                      break;
                    case 3:
                      GraphicsSystem.DrawAdditiveBlendedSprite(ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, ObjectSystem.scriptEng.operands[3] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].alpha, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                      break;
                    case 4:
                      GraphicsSystem.DrawSubtractiveBlendedSprite(ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, ObjectSystem.scriptEng.operands[3] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].alpha, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                      break;
                  }
                  break;
                case 4:
                  if (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].inkEffect != (byte) 2)
                  {
                    GraphicsSystem.DrawScaledSprite(ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3], -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].scale, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].scale, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                    break;
                  }
                  GraphicsSystem.DrawScaledTintMask(ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3], -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].scale, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].scale, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                  break;
                case 5:
                  switch (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction)
                  {
                    case 0:
                      GraphicsSystem.DrawSpriteFlipped(ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, ObjectSystem.scriptEng.operands[3] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                      break;
                    case 1:
                      GraphicsSystem.DrawSpriteFlipped(ObjectSystem.scriptEng.operands[2] - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, ObjectSystem.scriptEng.operands[3] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                      break;
                    case 2:
                      GraphicsSystem.DrawSpriteFlipped(ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, ObjectSystem.scriptEng.operands[3] - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                      break;
                    case 3:
                      GraphicsSystem.DrawSpriteFlipped(ObjectSystem.scriptEng.operands[2] - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, ObjectSystem.scriptEng.operands[3] - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
                      break;
                  }
                  break;
              }
              break;
            case 67:
              num3 = (sbyte) 0;
              ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].animationFile = AnimationSystem.AddAnimationFile(ObjectSystem.scriptText);
              break;
            case 68:
              num3 = (sbyte) 0;
              TextSystem.SetupTextMenu(StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]], ObjectSystem.scriptEng.operands[1]);
              StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]].numSelections = (byte) ObjectSystem.scriptEng.operands[2];
              StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]].alignment = (byte) ObjectSystem.scriptEng.operands[3];
              break;
            case 69:
              num3 = (sbyte) 0;
              StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]].entryHighlight[(int) StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]].numRows] = (byte) ObjectSystem.scriptEng.operands[2];
              TextSystem.AddTextMenuEntry(StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]], ObjectSystem.scriptText);
              break;
            case 70:
              num3 = (sbyte) 0;
              TextSystem.EditTextMenuEntry(StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]], ObjectSystem.scriptText, ObjectSystem.scriptEng.operands[2]);
              StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]].entryHighlight[ObjectSystem.scriptEng.operands[2]] = (byte) ObjectSystem.scriptEng.operands[3];
              break;
            case 71:
              num3 = (sbyte) 0;
              StageSystem.stageMode = (byte) 0;
              break;
            case 72:
              num3 = (sbyte) 0;
              GraphicsSystem.DrawRectangle(ObjectSystem.scriptEng.operands[0], ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3], ObjectSystem.scriptEng.operands[4], ObjectSystem.scriptEng.operands[5], ObjectSystem.scriptEng.operands[6], ObjectSystem.scriptEng.operands[7]);
              break;
            case 73:
              num3 = (sbyte) 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].type = (byte) ObjectSystem.scriptEng.operands[1];
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].propertyValue = (byte) ObjectSystem.scriptEng.operands[2];
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].xPos = ObjectSystem.scriptEng.operands[3];
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].yPos = ObjectSystem.scriptEng.operands[4];
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].direction = (byte) 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].frame = (byte) 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].priority = (byte) 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].rotation = 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].state = (byte) 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].drawOrder = (byte) 3;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].scale = 512 /*0x0200*/;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].inkEffect = (byte) 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].value[0] = 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].value[1] = 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].value[2] = 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].value[3] = 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].value[4] = 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].value[5] = 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].value[6] = 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].value[7] = 0;
              break;
            case 74:
              num3 = (sbyte) 0;
              switch (ObjectSystem.scriptEng.operands[0])
              {
                case 0:
                  ObjectSystem.scriptEng.operands[5] = ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16 /*0x10*/;
                  ObjectSystem.scriptEng.operands[6] = ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos >> 16 /*0x10*/;
                  ObjectSystem.BasicCollision(ObjectSystem.scriptEng.operands[1] + ObjectSystem.scriptEng.operands[5], ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptEng.operands[6], ObjectSystem.scriptEng.operands[3] + ObjectSystem.scriptEng.operands[5], ObjectSystem.scriptEng.operands[4] + ObjectSystem.scriptEng.operands[6]);
                  break;
                case 1:
                case 2:
                  ObjectSystem.BoxCollision((ObjectSystem.scriptEng.operands[1] << 16 /*0x10*/) + ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos, (ObjectSystem.scriptEng.operands[2] << 16 /*0x10*/) + ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos, (ObjectSystem.scriptEng.operands[3] << 16 /*0x10*/) + ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos, (ObjectSystem.scriptEng.operands[4] << 16 /*0x10*/) + ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos);
                  break;
                case 3:
                  ObjectSystem.PlatformCollision((ObjectSystem.scriptEng.operands[1] << 16 /*0x10*/) + ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos, (ObjectSystem.scriptEng.operands[2] << 16 /*0x10*/) + ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos, (ObjectSystem.scriptEng.operands[3] << 16 /*0x10*/) + ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos, (ObjectSystem.scriptEng.operands[4] << 16 /*0x10*/) + ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos);
                  break;
              }
              break;
            case 75:
              num3 = (sbyte) 0;
              if (ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].type > (byte) 0)
              {
                ++ObjectSystem.scriptEng.arrayPosition[2];
                if (ObjectSystem.scriptEng.arrayPosition[2] == 1184)
                  ObjectSystem.scriptEng.arrayPosition[2] = 1056;
              }
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].type = (byte) ObjectSystem.scriptEng.operands[0];
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].propertyValue = (byte) ObjectSystem.scriptEng.operands[1];
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].xPos = ObjectSystem.scriptEng.operands[2];
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].yPos = ObjectSystem.scriptEng.operands[3];
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].direction = (byte) 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].frame = (byte) 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].priority = (byte) 1;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].rotation = 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].state = (byte) 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].drawOrder = (byte) 3;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].scale = 512 /*0x0200*/;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].inkEffect = (byte) 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].alpha = (byte) 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].animation = (byte) 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].prevAnimation = (byte) 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].animationSpeed = 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].animationTimer = 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].value[0] = 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].value[1] = 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].value[2] = 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].value[3] = 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].value[4] = 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].value[5] = 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].value[6] = 0;
              ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].value[7] = 0;
              break;
            case 76:
              num3 = (sbyte) 0;
              PlayerSystem.playerList[ObjectSystem.scriptEng.operands[0]].animationFile = ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[1]].type].animationFile;
              PlayerSystem.playerList[ObjectSystem.scriptEng.operands[0]].objectPtr = ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[1]];
              PlayerSystem.playerList[ObjectSystem.scriptEng.operands[0]].objectNum = ObjectSystem.scriptEng.operands[1];
              break;
            case 77:
              num3 = (sbyte) 0;
              if (PlayerSystem.playerList[ObjectSystem.playerNum].tileCollisions == (byte) 1)
              {
                PlayerSystem.ProcessPlayerTileCollisions(PlayerSystem.playerList[ObjectSystem.playerNum]);
                break;
              }
              PlayerSystem.playerList[ObjectSystem.playerNum].xPos += PlayerSystem.playerList[ObjectSystem.playerNum].xVelocity;
              PlayerSystem.playerList[ObjectSystem.playerNum].yPos += PlayerSystem.playerList[ObjectSystem.playerNum].yVelocity;
              break;
            case 78:
              num3 = (sbyte) 0;
              PlayerSystem.ProcessPlayerControl(PlayerSystem.playerList[ObjectSystem.playerNum]);
              break;
            case 79:
              AnimationSystem.ProcessObjectAnimation(AnimationSystem.animationList[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].animationFile.aniListOffset + (int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].animation], ObjectSystem.objectEntityList[ObjectSystem.objectLoop]);
              num3 = (sbyte) 0;
              break;
            case 80 /*0x50*/:
              num3 = (sbyte) 0;
              AnimationSystem.DrawObjectAnimation(AnimationSystem.animationList[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].animationFile.aniListOffset + (int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].animation], ObjectSystem.objectEntityList[ObjectSystem.objectLoop], (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16 /*0x10*/) - StageSystem.xScrollOffset, (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos >> 16 /*0x10*/) - StageSystem.yScrollOffset);
              break;
            case 81:
              num3 = (sbyte) 0;
              if (PlayerSystem.playerList[ObjectSystem.playerNum].visible == (byte) 1)
              {
                if ((int) StageSystem.cameraEnabled == ObjectSystem.playerNum)
                {
                  AnimationSystem.DrawObjectAnimation(AnimationSystem.animationList[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].animationFile.aniListOffset + (int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].animation], ObjectSystem.objectEntityList[ObjectSystem.objectLoop], PlayerSystem.playerList[ObjectSystem.playerNum].screenXPos, PlayerSystem.playerList[ObjectSystem.playerNum].screenYPos);
                  break;
                }
                AnimationSystem.DrawObjectAnimation(AnimationSystem.animationList[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].animationFile.aniListOffset + (int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].animation], ObjectSystem.objectEntityList[ObjectSystem.objectLoop], (PlayerSystem.playerList[ObjectSystem.playerNum].xPos >> 16 /*0x10*/) - StageSystem.xScrollOffset, (PlayerSystem.playerList[ObjectSystem.playerNum].yPos >> 16 /*0x10*/) - StageSystem.yScrollOffset);
                break;
              }
              break;
            case 82:
              num3 = (sbyte) 0;
              if (ObjectSystem.scriptEng.operands[2] > 1)
              {
                AudioPlayback.SetMusicTrack(ObjectSystem.scriptText, ObjectSystem.scriptEng.operands[1], (byte) 1, (uint) ObjectSystem.scriptEng.operands[2]);
                break;
              }
              AudioPlayback.SetMusicTrack(ObjectSystem.scriptText, ObjectSystem.scriptEng.operands[1], (byte) ObjectSystem.scriptEng.operands[2], 0U);
              break;
            case 83:
              num3 = (sbyte) 0;
              AudioPlayback.PlayMusic(ObjectSystem.scriptEng.operands[0]);
              break;
            case 84:
              num3 = (sbyte) 0;
              AudioPlayback.StopMusic();
              break;
            case 85:
              num3 = (sbyte) 0;
              AudioPlayback.PlaySfx(ObjectSystem.scriptEng.operands[0], (byte) ObjectSystem.scriptEng.operands[1]);
              break;
            case 86:
              num3 = (sbyte) 0;
              AudioPlayback.StopSfx(ObjectSystem.scriptEng.operands[0]);
              break;
            case 87:
              num3 = (sbyte) 0;
              AudioPlayback.SetSfxAttributes(ObjectSystem.scriptEng.operands[0], ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2]);
              break;
            case 88:
              num3 = (sbyte) 0;
              switch (ObjectSystem.scriptEng.operands[0])
              {
                case 0:
                  ObjectSystem.ObjectFloorCollision(ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
                  break;
                case 1:
                  ObjectSystem.ObjectLWallCollision(ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
                  break;
                case 2:
                  ObjectSystem.ObjectRWallCollision(ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
                  break;
                case 3:
                  ObjectSystem.ObjectRoofCollision(ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
                  break;
              }
              break;
            case 89:
              num3 = (sbyte) 0;
              switch (ObjectSystem.scriptEng.operands[0])
              {
                case 0:
                  ObjectSystem.ObjectFloorGrip(ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
                  break;
                case 1:
                  ObjectSystem.ObjectLWallGrip(ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
                  break;
                case 2:
                  ObjectSystem.ObjectRWallGrip(ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
                  break;
                case 3:
                  ObjectSystem.ObjectRoofGrip(ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
                  break;
              }
              break;
            case 90:
              num3 = (sbyte) 0;
              AudioPlayback.PauseSound();
              EngineCallbacks.PlayVideoFile(ObjectSystem.scriptText);
              AudioPlayback.ResumeSound();
              break;
            case 91:
              num3 = (sbyte) 0;
              break;
            case 92:
              num3 = (sbyte) 0;
              AudioPlayback.PlaySfx(ObjectSystem.scriptEng.operands[0] + AudioPlayback.numGlobalSFX, (byte) ObjectSystem.scriptEng.operands[1]);
              break;
            case 93:
              num3 = (sbyte) 0;
              AudioPlayback.StopSfx(ObjectSystem.scriptEng.operands[0] + AudioPlayback.numGlobalSFX);
              break;
            case 94:
              ObjectSystem.scriptEng.operands[0] = ~ObjectSystem.scriptEng.operands[0];
              break;
            case 95:
              num3 = (sbyte) 0;
              Scene3D.TransformVertexBuffer();
              Scene3D.Sort3DDrawList();
              Scene3D.Draw3DScene((int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
              break;
            case 96 /*0x60*/:
              num3 = (sbyte) 0;
              switch (ObjectSystem.scriptEng.operands[0])
              {
                case 0:
                  Scene3D.SetIdentityMatrix(ref Scene3D.matWorld);
                  break;
                case 1:
                  Scene3D.SetIdentityMatrix(ref Scene3D.matView);
                  break;
                case 2:
                  Scene3D.SetIdentityMatrix(ref Scene3D.matTemp);
                  break;
              }
              break;
            case 97:
              num3 = (sbyte) 0;
              switch (ObjectSystem.scriptEng.operands[0])
              {
                case 0:
                  switch (ObjectSystem.scriptEng.operands[1])
                  {
                    case 0:
                      Scene3D.MatrixMultiply(ref Scene3D.matWorld, ref Scene3D.matWorld);
                      break;
                    case 1:
                      Scene3D.MatrixMultiply(ref Scene3D.matWorld, ref Scene3D.matView);
                      break;
                    case 2:
                      Scene3D.MatrixMultiply(ref Scene3D.matWorld, ref Scene3D.matTemp);
                      break;
                  }
                  break;
                case 1:
                  switch (ObjectSystem.scriptEng.operands[1])
                  {
                    case 0:
                      Scene3D.MatrixMultiply(ref Scene3D.matView, ref Scene3D.matWorld);
                      break;
                    case 1:
                      Scene3D.MatrixMultiply(ref Scene3D.matView, ref Scene3D.matView);
                      break;
                    case 2:
                      Scene3D.MatrixMultiply(ref Scene3D.matView, ref Scene3D.matTemp);
                      break;
                  }
                  break;
                case 2:
                  switch (ObjectSystem.scriptEng.operands[1])
                  {
                    case 0:
                      Scene3D.MatrixMultiply(ref Scene3D.matTemp, ref Scene3D.matWorld);
                      break;
                    case 1:
                      Scene3D.MatrixMultiply(ref Scene3D.matTemp, ref Scene3D.matView);
                      break;
                    case 2:
                      Scene3D.MatrixMultiply(ref Scene3D.matTemp, ref Scene3D.matTemp);
                      break;
                  }
                  break;
              }
              break;
            case 98:
              num3 = (sbyte) 0;
              switch (ObjectSystem.scriptEng.operands[0])
              {
                case 0:
                  Scene3D.MatrixTranslateXYZ(ref Scene3D.matWorld, ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
                  break;
                case 1:
                  Scene3D.MatrixTranslateXYZ(ref Scene3D.matView, ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
                  break;
                case 2:
                  Scene3D.MatrixTranslateXYZ(ref Scene3D.matTemp, ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
                  break;
              }
              break;
            case 99:
              num3 = (sbyte) 0;
              switch (ObjectSystem.scriptEng.operands[0])
              {
                case 0:
                  Scene3D.MatrixScaleXYZ(ref Scene3D.matWorld, ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
                  break;
                case 1:
                  Scene3D.MatrixScaleXYZ(ref Scene3D.matView, ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
                  break;
                case 2:
                  Scene3D.MatrixScaleXYZ(ref Scene3D.matTemp, ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
                  break;
              }
              break;
            case 100:
              num3 = (sbyte) 0;
              switch (ObjectSystem.scriptEng.operands[0])
              {
                case 0:
                  Scene3D.MatrixRotateX(ref Scene3D.matWorld, ObjectSystem.scriptEng.operands[1]);
                  break;
                case 1:
                  Scene3D.MatrixRotateX(ref Scene3D.matView, ObjectSystem.scriptEng.operands[1]);
                  break;
                case 2:
                  Scene3D.MatrixRotateX(ref Scene3D.matTemp, ObjectSystem.scriptEng.operands[1]);
                  break;
              }
              break;
            case 101:
              num3 = (sbyte) 0;
              switch (ObjectSystem.scriptEng.operands[0])
              {
                case 0:
                  Scene3D.MatrixRotateY(ref Scene3D.matWorld, ObjectSystem.scriptEng.operands[1]);
                  break;
                case 1:
                  Scene3D.MatrixRotateY(ref Scene3D.matView, ObjectSystem.scriptEng.operands[1]);
                  break;
                case 2:
                  Scene3D.MatrixRotateY(ref Scene3D.matTemp, ObjectSystem.scriptEng.operands[1]);
                  break;
              }
              break;
            case 102:
              num3 = (sbyte) 0;
              switch (ObjectSystem.scriptEng.operands[0])
              {
                case 0:
                  Scene3D.MatrixRotateZ(ref Scene3D.matWorld, ObjectSystem.scriptEng.operands[1]);
                  break;
                case 1:
                  Scene3D.MatrixRotateZ(ref Scene3D.matView, ObjectSystem.scriptEng.operands[1]);
                  break;
                case 2:
                  Scene3D.MatrixRotateZ(ref Scene3D.matTemp, ObjectSystem.scriptEng.operands[1]);
                  break;
              }
              break;
            case 103:
              num3 = (sbyte) 0;
              switch (ObjectSystem.scriptEng.operands[0])
              {
                case 0:
                  Scene3D.MatrixRotateXYZ(ref Scene3D.matWorld, ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
                  break;
                case 1:
                  Scene3D.MatrixRotateXYZ(ref Scene3D.matView, ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
                  break;
                case 2:
                  Scene3D.MatrixRotateXYZ(ref Scene3D.matTemp, ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
                  break;
              }
              break;
            case 104:
              num3 = (sbyte) 0;
              switch (ObjectSystem.scriptEng.operands[0])
              {
                case 0:
                  Scene3D.TransformVertices(ref Scene3D.matWorld, ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2]);
                  break;
                case 1:
                  Scene3D.TransformVertices(ref Scene3D.matView, ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2]);
                  break;
                case 2:
                  Scene3D.TransformVertices(ref Scene3D.matTemp, ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2]);
                  break;
              }
              break;
            case 105:
              num3 = (sbyte) 0;
              ObjectSystem.functionStack[ObjectSystem.functionStackPos] = scriptCodePtr;
              ++ObjectSystem.functionStackPos;
              ObjectSystem.functionStack[ObjectSystem.functionStackPos] = jumpTablePtr;
              ++ObjectSystem.functionStackPos;
              ObjectSystem.functionStack[ObjectSystem.functionStackPos] = num1;
              ++ObjectSystem.functionStackPos;
              scriptCodePtr = ObjectSystem.functionScriptList[ObjectSystem.scriptEng.operands[0]].mainScript;
              num1 = scriptCodePtr;
              jumpTablePtr = ObjectSystem.functionScriptList[ObjectSystem.scriptEng.operands[0]].mainJumpTable;
              break;
            case 106:
              num3 = (sbyte) 0;
              --ObjectSystem.functionStackPos;
              num1 = ObjectSystem.functionStack[ObjectSystem.functionStackPos];
              --ObjectSystem.functionStackPos;
              jumpTablePtr = ObjectSystem.functionStack[ObjectSystem.functionStackPos];
              --ObjectSystem.functionStackPos;
              scriptCodePtr = ObjectSystem.functionStack[ObjectSystem.functionStackPos];
              break;
            case 107:
              num3 = (sbyte) 0;
              StageSystem.SetLayerDeformation(ObjectSystem.scriptEng.operands[0], ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3], ObjectSystem.scriptEng.operands[4], ObjectSystem.scriptEng.operands[5]);
              break;
            case 108:
              sbyte index5 = 0;
              ObjectSystem.scriptEng.checkResult = -1;
              for (; (int) index5 < StageSystem.gKeyDown.touches; ++index5)
              {
                if (StageSystem.gKeyDown.touchDown[(int) index5] == (byte) 1 && StageSystem.gKeyDown.touchX[(int) index5] > ObjectSystem.scriptEng.operands[0] && StageSystem.gKeyDown.touchX[(int) index5] < ObjectSystem.scriptEng.operands[2] && StageSystem.gKeyDown.touchY[(int) index5] > ObjectSystem.scriptEng.operands[1] && StageSystem.gKeyDown.touchY[(int) index5] < ObjectSystem.scriptEng.operands[3])
                  ObjectSystem.scriptEng.checkResult = (int) index5;
              }
              num3 = (sbyte) 0;
              break;
            case 109:
              ObjectSystem.scriptEng.operands[0] = ObjectSystem.scriptEng.operands[2] <= -1 || ObjectSystem.scriptEng.operands[3] <= -1 ? 0 : (int) StageSystem.stageLayouts[ObjectSystem.scriptEng.operands[1]].tileMap[ObjectSystem.scriptEng.operands[2] + (ObjectSystem.scriptEng.operands[3] << 8)];
              break;
            case 110:
              if (ObjectSystem.scriptEng.operands[2] > -1 && ObjectSystem.scriptEng.operands[3] > -1)
              {
                StageSystem.stageLayouts[ObjectSystem.scriptEng.operands[1]].tileMap[ObjectSystem.scriptEng.operands[2] + (ObjectSystem.scriptEng.operands[3] << 8)] = (ushort) ObjectSystem.scriptEng.operands[0];
                break;
              }
              break;
            case 111:
              ObjectSystem.scriptEng.operands[0] = (ObjectSystem.scriptEng.operands[1] & 1 << ObjectSystem.scriptEng.operands[2]) >> ObjectSystem.scriptEng.operands[2];
              break;
            case 112 /*0x70*/:
              if (ObjectSystem.scriptEng.operands[2] > 0)
              {
                ObjectSystem.scriptEng.operands[0] |= 1 << ObjectSystem.scriptEng.operands[1];
                break;
              }
              ObjectSystem.scriptEng.operands[0] &= ~(1 << ObjectSystem.scriptEng.operands[1]);
              break;
            case 113:
              num3 = (sbyte) 0;
              AudioPlayback.PauseSound();
              break;
            case 114:
              num3 = (sbyte) 0;
              AudioPlayback.ResumeSound();
              break;
            case 115:
              num3 = (sbyte) 0;
              ObjectSystem.objectDrawOrderList[ObjectSystem.scriptEng.operands[0]].listSize = 0;
              break;
            case 116:
              num3 = (sbyte) 0;
              ObjectSystem.objectDrawOrderList[ObjectSystem.scriptEng.operands[0]].entityRef[ObjectSystem.objectDrawOrderList[ObjectSystem.scriptEng.operands[0]].listSize] = ObjectSystem.scriptEng.operands[1];
              ++ObjectSystem.objectDrawOrderList[ObjectSystem.scriptEng.operands[0]].listSize;
              break;
            case 117:
              ObjectSystem.scriptEng.operands[0] = ObjectSystem.objectDrawOrderList[ObjectSystem.scriptEng.operands[1]].entityRef[ObjectSystem.scriptEng.operands[2]];
              break;
            case 118:
              num3 = (sbyte) 0;
              ObjectSystem.objectDrawOrderList[ObjectSystem.scriptEng.operands[1]].entityRef[ObjectSystem.scriptEng.operands[2]] = ObjectSystem.scriptEng.operands[0];
              break;
            case 119:
              ObjectSystem.scriptEng.operands[4] = ObjectSystem.scriptEng.operands[1] >> 7;
              ObjectSystem.scriptEng.operands[5] = ObjectSystem.scriptEng.operands[2] >> 7;
              ObjectSystem.scriptEng.operands[6] = ObjectSystem.scriptEng.operands[4] <= -1 || ObjectSystem.scriptEng.operands[5] <= -1 ? 0 : (int) StageSystem.stageLayouts[0].tileMap[ObjectSystem.scriptEng.operands[4] + (ObjectSystem.scriptEng.operands[5] << 8)] << 6;
              ObjectSystem.scriptEng.operands[6] += ((ObjectSystem.scriptEng.operands[1] & (int) sbyte.MaxValue) >> 4) + ((ObjectSystem.scriptEng.operands[2] & (int) sbyte.MaxValue) >> 4 << 3);
              switch (ObjectSystem.scriptEng.operands[3])
              {
                case 0:
                  ObjectSystem.scriptEng.operands[0] = (int) StageSystem.tile128x128.tile16x16[ObjectSystem.scriptEng.operands[6]];
                  break;
                case 1:
                  ObjectSystem.scriptEng.operands[0] = (int) StageSystem.tile128x128.direction[ObjectSystem.scriptEng.operands[6]];
                  break;
                case 2:
                  ObjectSystem.scriptEng.operands[0] = (int) StageSystem.tile128x128.visualPlane[ObjectSystem.scriptEng.operands[6]];
                  break;
                case 3:
                  ObjectSystem.scriptEng.operands[0] = (int) StageSystem.tile128x128.collisionFlag[0, ObjectSystem.scriptEng.operands[6]];
                  break;
                case 4:
                  ObjectSystem.scriptEng.operands[0] = (int) StageSystem.tile128x128.collisionFlag[1, ObjectSystem.scriptEng.operands[6]];
                  break;
                case 5:
                  ObjectSystem.scriptEng.operands[0] = (int) StageSystem.tileCollisions[0].flags[(int) StageSystem.tile128x128.tile16x16[ObjectSystem.scriptEng.operands[6]]];
                  break;
                case 6:
                  ObjectSystem.scriptEng.operands[0] = (int) StageSystem.tileCollisions[0].angle[(int) StageSystem.tile128x128.tile16x16[ObjectSystem.scriptEng.operands[6]]];
                  break;
                case 7:
                  ObjectSystem.scriptEng.operands[0] = (int) StageSystem.tileCollisions[1].flags[(int) StageSystem.tile128x128.tile16x16[ObjectSystem.scriptEng.operands[6]]];
                  break;
                case 8:
                  ObjectSystem.scriptEng.operands[0] = (int) StageSystem.tileCollisions[1].angle[(int) StageSystem.tile128x128.tile16x16[ObjectSystem.scriptEng.operands[6]]];
                  break;
              }
              break;
            case 120:
              num3 = (sbyte) 0;
              GraphicsSystem.Copy16x16Tile(ObjectSystem.scriptEng.operands[0], ObjectSystem.scriptEng.operands[1]);
              break;
            case 121:
              ObjectSystem.scriptEng.operands[4] = ObjectSystem.scriptEng.operands[1] >> 7;
              ObjectSystem.scriptEng.operands[5] = ObjectSystem.scriptEng.operands[2] >> 7;
              ObjectSystem.scriptEng.operands[6] = ObjectSystem.scriptEng.operands[4] <= -1 || ObjectSystem.scriptEng.operands[5] <= -1 ? 0 : (int) StageSystem.stageLayouts[0].tileMap[ObjectSystem.scriptEng.operands[4] + (ObjectSystem.scriptEng.operands[5] << 8)] << 6;
              ObjectSystem.scriptEng.operands[6] += ((ObjectSystem.scriptEng.operands[1] & (int) sbyte.MaxValue) >> 4) + ((ObjectSystem.scriptEng.operands[2] & (int) sbyte.MaxValue) >> 4 << 3);
              switch (ObjectSystem.scriptEng.operands[3])
              {
                case 0:
                  StageSystem.tile128x128.tile16x16[ObjectSystem.scriptEng.operands[6]] = (ushort) ObjectSystem.scriptEng.operands[0];
                  StageSystem.tile128x128.gfxDataPos[ObjectSystem.scriptEng.operands[6]] = (int) StageSystem.tile128x128.tile16x16[ObjectSystem.scriptEng.operands[6]] << 2;
                  break;
                case 1:
                  StageSystem.tile128x128.direction[ObjectSystem.scriptEng.operands[6]] = (byte) ObjectSystem.scriptEng.operands[0];
                  break;
                case 2:
                  StageSystem.tile128x128.visualPlane[ObjectSystem.scriptEng.operands[6]] = (byte) ObjectSystem.scriptEng.operands[0];
                  break;
                case 3:
                  StageSystem.tile128x128.collisionFlag[0, ObjectSystem.scriptEng.operands[6]] = (byte) ObjectSystem.scriptEng.operands[0];
                  break;
                case 4:
                  StageSystem.tile128x128.collisionFlag[1, ObjectSystem.scriptEng.operands[6]] = (byte) ObjectSystem.scriptEng.operands[0];
                  break;
                case 5:
                  StageSystem.tileCollisions[0].flags[(int) StageSystem.tile128x128.tile16x16[ObjectSystem.scriptEng.operands[6]]] = (byte) ObjectSystem.scriptEng.operands[0];
                  break;
                case 6:
                  StageSystem.tileCollisions[0].angle[(int) StageSystem.tile128x128.tile16x16[ObjectSystem.scriptEng.operands[6]]] = (uint) (byte) ObjectSystem.scriptEng.operands[0];
                  break;
              }
              break;
            case 122:
              ObjectSystem.scriptEng.operands[0] = -1;
              ObjectSystem.scriptEng.sRegister = 0;
              while (ObjectSystem.scriptEng.operands[0] == -1)
              {
                if (FileIO.StringComp(ref ObjectSystem.scriptText, ref AnimationSystem.animationList[PlayerSystem.playerList[ObjectSystem.playerNum].animationFile.aniListOffset + ObjectSystem.scriptEng.sRegister].name))
                {
                  ObjectSystem.scriptEng.operands[0] = ObjectSystem.scriptEng.sRegister;
                }
                else
                {
                  ++ObjectSystem.scriptEng.sRegister;
                  if (ObjectSystem.scriptEng.sRegister == PlayerSystem.playerList[ObjectSystem.playerNum].animationFile.numAnimations)
                    ObjectSystem.scriptEng.operands[0] = 0;
                }
              }
              break;
            case 123:
              num3 = (sbyte) 0;
              ObjectSystem.scriptEng.checkResult = (int) FileIO.ReadSaveRAMData();
              break;
            case 124:
              num3 = (sbyte) 0;
              ObjectSystem.scriptEng.checkResult = (int) FileIO.WriteSaveRAMData();
              break;
            case 125:
              num3 = (sbyte) 0;
              TextSystem.LoadFontFile(ObjectSystem.scriptText);
              break;
            case 126:
              num3 = (sbyte) 0;
              if (ObjectSystem.scriptEng.operands[2] == 0)
              {
                TextSystem.LoadTextFile(StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]], ObjectSystem.scriptText, (byte) 0);
                break;
              }
              TextSystem.LoadTextFile(StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]], ObjectSystem.scriptText, (byte) 1);
              break;
            case (int) sbyte.MaxValue:
              num3 = (sbyte) 0;
              TextSystem.textMenuSurfaceNo = (int) ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum;
              TextSystem.DrawBitmapText(StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]], ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3], ObjectSystem.scriptEng.operands[4], ObjectSystem.scriptEng.operands[5], ObjectSystem.scriptEng.operands[6]);
              break;
            case 128 /*0x80*/:
              switch (ObjectSystem.scriptEng.operands[2])
              {
                case 0:
                  ObjectSystem.scriptEng.operands[0] = (int) StageSystem.gameMenu[ObjectSystem.scriptEng.operands[1]].textData[StageSystem.gameMenu[ObjectSystem.scriptEng.operands[1]].entryStart[ObjectSystem.scriptEng.operands[3]] + ObjectSystem.scriptEng.operands[4]];
                  break;
                case 1:
                  ObjectSystem.scriptEng.operands[0] = StageSystem.gameMenu[ObjectSystem.scriptEng.operands[1]].entrySize[ObjectSystem.scriptEng.operands[3]];
                  break;
                case 2:
                  ObjectSystem.scriptEng.operands[0] = (int) StageSystem.gameMenu[ObjectSystem.scriptEng.operands[1]].numRows;
                  break;
              }
              break;
            case 129:
              num3 = (sbyte) 0;
              StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]].entryHighlight[(int) StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]].numRows] = (byte) ObjectSystem.scriptEng.operands[1];
              TextSystem.AddTextMenuEntry(StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]], GlobalAppDefinitions.gameVersion);
              break;
            case 130:
              num3 = (sbyte) 0;
              EngineCallbacks.OnlineSetAchievement(ObjectSystem.scriptEng.operands[0], ObjectSystem.scriptEng.operands[1]);
              break;
            case 131:
              num3 = (sbyte) 0;
              EngineCallbacks.OnlineSetLeaderboard(ObjectSystem.scriptEng.operands[0], ObjectSystem.scriptEng.operands[1]);
              break;
            case 132:
              num3 = (sbyte) 0;
              switch (ObjectSystem.scriptEng.operands[0])
              {
                case 0:
                  EngineCallbacks.OnlineLoadAchievementsMenu();
                  break;
                case 1:
                  EngineCallbacks.OnlineLoadLeaderboardsMenu();
                  break;
              }
              break;
            case 133:
              num3 = (sbyte) 0;
              EngineCallbacks.RetroEngineCallback(ObjectSystem.scriptEng.operands[0]);
              break;
            case 134:
              num3 = (sbyte) 0;
              break;
          }
          if (num3 > (sbyte) 0)
            scriptCodePtr -= num2;
          for (int index6 = 0; index6 < (int) num3; ++index6)
          {
            switch (ObjectSystem.scriptData[scriptCodePtr])
            {
              case 1:
                ++scriptCodePtr;
                switch (ObjectSystem.scriptData[scriptCodePtr])
                {
                  case 0:
                    index1 = ObjectSystem.objectLoop;
                    break;
                  case 1:
                    ++scriptCodePtr;
                    if (ObjectSystem.scriptData[scriptCodePtr] == 1)
                    {
                      ++scriptCodePtr;
                      index1 = ObjectSystem.scriptEng.arrayPosition[ObjectSystem.scriptData[scriptCodePtr]];
                      break;
                    }
                    ++scriptCodePtr;
                    index1 = ObjectSystem.scriptData[scriptCodePtr];
                    break;
                  case 2:
                    ++scriptCodePtr;
                    if (ObjectSystem.scriptData[scriptCodePtr] == 1)
                    {
                      ++scriptCodePtr;
                      index1 = ObjectSystem.objectLoop + ObjectSystem.scriptEng.arrayPosition[ObjectSystem.scriptData[scriptCodePtr]];
                      break;
                    }
                    ++scriptCodePtr;
                    index1 = ObjectSystem.objectLoop + ObjectSystem.scriptData[scriptCodePtr];
                    break;
                  case 3:
                    ++scriptCodePtr;
                    if (ObjectSystem.scriptData[scriptCodePtr] == 1)
                    {
                      ++scriptCodePtr;
                      index1 = ObjectSystem.objectLoop - ObjectSystem.scriptEng.arrayPosition[ObjectSystem.scriptData[scriptCodePtr]];
                      break;
                    }
                    ++scriptCodePtr;
                    index1 = ObjectSystem.objectLoop - ObjectSystem.scriptData[scriptCodePtr];
                    break;
                }
                ++scriptCodePtr;
                switch (ObjectSystem.scriptData[scriptCodePtr])
                {
                  case 0:
                    ObjectSystem.scriptEng.tempValue[0] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 1:
                    ObjectSystem.scriptEng.tempValue[1] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 2:
                    ObjectSystem.scriptEng.tempValue[2] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 3:
                    ObjectSystem.scriptEng.tempValue[3] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 4:
                    ObjectSystem.scriptEng.tempValue[4] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 5:
                    ObjectSystem.scriptEng.tempValue[5] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 6:
                    ObjectSystem.scriptEng.tempValue[6] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 7:
                    ObjectSystem.scriptEng.tempValue[7] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 8:
                    ObjectSystem.scriptEng.checkResult = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 9:
                    ObjectSystem.scriptEng.arrayPosition[0] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 10:
                    ObjectSystem.scriptEng.arrayPosition[1] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 11:
                    ObjectSystem.globalVariables[index1] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 13:
                    ObjectSystem.objectEntityList[index1].type = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 14:
                    ObjectSystem.objectEntityList[index1].propertyValue = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 15:
                    ObjectSystem.objectEntityList[index1].xPos = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 16 /*0x10*/:
                    ObjectSystem.objectEntityList[index1].yPos = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 17:
                    ObjectSystem.objectEntityList[index1].xPos = ObjectSystem.scriptEng.operands[index6] << 16 /*0x10*/;
                    break;
                  case 18:
                    ObjectSystem.objectEntityList[index1].yPos = ObjectSystem.scriptEng.operands[index6] << 16 /*0x10*/;
                    break;
                  case 19:
                    ObjectSystem.objectEntityList[index1].state = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 20:
                    ObjectSystem.objectEntityList[index1].rotation = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 21:
                    ObjectSystem.objectEntityList[index1].scale = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 22:
                    ObjectSystem.objectEntityList[index1].priority = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 23:
                    ObjectSystem.objectEntityList[index1].drawOrder = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 24:
                    ObjectSystem.objectEntityList[index1].direction = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 25:
                    ObjectSystem.objectEntityList[index1].inkEffect = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 26:
                    ObjectSystem.objectEntityList[index1].alpha = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 27:
                    ObjectSystem.objectEntityList[index1].frame = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 28:
                    ObjectSystem.objectEntityList[index1].animation = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 29:
                    ObjectSystem.objectEntityList[index1].prevAnimation = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 30:
                    ObjectSystem.objectEntityList[index1].animationSpeed = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 31 /*0x1F*/:
                    ObjectSystem.objectEntityList[index1].animationTimer = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 32 /*0x20*/:
                    ObjectSystem.objectEntityList[index1].value[0] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 33:
                    ObjectSystem.objectEntityList[index1].value[1] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 34:
                    ObjectSystem.objectEntityList[index1].value[2] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 35:
                    ObjectSystem.objectEntityList[index1].value[3] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 36:
                    ObjectSystem.objectEntityList[index1].value[4] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 37:
                    ObjectSystem.objectEntityList[index1].value[5] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 38:
                    ObjectSystem.objectEntityList[index1].value[6] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 39:
                    ObjectSystem.objectEntityList[index1].value[7] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 41:
                    PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.state = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 42:
                    PlayerSystem.playerList[ObjectSystem.playerNum].controlMode = (sbyte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 43:
                    PlayerSystem.playerList[ObjectSystem.playerNum].controlLock = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 44:
                    PlayerSystem.playerList[ObjectSystem.playerNum].collisionMode = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 45:
                    PlayerSystem.playerList[ObjectSystem.playerNum].collisionPlane = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 46:
                    PlayerSystem.playerList[ObjectSystem.playerNum].xPos = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 47:
                    PlayerSystem.playerList[ObjectSystem.playerNum].yPos = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 48 /*0x30*/:
                    PlayerSystem.playerList[ObjectSystem.playerNum].xPos = ObjectSystem.scriptEng.operands[index6] << 16 /*0x10*/;
                    break;
                  case 49:
                    PlayerSystem.playerList[ObjectSystem.playerNum].yPos = ObjectSystem.scriptEng.operands[index6] << 16 /*0x10*/;
                    break;
                  case 50:
                    PlayerSystem.playerList[ObjectSystem.playerNum].screenXPos = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 51:
                    PlayerSystem.playerList[ObjectSystem.playerNum].screenYPos = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 52:
                    PlayerSystem.playerList[ObjectSystem.playerNum].speed = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 53:
                    PlayerSystem.playerList[ObjectSystem.playerNum].xVelocity = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 54:
                    PlayerSystem.playerList[ObjectSystem.playerNum].yVelocity = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 55:
                    PlayerSystem.playerList[ObjectSystem.playerNum].gravity = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 56:
                    PlayerSystem.playerList[ObjectSystem.playerNum].angle = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 57:
                    PlayerSystem.playerList[ObjectSystem.playerNum].skidding = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 58:
                    PlayerSystem.playerList[ObjectSystem.playerNum].pushing = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 59:
                    PlayerSystem.playerList[ObjectSystem.playerNum].trackScroll = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 60:
                    PlayerSystem.playerList[ObjectSystem.playerNum].up = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 61:
                    PlayerSystem.playerList[ObjectSystem.playerNum].down = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 62:
                    PlayerSystem.playerList[ObjectSystem.playerNum].left = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 63 /*0x3F*/:
                    PlayerSystem.playerList[ObjectSystem.playerNum].right = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 64 /*0x40*/:
                    PlayerSystem.playerList[ObjectSystem.playerNum].jumpPress = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 65:
                    PlayerSystem.playerList[ObjectSystem.playerNum].jumpHold = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 66:
                    PlayerSystem.playerList[ObjectSystem.playerNum].followPlayer1 = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 67:
                    PlayerSystem.playerList[ObjectSystem.playerNum].lookPos = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 68:
                    PlayerSystem.playerList[ObjectSystem.playerNum].water = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 69:
                    PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.topSpeed = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 70:
                    PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.acceleration = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 71:
                    PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.deceleration = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 72:
                    PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.airAcceleration = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 73:
                    PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.airDeceleration = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 74:
                    PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.gravity = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 75:
                    PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.jumpStrength = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 76:
                    PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.jumpCap = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 77:
                    PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.rollingAcceleration = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 78:
                    PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.rollingDeceleration = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 84:
                    PlayerSystem.playerList[ObjectSystem.playerNum].flailing[index1] = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 85:
                    PlayerSystem.playerList[ObjectSystem.playerNum].timer = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 86:
                    PlayerSystem.playerList[ObjectSystem.playerNum].tileCollisions = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 87:
                    PlayerSystem.playerList[ObjectSystem.playerNum].objectInteraction = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 88:
                    PlayerSystem.playerList[ObjectSystem.playerNum].visible = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 89:
                    PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.rotation = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 90:
                    PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.scale = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 91:
                    PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.priority = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 92:
                    PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.drawOrder = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 93:
                    PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.direction = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 94:
                    PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.inkEffect = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 95:
                    PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.alpha = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 96 /*0x60*/:
                    PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.frame = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 97:
                    PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.animation = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 98:
                    PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.prevAnimation = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 99:
                    PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.animationSpeed = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 100:
                    PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.animationTimer = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 101:
                    PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[0] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 102:
                    PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[1] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 103:
                    PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[2] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 104:
                    PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[3] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 105:
                    PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[4] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 106:
                    PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[5] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 107:
                    PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[6] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 108:
                    PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[7] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 109:
                    PlayerSystem.playerList[ObjectSystem.playerNum].value[0] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 110:
                    PlayerSystem.playerList[ObjectSystem.playerNum].value[1] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 111:
                    PlayerSystem.playerList[ObjectSystem.playerNum].value[2] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 112 /*0x70*/:
                    PlayerSystem.playerList[ObjectSystem.playerNum].value[3] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 113:
                    PlayerSystem.playerList[ObjectSystem.playerNum].value[4] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 114:
                    PlayerSystem.playerList[ObjectSystem.playerNum].value[5] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 115:
                    PlayerSystem.playerList[ObjectSystem.playerNum].value[6] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 116:
                    PlayerSystem.playerList[ObjectSystem.playerNum].value[7] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 118:
                    StageSystem.stageMode = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 119:
                    FileIO.activeStageList = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 120:
                    StageSystem.stageListPosition = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 121:
                    StageSystem.timeEnabled = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 122:
                    StageSystem.milliSeconds = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 123:
                    StageSystem.seconds = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 124:
                    StageSystem.minutes = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 125:
                    FileIO.actNumber = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 126:
                    StageSystem.pauseEnabled = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 128 /*0x80*/:
                    StageSystem.newXBoundary1 = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 129:
                    StageSystem.newXBoundary2 = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 130:
                    StageSystem.newYBoundary1 = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 131:
                    StageSystem.newYBoundary2 = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 132:
                    if (StageSystem.xBoundary1 != ObjectSystem.scriptEng.operands[index6])
                    {
                      StageSystem.xBoundary1 = ObjectSystem.scriptEng.operands[index6];
                      StageSystem.newXBoundary1 = ObjectSystem.scriptEng.operands[index6];
                      break;
                    }
                    break;
                  case 133:
                    if (StageSystem.xBoundary2 != ObjectSystem.scriptEng.operands[index6])
                    {
                      StageSystem.xBoundary2 = ObjectSystem.scriptEng.operands[index6];
                      StageSystem.newXBoundary2 = ObjectSystem.scriptEng.operands[index6];
                      break;
                    }
                    break;
                  case 134:
                    if (StageSystem.yBoundary1 != ObjectSystem.scriptEng.operands[index6])
                    {
                      StageSystem.yBoundary1 = ObjectSystem.scriptEng.operands[index6];
                      StageSystem.newYBoundary1 = ObjectSystem.scriptEng.operands[index6];
                      break;
                    }
                    break;
                  case 135:
                    if (StageSystem.yBoundary2 != ObjectSystem.scriptEng.operands[index6])
                    {
                      StageSystem.yBoundary2 = ObjectSystem.scriptEng.operands[index6];
                      StageSystem.newYBoundary2 = ObjectSystem.scriptEng.operands[index6];
                      break;
                    }
                    break;
                  case 136:
                    StageSystem.bgDeformationData0[index1] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 137:
                    StageSystem.bgDeformationData1[index1] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 138:
                    StageSystem.bgDeformationData2[index1] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 139:
                    StageSystem.bgDeformationData3[index1] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 140:
                    StageSystem.waterLevel = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 141:
                    StageSystem.activeTileLayers[index1] = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 142:
                    StageSystem.tLayerMidPoint = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 143:
                    PlayerSystem.playerMenuNum = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 144 /*0x90*/:
                    ObjectSystem.playerNum = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 145:
                    StageSystem.cameraEnabled = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 146:
                    StageSystem.cameraTarget = (sbyte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 147:
                    StageSystem.cameraStyle = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 148:
                    ObjectSystem.objectDrawOrderList[index1].listSize = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 153:
                    StageSystem.xScrollOffset = ObjectSystem.scriptEng.operands[index6];
                    StageSystem.xScrollA = StageSystem.xScrollOffset;
                    StageSystem.xScrollB = StageSystem.xScrollOffset + GlobalAppDefinitions.SCREEN_XSIZE;
                    break;
                  case 154:
                    StageSystem.yScrollOffset = ObjectSystem.scriptEng.operands[index6];
                    StageSystem.yScrollA = StageSystem.yScrollOffset;
                    StageSystem.yScrollB = StageSystem.yScrollOffset + 240 /*0xF0*/;
                    break;
                  case 155:
                    StageSystem.screenShakeX = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 156:
                    StageSystem.screenShakeY = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 157:
                    StageSystem.cameraAdjustY = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 161:
                    AudioPlayback.SetMusicVolume(ObjectSystem.scriptEng.operands[index6]);
                    break;
                  case 163:
                    StageSystem.gKeyDown.up = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 164:
                    StageSystem.gKeyDown.down = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 165:
                    StageSystem.gKeyDown.left = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 166:
                    StageSystem.gKeyDown.right = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 167:
                    StageSystem.gKeyDown.buttonA = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 168:
                    StageSystem.gKeyDown.buttonB = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 169:
                    StageSystem.gKeyDown.buttonC = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 170:
                    StageSystem.gKeyDown.start = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 171:
                    StageSystem.gKeyPress.up = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 172:
                    StageSystem.gKeyPress.down = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 173:
                    StageSystem.gKeyPress.left = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 174:
                    StageSystem.gKeyPress.right = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 175:
                    StageSystem.gKeyPress.buttonA = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 176 /*0xB0*/:
                    StageSystem.gKeyPress.buttonB = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 177:
                    StageSystem.gKeyPress.buttonC = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 178:
                    StageSystem.gKeyPress.start = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 179:
                    StageSystem.gameMenu[0].selection1 = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 180:
                    StageSystem.gameMenu[1].selection1 = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 181:
                    StageSystem.stageLayouts[index1].xSize = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 182:
                    StageSystem.stageLayouts[index1].ySize = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 183:
                    StageSystem.stageLayouts[index1].type = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 184:
                    StageSystem.stageLayouts[index1].angle = ObjectSystem.scriptEng.operands[index6];
                    if (StageSystem.stageLayouts[index1].angle < 0)
                      StageSystem.stageLayouts[index1].angle += 512 /*0x0200*/;
                    StageSystem.stageLayouts[index1].angle &= 511 /*0x01FF*/;
                    break;
                  case 185:
                    StageSystem.stageLayouts[index1].xPos = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 186:
                    StageSystem.stageLayouts[index1].yPos = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 187:
                    StageSystem.stageLayouts[index1].zPos = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 188:
                    StageSystem.stageLayouts[index1].parallaxFactor = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 189:
                    StageSystem.stageLayouts[index1].scrollSpeed = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 190:
                    StageSystem.stageLayouts[index1].scrollPosition = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 191:
                    StageSystem.stageLayouts[index1].deformationPos = ObjectSystem.scriptEng.operands[index6];
                    StageSystem.stageLayouts[index1].deformationPos &= (int) byte.MaxValue;
                    break;
                  case 192 /*0xC0*/:
                    StageSystem.stageLayouts[index1].deformationPosW = ObjectSystem.scriptEng.operands[index6];
                    StageSystem.stageLayouts[index1].deformationPosW &= (int) byte.MaxValue;
                    break;
                  case 193:
                    StageSystem.hParallax.parallaxFactor[index1] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 194:
                    StageSystem.hParallax.scrollSpeed[index1] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 195:
                    StageSystem.hParallax.scrollPosition[index1] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 196:
                    StageSystem.vParallax.parallaxFactor[index1] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 197:
                    StageSystem.vParallax.scrollSpeed[index1] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 198:
                    StageSystem.vParallax.scrollPosition[index1] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 199:
                    Scene3D.numVertices = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 200:
                    Scene3D.numFaces = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 201:
                    Scene3D.vertexBuffer[index1].x = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 202:
                    Scene3D.vertexBuffer[index1].y = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 203:
                    Scene3D.vertexBuffer[index1].z = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 204:
                    Scene3D.vertexBuffer[index1].u = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 205:
                    Scene3D.vertexBuffer[index1].v = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 206:
                    Scene3D.indexBuffer[index1].a = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 207:
                    Scene3D.indexBuffer[index1].b = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 208 /*0xD0*/:
                    Scene3D.indexBuffer[index1].c = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 209:
                    Scene3D.indexBuffer[index1].d = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 210:
                    Scene3D.indexBuffer[index1].flag = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 211:
                    Scene3D.indexBuffer[index1].color = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 212:
                    Scene3D.projectionX = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 213:
                    Scene3D.projectionY = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 214:
                    GlobalAppDefinitions.gameMode = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 215:
                    StageSystem.debugMode = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 217:
                    FileIO.saveRAM[index1] = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 218:
                    GlobalAppDefinitions.gameLanguage = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 219:
                    ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 221:
                    GlobalAppDefinitions.frameSkipTimer = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 222:
                    GlobalAppDefinitions.frameSkipSetting = ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 223:
                    GlobalAppDefinitions.gameSFXVolume = ObjectSystem.scriptEng.operands[index6];
                    AudioPlayback.SetGameVolumes(GlobalAppDefinitions.gameBGMVolume, GlobalAppDefinitions.gameSFXVolume);
                    break;
                  case 224 /*0xE0*/:
                    GlobalAppDefinitions.gameBGMVolume = ObjectSystem.scriptEng.operands[index6];
                    AudioPlayback.SetGameVolumes(GlobalAppDefinitions.gameBGMVolume, GlobalAppDefinitions.gameSFXVolume);
                    break;
                  case 227:
                    StageSystem.gKeyPress.start = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                  case 228:
                    GlobalAppDefinitions.gameHapticsEnabled = (byte) ObjectSystem.scriptEng.operands[index6];
                    break;
                }
                ++scriptCodePtr;
                break;
              case 2:
                scriptCodePtr += 2;
                break;
              case 3:
                ++scriptCodePtr;
                int num7 = 0;
                index1 = 0;
                for (ObjectSystem.scriptEng.sRegister = ObjectSystem.scriptData[scriptCodePtr]; num7 < ObjectSystem.scriptEng.sRegister; ++num7)
                {
                  switch (index1)
                  {
                    case 0:
                      ++scriptCodePtr;
                      ++index1;
                      break;
                    case 1:
                      ++index1;
                      break;
                    case 2:
                      ++index1;
                      break;
                    case 3:
                      index1 = 0;
                      break;
                  }
                }
                if (index1 == 0)
                {
                  scriptCodePtr += 2;
                  break;
                }
                ++scriptCodePtr;
                break;
            }
          }
        }
      }

      public static void ProcessStartupScripts()
      {
        ObjectSystem.objectEntityList[1057].type = ObjectSystem.objectEntityList[0].type;
        ObjectSystem.scriptFramesNo = 0;
        ObjectSystem.playerNum = 0;
        ObjectSystem.scriptEng.arrayPosition[2] = 1056;
        for (int index = 0; index < 256 /*0x0100*/; ++index)
        {
          ObjectSystem.objectLoop = 1056;
          ObjectSystem.objectEntityList[1056].type = (byte) index;
          ObjectSystem.objectScriptList[index].numFrames = 0;
          ObjectSystem.objectScriptList[index].surfaceNum = (byte) 0;
          ObjectSystem.objectScriptList[index].frameListOffset = ObjectSystem.scriptFramesNo;
          ObjectSystem.objectScriptList[index].numFrames = ObjectSystem.scriptFramesNo;
          if (ObjectSystem.scriptData[ObjectSystem.objectScriptList[index].startupScript] > 0)
            ObjectSystem.ProcessScript(ObjectSystem.objectScriptList[index].startupScript, ObjectSystem.objectScriptList[index].startupJumpTable, 3);
          ObjectSystem.objectScriptList[index].numFrames = ObjectSystem.scriptFramesNo - ObjectSystem.objectScriptList[index].numFrames;
        }
        ObjectSystem.objectEntityList[1056].type = ObjectSystem.objectEntityList[1057].type;
        ObjectSystem.objectEntityList[1056].type = (byte) 0;
      }

      public static void ProcessObjects()
      {
        bool flag = false;
        ObjectSystem.objectDrawOrderList[0].listSize = 0;
        ObjectSystem.objectDrawOrderList[1].listSize = 0;
        ObjectSystem.objectDrawOrderList[2].listSize = 0;
        ObjectSystem.objectDrawOrderList[3].listSize = 0;
        ObjectSystem.objectDrawOrderList[4].listSize = 0;
        ObjectSystem.objectDrawOrderList[5].listSize = 0;
        ObjectSystem.objectDrawOrderList[6].listSize = 0;
        for (ObjectSystem.objectLoop = 0; ObjectSystem.objectLoop < 1184; ++ObjectSystem.objectLoop)
        {
          switch (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].priority)
          {
            case 0:
              int num1 = ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16 /*0x10*/;
              int num2 = ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos >> 16 /*0x10*/;
              flag = num1 > StageSystem.xScrollOffset - GlobalAppDefinitions.OBJECT_BORDER_X1 && num1 < StageSystem.xScrollOffset + GlobalAppDefinitions.OBJECT_BORDER_X2 && num2 > StageSystem.yScrollOffset - 256 /*0x0100*/ && num2 < StageSystem.yScrollOffset + 496;
              break;
            case 1:
              flag = true;
              break;
            case 2:
              flag = true;
              break;
            case 3:
              int num3 = ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16 /*0x10*/;
              flag = num3 > StageSystem.xScrollOffset - GlobalAppDefinitions.OBJECT_BORDER_X1 && num3 < StageSystem.xScrollOffset + GlobalAppDefinitions.OBJECT_BORDER_X2;
              break;
            case 4:
              int num4 = ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16 /*0x10*/;
              int num5 = ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos >> 16 /*0x10*/;
              if (num4 > StageSystem.xScrollOffset - GlobalAppDefinitions.OBJECT_BORDER_X1 && num4 < StageSystem.xScrollOffset + GlobalAppDefinitions.OBJECT_BORDER_X2 && num5 > StageSystem.yScrollOffset - 256 /*0x0100*/ && num5 < StageSystem.yScrollOffset + 496)
              {
                flag = true;
                break;
              }
              flag = false;
              ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type = (byte) 0;
              break;
            case 5:
              flag = false;
              break;
          }
          if (flag && ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type > (byte) 0)
          {
            int type = (int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type;
            ObjectSystem.playerNum = 0;
            if (ObjectSystem.scriptData[ObjectSystem.objectScriptList[type].mainScript] > 0)
              ObjectSystem.ProcessScript(ObjectSystem.objectScriptList[type].mainScript, ObjectSystem.objectScriptList[type].mainJumpTable, 0);
            if (ObjectSystem.scriptData[ObjectSystem.objectScriptList[type].playerScript] > 0)
            {
              for (; ObjectSystem.playerNum < (int) PlayerSystem.numActivePlayers; ++ObjectSystem.playerNum)
              {
                if (PlayerSystem.playerList[ObjectSystem.playerNum].objectInteraction == (byte) 1)
                  ObjectSystem.ProcessScript(ObjectSystem.objectScriptList[type].playerScript, ObjectSystem.objectScriptList[type].playerJumpTable, 1);
              }
            }
            int drawOrder = (int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].drawOrder;
            if (drawOrder < 7)
            {
              ObjectSystem.objectDrawOrderList[drawOrder].entityRef[ObjectSystem.objectDrawOrderList[drawOrder].listSize] = ObjectSystem.objectLoop;
              ++ObjectSystem.objectDrawOrderList[drawOrder].listSize;
            }
          }
        }
      }

      public static void ProcessPausedObjects()
      {
        ObjectSystem.objectDrawOrderList[0].listSize = 0;
        ObjectSystem.objectDrawOrderList[1].listSize = 0;
        ObjectSystem.objectDrawOrderList[2].listSize = 0;
        ObjectSystem.objectDrawOrderList[3].listSize = 0;
        ObjectSystem.objectDrawOrderList[4].listSize = 0;
        ObjectSystem.objectDrawOrderList[5].listSize = 0;
        ObjectSystem.objectDrawOrderList[6].listSize = 0;
        for (ObjectSystem.objectLoop = 0; ObjectSystem.objectLoop < 1184; ++ObjectSystem.objectLoop)
        {
          if (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].priority == (byte) 2 && ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type > (byte) 0)
          {
            int type = (int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type;
            ObjectSystem.playerNum = 0;
            if (ObjectSystem.scriptData[ObjectSystem.objectScriptList[type].mainScript] > 0)
              ObjectSystem.ProcessScript(ObjectSystem.objectScriptList[type].mainScript, ObjectSystem.objectScriptList[type].mainJumpTable, 0);
            if (ObjectSystem.scriptData[ObjectSystem.objectScriptList[type].playerScript] > 0)
            {
              for (; ObjectSystem.playerNum < (int) PlayerSystem.numActivePlayers; ++ObjectSystem.playerNum)
              {
                if (PlayerSystem.playerList[ObjectSystem.playerNum].objectInteraction == (byte) 1)
                  ObjectSystem.ProcessScript(ObjectSystem.objectScriptList[type].playerScript, ObjectSystem.objectScriptList[type].playerJumpTable, 1);
              }
            }
            int drawOrder = (int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].drawOrder;
            if (drawOrder < 7)
            {
              ObjectSystem.objectDrawOrderList[drawOrder].entityRef[ObjectSystem.objectDrawOrderList[drawOrder].listSize] = ObjectSystem.objectLoop;
              ++ObjectSystem.objectDrawOrderList[drawOrder].listSize;
            }
          }
        }
      }

      public static void DrawObjectList(int DrawListNo)
      {
        int listSize = ObjectSystem.objectDrawOrderList[DrawListNo].listSize;
        for (int index = 0; index < listSize; ++index)
        {
          ObjectSystem.objectLoop = ObjectSystem.objectDrawOrderList[DrawListNo].entityRef[index];
          if (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type > (byte) 0)
          {
            ObjectSystem.playerNum = 0;
            if (ObjectSystem.scriptData[ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].drawScript] > 0)
              ObjectSystem.ProcessScript(ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].drawScript, ObjectSystem.objectScriptList[(int) ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].drawJumpTable, 2);
          }
        }
      }

      public static void BasicCollision(int cLeft, int cTop, int cRight, int cBottom)
      {
        PlayerObject player = PlayerSystem.playerList[ObjectSystem.playerNum];
        CollisionBox collisionBox = AnimationSystem.collisionBoxList[player.animationFile.cbListOffset + (int) AnimationSystem.animationFrames[AnimationSystem.animationList[player.animationFile.aniListOffset + (int) player.objectPtr.animation].frameListOffset + (int) player.objectPtr.frame].collisionBox];
        PlayerSystem.collisionLeft = player.xPos >> 16 /*0x10*/;
        PlayerSystem.collisionTop = player.yPos >> 16 /*0x10*/;
        PlayerSystem.collisionRight = PlayerSystem.collisionLeft;
        PlayerSystem.collisionBottom = PlayerSystem.collisionTop;
        PlayerSystem.collisionLeft += (int) collisionBox.left[0];
        PlayerSystem.collisionTop += (int) collisionBox.top[0];
        PlayerSystem.collisionRight += (int) collisionBox.right[0];
        PlayerSystem.collisionBottom += (int) collisionBox.bottom[0];
        if (PlayerSystem.collisionRight > cLeft && PlayerSystem.collisionLeft < cRight && PlayerSystem.collisionBottom > cTop && PlayerSystem.collisionTop < cBottom)
          ObjectSystem.scriptEng.checkResult = 1;
        else
          ObjectSystem.scriptEng.checkResult = 0;
      }

      public static void BoxCollision(int cLeft, int cTop, int cRight, int cBottom)
      {
        int num = 0;
        PlayerObject player = PlayerSystem.playerList[ObjectSystem.playerNum];
        CollisionBox collisionBox = AnimationSystem.collisionBoxList[player.animationFile.cbListOffset + (int) AnimationSystem.animationFrames[AnimationSystem.animationList[player.animationFile.aniListOffset + (int) player.objectPtr.animation].frameListOffset + (int) player.objectPtr.frame].collisionBox];
        PlayerSystem.collisionLeft = (int) collisionBox.left[0];
        PlayerSystem.collisionTop = (int) collisionBox.top[0];
        PlayerSystem.collisionRight = (int) collisionBox.right[0];
        PlayerSystem.collisionBottom = (int) collisionBox.bottom[0];
        ObjectSystem.scriptEng.checkResult = 0;
        switch (player.collisionMode)
        {
          case 0:
          case 2:
            num = player.xVelocity == 0 ? Math.Abs(player.speed) : Math.Abs(player.xVelocity);
            break;
          case 1:
          case 3:
            num = Math.Abs(player.xVelocity);
            break;
        }
        if (num > Math.Abs(player.yVelocity))
        {
          ObjectSystem.cSensor[0].collided = (byte) 0;
          ObjectSystem.cSensor[1].collided = (byte) 0;
          ObjectSystem.cSensor[0].xPos = player.xPos + (PlayerSystem.collisionRight << 16 /*0x10*/);
          ObjectSystem.cSensor[1].xPos = ObjectSystem.cSensor[0].xPos;
          ObjectSystem.cSensor[0].yPos = player.yPos - 131072 /*0x020000*/;
          ObjectSystem.cSensor[1].yPos = player.yPos + 524288 /*0x080000*/;
          for (int index = 0; index < 2; ++index)
          {
            if (ObjectSystem.cSensor[index].xPos >= cLeft && player.xPos - player.xVelocity < cLeft && ObjectSystem.cSensor[1].yPos > cTop && ObjectSystem.cSensor[0].yPos < cBottom)
              ObjectSystem.cSensor[index].collided = (byte) 1;
          }
          if (ObjectSystem.cSensor[0].collided == (byte) 1 || ObjectSystem.cSensor[1].collided == (byte) 1)
          {
            player.xPos = cLeft - (PlayerSystem.collisionRight << 16 /*0x10*/);
            if (player.xVelocity > 0)
            {
              if (player.objectPtr.direction == (byte) 0)
                player.pushing = (byte) 2;
              player.xVelocity = 0;
              player.speed = 0;
            }
            ObjectSystem.scriptEng.checkResult = 2;
          }
          else
          {
            ObjectSystem.cSensor[0].collided = (byte) 0;
            ObjectSystem.cSensor[1].collided = (byte) 0;
            ObjectSystem.cSensor[0].xPos = player.xPos + (PlayerSystem.collisionLeft << 16 /*0x10*/);
            ObjectSystem.cSensor[1].xPos = ObjectSystem.cSensor[0].xPos;
            ObjectSystem.cSensor[0].yPos = player.yPos - 131072 /*0x020000*/;
            ObjectSystem.cSensor[1].yPos = player.yPos + 524288 /*0x080000*/;
            for (int index = 0; index < 2; ++index)
            {
              if (ObjectSystem.cSensor[index].xPos <= cRight && player.xPos - player.xVelocity > cRight && ObjectSystem.cSensor[1].yPos > cTop && ObjectSystem.cSensor[0].yPos < cBottom)
                ObjectSystem.cSensor[index].collided = (byte) 1;
            }
            if (ObjectSystem.cSensor[0].collided == (byte) 1 || ObjectSystem.cSensor[1].collided == (byte) 1)
            {
              player.xPos = cRight - (PlayerSystem.collisionLeft << 16 /*0x10*/);
              if (player.xVelocity < 0)
              {
                if (player.objectPtr.direction == (byte) 1)
                  player.pushing = (byte) 2;
                player.xVelocity = 0;
                player.speed = 0;
              }
              ObjectSystem.scriptEng.checkResult = 3;
            }
            else
            {
              ObjectSystem.cSensor[0].collided = (byte) 0;
              ObjectSystem.cSensor[1].collided = (byte) 0;
              ObjectSystem.cSensor[2].collided = (byte) 0;
              ObjectSystem.cSensor[0].xPos = player.xPos + (PlayerSystem.collisionLeft + 2 << 16 /*0x10*/);
              ObjectSystem.cSensor[1].xPos = player.xPos;
              ObjectSystem.cSensor[2].xPos = player.xPos + (PlayerSystem.collisionRight - 2 << 16 /*0x10*/);
              ObjectSystem.cSensor[0].yPos = player.yPos + (PlayerSystem.collisionBottom << 16 /*0x10*/);
              ObjectSystem.cSensor[1].yPos = ObjectSystem.cSensor[0].yPos;
              ObjectSystem.cSensor[2].yPos = ObjectSystem.cSensor[0].yPos;
              if (player.yVelocity > -1)
              {
                for (int index = 0; index < 3; ++index)
                {
                  if (ObjectSystem.cSensor[index].xPos > cLeft && ObjectSystem.cSensor[index].xPos < cRight && ObjectSystem.cSensor[index].yPos >= cTop && player.yPos - player.yVelocity < cTop)
                  {
                    ObjectSystem.cSensor[index].collided = (byte) 1;
                    player.flailing[index] = (byte) 1;
                  }
                }
              }
              if (ObjectSystem.cSensor[0].collided == (byte) 1 || ObjectSystem.cSensor[1].collided == (byte) 1 || ObjectSystem.cSensor[2].collided == (byte) 1)
              {
                if (player.gravity == (byte) 0 && (player.collisionMode == (byte) 1 || player.collisionMode == (byte) 3))
                {
                  player.xVelocity = 0;
                  player.speed = 0;
                }
                player.yPos = cTop - (PlayerSystem.collisionBottom << 16 /*0x10*/);
                player.gravity = (byte) 0;
                player.yVelocity = 0;
                player.angle = 0;
                player.objectPtr.rotation = 0;
                player.controlLock = (byte) 0;
                ObjectSystem.scriptEng.checkResult = 1;
              }
              else
              {
                ObjectSystem.cSensor[0].collided = (byte) 0;
                ObjectSystem.cSensor[1].collided = (byte) 0;
                ObjectSystem.cSensor[0].xPos = player.xPos + (PlayerSystem.collisionLeft + 2 << 16 /*0x10*/);
                ObjectSystem.cSensor[1].xPos = player.xPos + (PlayerSystem.collisionRight - 2 << 16 /*0x10*/);
                ObjectSystem.cSensor[0].yPos = player.yPos + (PlayerSystem.collisionTop << 16 /*0x10*/);
                ObjectSystem.cSensor[1].yPos = ObjectSystem.cSensor[0].yPos;
                for (int index = 0; index < 2; ++index)
                {
                  if (ObjectSystem.cSensor[index].xPos > cLeft && ObjectSystem.cSensor[index].xPos < cRight && ObjectSystem.cSensor[index].yPos <= cBottom && player.yPos - player.yVelocity > cBottom)
                    ObjectSystem.cSensor[index].collided = (byte) 1;
                }
                if (ObjectSystem.cSensor[0].collided != (byte) 1 && ObjectSystem.cSensor[1].collided != (byte) 1)
                  return;
                if (player.gravity == (byte) 1)
                  player.yPos = cBottom - (PlayerSystem.collisionTop << 16 /*0x10*/);
                if (player.yVelocity < 1)
                  player.yVelocity = 0;
                ObjectSystem.scriptEng.checkResult = 4;
              }
            }
          }
        }
        else
        {
          ObjectSystem.cSensor[0].collided = (byte) 0;
          ObjectSystem.cSensor[1].collided = (byte) 0;
          ObjectSystem.cSensor[2].collided = (byte) 0;
          ObjectSystem.cSensor[0].xPos = player.xPos + (PlayerSystem.collisionLeft + 2 << 16 /*0x10*/);
          ObjectSystem.cSensor[1].xPos = player.xPos;
          ObjectSystem.cSensor[2].xPos = player.xPos + (PlayerSystem.collisionRight - 2 << 16 /*0x10*/);
          ObjectSystem.cSensor[0].yPos = player.yPos + (PlayerSystem.collisionBottom << 16 /*0x10*/);
          ObjectSystem.cSensor[1].yPos = ObjectSystem.cSensor[0].yPos;
          ObjectSystem.cSensor[2].yPos = ObjectSystem.cSensor[0].yPos;
          if (player.yVelocity > -1)
          {
            for (int index = 0; index < 3; ++index)
            {
              if (ObjectSystem.cSensor[index].xPos > cLeft && ObjectSystem.cSensor[index].xPos < cRight && ObjectSystem.cSensor[index].yPos >= cTop && player.yPos - player.yVelocity < cTop)
              {
                ObjectSystem.cSensor[index].collided = (byte) 1;
                player.flailing[index] = (byte) 1;
              }
            }
          }
          if (ObjectSystem.cSensor[0].collided == (byte) 1 || ObjectSystem.cSensor[1].collided == (byte) 1 || ObjectSystem.cSensor[2].collided == (byte) 1)
          {
            if (player.gravity == (byte) 0 && (player.collisionMode == (byte) 1 || player.collisionMode == (byte) 3))
            {
              player.xVelocity = 0;
              player.speed = 0;
            }
            player.yPos = cTop - (PlayerSystem.collisionBottom << 16 /*0x10*/);
            player.gravity = (byte) 0;
            player.yVelocity = 0;
            player.angle = 0;
            player.objectPtr.rotation = 0;
            player.controlLock = (byte) 0;
            ObjectSystem.scriptEng.checkResult = 1;
          }
          else
          {
            ObjectSystem.cSensor[0].collided = (byte) 0;
            ObjectSystem.cSensor[1].collided = (byte) 0;
            ObjectSystem.cSensor[0].xPos = player.xPos + (PlayerSystem.collisionLeft + 2 << 16 /*0x10*/);
            ObjectSystem.cSensor[1].xPos = player.xPos + (PlayerSystem.collisionRight - 2 << 16 /*0x10*/);
            ObjectSystem.cSensor[0].yPos = player.yPos + (PlayerSystem.collisionTop << 16 /*0x10*/);
            ObjectSystem.cSensor[1].yPos = ObjectSystem.cSensor[0].yPos;
            for (int index = 0; index < 2; ++index)
            {
              if (ObjectSystem.cSensor[index].xPos > cLeft && ObjectSystem.cSensor[index].xPos < cRight && ObjectSystem.cSensor[index].yPos <= cBottom && player.yPos - player.yVelocity > cBottom)
                ObjectSystem.cSensor[index].collided = (byte) 1;
            }
            if (ObjectSystem.cSensor[0].collided == (byte) 1 || ObjectSystem.cSensor[1].collided == (byte) 1)
            {
              if (player.gravity == (byte) 1)
                player.yPos = cBottom - (PlayerSystem.collisionTop << 16 /*0x10*/);
              if (player.yVelocity < 1)
                player.yVelocity = 0;
              ObjectSystem.scriptEng.checkResult = 4;
            }
            else
            {
              ObjectSystem.cSensor[0].collided = (byte) 0;
              ObjectSystem.cSensor[1].collided = (byte) 0;
              ObjectSystem.cSensor[0].xPos = player.xPos + (PlayerSystem.collisionRight << 16 /*0x10*/);
              ObjectSystem.cSensor[1].xPos = ObjectSystem.cSensor[0].xPos;
              ObjectSystem.cSensor[0].yPos = player.yPos - 131072 /*0x020000*/;
              ObjectSystem.cSensor[1].yPos = player.yPos + 524288 /*0x080000*/;
              for (int index = 0; index < 2; ++index)
              {
                if (ObjectSystem.cSensor[index].xPos >= cLeft && player.xPos - player.xVelocity < cLeft && ObjectSystem.cSensor[1].yPos > cTop && ObjectSystem.cSensor[0].yPos < cBottom)
                  ObjectSystem.cSensor[index].collided = (byte) 1;
              }
              if (ObjectSystem.cSensor[0].collided == (byte) 1 || ObjectSystem.cSensor[1].collided == (byte) 1)
              {
                player.xPos = cLeft - (PlayerSystem.collisionRight << 16 /*0x10*/);
                if (player.xVelocity > 0)
                {
                  if (player.objectPtr.direction == (byte) 0)
                    player.pushing = (byte) 2;
                  player.xVelocity = 0;
                  player.speed = 0;
                }
                ObjectSystem.scriptEng.checkResult = 2;
              }
              else
              {
                ObjectSystem.cSensor[0].collided = (byte) 0;
                ObjectSystem.cSensor[1].collided = (byte) 0;
                ObjectSystem.cSensor[0].xPos = player.xPos + (PlayerSystem.collisionLeft << 16 /*0x10*/);
                ObjectSystem.cSensor[1].xPos = ObjectSystem.cSensor[0].xPos;
                ObjectSystem.cSensor[0].yPos = player.yPos - 131072 /*0x020000*/;
                ObjectSystem.cSensor[1].yPos = player.yPos + 524288 /*0x080000*/;
                for (int index = 0; index < 2; ++index)
                {
                  if (ObjectSystem.cSensor[index].xPos <= cRight && player.xPos - player.xVelocity > cRight && ObjectSystem.cSensor[1].yPos > cTop && ObjectSystem.cSensor[0].yPos < cBottom)
                    ObjectSystem.cSensor[index].collided = (byte) 1;
                }
                if (ObjectSystem.cSensor[0].collided != (byte) 1 && ObjectSystem.cSensor[1].collided != (byte) 1)
                  return;
                player.xPos = cRight - (PlayerSystem.collisionLeft << 16 /*0x10*/);
                if (player.xVelocity < 0)
                {
                  if (player.objectPtr.direction == (byte) 1)
                    player.pushing = (byte) 2;
                  player.xVelocity = 0;
                  player.speed = 0;
                }
                ObjectSystem.scriptEng.checkResult = 3;
              }
            }
          }
        }
      }

      public static void PlatformCollision(int cLeft, int cTop, int cRight, int cBottom)
      {
        PlayerObject player = PlayerSystem.playerList[ObjectSystem.playerNum];
        CollisionBox collisionBox = AnimationSystem.collisionBoxList[player.animationFile.cbListOffset + (int) AnimationSystem.animationFrames[AnimationSystem.animationList[player.animationFile.aniListOffset + (int) player.objectPtr.animation].frameListOffset + (int) player.objectPtr.frame].collisionBox];
        PlayerSystem.collisionLeft = (int) collisionBox.left[0];
        PlayerSystem.collisionTop = (int) collisionBox.top[0];
        PlayerSystem.collisionRight = (int) collisionBox.right[0];
        PlayerSystem.collisionBottom = (int) collisionBox.bottom[0];
        ObjectSystem.cSensor[0].collided = (byte) 0;
        ObjectSystem.cSensor[1].collided = (byte) 0;
        ObjectSystem.cSensor[2].collided = (byte) 0;
        ObjectSystem.cSensor[0].xPos = player.xPos + (PlayerSystem.collisionLeft + 1 << 16 /*0x10*/);
        ObjectSystem.cSensor[1].xPos = player.xPos;
        ObjectSystem.cSensor[2].xPos = player.xPos + (PlayerSystem.collisionRight << 16 /*0x10*/);
        ObjectSystem.cSensor[0].yPos = player.yPos + (PlayerSystem.collisionBottom << 16 /*0x10*/);
        ObjectSystem.cSensor[1].yPos = ObjectSystem.cSensor[0].yPos;
        ObjectSystem.cSensor[2].yPos = ObjectSystem.cSensor[0].yPos;
        ObjectSystem.scriptEng.checkResult = 0;
        for (int index = 0; index < 3; ++index)
        {
          if (ObjectSystem.cSensor[index].xPos > cLeft && ObjectSystem.cSensor[index].xPos < cRight && ObjectSystem.cSensor[index].yPos > cTop - 2 && ObjectSystem.cSensor[index].yPos < cBottom && player.yVelocity >= 0)
          {
            ObjectSystem.cSensor[index].collided = (byte) 1;
            player.flailing[index] = (byte) 1;
          }
        }
        if (ObjectSystem.cSensor[0].collided != (byte) 1 && ObjectSystem.cSensor[1].collided != (byte) 1 && ObjectSystem.cSensor[2].collided != (byte) 1)
          return;
        if (player.gravity == (byte) 0 && (player.collisionMode == (byte) 1 || player.collisionMode == (byte) 3))
        {
          player.xVelocity = 0;
          player.speed = 0;
        }
        player.yPos = cTop - (PlayerSystem.collisionBottom << 16 /*0x10*/);
        player.gravity = (byte) 0;
        player.yVelocity = 0;
        player.angle = 0;
        player.objectPtr.rotation = 0;
        player.controlLock = (byte) 0;
        ObjectSystem.scriptEng.checkResult = 1;
      }

      public static void ObjectFloorCollision(int xOffset, int yOffset, int cPlane)
      {
        ObjectSystem.scriptEng.checkResult = 0;
        int num1 = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16 /*0x10*/) + xOffset;
        int num2 = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos >> 16 /*0x10*/) + yOffset;
        if (num1 <= 0 || num1 >= (int) StageSystem.stageLayouts[0].xSize << 7 || num2 <= 0 || num2 >= (int) StageSystem.stageLayouts[0].ySize << 7)
          return;
        int num3 = num1 >> 7;
        int num4 = (num1 & (int) sbyte.MaxValue) >> 4;
        int num5 = num2 >> 7;
        int num6 = (num2 & (int) sbyte.MaxValue) >> 4;
        int index1 = ((int) StageSystem.stageLayouts[0].tileMap[num3 + (num5 << 8)] << 6) + (num4 + (num6 << 3));
        int num7 = (int) StageSystem.tile128x128.tile16x16[index1];
        if (StageSystem.tile128x128.collisionFlag[cPlane, index1] != (byte) 2 && StageSystem.tile128x128.collisionFlag[cPlane, index1] != (byte) 3)
        {
          switch (StageSystem.tile128x128.direction[index1])
          {
            case 0:
              int index2 = (num1 & 15) + (num7 << 4);
              if ((num2 & 15) > (int) StageSystem.tileCollisions[cPlane].floorMask[index2])
              {
                num2 = (int) StageSystem.tileCollisions[cPlane].floorMask[index2] + (num5 << 7) + (num6 << 4);
                ObjectSystem.scriptEng.checkResult = 1;
                break;
              }
              break;
            case 1:
              int index3 = 15 - (num1 & 15) + (num7 << 4);
              if ((num2 & 15) > (int) StageSystem.tileCollisions[cPlane].floorMask[index3])
              {
                num2 = (int) StageSystem.tileCollisions[cPlane].floorMask[index3] + (num5 << 7) + (num6 << 4);
                ObjectSystem.scriptEng.checkResult = 1;
                break;
              }
              break;
            case 2:
              int index4 = (num1 & 15) + (num7 << 4);
              if ((num2 & 15) > 15 - (int) StageSystem.tileCollisions[cPlane].roofMask[index4])
              {
                num2 = 15 - (int) StageSystem.tileCollisions[cPlane].roofMask[index4] + (num5 << 7) + (num6 << 4);
                ObjectSystem.scriptEng.checkResult = 1;
                break;
              }
              break;
            case 3:
              int index5 = 15 - (num1 & 15) + (num7 << 4);
              if ((num2 & 15) > 15 - (int) StageSystem.tileCollisions[cPlane].roofMask[index5])
              {
                num2 = 15 - (int) StageSystem.tileCollisions[cPlane].roofMask[index5] + (num5 << 7) + (num6 << 4);
                ObjectSystem.scriptEng.checkResult = 1;
                break;
              }
              break;
          }
        }
        if (ObjectSystem.scriptEng.checkResult != 1)
          return;
        ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos = num2 - yOffset << 16 /*0x10*/;
      }

      public static void ObjectLWallCollision(int xOffset, int yOffset, int cPlane)
      {
        ObjectSystem.scriptEng.checkResult = 0;
        int num1 = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16 /*0x10*/) + xOffset;
        int num2 = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos >> 16 /*0x10*/) + yOffset;
        if (num1 <= 0 || num1 >= (int) StageSystem.stageLayouts[0].xSize << 7 || num2 <= 0 || num2 >= (int) StageSystem.stageLayouts[0].ySize << 7)
          return;
        int num3 = num1 >> 7;
        int num4 = (num1 & (int) sbyte.MaxValue) >> 4;
        int num5 = num2 >> 7;
        int num6 = (num2 & (int) sbyte.MaxValue) >> 4;
        int index1 = ((int) StageSystem.stageLayouts[0].tileMap[num3 + (num5 << 8)] << 6) + (num4 + (num6 << 3));
        int num7 = (int) StageSystem.tile128x128.tile16x16[index1];
        if (StageSystem.tile128x128.collisionFlag[cPlane, index1] != (byte) 1 && StageSystem.tile128x128.collisionFlag[cPlane, index1] < (byte) 3)
        {
          switch (StageSystem.tile128x128.direction[index1])
          {
            case 0:
              int index2 = (num2 & 15) + (num7 << 4);
              if ((num1 & 15) > (int) StageSystem.tileCollisions[cPlane].leftWallMask[index2])
              {
                num1 = (int) StageSystem.tileCollisions[cPlane].leftWallMask[index2] + (num3 << 7) + (num4 << 4);
                ObjectSystem.scriptEng.checkResult = 1;
                break;
              }
              break;
            case 1:
              int index3 = (num2 & 15) + (num7 << 4);
              if ((num1 & 15) > 15 - (int) StageSystem.tileCollisions[cPlane].rightWallMask[index3])
              {
                num1 = 15 - (int) StageSystem.tileCollisions[cPlane].rightWallMask[index3] + (num3 << 7) + (num4 << 4);
                ObjectSystem.scriptEng.checkResult = 1;
                break;
              }
              break;
            case 2:
              int index4 = 15 - (num2 & 15) + (num7 << 4);
              if ((num1 & 15) > (int) StageSystem.tileCollisions[cPlane].leftWallMask[index4])
              {
                num1 = (int) StageSystem.tileCollisions[cPlane].leftWallMask[index4] + (num3 << 7) + (num4 << 4);
                ObjectSystem.scriptEng.checkResult = 1;
                break;
              }
              break;
            case 3:
              int index5 = 15 - (num2 & 15) + (num7 << 4);
              if ((num1 & 15) > 15 - (int) StageSystem.tileCollisions[cPlane].rightWallMask[index5])
              {
                num1 = 15 - (int) StageSystem.tileCollisions[cPlane].rightWallMask[index5] + (num3 << 7) + (num4 << 4);
                ObjectSystem.scriptEng.checkResult = 1;
                break;
              }
              break;
          }
        }
        if (ObjectSystem.scriptEng.checkResult != 1)
          return;
        ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos = num1 - xOffset << 16 /*0x10*/;
      }

      public static void ObjectRWallCollision(int xOffset, int yOffset, int cPlane)
      {
        ObjectSystem.scriptEng.checkResult = 0;
        int num1 = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16 /*0x10*/) + xOffset;
        int num2 = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos >> 16 /*0x10*/) + yOffset;
        if (num1 <= 0 || num1 >= (int) StageSystem.stageLayouts[0].xSize << 7 || num2 <= 0 || num2 >= (int) StageSystem.stageLayouts[0].ySize << 7)
          return;
        int num3 = num1 >> 7;
        int num4 = (num1 & (int) sbyte.MaxValue) >> 4;
        int num5 = num2 >> 7;
        int num6 = (num2 & (int) sbyte.MaxValue) >> 4;
        int index1 = ((int) StageSystem.stageLayouts[0].tileMap[num3 + (num5 << 8)] << 6) + (num4 + (num6 << 3));
        int num7 = (int) StageSystem.tile128x128.tile16x16[index1];
        if (StageSystem.tile128x128.collisionFlag[cPlane, index1] != (byte) 1 && StageSystem.tile128x128.collisionFlag[cPlane, index1] < (byte) 3)
        {
          switch (StageSystem.tile128x128.direction[index1])
          {
            case 0:
              int index2 = (num2 & 15) + (num7 << 4);
              if ((num1 & 15) < (int) StageSystem.tileCollisions[cPlane].rightWallMask[index2])
              {
                num1 = (int) StageSystem.tileCollisions[cPlane].rightWallMask[index2] + (num3 << 7) + (num4 << 4);
                ObjectSystem.scriptEng.checkResult = 1;
                break;
              }
              break;
            case 1:
              int index3 = (num2 & 15) + (num7 << 4);
              if ((num1 & 15) < 15 - (int) StageSystem.tileCollisions[cPlane].leftWallMask[index3])
              {
                num1 = 15 - (int) StageSystem.tileCollisions[cPlane].leftWallMask[index3] + (num3 << 7) + (num4 << 4);
                ObjectSystem.scriptEng.checkResult = 1;
                break;
              }
              break;
            case 2:
              int index4 = 15 - (num2 & 15) + (num7 << 4);
              if ((num1 & 15) < (int) StageSystem.tileCollisions[cPlane].rightWallMask[index4])
              {
                num1 = (int) StageSystem.tileCollisions[cPlane].rightWallMask[index4] + (num3 << 7) + (num4 << 4);
                ObjectSystem.scriptEng.checkResult = 1;
                break;
              }
              break;
            case 3:
              int index5 = 15 - (num2 & 15) + (num7 << 4);
              if ((num1 & 15) < 15 - (int) StageSystem.tileCollisions[cPlane].leftWallMask[index5])
              {
                num1 = 15 - (int) StageSystem.tileCollisions[cPlane].leftWallMask[index5] + (num3 << 7) + (num4 << 4);
                ObjectSystem.scriptEng.checkResult = 1;
                break;
              }
              break;
          }
        }
        if (ObjectSystem.scriptEng.checkResult != 1)
          return;
        ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos = num1 - xOffset << 16 /*0x10*/;
      }

      public static void ObjectRoofCollision(int xOffset, int yOffset, int cPlane)
      {
        ObjectSystem.scriptEng.checkResult = 0;
        int num1 = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16 /*0x10*/) + xOffset;
        int num2 = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos >> 16 /*0x10*/) + yOffset;
        if (num1 <= 0 || num1 >= (int) StageSystem.stageLayouts[0].xSize << 7 || num2 <= 0 || num2 >= (int) StageSystem.stageLayouts[0].ySize << 7)
          return;
        int num3 = num1 >> 7;
        int num4 = (num1 & (int) sbyte.MaxValue) >> 4;
        int num5 = num2 >> 7;
        int num6 = (num2 & (int) sbyte.MaxValue) >> 4;
        int index1 = ((int) StageSystem.stageLayouts[0].tileMap[num3 + (num5 << 8)] << 6) + (num4 + (num6 << 3));
        int num7 = (int) StageSystem.tile128x128.tile16x16[index1];
        if (StageSystem.tile128x128.collisionFlag[cPlane, index1] != (byte) 1 && StageSystem.tile128x128.collisionFlag[cPlane, index1] < (byte) 3)
        {
          switch (StageSystem.tile128x128.direction[index1])
          {
            case 0:
              int index2 = (num1 & 15) + (num7 << 4);
              if ((num2 & 15) < (int) StageSystem.tileCollisions[cPlane].roofMask[index2])
              {
                num2 = (int) StageSystem.tileCollisions[cPlane].roofMask[index2] + (num5 << 7) + (num6 << 4);
                ObjectSystem.scriptEng.checkResult = 1;
                break;
              }
              break;
            case 1:
              int index3 = 15 - (num1 & 15) + (num7 << 4);
              if ((num2 & 15) < (int) StageSystem.tileCollisions[cPlane].roofMask[index3])
              {
                num2 = (int) StageSystem.tileCollisions[cPlane].roofMask[index3] + (num5 << 7) + (num6 << 4);
                ObjectSystem.scriptEng.checkResult = 1;
                break;
              }
              break;
            case 2:
              int index4 = (num1 & 15) + (num7 << 4);
              if ((num2 & 15) < 15 - (int) StageSystem.tileCollisions[cPlane].floorMask[index4])
              {
                num2 = 15 - (int) StageSystem.tileCollisions[cPlane].floorMask[index4] + (num5 << 7) + (num6 << 4);
                ObjectSystem.scriptEng.checkResult = 1;
                break;
              }
              break;
            case 3:
              int index5 = 15 - (num1 & 15) + (num7 << 4);
              if ((num2 & 15) < 15 - (int) StageSystem.tileCollisions[cPlane].floorMask[index5])
              {
                num2 = 15 - (int) StageSystem.tileCollisions[cPlane].floorMask[index5] + (num5 << 7) + (num6 << 4);
                ObjectSystem.scriptEng.checkResult = 1;
                break;
              }
              break;
          }
        }
        if (ObjectSystem.scriptEng.checkResult != 1)
          return;
        ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos = num2 - yOffset << 16 /*0x10*/;
      }

      public static void ObjectFloorGrip(int xOffset, int yOffset, int cPlane)
      {
        ObjectSystem.scriptEng.checkResult = 0;
        int num1 = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16 /*0x10*/) + xOffset;
        int num2 = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos >> 16 /*0x10*/) + yOffset;
        int num3 = num2;
        int num4 = num2 - 16 /*0x10*/;
        for (int index1 = 3; index1 > 0; --index1)
        {
          if (num1 > 0 && num1 < (int) StageSystem.stageLayouts[0].xSize << 7 && num4 > 0 && num4 < (int) StageSystem.stageLayouts[0].ySize << 7 && ObjectSystem.scriptEng.checkResult == 0)
          {
            int num5 = num1 >> 7;
            int num6 = (num1 & (int) sbyte.MaxValue) >> 4;
            int num7 = num4 >> 7;
            int num8 = (num4 & (int) sbyte.MaxValue) >> 4;
            int index2 = ((int) StageSystem.stageLayouts[0].tileMap[num5 + (num7 << 8)] << 6) + (num6 + (num8 << 3));
            int num9 = (int) StageSystem.tile128x128.tile16x16[index2];
            if (StageSystem.tile128x128.collisionFlag[cPlane, index2] != (byte) 2 && StageSystem.tile128x128.collisionFlag[cPlane, index2] != (byte) 3)
            {
              switch (StageSystem.tile128x128.direction[index2])
              {
                case 0:
                  int index3 = (num1 & 15) + (num9 << 4);
                  if (StageSystem.tileCollisions[cPlane].floorMask[index3] < (sbyte) 64 /*0x40*/)
                  {
                    ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos = (int) StageSystem.tileCollisions[cPlane].floorMask[index3] + (num7 << 7) + (num8 << 4);
                    ObjectSystem.scriptEng.checkResult = 1;
                    break;
                  }
                  break;
                case 1:
                  int index4 = 15 - (num1 & 15) + (num9 << 4);
                  if (StageSystem.tileCollisions[cPlane].floorMask[index4] < (sbyte) 64 /*0x40*/)
                  {
                    ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos = (int) StageSystem.tileCollisions[cPlane].floorMask[index4] + (num7 << 7) + (num8 << 4);
                    ObjectSystem.scriptEng.checkResult = 1;
                    break;
                  }
                  break;
                case 2:
                  int index5 = (num1 & 15) + (num9 << 4);
                  if (StageSystem.tileCollisions[cPlane].roofMask[index5] > (sbyte) -64 /*0xC0*/)
                  {
                    ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos = 15 - (int) StageSystem.tileCollisions[cPlane].roofMask[index5] + (num7 << 7) + (num8 << 4);
                    ObjectSystem.scriptEng.checkResult = 1;
                    break;
                  }
                  break;
                case 3:
                  int index6 = 15 - (num1 & 15) + (num9 << 4);
                  if (StageSystem.tileCollisions[cPlane].roofMask[index6] > (sbyte) -64 /*0xC0*/)
                  {
                    ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos = 15 - (int) StageSystem.tileCollisions[cPlane].roofMask[index6] + (num7 << 7) + (num8 << 4);
                    ObjectSystem.scriptEng.checkResult = 1;
                    break;
                  }
                  break;
              }
            }
          }
          num4 += 16 /*0x10*/;
        }
        if (ObjectSystem.scriptEng.checkResult != 1)
          return;
        if (Math.Abs(ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos - num3) < 16 /*0x10*/)
        {
          ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos = ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos - yOffset << 16 /*0x10*/;
        }
        else
        {
          ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos = num3 - yOffset << 16 /*0x10*/;
          ObjectSystem.scriptEng.checkResult = 0;
        }
      }

      public static void ObjectLWallGrip(int xOffset, int yOffset, int cPlane)
      {
        ObjectSystem.scriptEng.checkResult = 0;
        int num1 = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16 /*0x10*/) + xOffset;
        int num2 = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos >> 16 /*0x10*/) + yOffset;
        int num3 = num1;
        int num4 = num1 - 16 /*0x10*/;
        for (int index1 = 3; index1 > 0; --index1)
        {
          if (num4 > 0 && num4 < (int) StageSystem.stageLayouts[0].xSize << 7 && num2 > 0 && num2 < (int) StageSystem.stageLayouts[0].ySize << 7 && ObjectSystem.scriptEng.checkResult == 0)
          {
            int num5 = num4 >> 7;
            int num6 = (num4 & (int) sbyte.MaxValue) >> 4;
            int num7 = num2 >> 7;
            int num8 = (num2 & (int) sbyte.MaxValue) >> 4;
            int index2 = ((int) StageSystem.stageLayouts[0].tileMap[num5 + (num7 << 8)] << 6) + (num6 + (num8 << 3));
            int num9 = (int) StageSystem.tile128x128.tile16x16[index2];
            if (StageSystem.tile128x128.collisionFlag[cPlane, index2] < (byte) 3)
            {
              switch (StageSystem.tile128x128.direction[index2])
              {
                case 0:
                  int index3 = (num2 & 15) + (num9 << 4);
                  if (StageSystem.tileCollisions[cPlane].leftWallMask[index3] < (sbyte) 64 /*0x40*/)
                  {
                    ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos = (int) StageSystem.tileCollisions[cPlane].leftWallMask[index3] + (num5 << 7) + (num6 << 4);
                    ObjectSystem.scriptEng.checkResult = 1;
                    break;
                  }
                  break;
                case 1:
                  int index4 = (num2 & 15) + (num9 << 4);
                  if (StageSystem.tileCollisions[cPlane].rightWallMask[index4] > (sbyte) -64 /*0xC0*/)
                  {
                    ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos = 15 - (int) StageSystem.tileCollisions[cPlane].rightWallMask[index4] + (num5 << 7) + (num6 << 4);
                    ObjectSystem.scriptEng.checkResult = 1;
                    break;
                  }
                  break;
                case 2:
                  int index5 = 15 - (num2 & 15) + (num9 << 4);
                  if (StageSystem.tileCollisions[cPlane].leftWallMask[index5] < (sbyte) 64 /*0x40*/)
                  {
                    ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos = (int) StageSystem.tileCollisions[cPlane].leftWallMask[index5] + (num5 << 7) + (num6 << 4);
                    ObjectSystem.scriptEng.checkResult = 1;
                    break;
                  }
                  break;
                case 3:
                  int index6 = 15 - (num2 & 15) + (num9 << 4);
                  if (StageSystem.tileCollisions[cPlane].rightWallMask[index6] > (sbyte) -64 /*0xC0*/)
                  {
                    ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos = 15 - (int) StageSystem.tileCollisions[cPlane].rightWallMask[index6] + (num5 << 7) + (num6 << 4);
                    ObjectSystem.scriptEng.checkResult = 1;
                    break;
                  }
                  break;
              }
            }
          }
          num4 += 16 /*0x10*/;
        }
        if (ObjectSystem.scriptEng.checkResult != 1)
          return;
        if (Math.Abs(ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos - num3) < 16 /*0x10*/)
        {
          ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos = ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos - xOffset << 16 /*0x10*/;
        }
        else
        {
          ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos = num3 - xOffset << 16 /*0x10*/;
          ObjectSystem.scriptEng.checkResult = 0;
        }
      }

      public static void ObjectRWallGrip(int xOffset, int yOffset, int cPlane)
      {
        ObjectSystem.scriptEng.checkResult = 0;
        int num1 = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16 /*0x10*/) + xOffset;
        int num2 = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos >> 16 /*0x10*/) + yOffset;
        int num3 = num1;
        int num4 = num1 + 16 /*0x10*/;
        for (int index1 = 3; index1 > 0; --index1)
        {
          if (num4 > 0 && num4 < (int) StageSystem.stageLayouts[0].xSize << 7 && num2 > 0 && num2 < (int) StageSystem.stageLayouts[0].ySize << 7 && ObjectSystem.scriptEng.checkResult == 0)
          {
            int num5 = num4 >> 7;
            int num6 = (num4 & (int) sbyte.MaxValue) >> 4;
            int num7 = num2 >> 7;
            int num8 = (num2 & (int) sbyte.MaxValue) >> 4;
            int index2 = ((int) StageSystem.stageLayouts[0].tileMap[num5 + (num7 << 8)] << 6) + (num6 + (num8 << 3));
            int num9 = (int) StageSystem.tile128x128.tile16x16[index2];
            if (StageSystem.tile128x128.collisionFlag[cPlane, index2] < (byte) 3)
            {
              switch (StageSystem.tile128x128.direction[index2])
              {
                case 0:
                  int index3 = (num2 & 15) + (num9 << 4);
                  if (StageSystem.tileCollisions[cPlane].rightWallMask[index3] > (sbyte) -64 /*0xC0*/)
                  {
                    ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos = (int) StageSystem.tileCollisions[cPlane].rightWallMask[index3] + (num5 << 7) + (num6 << 4);
                    ObjectSystem.scriptEng.checkResult = 1;
                    break;
                  }
                  break;
                case 1:
                  int index4 = (num2 & 15) + (num9 << 4);
                  if (StageSystem.tileCollisions[cPlane].leftWallMask[index4] < (sbyte) 64 /*0x40*/)
                  {
                    ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos = 15 - (int) StageSystem.tileCollisions[cPlane].leftWallMask[index4] + (num5 << 7) + (num6 << 4);
                    ObjectSystem.scriptEng.checkResult = 1;
                    break;
                  }
                  break;
                case 2:
                  int index5 = 15 - (num2 & 15) + (num9 << 4);
                  if (StageSystem.tileCollisions[cPlane].rightWallMask[index5] > (sbyte) -64 /*0xC0*/)
                  {
                    ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos = (int) StageSystem.tileCollisions[cPlane].rightWallMask[index5] + (num5 << 7) + (num6 << 4);
                    ObjectSystem.scriptEng.checkResult = 1;
                    break;
                  }
                  break;
                case 3:
                  int index6 = 15 - (num2 & 15) + (num9 << 4);
                  if (StageSystem.tileCollisions[cPlane].leftWallMask[index6] < (sbyte) 64 /*0x40*/)
                  {
                    ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos = 15 - (int) StageSystem.tileCollisions[cPlane].leftWallMask[index6] + (num5 << 7) + (num6 << 4);
                    ObjectSystem.scriptEng.checkResult = 1;
                    break;
                  }
                  break;
              }
            }
          }
          num4 -= 16 /*0x10*/;
        }
        if (ObjectSystem.scriptEng.checkResult != 1)
          return;
        if (Math.Abs(ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos - num3) < 16 /*0x10*/)
        {
          ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos = ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos - xOffset << 16 /*0x10*/;
        }
        else
        {
          ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos = num3 - xOffset << 16 /*0x10*/;
          ObjectSystem.scriptEng.checkResult = 0;
        }
      }

      public static void ObjectRoofGrip(int xOffset, int yOffset, int cPlane)
      {
        ObjectSystem.scriptEng.checkResult = 0;
        int num1 = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16 /*0x10*/) + xOffset;
        int num2 = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos >> 16 /*0x10*/) + yOffset;
        int num3 = num2;
        int num4 = num2 + 16 /*0x10*/;
        for (int index1 = 3; index1 > 0; --index1)
        {
          if (num1 > 0 && num1 < (int) StageSystem.stageLayouts[0].xSize << 7 && num4 > 0 && num4 < (int) StageSystem.stageLayouts[0].ySize << 7 && ObjectSystem.scriptEng.checkResult == 0)
          {
            int num5 = num1 >> 7;
            int num6 = (num1 & (int) sbyte.MaxValue) >> 4;
            int num7 = num4 >> 7;
            int num8 = (num4 & (int) sbyte.MaxValue) >> 4;
            int index2 = ((int) StageSystem.stageLayouts[0].tileMap[num5 + (num7 << 8)] << 6) + (num6 + (num8 << 3));
            int num9 = (int) StageSystem.tile128x128.tile16x16[index2];
            if (StageSystem.tile128x128.collisionFlag[cPlane, index2] < (byte) 3)
            {
              switch (StageSystem.tile128x128.direction[index2])
              {
                case 0:
                  int index3 = (num1 & 15) + (num9 << 4);
                  if (StageSystem.tileCollisions[cPlane].roofMask[index3] > (sbyte) -64 /*0xC0*/)
                  {
                    ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos = (int) StageSystem.tileCollisions[cPlane].roofMask[index3] + (num7 << 7) + (num8 << 4);
                    ObjectSystem.scriptEng.checkResult = 1;
                    break;
                  }
                  break;
                case 1:
                  int index4 = 15 - (num1 & 15) + (num9 << 4);
                  if (StageSystem.tileCollisions[cPlane].roofMask[index4] > (sbyte) -64 /*0xC0*/)
                  {
                    ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos = (int) StageSystem.tileCollisions[cPlane].roofMask[index4] + (num7 << 7) + (num8 << 4);
                    ObjectSystem.scriptEng.checkResult = 1;
                    break;
                  }
                  break;
                case 2:
                  int index5 = (num1 & 15) + (num9 << 4);
                  if (StageSystem.tileCollisions[cPlane].floorMask[index5] < (sbyte) 64 /*0x40*/)
                  {
                    ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos = 15 - (int) StageSystem.tileCollisions[cPlane].floorMask[index5] + (num7 << 7) + (num8 << 4);
                    ObjectSystem.scriptEng.checkResult = 1;
                    break;
                  }
                  break;
                case 3:
                  int index6 = 15 - (num1 & 15) + (num9 << 4);
                  if (StageSystem.tileCollisions[cPlane].floorMask[index6] < (sbyte) 64 /*0x40*/)
                  {
                    ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos = 15 - (int) StageSystem.tileCollisions[cPlane].floorMask[index6] + (num7 << 7) + (num8 << 4);
                    ObjectSystem.scriptEng.checkResult = 1;
                    break;
                  }
                  break;
              }
            }
          }
          num4 -= 16 /*0x10*/;
        }
        if (ObjectSystem.scriptEng.checkResult != 1)
          return;
        if (Math.Abs(ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos - num3) < 16 /*0x10*/)
        {
          ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos = ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos - yOffset << 16 /*0x10*/;
        }
        else
        {
          ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos = num3 - yOffset << 16 /*0x10*/;
          ObjectSystem.scriptEng.checkResult = 0;
        }
      }
    }
}