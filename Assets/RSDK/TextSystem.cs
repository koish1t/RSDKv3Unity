namespace Retro_Engine
{

    public static class TextSystem
    {
      public static int textMenuSurfaceNo;
      public static FontCharacter[] fontCharacterList = new FontCharacter[1024 /*0x0400*/];

      static TextSystem()
      {
        for (int index = 0; index < TextSystem.fontCharacterList.Length; ++index)
          TextSystem.fontCharacterList[index] = new FontCharacter();
      }

      public static void LoadFontFile(char[] fileName)
      {
        int index = 0;
        FileData fData = new FileData();
        if (!FileIO.LoadFile(fileName, fData))
          return;
        while (!FileIO.ReachedEndOfFile())
        {
          byte num1 = FileIO.ReadByte();
          TextSystem.fontCharacterList[index].id = (int) num1;
          byte num2 = FileIO.ReadByte();
          TextSystem.fontCharacterList[index].id += (int) num2 << 8;
          byte num3 = FileIO.ReadByte();
          TextSystem.fontCharacterList[index].id += (int) num3 << 16 /*0x10*/;
          byte num4 = FileIO.ReadByte();
          TextSystem.fontCharacterList[index].id += (int) num4 << 24;
          byte num5 = FileIO.ReadByte();
          TextSystem.fontCharacterList[index].left = (short) num5;
          byte num6 = FileIO.ReadByte();
          TextSystem.fontCharacterList[index].left += (short) ((int) num6 << 8);
          byte num7 = FileIO.ReadByte();
          TextSystem.fontCharacterList[index].top = (short) num7;
          byte num8 = FileIO.ReadByte();
          TextSystem.fontCharacterList[index].top += (short) ((int) num8 << 8);
          byte num9 = FileIO.ReadByte();
          TextSystem.fontCharacterList[index].xSize = (short) num9;
          byte num10 = FileIO.ReadByte();
          TextSystem.fontCharacterList[index].xSize += (short) ((int) num10 << 8);
          byte num11 = FileIO.ReadByte();
          TextSystem.fontCharacterList[index].ySize = (short) num11;
          byte num12 = FileIO.ReadByte();
          TextSystem.fontCharacterList[index].ySize += (short) ((int) num12 << 8);
          byte num13 = FileIO.ReadByte();
          TextSystem.fontCharacterList[index].xPivot = (short) num13;
          byte num14 = FileIO.ReadByte();
          if (num14 > (byte) 128 /*0x80*/)
          {
            TextSystem.fontCharacterList[index].xPivot += (short) ((int) num14 - 128 /*0x80*/ << 8);
            TextSystem.fontCharacterList[index].xPivot = (short) -(32768 /*0x8000*/ - (int) TextSystem.fontCharacterList[index].xPivot);
          }
          else
            TextSystem.fontCharacterList[index].xPivot += (short) ((int) num14 << 8);
          byte num15 = FileIO.ReadByte();
          TextSystem.fontCharacterList[index].yPivot = (short) num15;
          byte num16 = FileIO.ReadByte();
          if (num16 > (byte) 128 /*0x80*/)
          {
            TextSystem.fontCharacterList[index].yPivot += (short) ((int) num16 - 128 /*0x80*/ << 8);
            TextSystem.fontCharacterList[index].yPivot = (short) -(32768 /*0x8000*/ - (int) TextSystem.fontCharacterList[index].xPivot);
          }
          else
            TextSystem.fontCharacterList[index].yPivot += (short) ((int) num16 << 8);
          byte num17 = FileIO.ReadByte();
          TextSystem.fontCharacterList[index].xAdvance = (short) num17;
          byte num18 = FileIO.ReadByte();
          if (num18 > (byte) 128 /*0x80*/)
          {
            TextSystem.fontCharacterList[index].xAdvance += (short) ((int) num18 - 128 /*0x80*/ << 8);
            TextSystem.fontCharacterList[index].xAdvance = (short) -(32768 /*0x8000*/ - (int) TextSystem.fontCharacterList[index].xAdvance);
          }
          else
            TextSystem.fontCharacterList[index].xAdvance += (short) ((int) num18 << 8);
          byte num19 = FileIO.ReadByte();
          num19 = FileIO.ReadByte();
          ++index;
        }
        FileIO.CloseFile();
      }

      public static void LoadTextFile(TextMenu tMenu, char[] fileName, byte mapCode)
      {
        bool flag = false;
        FileData fData = new FileData();
        if (!FileIO.LoadFile(fileName, fData))
          return;
        tMenu.textDataPos = 0;
        tMenu.numRows = (ushort) 0;
        tMenu.entryStart[(int) tMenu.numRows] = tMenu.textDataPos;
        tMenu.entrySize[(int) tMenu.numRows] = 0;
        byte num1 = FileIO.ReadByte();
        if (num1 == byte.MaxValue)
        {
          FileIO.ReadByte();
          while (!flag)
          {
            ushort num2 = (ushort) ((uint) (ushort) FileIO.ReadByte() + (uint) (ushort) ((uint) FileIO.ReadByte() << 8));
            switch (num2)
            {
              case 10:
                if (!flag)
                {
                  flag = FileIO.ReachedEndOfFile();
                  if (tMenu.textDataPos >= 10240)
                  {
                    flag = true;
                    continue;
                  }
                  continue;
                }
                continue;
              case 13:
                ++tMenu.numRows;
                if (tMenu.numRows > (ushort) 511 /*0x01FF*/)
                {
                  flag = true;
                  goto case 10;
                }
                tMenu.entryStart[(int) tMenu.numRows] = tMenu.textDataPos;
                tMenu.entrySize[(int) tMenu.numRows] = 0;
                goto case 10;
              default:
                if (mapCode == (byte) 1)
                {
                  int index = 0;
                  while (index < 1024 /*0x0400*/)
                  {
                    if (TextSystem.fontCharacterList[index].id == (int) num2)
                    {
                      num2 = (ushort) index;
                      index = 1025;
                    }
                    else
                      ++index;
                  }
                  if (index == 1024 /*0x0400*/)
                    num2 = (ushort) 0;
                }
                tMenu.textData[tMenu.textDataPos] = (char) num2;
                ++tMenu.textDataPos;
                ++tMenu.entrySize[(int) tMenu.numRows];
                goto case 10;
            }
          }
        }
        else
        {
          ushort num3 = (ushort) num1;
          switch (num3)
          {
            case 10:
              while (!flag)
              {
                ushort num4 = (ushort) FileIO.ReadByte();
                switch (num4)
                {
                  case 10:
                    if (!flag)
                    {
                      flag = FileIO.ReachedEndOfFile();
                      if (tMenu.textDataPos >= 10240)
                      {
                        flag = true;
                        continue;
                      }
                      continue;
                    }
                    continue;
                  case 13:
                    ++tMenu.numRows;
                    if (tMenu.numRows > (ushort) 511 /*0x01FF*/)
                    {
                      flag = true;
                      goto case 10;
                    }
                    tMenu.entryStart[(int) tMenu.numRows] = tMenu.textDataPos;
                    tMenu.entrySize[(int) tMenu.numRows] = 0;
                    goto case 10;
                  default:
                    if (mapCode == (byte) 1)
                    {
                      int index = 0;
                      while (index < 1024 /*0x0400*/)
                      {
                        if (TextSystem.fontCharacterList[index].id == (int) num4)
                        {
                          num4 = (ushort) index;
                          index = 1025;
                        }
                        else
                          ++index;
                      }
                      if (index == 1024 /*0x0400*/)
                        num4 = (ushort) 0;
                    }
                    tMenu.textData[tMenu.textDataPos] = (char) num4;
                    ++tMenu.textDataPos;
                    ++tMenu.entrySize[(int) tMenu.numRows];
                    goto case 10;
                }
              }
              break;
            case 13:
              ++tMenu.numRows;
              tMenu.entryStart[(int) tMenu.numRows] = tMenu.textDataPos;
              tMenu.entrySize[(int) tMenu.numRows] = 0;
              goto case 10;
            default:
              if (mapCode == (byte) 1)
              {
                int index = 0;
                while (index < 1024 /*0x0400*/)
                {
                  if (TextSystem.fontCharacterList[index].id == (int) num3)
                  {
                    num3 = (ushort) index;
                    index = 1025;
                  }
                  else
                    ++index;
                }
                if (index == 1024 /*0x0400*/)
                  num3 = (ushort) 0;
              }
              tMenu.textData[tMenu.textDataPos] = (char) num3;
              ++tMenu.textDataPos;
              ++tMenu.entrySize[(int) tMenu.numRows];
              goto case 10;
          }
        }
        ++tMenu.numRows;
        FileIO.CloseFile();
      }

      public static void DrawBitmapText(
        TextMenu tMenu,
        int xPos,
        int yPos,
        int scale,
        int spacing,
        int rowStart,
        int numRows)
      {
        int num1 = yPos << 9;
        if (numRows < 0)
          numRows = (int) tMenu.numRows;
        if (rowStart + numRows > (int) tMenu.numRows)
          numRows = (int) tMenu.numRows - rowStart;
        for (; numRows > 0; --numRows)
        {
          int num2 = 0;
          int num3 = tMenu.entrySize[rowStart];
          int num4 = xPos << 9;
          for (; num3 > 0; --num3)
          {
            char index = tMenu.textData[tMenu.entryStart[rowStart] + num2];
            GraphicsSystem.DrawScaledChar((byte) 0, num4 >> 5, num1 >> 5, (int) -TextSystem.fontCharacterList[(int) index].xPivot, (int) -TextSystem.fontCharacterList[(int) index].yPivot, scale, scale, (int) TextSystem.fontCharacterList[(int) index].xSize, (int) TextSystem.fontCharacterList[(int) index].ySize, (int) TextSystem.fontCharacterList[(int) index].left, (int) TextSystem.fontCharacterList[(int) index].top, TextSystem.textMenuSurfaceNo);
            num4 += (int) TextSystem.fontCharacterList[(int) index].xAdvance * scale;
            ++num2;
          }
          num1 += spacing * scale;
          ++rowStart;
        }
      }

      public static void SetupTextMenu(TextMenu tMenu, int numRows)
      {
        tMenu.textDataPos = 0;
        tMenu.numRows = (ushort) numRows;
      }

      public static void AddTextMenuEntry(TextMenu tMenu, char[] inputTxt)
      {
        int index = 0;
        tMenu.entryStart[(int) tMenu.numRows] = tMenu.textDataPos;
        tMenu.entrySize[(int) tMenu.numRows] = 0;
        while (index < inputTxt.Length)
        {
          if (inputTxt[index] != char.MinValue)
          {
            tMenu.textData[tMenu.textDataPos] = inputTxt[index];
            ++tMenu.textDataPos;
            ++tMenu.entrySize[(int) tMenu.numRows];
            ++index;
          }
          else
            index = inputTxt.Length;
        }
        ++tMenu.numRows;
      }

      public static void addTextMenuEntry(TextMenu tMenu, char[] inputTxt)
      {
        AddTextMenuEntry(tMenu, inputTxt);
      }

      public static void AddTextMenuEntryMapped(TextMenu tMenu, char[] inputTxt)
      {
        int index1 = 0;
        tMenu.entryStart[(int) tMenu.numRows] = tMenu.textDataPos;
        tMenu.entrySize[(int) tMenu.numRows] = 0;
        while (index1 < inputTxt.Length)
        {
          if (inputTxt[index1] != char.MinValue)
          {
            ushort num = (ushort) inputTxt[index1];
            int index2 = 0;
            while (index2 < 1024 /*0x0400*/)
            {
              if (TextSystem.fontCharacterList[index2].id == (int) num)
              {
                num = (ushort) index2;
                index2 = 1025;
              }
              else
                ++index2;
            }
            if (index2 == 1024 /*0x0400*/)
              num = (ushort) 0;
            tMenu.textData[tMenu.textDataPos] = (char) num;
            ++tMenu.textDataPos;
            ++tMenu.entrySize[(int) tMenu.numRows];
            ++index1;
          }
          else
            index1 = inputTxt.Length;
        }
        ++tMenu.numRows;
      }

      public static void SetTextMenuEntry(TextMenu tMenu, char[] inputTxt, int rowNum)
      {
        int index = 0;
        tMenu.entryStart[rowNum] = tMenu.textDataPos;
        tMenu.entrySize[rowNum] = 0;
        while (index < inputTxt.Length)
        {
          if (inputTxt[index] != char.MinValue)
          {
            tMenu.textData[tMenu.textDataPos] = inputTxt[index];
            ++tMenu.textDataPos;
            ++tMenu.entrySize[rowNum];
            ++index;
          }
          else
            index = inputTxt.Length;
        }
      }

      public static void EditTextMenuEntry(TextMenu tMenu, char[] inputTxt, int rowNum)
      {
        int index1 = 0;
        int index2 = tMenu.entryStart[rowNum];
        tMenu.entrySize[rowNum] = 0;
        while (index1 < inputTxt.Length)
        {
          if (inputTxt[index1] != char.MinValue)
          {
            tMenu.textData[index2] = inputTxt[index1];
            ++index2;
            ++tMenu.entrySize[rowNum];
            ++index1;
          }
          else
            index1 = inputTxt.Length;
        }
      }

      public static void DrawTextMenuEntry(
        TextMenu tMenu,
        int rowNum,
        int xPos,
        int yPos,
        int textHighL)
      {
        int index1 = tMenu.entryStart[rowNum];
        for (int index2 = 0; index2 < tMenu.entrySize[rowNum]; ++index2)
        {
          GraphicsSystem.DrawSprite(xPos + (index2 << 3), yPos, 8, 8, ((int) tMenu.textData[index1] & 15) << 3, ((int) tMenu.textData[index1] >> 4 << 3) + textHighL, TextSystem.textMenuSurfaceNo);
          ++index1;
        }
      }

      public static void DrawStageTextEntry(
        TextMenu tMenu,
        int rowNum,
        int xPos,
        int yPos,
        int textHighL)
      {
        int index1 = tMenu.entryStart[rowNum];
        for (int index2 = 0; index2 < tMenu.entrySize[rowNum]; ++index2)
        {
          if (index2 == tMenu.entrySize[rowNum] - 1)
            GraphicsSystem.DrawSprite(xPos + (index2 << 3), yPos, 8, 8, ((int) tMenu.textData[index1] & 15) << 3, (int) tMenu.textData[index1] >> 4 << 3, TextSystem.textMenuSurfaceNo);
          else
            GraphicsSystem.DrawSprite(xPos + (index2 << 3), yPos, 8, 8, ((int) tMenu.textData[index1] & 15) << 3, ((int) tMenu.textData[index1] >> 4 << 3) + textHighL, TextSystem.textMenuSurfaceNo);
          ++index1;
        }
      }

      public static void DrawBlendedTextMenuEntry(
        TextMenu tMenu,
        int rowNum,
        int xPos,
        int yPos,
        int textHighL)
      {
        int index1 = tMenu.entryStart[rowNum];
        for (int index2 = 0; index2 < tMenu.entrySize[rowNum]; ++index2)
        {
          GraphicsSystem.DrawBlendedSprite(xPos + (index2 << 3), yPos, 8, 8, ((int) tMenu.textData[index1] & 15) << 3, ((int) tMenu.textData[index1] >> 4 << 3) + textHighL, TextSystem.textMenuSurfaceNo);
          ++index1;
        }
      }

      public static void DrawTextMenu(TextMenu tMenu, int xPos, int yPos)
      {
        int num;
        if (tMenu.numVisibleRows > (ushort) 0)
        {
          num = (int) tMenu.numVisibleRows + (int) tMenu.visibleRowOffset;
        }
        else
        {
          tMenu.visibleRowOffset = (ushort) 0;
          num = (int) tMenu.numRows;
        }
        if (tMenu.numSelections == (byte) 3)
        {
          tMenu.selection2 = -1;
          for (int index = 0; index < tMenu.selection1 + 1; ++index)
          {
            if (tMenu.entryHighlight[index] == (byte) 1)
              tMenu.selection2 = index;
          }
        }
        switch (tMenu.alignment)
        {
          case 0:
            for (int visibleRowOffset = (int) tMenu.visibleRowOffset; visibleRowOffset < num; ++visibleRowOffset)
            {
              int xPos1 = xPos;
              switch (tMenu.numSelections)
              {
                case 1:
                  if (visibleRowOffset == tMenu.selection1)
                  {
                    TextSystem.DrawTextMenuEntry(tMenu, visibleRowOffset, xPos1, yPos, 128 /*0x80*/);
                    break;
                  }
                  TextSystem.DrawTextMenuEntry(tMenu, visibleRowOffset, xPos1, yPos, 0);
                  break;
                case 2:
                  if (visibleRowOffset == tMenu.selection1 || visibleRowOffset == tMenu.selection2)
                  {
                    TextSystem.DrawTextMenuEntry(tMenu, visibleRowOffset, xPos1, yPos, 128 /*0x80*/);
                    break;
                  }
                  TextSystem.DrawTextMenuEntry(tMenu, visibleRowOffset, xPos1, yPos, 0);
                  break;
                case 3:
                  if (visibleRowOffset == tMenu.selection1)
                    TextSystem.DrawTextMenuEntry(tMenu, visibleRowOffset, xPos1, yPos, 128 /*0x80*/);
                  else
                    TextSystem.DrawTextMenuEntry(tMenu, visibleRowOffset, xPos1, yPos, 0);
                  if (visibleRowOffset == tMenu.selection2 && visibleRowOffset != tMenu.selection1)
                  {
                    TextSystem.DrawStageTextEntry(tMenu, visibleRowOffset, xPos1, yPos, 128 /*0x80*/);
                    break;
                  }
                  break;
              }
              yPos += 8;
            }
            break;
          case 1:
            for (int visibleRowOffset = (int) tMenu.visibleRowOffset; visibleRowOffset < num; ++visibleRowOffset)
            {
              int xPos2 = xPos - (tMenu.entrySize[visibleRowOffset] << 3);
              switch (tMenu.numSelections)
              {
                case 1:
                  if (visibleRowOffset == tMenu.selection1)
                  {
                    TextSystem.DrawTextMenuEntry(tMenu, visibleRowOffset, xPos2, yPos, 128 /*0x80*/);
                    break;
                  }
                  TextSystem.DrawTextMenuEntry(tMenu, visibleRowOffset, xPos2, yPos, 0);
                  break;
                case 2:
                  if (visibleRowOffset == tMenu.selection1 || visibleRowOffset == tMenu.selection2)
                  {
                    TextSystem.DrawTextMenuEntry(tMenu, visibleRowOffset, xPos2, yPos, 128 /*0x80*/);
                    break;
                  }
                  TextSystem.DrawTextMenuEntry(tMenu, visibleRowOffset, xPos2, yPos, 0);
                  break;
                case 3:
                  if (visibleRowOffset == tMenu.selection1)
                    TextSystem.DrawTextMenuEntry(tMenu, visibleRowOffset, xPos2, yPos, 128 /*0x80*/);
                  else
                    TextSystem.DrawTextMenuEntry(tMenu, visibleRowOffset, xPos2, yPos, 0);
                  if (visibleRowOffset == tMenu.selection2 && visibleRowOffset != tMenu.selection1)
                  {
                    TextSystem.DrawStageTextEntry(tMenu, visibleRowOffset, xPos2, yPos, 128 /*0x80*/);
                    break;
                  }
                  break;
              }
              yPos += 8;
            }
            break;
          case 2:
            for (int visibleRowOffset = (int) tMenu.visibleRowOffset; visibleRowOffset < num; ++visibleRowOffset)
            {
              int xPos3 = xPos - (tMenu.entrySize[visibleRowOffset] >> 1 << 3);
              switch (tMenu.numSelections)
              {
                case 1:
                  if (visibleRowOffset == tMenu.selection1)
                  {
                    TextSystem.DrawTextMenuEntry(tMenu, visibleRowOffset, xPos3, yPos, 128 /*0x80*/);
                    break;
                  }
                  TextSystem.DrawTextMenuEntry(tMenu, visibleRowOffset, xPos3, yPos, 0);
                  break;
                case 2:
                  if (visibleRowOffset == tMenu.selection1 || visibleRowOffset == tMenu.selection2)
                  {
                    TextSystem.DrawTextMenuEntry(tMenu, visibleRowOffset, xPos3, yPos, 128 /*0x80*/);
                    break;
                  }
                  TextSystem.DrawTextMenuEntry(tMenu, visibleRowOffset, xPos3, yPos, 0);
                  break;
                case 3:
                  if (visibleRowOffset == tMenu.selection1)
                    TextSystem.DrawTextMenuEntry(tMenu, visibleRowOffset, xPos3, yPos, 128 /*0x80*/);
                  else
                    TextSystem.DrawTextMenuEntry(tMenu, visibleRowOffset, xPos3, yPos, 0);
                  if (visibleRowOffset == tMenu.selection2 && visibleRowOffset != tMenu.selection1)
                  {
                    TextSystem.DrawStageTextEntry(tMenu, visibleRowOffset, xPos3, yPos, 128 /*0x80*/);
                    break;
                  }
                  break;
              }
              yPos += 8;
            }
            break;
        }
      }

      public static void LoadConfigListText(TextMenu tMenu, int listNo)
      {
        FileData fData = new FileData();
        char[] inputTxt = new char[32 /*0x20*/];
        if (!FileIO.LoadFile("Data/Game/GameConfig.bin".ToCharArray(), fData))
          return;
        byte num1 = FileIO.ReadByte();
        byte num2;
        for (int index = 0; index < (int) num1; ++index)
          num2 = FileIO.ReadByte();
        byte num3 = FileIO.ReadByte();
        for (int index = 0; index < (int) num3; ++index)
          num2 = FileIO.ReadByte();
        byte num4 = FileIO.ReadByte();
        for (int index = 0; index < (int) num4; ++index)
          num2 = FileIO.ReadByte();
        byte num5 = FileIO.ReadByte();
        for (int index1 = 0; index1 < (int) num5; ++index1)
        {
          byte num6 = FileIO.ReadByte();
          for (int index2 = 0; index2 < (int) num6; ++index2)
            num2 = FileIO.ReadByte();
        }
        for (int index3 = 0; index3 < (int) num5; ++index3)
        {
          byte num7 = FileIO.ReadByte();
          for (int index4 = 0; index4 < (int) num7; ++index4)
            num2 = FileIO.ReadByte();
        }
        byte num8 = FileIO.ReadByte();
        for (int index5 = 0; index5 < (int) num8; ++index5)
        {
          byte num9 = FileIO.ReadByte();
          for (int index6 = 0; index6 < (int) num9; ++index6)
            num2 = FileIO.ReadByte();
          num2 = FileIO.ReadByte();
          num2 = FileIO.ReadByte();
          num2 = FileIO.ReadByte();
          num2 = FileIO.ReadByte();
        }
        byte num10 = FileIO.ReadByte();
        for (int index7 = 0; index7 < (int) num10; ++index7)
        {
          byte num11 = FileIO.ReadByte();
          for (int index8 = 0; index8 < (int) num11; ++index8)
            num2 = FileIO.ReadByte();
        }
        byte num12 = FileIO.ReadByte();
        for (int index9 = 0; index9 < (int) num12; ++index9)
        {
          byte num13 = FileIO.ReadByte();
          int index10;
          for (index10 = 0; index10 < (int) num13; ++index10)
          {
            byte num14 = FileIO.ReadByte();
            inputTxt[index10] = (char) num14;
          }
          inputTxt[index10] = char.MinValue;
          if (listNo == 0)
            TextSystem.AddTextMenuEntry(tMenu, inputTxt);
        }
        for (int index11 = 1; index11 < 5; ++index11)
        {
          byte num15 = FileIO.ReadByte();
          for (int index12 = 0; index12 < (int) num15; ++index12)
          {
            byte num16 = FileIO.ReadByte();
            for (int index13 = 0; index13 < (int) num16; ++index13)
              num2 = FileIO.ReadByte();
            byte num17 = FileIO.ReadByte();
            for (int index14 = 0; index14 < (int) num17; ++index14)
              num2 = FileIO.ReadByte();
            byte num18 = FileIO.ReadByte();
            int index15;
            for (index15 = 0; index15 < (int) num18; ++index15)
            {
              byte num19 = FileIO.ReadByte();
              inputTxt[index15] = (char) num19;
            }
            inputTxt[index15] = char.MinValue;
            byte num20 = FileIO.ReadByte();
            if (listNo == index11)
            {
              tMenu.entryHighlight[index12] = num20;
              TextSystem.AddTextMenuEntry(tMenu, inputTxt);
            }
          }
        }
        FileIO.CloseFile();
      }
    }
}