using UnityEngine;
using System;
using System.IO;
using System.IO.IsolatedStorage;

namespace Retro_Engine
{

    public static class FileIO
    {
      public const int PRESENTATION_STAGE = 0;
      public const int ZONE_STAGE = 1;
      public const int BONUS_STAGE = 2;
      public const int SPECIAL_STAGE = 3;
      public static byte[] fileBuffer = new byte[8192 /*0x2000*/];
      public static uint bufferPosition = 0;
      public static uint fileSize = 0;
      public static uint readSize = 0;
      public static uint readPos = 0;
      public static bool useRSDKFile = false;
      public static bool useByteCode = false;
      public static uint vFileSize = 0;
      public static uint virtualFileOffset = 0;
      public static int[] saveRAM = new int[8192 /*0x2000*/];
      public static char[] encryptionStringA = "4RaS9D7KaEbxcp2o5r6t".ToCharArray();
      public static char[] encryptionStringB = "3tRaUxLmEaSn".ToCharArray();
      public static byte eStringPosA;
      public static byte eStringPosB;
      public static byte eStringNo;
      public static bool eNybbleSwap;
      public static char[] currentStageFolder = new char[8];
      public static StageList[] pStageList = new StageList[64 /*0x40*/];
      public static StageList[] zStageList = new StageList[128 /*0x80*/];
      public static StageList[] bStageList = new StageList[64 /*0x40*/];
      public static StageList[] sStageList = new StageList[64 /*0x40*/];
      public static byte activeStageList;
      public static byte noPresentationStages;
      public static byte noZoneStages;
      public static byte noBonusStages;
      public static byte noSpecialStages;
      public static int actNumber;
      private static Stream fileReader;

      static FileIO()
      {
        for (int index = 0; index < FileIO.pStageList.Length; ++index)
          FileIO.pStageList[index] = new StageList();
        for (int index = 0; index < FileIO.zStageList.Length; ++index)
          FileIO.zStageList[index] = new StageList();
        for (int index = 0; index < FileIO.bStageList.Length; ++index)
          FileIO.bStageList[index] = new StageList();
        for (int index = 0; index < FileIO.sStageList.Length; ++index)
          FileIO.sStageList[index] = new StageList();
      }

      public static void StrCopy(ref char[] strA, ref char[] strB)
      {
        int index = 0;
        bool flag = true;
        if (index == strB.Length || index == strA.Length)
          flag = false;
        while (flag)
        {
          strA[index] = strB[index];
          if (strB[index] == char.MinValue)
            flag = false;
          ++index;
          if (index == strB.Length || index == strA.Length)
            flag = false;
        }
        for (; index < strA.Length; ++index)
          strA[index] = char.MinValue;
      }

      public static void StrClear(ref char[] strA)
      {
        for (int index = 0; index < strA.Length; ++index)
          strA[index] = char.MinValue;
      }

      public static void StrCopy2D(ref char[,] strA, ref char[] strB, int strPos)
      {
        int index = 0;
        bool flag = true;
        if (index == strB.Length || index == strA.GetLength(1))
          flag = false;
        while (flag)
        {
          strA[strPos, index] = strB[index];
          if (strB[index] == char.MinValue)
            flag = false;
          ++index;
          if (index == strB.Length || index == strA.GetLength(1))
            flag = false;
        }
        for (; index < strA.GetLength(1); ++index)
          strA[strPos, index] = char.MinValue;
      }

      public static void StrAdd(ref char[] strA, ref char[] strB)
      {
        int index1 = 0;
        int index2 = 0;
        bool flag = true;
        while (strA[index1] != char.MinValue)
          ++index1;
        if (index2 == strB.Length || index1 == strA.Length)
          flag = false;
        while (flag)
        {
          strA[index1] = strB[index2];
          if (strB[index2] == char.MinValue)
            flag = false;
          ++index1;
          ++index2;
          if (index2 == strB.Length || index1 == strA.Length)
            flag = false;
        }
        for (; index1 < strA.Length; ++index1)
          strA[index1] = char.MinValue;
      }

      public static bool StringComp(ref char[] strA, ref char[] strB)
      {
        bool flag = true;
        int num = 0;
        int index1 = 0;
        int index2 = 0;
        while (num < 1)
        {
          if ((int) strA[index1] != (int) strB[index2] && (int) strA[index1] != (int) strB[index2] + 32 /*0x20*/ && (int) strA[index1] != (int) strB[index2] - 32 /*0x20*/)
          {
            flag = false;
            num = 1;
          }
          else if (strA[index1] == char.MinValue)
          {
            num = 1;
          }
          else
          {
            ++index1;
            ++index2;
          }
        }
        return flag;
      }

      public static int StringLength(ref char[] strA)
      {
        int index = 0;
        if (strA.Length == 0)
          return index;
        while (strA[index] != char.MinValue && index < strA.Length)
          ++index;
        return index;
      }

      public static int FindStringToken(ref char[] strA, ref char[] token, char instance)
      {
        int stringToken1 = 0;
        int stringToken2 = -1;
        int num = 0;
        for (; strA[stringToken1] != char.MinValue; ++stringToken1)
        {
          int index = 0;
          bool flag = true;
          for (; token[index] != char.MinValue; ++index)
          {
            if (strA[stringToken1 + index] == char.MinValue)
              return stringToken2;
            if ((int) strA[stringToken1 + index] != (int) token[index])
              flag = false;
          }
          if (flag)
          {
            ++num;
            if (num == (int) instance)
              return stringToken1;
          }
        }
        return stringToken2;
      }

      public static bool ConvertStringToInteger(ref char[] strA, ref int sValue)
      {
        int index1 = 0;
        bool flag = false;
        sValue = 0;
        if ((strA[index1] <= '/' || strA[index1] >= ':') && strA[index1] != '-' && strA[index1] != '+')
          return false;
        int num1 = FileIO.StringLength(ref strA) - 1;
        if (strA[index1] == '-')
        {
          flag = true;
          ++index1;
          --num1;
        }
        else if (strA[index1] == '+')
        {
          ++index1;
          --num1;
        }
        while (num1 > -1)
        {
          if (strA[index1] <= '/' || strA[index1] >= ':')
            return false;
          if (num1 > 0)
          {
            int num2 = (int) strA[index1] - 48 /*0x30*/;
            for (int index2 = num1; index2 > 0; --index2)
              num2 *= 10;
            sValue += num2;
          }
          else
            sValue += (int) strA[index1] - 48 /*0x30*/;
          --num1;
          ++index1;
        }
        if (flag)
          sValue = -sValue;
        return true;
      }

      public static bool CheckRSDKFile()
      {
        FileData fData = new FileData();
        FileIO.fileReader = File.OpenRead(Path.Combine(Application.streamingAssetsPath, "Data.rsdk"));
        long len = FileIO.fileReader.Length;
        if (len > 0L)
        {
          FileIO.useRSDKFile = true;
          FileIO.useByteCode = false;
          FileIO.fileReader.Close();
          if (!FileIO.LoadFile("Data/Scripts/ByteCode/GlobalCode.bin".ToCharArray(), fData))
            return false;
          FileIO.useByteCode = true;
          FileIO.CloseFile();
          return true;
        }
        FileIO.fileReader.Close();
        FileIO.useRSDKFile = false;
        FileIO.useByteCode = false;
        return false;
      }

      public static bool LoadFile(char[] filePath, FileData fData)
      {
        int index = 0;
        bool flag = true;
        while (flag)
        {
          fData.fileName[index] = filePath[index];
          if (filePath[index] == char.MinValue)
            flag = false;
          ++index;
          if (index == filePath.Length)
            flag = false;
        }
        for (; index < fData.fileName.Length; ++index)
          fData.fileName[index] = char.MinValue;
        if (FileIO.fileReader != null)
          FileIO.fileReader.Close();
        FileIO.fileReader = File.OpenRead(Path.Combine(Application.streamingAssetsPath, "Data.rsdk"));
        FileIO.fileSize = (uint) FileIO.fileReader.Length;
        FileIO.bufferPosition = 0U;
        FileIO.readSize = 0U;
        FileIO.readPos = 0U;
        if (!FileIO.ParseVirtualFileSystem(fData.fileName))
        {
          FileIO.fileReader.Close();
          return false;
        }
        fData.fileSize = FileIO.vFileSize;
        fData.virtualFileOffset = FileIO.virtualFileOffset;
        FileIO.bufferPosition = 0U;
        FileIO.readSize = 0U;
        return true;
      }

      public static void CloseFile()
      {
        if (FileIO.fileReader != null)
          FileIO.fileReader.Close();
      }

      public static bool CheckCurrentStageFolder(int sNumber)
      {
        switch (FileIO.activeStageList)
        {
          case 0:
            if (FileIO.StringComp(ref FileIO.currentStageFolder, ref FileIO.pStageList[sNumber].stageFolderName))
              return true;
            FileIO.StrCopy(ref FileIO.currentStageFolder, ref FileIO.pStageList[sNumber].stageFolderName);
            return false;
          case 1:
            if (FileIO.StringComp(ref FileIO.currentStageFolder, ref FileIO.zStageList[sNumber].stageFolderName))
              return true;
            FileIO.StrCopy(ref FileIO.currentStageFolder, ref FileIO.zStageList[sNumber].stageFolderName);
            return false;
          case 2:
            if (FileIO.StringComp(ref FileIO.currentStageFolder, ref FileIO.bStageList[sNumber].stageFolderName))
              return true;
            FileIO.StrCopy(ref FileIO.currentStageFolder, ref FileIO.bStageList[sNumber].stageFolderName);
            return false;
          case 3:
            if (FileIO.StringComp(ref FileIO.currentStageFolder, ref FileIO.sStageList[sNumber].stageFolderName))
              return true;
            FileIO.StrCopy(ref FileIO.currentStageFolder, ref FileIO.sStageList[sNumber].stageFolderName);
            return false;
          default:
            return false;
        }
      }

      public static void ResetCurrentStageFolder()
      {
        char[] charArray = "".ToCharArray();
        FileIO.StrCopy(ref FileIO.currentStageFolder, ref charArray);
      }

      public static bool LoadStageFile(char[] filePath, int sNumber, FileData fData)
      {
        char[] strA = new char[64 /*0x40*/];
        char[] charArray1 = "Data/Stages/".ToCharArray();
        FileIO.StrCopy(ref strA, ref charArray1);
        switch (FileIO.activeStageList)
        {
          case 0:
            FileIO.StrAdd(ref strA, ref FileIO.pStageList[sNumber].stageFolderName);
            break;
          case 1:
            FileIO.StrAdd(ref strA, ref FileIO.zStageList[sNumber].stageFolderName);
            break;
          case 2:
            FileIO.StrAdd(ref strA, ref FileIO.bStageList[sNumber].stageFolderName);
            break;
          case 3:
            FileIO.StrAdd(ref strA, ref FileIO.sStageList[sNumber].stageFolderName);
            break;
        }
        char[] charArray2 = "/".ToCharArray();
        FileIO.StrAdd(ref strA, ref charArray2);
        FileIO.StrAdd(ref strA, ref filePath);
        return FileIO.LoadFile(strA, fData);
      }

      public static bool LoadActFile(char[] filePath, int sNumber, FileData fData)
      {
        char[] strA = new char[64 /*0x40*/];
        char[] charArray1 = "Data/Stages/".ToCharArray();
        FileIO.StrCopy(ref strA, ref charArray1);
        switch (FileIO.activeStageList)
        {
          case 0:
            FileIO.StrAdd(ref strA, ref FileIO.pStageList[sNumber].stageFolderName);
            break;
          case 1:
            FileIO.StrAdd(ref strA, ref FileIO.zStageList[sNumber].stageFolderName);
            break;
          case 2:
            FileIO.StrAdd(ref strA, ref FileIO.bStageList[sNumber].stageFolderName);
            break;
          case 3:
            FileIO.StrAdd(ref strA, ref FileIO.sStageList[sNumber].stageFolderName);
            break;
        }
        char[] charArray2 = "/Act".ToCharArray();
        FileIO.StrAdd(ref strA, ref charArray2);
        switch (FileIO.activeStageList)
        {
          case 0:
            FileIO.StrAdd(ref strA, ref FileIO.pStageList[sNumber].actNumber);
            FileIO.ConvertStringToInteger(ref FileIO.pStageList[sNumber].actNumber, ref FileIO.actNumber);
            break;
          case 1:
            FileIO.StrAdd(ref strA, ref FileIO.zStageList[sNumber].actNumber);
            FileIO.ConvertStringToInteger(ref FileIO.zStageList[sNumber].actNumber, ref FileIO.actNumber);
            break;
          case 2:
            FileIO.StrAdd(ref strA, ref FileIO.bStageList[sNumber].actNumber);
            FileIO.ConvertStringToInteger(ref FileIO.bStageList[sNumber].actNumber, ref FileIO.actNumber);
            break;
          case 3:
            FileIO.StrAdd(ref strA, ref FileIO.sStageList[sNumber].actNumber);
            FileIO.ConvertStringToInteger(ref FileIO.sStageList[sNumber].actNumber, ref FileIO.actNumber);
            break;
        }
        FileIO.StrAdd(ref strA, ref filePath);
        return FileIO.LoadFile(strA, fData);
      }

      public static bool ParseVirtualFileSystem(char[] filePath)
      {
        char[] strA1 = new char[64 /*0x40*/];
        char[] strA2 = new char[64 /*0x40*/];
        char[] strB = new char[64 /*0x40*/];
        int num1 = 0;
        int index1 = 0;
        FileIO.virtualFileOffset = 0U;
        for (int index2 = 0; filePath[index2] != char.MinValue; ++index2)
        {
          if (filePath[index2] == '/')
          {
            num1 = index2;
            index1 = 0;
          }
          else
            ++index1;
          strA1[index2] = filePath[index2];
        }
        int index3 = num1 + 1;
        for (int index4 = 0; index4 < index1; ++index4)
          strA2[index4] = filePath[index3 + index4];
        strA2[index1] = char.MinValue;
        strA1[index3] = char.MinValue;
        FileIO.fileReader.Seek(0L, SeekOrigin.Begin);
        FileIO.useRSDKFile = false;
        FileIO.bufferPosition = 0U;
        FileIO.readSize = 0U;
        FileIO.readPos = 0U;
        int num2 = (int) FileIO.ReadByte() + ((int) FileIO.ReadByte() << 8) + ((int) FileIO.ReadByte() << 16 /*0x10*/) + ((int) FileIO.ReadByte() << 24);
        byte num3 = (byte) ((uint) FileIO.ReadByte() + (uint) (byte) ((uint) FileIO.ReadByte() << 8));
        int num4 = 0;
        int num5 = 0;
        while (num4 < (int) num3)
        {
          byte num6 = FileIO.ReadByte();
          int index5;
          for (index5 = 0; index5 < (int) num6; ++index5)
            strB[index5] = (char) ((uint) FileIO.ReadByte() ^ (uint) byte.MaxValue - (uint) num6);
          strB[index5] = char.MinValue;
          if ((!FileIO.StringComp(ref strA1, ref strB) ? 0 : 1) == 1)
          {
            num4 = (int) num3;
            num5 = (int) FileIO.ReadByte() + ((int) FileIO.ReadByte() << 8) + ((int) FileIO.ReadByte() << 16 /*0x10*/) + ((int) FileIO.ReadByte() << 24);
          }
          else
          {
            num5 = -1;
            byte num7 = FileIO.ReadByte();
            num7 = FileIO.ReadByte();
            num7 = FileIO.ReadByte();
            num7 = FileIO.ReadByte();
            ++num4;
          }
        }
        if (num5 == -1)
        {
          FileIO.useRSDKFile = true;
          return false;
        }
        FileIO.fileReader.Seek((long) (num2 + num5), SeekOrigin.Begin);
        FileIO.bufferPosition = 0U;
        FileIO.readSize = 0U;
        FileIO.readPos = 0U;
        FileIO.virtualFileOffset = (uint) (num2 + num5);
        int num8 = 0;
        while (num8 < 1)
        {
          byte num9 = FileIO.ReadByte();
          ++FileIO.virtualFileOffset;
          int index6 = 0;
          while (index6 < (int) num9)
          {
            strB[index6] = (char) ((uint) FileIO.ReadByte() ^ (uint) byte.MaxValue);
            ++index6;
            ++FileIO.virtualFileOffset;
          }
          strB[index6] = char.MinValue;
          if (FileIO.StringComp(ref strA2, ref strB))
          {
            num8 = 1;
            int num10 = (int) FileIO.ReadByte() + ((int) FileIO.ReadByte() << 8) + ((int) FileIO.ReadByte() << 16 /*0x10*/) + ((int) FileIO.ReadByte() << 24);
            FileIO.virtualFileOffset += 4U;
            FileIO.vFileSize = (uint) num10;
            FileIO.fileReader.Seek((long) FileIO.virtualFileOffset, SeekOrigin.Begin);
            FileIO.bufferPosition = 0U;
            FileIO.readSize = 0U;
            FileIO.readPos = FileIO.virtualFileOffset;
          }
          else
          {
            int num11 = (int) FileIO.ReadByte() + ((int) FileIO.ReadByte() << 8) + ((int) FileIO.ReadByte() << 16 /*0x10*/) + ((int) FileIO.ReadByte() << 24);
            FileIO.virtualFileOffset += 4U;
            FileIO.virtualFileOffset += (uint) num11;
            FileIO.fileReader.Seek((long) FileIO.virtualFileOffset, SeekOrigin.Begin);
            FileIO.bufferPosition = 0U;
            FileIO.readSize = 0U;
            FileIO.readPos = FileIO.virtualFileOffset;
          }
        }
        FileIO.eStringNo = (byte) ((FileIO.vFileSize & 508U) >> 2);
        FileIO.eStringPosB = (byte) (1 + (int) FileIO.eStringNo % 9);
        FileIO.eStringPosA = (byte) (1 + (int) FileIO.eStringNo % (int) FileIO.eStringPosB);
        FileIO.eNybbleSwap = false;
        FileIO.useRSDKFile = true;
        return true;
      }

      public static byte ReadByte()
      {
        byte num1 = 0;
        if (FileIO.readPos <= FileIO.fileSize)
        {
          if (!FileIO.useRSDKFile)
          {
            if ((int) FileIO.bufferPosition == (int) FileIO.readSize)
              FileIO.FillFileBuffer();
            num1 = FileIO.fileBuffer[(int) FileIO.bufferPosition];
            ++FileIO.bufferPosition;
          }
          else
          {
            if ((int) FileIO.bufferPosition == (int) FileIO.readSize)
              FileIO.FillFileBuffer();
            byte num2 = (byte) ((uint) FileIO.fileBuffer[(int) FileIO.bufferPosition] ^ (uint) FileIO.eStringNo ^ (uint) FileIO.encryptionStringB[(int) FileIO.eStringPosB]);
            if (FileIO.eNybbleSwap)
              num2 = (byte) (((int) num2 >> 4) + (((int) num2 & 15) << 4));
            num1 = (byte) ((uint) num2 ^ (uint) (byte) FileIO.encryptionStringA[(int) FileIO.eStringPosA]);
            ++FileIO.eStringPosA;
            ++FileIO.eStringPosB;
            if (FileIO.eStringPosA > (byte) 19 && FileIO.eStringPosB > (byte) 11)
            {
              ++FileIO.eStringNo;
              FileIO.eStringNo &= (byte) 127 /*0x7F*/;
              if (!FileIO.eNybbleSwap)
              {
                FileIO.eNybbleSwap = true;
                FileIO.eStringPosA = (byte) (3 + (int) FileIO.eStringNo % 15);
                FileIO.eStringPosB = (byte) (1 + (int) FileIO.eStringNo % 7);
              }
              else
              {
                FileIO.eNybbleSwap = false;
                FileIO.eStringPosA = (byte) (6 + (int) FileIO.eStringNo % 12);
                FileIO.eStringPosB = (byte) (4 + (int) FileIO.eStringNo % 5);
              }
            }
            else
            {
              if (FileIO.eStringPosA > (byte) 19)
              {
                FileIO.eStringPosA = (byte) 1;
                FileIO.eNybbleSwap = !FileIO.eNybbleSwap;
              }
              if (FileIO.eStringPosB > (byte) 11)
              {
                FileIO.eStringPosB = (byte) 1;
                FileIO.eNybbleSwap = !FileIO.eNybbleSwap;
              }
            }
            ++FileIO.bufferPosition;
          }
        }
        return num1;
      }

      public static void ReadByteArray(ref byte[] byteP, int numBytes)
      {
        int index = 0;
        if (FileIO.readPos > FileIO.fileSize)
          return;
        if (!FileIO.useRSDKFile)
        {
          for (; numBytes > 0; --numBytes)
          {
            if ((int) FileIO.bufferPosition == (int) FileIO.readSize)
              FileIO.FillFileBuffer();
            byteP[index] = FileIO.fileBuffer[(int) FileIO.bufferPosition];
            ++FileIO.bufferPosition;
            ++index;
          }
        }
        else
        {
          for (; numBytes > 0; --numBytes)
          {
            if (FileIO.bufferPosition >= FileIO.readSize)
              FileIO.FillFileBuffer();
            byteP[index] = (byte) ((uint) FileIO.fileBuffer[(int) FileIO.bufferPosition] ^ (uint) FileIO.eStringNo ^ (uint) FileIO.encryptionStringB[(int) FileIO.eStringPosB]);
            if (FileIO.eNybbleSwap)
              byteP[index] = (byte) (((int) byteP[index] >> 4) + (((int) byteP[index] & 15) << 4));
            byteP[index] ^= (byte) FileIO.encryptionStringA[(int) FileIO.eStringPosA];
            ++FileIO.eStringPosA;
            ++FileIO.eStringPosB;
            if (FileIO.eStringPosA > (byte) 19 && FileIO.eStringPosB > (byte) 11)
            {
              ++FileIO.eStringNo;
              FileIO.eStringNo &= (byte) 127 /*0x7F*/;
              if (!FileIO.eNybbleSwap)
              {
                FileIO.eNybbleSwap = true;
                FileIO.eStringPosA = (byte) (3 + (int) FileIO.eStringNo % 15);
                FileIO.eStringPosB = (byte) (1 + (int) FileIO.eStringNo % 7);
              }
              else
              {
                FileIO.eNybbleSwap = false;
                FileIO.eStringPosA = (byte) (6 + (int) FileIO.eStringNo % 12);
                FileIO.eStringPosB = (byte) (4 + (int) FileIO.eStringNo % 5);
              }
            }
            else
            {
              if (FileIO.eStringPosA > (byte) 19)
              {
                FileIO.eStringPosA = (byte) 1;
                FileIO.eNybbleSwap = !FileIO.eNybbleSwap;
              }
              if (FileIO.eStringPosB > (byte) 11)
              {
                FileIO.eStringPosB = (byte) 1;
                FileIO.eNybbleSwap = !FileIO.eNybbleSwap;
              }
            }
            ++FileIO.bufferPosition;
            ++index;
          }
        }
      }

      public static void ReadCharArray(ref char[] charP, int numBytes)
      {
        int index = 0;
        if (FileIO.readPos > FileIO.fileSize)
          return;
        if (!FileIO.useRSDKFile)
        {
          for (; numBytes > 0; --numBytes)
          {
            if ((int) FileIO.bufferPosition == (int) FileIO.readSize)
              FileIO.FillFileBuffer();
            charP[index] = (char) FileIO.fileBuffer[(int) FileIO.bufferPosition];
            ++FileIO.bufferPosition;
            ++index;
          }
        }
        else
        {
          for (; numBytes > 0; --numBytes)
          {
            if ((int) FileIO.bufferPosition == (int) FileIO.readSize)
              FileIO.FillFileBuffer();
            byte num1 = (byte) ((uint) FileIO.fileBuffer[(int) FileIO.bufferPosition] ^ (uint) FileIO.eStringNo ^ (uint) FileIO.encryptionStringB[(int) FileIO.eStringPosB]);
            if (FileIO.eNybbleSwap)
              num1 = (byte) (((int) num1 >> 4) + (((int) num1 & 15) << 4));
            byte num2 = (byte) ((uint) num1 ^ (uint) (byte) FileIO.encryptionStringA[(int) FileIO.eStringPosA]);
            charP[index] = (char) num2;
            ++FileIO.eStringPosA;
            ++FileIO.eStringPosB;
            if (FileIO.eStringPosA > (byte) 19 && FileIO.eStringPosB > (byte) 11)
            {
              ++FileIO.eStringNo;
              FileIO.eStringNo &= (byte) 127 /*0x7F*/;
              if (!FileIO.eNybbleSwap)
              {
                FileIO.eNybbleSwap = true;
                FileIO.eStringPosA = (byte) (3 + (int) FileIO.eStringNo % 15);
                FileIO.eStringPosB = (byte) (1 + (int) FileIO.eStringNo % 7);
              }
              else
              {
                FileIO.eNybbleSwap = false;
                FileIO.eStringPosA = (byte) (6 + (int) FileIO.eStringNo % 12);
                FileIO.eStringPosB = (byte) (4 + (int) FileIO.eStringNo % 5);
              }
            }
            else
            {
              if (FileIO.eStringPosA > (byte) 19)
              {
                FileIO.eStringPosA = (byte) 1;
                FileIO.eNybbleSwap = !FileIO.eNybbleSwap;
              }
              if (FileIO.eStringPosB > (byte) 11)
              {
                FileIO.eStringPosB = (byte) 1;
                FileIO.eNybbleSwap = !FileIO.eNybbleSwap;
              }
            }
            ++FileIO.bufferPosition;
            ++index;
          }
        }
      }

      public static void FillFileBuffer()
      {
        FileIO.readSize = FileIO.readPos + 8192U /*0x2000*/ <= FileIO.fileSize ? 8192U /*0x2000*/ : FileIO.fileSize - FileIO.readPos;
        FileIO.fileReader.Read(FileIO.fileBuffer, 0, (int) FileIO.readSize);
        FileIO.readPos += FileIO.readSize;
        FileIO.bufferPosition = 0U;
      }

      public static void GetFileInfo(FileData fData)
      {
        fData.bufferPos = FileIO.bufferPosition;
        fData.position = FileIO.readPos - FileIO.readSize;
        fData.eStringPosA = FileIO.eStringPosA;
        fData.eStringPosB = FileIO.eStringPosB;
        fData.eStringNo = FileIO.eStringNo;
        fData.eNybbleSwap = FileIO.eNybbleSwap;
      }

      public static void SetFileInfo(FileData fData)
      {
        if (!FileIO.useRSDKFile)
          return;
        if (FileIO.fileReader != null)
          FileIO.fileReader.Close();
        FileIO.fileReader = File.OpenRead(Path.Combine(Application.streamingAssetsPath, "Data.rsdk"));
        FileIO.virtualFileOffset = fData.virtualFileOffset;
        FileIO.vFileSize = fData.fileSize;
        FileIO.fileSize = (uint) FileIO.fileReader.Length;
        FileIO.readPos = fData.position;
        FileIO.fileReader.Seek((long) FileIO.readPos, SeekOrigin.Begin);
        FileIO.FillFileBuffer();
        FileIO.bufferPosition = fData.bufferPos;
        FileIO.eStringPosA = fData.eStringPosA;
        FileIO.eStringPosB = fData.eStringPosB;
        FileIO.eStringNo = fData.eStringNo;
        FileIO.eNybbleSwap = fData.eNybbleSwap;
      }

      public static uint GetFilePosition()
      {
        return FileIO.useRSDKFile ? FileIO.readPos - FileIO.readSize + FileIO.bufferPosition - FileIO.virtualFileOffset : FileIO.readPos - FileIO.readSize + FileIO.bufferPosition;
      }

      public static void SetFilePosition(uint newFilePos)
      {
        if (FileIO.useRSDKFile)
        {
          FileIO.readPos = newFilePos + FileIO.virtualFileOffset;
          FileIO.eStringNo = (byte) ((FileIO.vFileSize & 508U) >> 2);
          FileIO.eStringPosB = (byte) (1 + (int) FileIO.eStringNo % 9);
          FileIO.eStringPosA = (byte) (1 + (int) FileIO.eStringNo % (int) FileIO.eStringPosB);
          FileIO.eNybbleSwap = false;
          for (; newFilePos > 0U; --newFilePos)
          {
            ++FileIO.eStringPosA;
            ++FileIO.eStringPosB;
            if (FileIO.eStringPosA > (byte) 19 && FileIO.eStringPosB > (byte) 11)
            {
              ++FileIO.eStringNo;
              FileIO.eStringNo &= (byte) 127 /*0x7F*/;
              if (!FileIO.eNybbleSwap)
              {
                FileIO.eNybbleSwap = true;
                FileIO.eStringPosA = (byte) (3 + (int) FileIO.eStringNo % 15);
                FileIO.eStringPosB = (byte) (1 + (int) FileIO.eStringNo % 7);
              }
              else
              {
                FileIO.eNybbleSwap = false;
                FileIO.eStringPosA = (byte) (6 + (int) FileIO.eStringNo % 12);
                FileIO.eStringPosB = (byte) (4 + (int) FileIO.eStringNo % 5);
              }
            }
            else
            {
              if (FileIO.eStringPosA > (byte) 19)
              {
                FileIO.eStringPosA = (byte) 1;
                FileIO.eNybbleSwap = !FileIO.eNybbleSwap;
              }
              if (FileIO.eStringPosB > (byte) 11)
              {
                FileIO.eStringPosB = (byte) 1;
                FileIO.eNybbleSwap = !FileIO.eNybbleSwap;
              }
            }
          }
        }
        else
          FileIO.readPos = newFilePos;
        FileIO.fileReader.Seek((long) FileIO.readPos, SeekOrigin.Begin);
        FileIO.FillFileBuffer();
      }

      public static bool ReachedEndOfFile()
      {
        return !FileIO.useRSDKFile ? FileIO.readPos - FileIO.readSize + FileIO.bufferPosition >= FileIO.fileSize : FileIO.readPos - FileIO.readSize + FileIO.bufferPosition - FileIO.virtualFileOffset >= FileIO.vFileSize;
      }

      public static byte ReadSaveRAMData()
      {
        #if WINDOWS_PHONE
        IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
        try
        {
          BinaryReader binaryReader = new BinaryReader(new IsolatedStorageFileStream("SGame.bin", FileMode.Open, userStoreForApplication));
          for (int i = 0; i < FileIO.saveRAM.Length; i++)
          {
            FileIO.saveRAM[i] = binaryReader.ReadInt32();
          }
          binaryReader.Close();
        }
        catch
        {
        }
        #else
        try
        {
          string savePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "SGame.bin");
          if (File.Exists(savePath))
          {
            using (BinaryReader binaryReader = new BinaryReader(File.Open(savePath, FileMode.Open)))
            {
              for (int i = 0; i < FileIO.saveRAM.Length; i++)
              {
                FileIO.saveRAM[i] = binaryReader.ReadInt32();
              }
            }
          }
        }
        catch
        {
        }
        #endif
        return 1;
      }

      public static byte WriteSaveRAMData()
      {
        #if WINDOWS_PHONE
        IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
        try
        {
          BinaryWriter binaryWriter = new BinaryWriter(new IsolatedStorageFileStream("SGame.bin", FileMode.Create, userStoreForApplication));
          for (int i = 0; i < FileIO.saveRAM.Length; i++)
          {
            binaryWriter.Write(FileIO.saveRAM[i]);
          }
          binaryWriter.Close();
        }
        catch
        {
        }
        #else
        try
        {
          string savePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "SGame.bin");
          using (BinaryWriter binaryWriter = new BinaryWriter(File.Create(savePath)))
          {
            for (int i = 0; i < FileIO.saveRAM.Length; i++)
            {
              binaryWriter.Write(FileIO.saveRAM[i]);
            }
          }
        }
        catch
        {
        }
        #endif
        return 1;
      }
    }
}