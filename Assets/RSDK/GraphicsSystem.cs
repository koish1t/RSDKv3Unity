using System;
using UnityEngine;

namespace Retro_Engine
{

    public static class GraphicsSystem
    {
      public const int NUM_SPRITESHEETS = 24;
      public const int GRAPHIC_DATASIZE = 2097152 /*0x200000*/;
      public const int VERTEX_LIMIT = 8192 /*0x2000*/;
      public const int INDEX_LIMIT = 49152 /*0xC000*/;
      public static bool render3DEnabled = false;
      public static byte fadeMode = 0;
      public static byte fadeR = 0;
      public static byte fadeG = 0;
      public static byte fadeB = 0;
      public static byte fadeA = 0;
      public static byte paletteMode = 0;
      public static byte colourMode = 0;
      public static ushort[] texBuffer = new ushort[1048576 /*0x100000*/];
      public static byte texBufferMode = 0;
      public static byte[] tileGfx = new byte[262144 /*0x040000*/];
      public static byte[] graphicData = new byte[2097152 /*0x200000*/];
      public static GfxSurfaceDesc[] gfxSurface = new GfxSurfaceDesc[24];
      public static uint gfxDataPosition;
      public static DrawVertex[] gfxPolyList = new DrawVertex[8192 /*0x2000*/];
      public static DrawVertex3D[] polyList3D = new DrawVertex3D[6404];
      public static short[] gfxPolyListIndex = new short[49152 /*0xC000*/];
      public static ushort gfxVertexSize = 0;
      public static ushort gfxVertexSizeOpaque = 0;
      public static ushort gfxIndexSize = 0;
      public static ushort gfxIndexSizeOpaque = 0;
      public static ushort vertexSize3D = 0;
      public static ushort indexSize3D = 0;
      public static float[] tileUVArray = new float[4096 /*0x1000*/];
      public static Vector3 floor3DPos = Vector3.zero;
      public static float floor3DAngle;
      public static ushort[] blendLookupTable = new ushort[8192 /*0x2000*/];
      public static ushort[] subtractiveLookupTable = new ushort[8192 /*0x2000*/];
      public static PaletteEntry[] tilePalette = new PaletteEntry[256 /*0x0100*/];
      public static ushort[,] tilePalette16_Data = new ushort[8, 256 /*0x0100*/];
      public static int texPaletteNum = 0;
      public static int waterDrawPos = 320;
      public static bool videoPlaying = false;
      public static int currentVideoFrame;
      public static RenderTexture renderTexture;
      public static Camera renderCamera;
      public static Material renderMaterial;

      static GraphicsSystem()
      {
        for (int index = 0; index < GraphicsSystem.gfxSurface.Length; ++index)
          GraphicsSystem.gfxSurface[index] = new GfxSurfaceDesc();
        for (int index = 0; index < GraphicsSystem.gfxPolyList.Length; ++index)
          GraphicsSystem.gfxPolyList[index] = new DrawVertex();
        for (int index = 0; index < GraphicsSystem.polyList3D.Length; ++index)
          GraphicsSystem.polyList3D[index] = new DrawVertex3D();
        for (int index = 0; index < GraphicsSystem.tilePalette.Length; ++index)
          GraphicsSystem.tilePalette[index] = new PaletteEntry();
      }

      public static void SetScreenRenderSize(int gfxWidth, int gfxPitch)
      {
        GlobalAppDefinitions.SCREEN_XSIZE = gfxWidth;
        GlobalAppDefinitions.SCREEN_CENTER = GlobalAppDefinitions.SCREEN_XSIZE / 2;
        GlobalAppDefinitions.SCREEN_SCROLL_LEFT = GlobalAppDefinitions.SCREEN_CENTER - 8;
        GlobalAppDefinitions.SCREEN_SCROLL_RIGHT = GlobalAppDefinitions.SCREEN_CENTER + 8;
        GlobalAppDefinitions.OBJECT_BORDER_X1 = 128 /*0x80*/;
        GlobalAppDefinitions.OBJECT_BORDER_X2 = GlobalAppDefinitions.SCREEN_XSIZE + 128 /*0x80*/;
      }

      public static void InitializeRenderTexture()
      {
        if (renderTexture == null)
        {
          renderTexture = new RenderTexture(1280, 720, 24);
          renderTexture.Create();
        }
        
        if (renderCamera == null)
        {
          GameObject cameraObj = new GameObject("RetroEngineCamera");
          renderCamera = cameraObj.AddComponent<Camera>();
          renderCamera.targetTexture = renderTexture;
          renderCamera.clearFlags = CameraClearFlags.SolidColor;
          renderCamera.backgroundColor = Color.black;
          renderCamera.orthographic = true;
          renderCamera.orthographicSize = 360;
          renderCamera.nearClipPlane = 0.1f;
          renderCamera.farClipPlane = 1000f;
        }
        
        if (renderMaterial == null)
        {
          renderMaterial = new Material(Shader.Find("Unlit/Transparent"));
        }
      }

      public static ushort RGB_16BIT5551(byte r, byte g, byte b, byte a)
      {
        return (ushort) (((int) a << 15) + ((int) r >> 3 << 10) + ((int) g >> 3 << 5) + ((int) b >> 3));
      }

      public static void LoadPalette(
        char[] fileName,
        int paletteNum,
        int destPoint,
        int startPoint,
        int endPoint)
      {
        char[] strA = new char[64 /*0x40*/];
        char[] charArray = "Data/Palettes/".ToCharArray();
        FileIO.StrCopy(ref strA, ref charArray);
        FileIO.StrAdd(ref strA, ref fileName);
        FileData fData = new FileData();
        byte[] byteP = new byte[3];
        if (!FileIO.LoadFile(strA, fData))
          return;
        FileIO.SetFilePosition((uint) (startPoint * 3));
        if (paletteNum < 0 || paletteNum > 7)
          paletteNum = 0;
        if (paletteNum == 0)
        {
          for (int index = startPoint; index < endPoint; ++index)
          {
            FileIO.ReadByteArray(ref byteP, 3);
            GraphicsSystem.tilePalette16_Data[0, destPoint] = GraphicsSystem.RGB_16BIT5551(byteP[0], byteP[1], byteP[2], (byte) 1);
            GraphicsSystem.tilePalette[destPoint].red = byteP[0];
            GraphicsSystem.tilePalette[destPoint].green = byteP[1];
            GraphicsSystem.tilePalette[destPoint].blue = byteP[2];
            ++destPoint;
          }
          GraphicsSystem.tilePalette16_Data[0, 0] = GraphicsSystem.RGB_16BIT5551(byteP[0], byteP[1], byteP[2], (byte) 0);
        }
        else
        {
          for (int index = startPoint; index < endPoint; ++index)
          {
            FileIO.ReadByteArray(ref byteP, 3);
            GraphicsSystem.tilePalette16_Data[paletteNum, destPoint] = GraphicsSystem.RGB_16BIT5551(byteP[0], byteP[1], byteP[2], (byte) 1);
            ++destPoint;
          }
          GraphicsSystem.tilePalette16_Data[paletteNum, 0] = GraphicsSystem.RGB_16BIT5551(byteP[0], byteP[1], byteP[2], (byte) 0);
        }
        FileIO.CloseFile();
      }

      public static byte AddGraphicsFile(char[] fileName)
      {
        byte surfaceNum = 0;
        char[] fileName1 = new char[64 /*0x40*/];
        char[] charArray = "Data/Sprites/".ToCharArray();
        FileIO.StrCopy(ref fileName1, ref charArray);
        FileIO.StrAdd(ref fileName1, ref fileName);
        for (; surfaceNum < (byte) 24; ++surfaceNum)
        {
          if (FileIO.StringLength(ref GraphicsSystem.gfxSurface[(int) surfaceNum].fileName) > 0)
          {
            if (FileIO.StringComp(ref GraphicsSystem.gfxSurface[(int) surfaceNum].fileName, ref fileName1))
              return surfaceNum;
          }
          else
          {
            int index = FileIO.StringLength(ref fileName1) - 1;
            switch (fileName1[index])
            {
              case 'f':
                GraphicsSystem.LoadGIFFile(fileName1, (int) surfaceNum);
                break;
              case 'p':
                GraphicsSystem.LoadBMPFile(fileName1, (int) surfaceNum);
                break;
            }
            return surfaceNum;
          }
        }
        return 0;
      }

      public static void RemoveGraphicsFile(char[] fileName, int surfaceNum)
      {
        if (surfaceNum < 0)
        {
          for (uint index = 0; index < 24U; ++index)
          {
            if (FileIO.StringLength(ref GraphicsSystem.gfxSurface[(int) index].fileName) > 0 && FileIO.StringComp(ref GraphicsSystem.gfxSurface[(int) index].fileName, ref fileName))
              surfaceNum = (int) index;
          }
        }
        if (surfaceNum < 0 || FileIO.StringLength(ref GraphicsSystem.gfxSurface[surfaceNum].fileName) == 0)
          return;
        FileIO.StrClear(ref GraphicsSystem.gfxSurface[surfaceNum].fileName);
        uint dataStart = GraphicsSystem.gfxSurface[surfaceNum].dataStart;
        uint index1 = (uint) ((ulong) GraphicsSystem.gfxSurface[surfaceNum].dataStart + (ulong) (GraphicsSystem.gfxSurface[surfaceNum].width * GraphicsSystem.gfxSurface[surfaceNum].height));
        for (uint index2 = 2097152U /*0x200000*/ - index1; index2 > 0U; --index2)
        {
          GraphicsSystem.graphicData[(int) dataStart] = GraphicsSystem.graphicData[(int) index1];
          ++dataStart;
          ++index1;
        }
        GraphicsSystem.gfxDataPosition -= (uint) (GraphicsSystem.gfxSurface[surfaceNum].width * GraphicsSystem.gfxSurface[surfaceNum].height);
        for (uint index3 = 0; index3 < 24U; ++index3)
        {
          if (GraphicsSystem.gfxSurface[(int) index3].dataStart > GraphicsSystem.gfxSurface[surfaceNum].dataStart)
            GraphicsSystem.gfxSurface[(int) index3].dataStart -= (uint) (GraphicsSystem.gfxSurface[surfaceNum].width * GraphicsSystem.gfxSurface[surfaceNum].height);
        }
      }

      public static void removeGraphicsFile(char[] fileName, int surfaceNum)
      {
        RemoveGraphicsFile(fileName, surfaceNum);
      }

      public static void ClearGraphicsData()
      {
        for (int index = 0; index < 24; ++index)
          FileIO.StrClear(ref GraphicsSystem.gfxSurface[index].fileName);
        GraphicsSystem.gfxDataPosition = 0U;
      }

      public static bool CheckSurfaceSize(int size)
      {
        for (int index = 2; index < 2048 /*0x0800*/; index <<= 1)
        {
          if (index == size)
            return true;
        }
        return false;
      }

      public static void SetupPolygonLists()
      {
        int index1 = 0;
        for (int index2 = 0; index2 < 8192 /*0x2000*/; ++index2)
        {
          GraphicsSystem.gfxPolyListIndex[index1] = (short) (index2 << 2);
          int index3 = index1 + 1;
          GraphicsSystem.gfxPolyListIndex[index3] = (short) ((index2 << 2) + 1);
          int index4 = index3 + 1;
          GraphicsSystem.gfxPolyListIndex[index4] = (short) ((index2 << 2) + 2);
          int index5 = index4 + 1;
          GraphicsSystem.gfxPolyListIndex[index5] = (short) ((index2 << 2) + 1);
          int index6 = index5 + 1;
          GraphicsSystem.gfxPolyListIndex[index6] = (short) ((index2 << 2) + 3);
          int index7 = index6 + 1;
          GraphicsSystem.gfxPolyListIndex[index7] = (short) ((index2 << 2) + 2);
          index1 = index7 + 1;
        }
        for (int index8 = 0; index8 < 8192 /*0x2000*/; ++index8)
        {
          GraphicsSystem.gfxPolyList[index8].color.r = byte.MaxValue;
          GraphicsSystem.gfxPolyList[index8].color.g = byte.MaxValue;
          GraphicsSystem.gfxPolyList[index8].color.b = byte.MaxValue;
          GraphicsSystem.gfxPolyList[index8].color.a = byte.MaxValue;
        }
        for (int index9 = 0; index9 < 6404; ++index9)
        {
          GraphicsSystem.polyList3D[index9].color.r = byte.MaxValue;
          GraphicsSystem.polyList3D[index9].color.g = byte.MaxValue;
          GraphicsSystem.polyList3D[index9].color.b = byte.MaxValue;
          GraphicsSystem.polyList3D[index9].color.a = byte.MaxValue;
        }
      }

      public static void UpdateTextureBufferWithTiles()
      {
        int num1 = 0;
        if (GraphicsSystem.texBufferMode == (byte) 0)
        {
          for (int index1 = 0; index1 < 512 /*0x0200*/; index1 += 16 /*0x10*/)
          {
            for (int index2 = 0; index2 < 512 /*0x0200*/; index2 += 16 /*0x10*/)
            {
              int index3 = num1 << 8;
              ++num1;
              int index4 = index2 + (index1 << 10);
              for (int index5 = 0; index5 < 16 /*0x10*/; ++index5)
              {
                for (int index6 = 0; index6 < 16 /*0x10*/; ++index6)
                {
                  GraphicsSystem.texBuffer[index4] = GraphicsSystem.tileGfx[index3] <= (byte) 0 ? (ushort) 0 : GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int) GraphicsSystem.tileGfx[index3]];
                  ++index4;
                  ++index3;
                }
                index4 += 1008;
              }
            }
          }
        }
        else
        {
          for (int index7 = 0; index7 < 504; index7 += 18)
          {
            for (int index8 = 0; index8 < 504; index8 += 18)
            {
              int index9 = num1 << 8;
              ++num1;
              if (num1 == 783)
                num1 = 1023 /*0x03FF*/;
              int index10 = index8 + (index7 << 10);
              GraphicsSystem.texBuffer[index10] = GraphicsSystem.tileGfx[index9] <= (byte) 0 ? (ushort) 0 : GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int) GraphicsSystem.tileGfx[index9]];
              int index11 = index10 + 1;
              for (int index12 = 0; index12 < 15; ++index12)
              {
                GraphicsSystem.texBuffer[index11] = GraphicsSystem.tileGfx[index9] <= (byte) 0 ? (ushort) 0 : GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int) GraphicsSystem.tileGfx[index9]];
                ++index11;
                ++index9;
              }
              int index13;
              if (GraphicsSystem.tileGfx[index9] > (byte) 0)
              {
                GraphicsSystem.texBuffer[index11] = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int) GraphicsSystem.tileGfx[index9]];
                index13 = index11 + 1;
                GraphicsSystem.texBuffer[index13] = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int) GraphicsSystem.tileGfx[index9]];
              }
              else
              {
                GraphicsSystem.texBuffer[index11] = (ushort) 0;
                index13 = index11 + 1;
                GraphicsSystem.texBuffer[index13] = (ushort) 0;
              }
              int num2 = index13 + 1;
              int index14 = index9 - 15;
              int index15 = num2 + 1006;
              for (int index16 = 0; index16 < 16 /*0x10*/; ++index16)
              {
                GraphicsSystem.texBuffer[index15] = GraphicsSystem.tileGfx[index14] <= (byte) 0 ? (ushort) 0 : GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int) GraphicsSystem.tileGfx[index14]];
                int index17 = index15 + 1;
                for (int index18 = 0; index18 < 15; ++index18)
                {
                  GraphicsSystem.texBuffer[index17] = GraphicsSystem.tileGfx[index14] <= (byte) 0 ? (ushort) 0 : GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int) GraphicsSystem.tileGfx[index14]];
                  ++index17;
                  ++index14;
                }
                int index19;
                if (GraphicsSystem.tileGfx[index14] > (byte) 0)
                {
                  GraphicsSystem.texBuffer[index17] = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int) GraphicsSystem.tileGfx[index14]];
                  index19 = index17 + 1;
                  GraphicsSystem.texBuffer[index19] = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int) GraphicsSystem.tileGfx[index14]];
                }
                else
                {
                  GraphicsSystem.texBuffer[index17] = (ushort) 0;
                  index19 = index17 + 1;
                  GraphicsSystem.texBuffer[index19] = (ushort) 0;
                }
                int num3 = index19 + 1;
                ++index14;
                index15 = num3 + 1006;
              }
              int index20 = index14 - 16 /*0x10*/;
              GraphicsSystem.texBuffer[index15] = GraphicsSystem.tileGfx[index20] <= (byte) 0 ? (ushort) 0 : GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int) GraphicsSystem.tileGfx[index20]];
              int index21 = index15 + 1;
              for (int index22 = 0; index22 < 15; ++index22)
              {
                GraphicsSystem.texBuffer[index21] = GraphicsSystem.tileGfx[index20] <= (byte) 0 ? (ushort) 0 : GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int) GraphicsSystem.tileGfx[index20]];
                ++index21;
                ++index20;
              }
              int index23;
              if (GraphicsSystem.tileGfx[index20] > (byte) 0)
              {
                GraphicsSystem.texBuffer[index21] = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int) GraphicsSystem.tileGfx[index20]];
                index23 = index21 + 1;
                GraphicsSystem.texBuffer[index23] = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int) GraphicsSystem.tileGfx[index20]];
              }
              else
              {
                GraphicsSystem.texBuffer[index21] = (ushort) 0;
                index23 = index21 + 1;
                GraphicsSystem.texBuffer[index23] = (ushort) 0;
              }
              int num4 = index23 + 1 + 1006;
            }
          }
        }
        int index24 = 0;
        for (int index25 = 0; index25 < 16 /*0x10*/; ++index25)
        {
          for (int index26 = 0; index26 < 16 /*0x10*/; ++index26)
          {
            GraphicsSystem.texBuffer[index24] = GraphicsSystem.RGB_16BIT5551(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) 1);
            ++index24;
          }
          index24 += 1008;
        }
      }

      public static void UpdateTextureBufferWithSortedSprites()
      {
        byte index1 = 0;
        byte[] numArray = new byte[24];
        bool flag = true;
        for (int index2 = 0; index2 < 24; ++index2)
          GraphicsSystem.gfxSurface[index2].texStartX = -1;
        for (int index3 = 0; index3 < 24; ++index3)
        {
          int num = 0;
          sbyte index4 = -1;
          for (int index5 = 0; index5 < 24; ++index5)
          {
            if (FileIO.StringLength(ref GraphicsSystem.gfxSurface[index5].fileName) > 0 && GraphicsSystem.gfxSurface[index5].texStartX == -1)
            {
              if (GraphicsSystem.CheckSurfaceSize(GraphicsSystem.gfxSurface[index5].width) && GraphicsSystem.CheckSurfaceSize(GraphicsSystem.gfxSurface[index5].height))
              {
                if (GraphicsSystem.gfxSurface[index5].width + GraphicsSystem.gfxSurface[index5].height > num)
                {
                  num = GraphicsSystem.gfxSurface[index5].width + GraphicsSystem.gfxSurface[index5].height;
                  index4 = (sbyte) index5;
                }
              }
              else
                GraphicsSystem.gfxSurface[index5].texStartX = 0;
            }
          }
          if (index4 == (sbyte) -1)
          {
            index3 = 24;
          }
          else
          {
            GraphicsSystem.gfxSurface[(int) index4].texStartX = 0;
            numArray[(int) index1] = (byte) index4;
            ++index1;
          }
        }
        for (int index6 = 0; index6 < 24; ++index6)
          GraphicsSystem.gfxSurface[index6].texStartX = -1;
        for (int index7 = 0; index7 < (int) index1; ++index7)
        {
          sbyte index8 = (sbyte) numArray[index7];
          GraphicsSystem.gfxSurface[(int) index8].texStartX = 0;
          GraphicsSystem.gfxSurface[(int) index8].texStartY = 0;
          int num = 0;
          while (num == 0)
          {
            num = 1;
            if (GraphicsSystem.gfxSurface[(int) index8].height == 1024 /*0x0400*/)
              flag = false;
            if (flag)
            {
              if (GraphicsSystem.gfxSurface[(int) index8].texStartX < 512 /*0x0200*/ && GraphicsSystem.gfxSurface[(int) index8].texStartY < 512 /*0x0200*/)
              {
                num = 0;
                GraphicsSystem.gfxSurface[(int) index8].texStartX += GraphicsSystem.gfxSurface[(int) index8].width;
                if (GraphicsSystem.gfxSurface[(int) index8].texStartX + GraphicsSystem.gfxSurface[(int) index8].width > 1024 /*0x0400*/)
                {
                  GraphicsSystem.gfxSurface[(int) index8].texStartX = 0;
                  GraphicsSystem.gfxSurface[(int) index8].texStartY += GraphicsSystem.gfxSurface[(int) index8].height;
                }
              }
              else
              {
                for (int index9 = 0; index9 < 24; ++index9)
                {
                  if (GraphicsSystem.gfxSurface[index9].texStartX > -1 && index9 != (int) index8 && GraphicsSystem.gfxSurface[(int) index8].texStartX < GraphicsSystem.gfxSurface[index9].texStartX + GraphicsSystem.gfxSurface[index9].width && GraphicsSystem.gfxSurface[(int) index8].texStartX >= GraphicsSystem.gfxSurface[index9].texStartX && GraphicsSystem.gfxSurface[(int) index8].texStartY < GraphicsSystem.gfxSurface[index9].texStartY + GraphicsSystem.gfxSurface[index9].height)
                  {
                    num = 0;
                    GraphicsSystem.gfxSurface[(int) index8].texStartX += GraphicsSystem.gfxSurface[(int) index8].width;
                    if (GraphicsSystem.gfxSurface[(int) index8].texStartX + GraphicsSystem.gfxSurface[(int) index8].width > 1024 /*0x0400*/)
                    {
                      GraphicsSystem.gfxSurface[(int) index8].texStartX = 0;
                      GraphicsSystem.gfxSurface[(int) index8].texStartY += GraphicsSystem.gfxSurface[(int) index8].height;
                    }
                    index9 = 24;
                  }
                }
              }
            }
            else if (GraphicsSystem.gfxSurface[(int) index8].width < 1024 /*0x0400*/)
            {
              if (GraphicsSystem.gfxSurface[(int) index8].texStartX < 16 /*0x10*/ && GraphicsSystem.gfxSurface[(int) index8].texStartY < 16 /*0x10*/)
              {
                num = 0;
                GraphicsSystem.gfxSurface[(int) index8].texStartX += GraphicsSystem.gfxSurface[(int) index8].width;
                if (GraphicsSystem.gfxSurface[(int) index8].texStartX + GraphicsSystem.gfxSurface[(int) index8].width > 1024 /*0x0400*/)
                {
                  GraphicsSystem.gfxSurface[(int) index8].texStartX = 0;
                  GraphicsSystem.gfxSurface[(int) index8].texStartY += GraphicsSystem.gfxSurface[(int) index8].height;
                }
              }
              else
              {
                for (int index10 = 0; index10 < 24; ++index10)
                {
                  if (GraphicsSystem.gfxSurface[index10].texStartX > -1 && index10 != (int) index8 && GraphicsSystem.gfxSurface[(int) index8].texStartX < GraphicsSystem.gfxSurface[index10].texStartX + GraphicsSystem.gfxSurface[index10].width && GraphicsSystem.gfxSurface[(int) index8].texStartX >= GraphicsSystem.gfxSurface[index10].texStartX && GraphicsSystem.gfxSurface[(int) index8].texStartY < GraphicsSystem.gfxSurface[index10].texStartY + GraphicsSystem.gfxSurface[index10].height)
                  {
                    num = 0;
                    GraphicsSystem.gfxSurface[(int) index8].texStartX += GraphicsSystem.gfxSurface[(int) index8].width;
                    if (GraphicsSystem.gfxSurface[(int) index8].texStartX + GraphicsSystem.gfxSurface[(int) index8].width > 1024 /*0x0400*/)
                    {
                      GraphicsSystem.gfxSurface[(int) index8].texStartX = 0;
                      GraphicsSystem.gfxSurface[(int) index8].texStartY += GraphicsSystem.gfxSurface[(int) index8].height;
                    }
                    index10 = 24;
                  }
                }
              }
            }
          }
          if (GraphicsSystem.gfxSurface[(int) index8].texStartY + GraphicsSystem.gfxSurface[(int) index8].height <= 1024 /*0x0400*/)
          {
            int dataStart = (int) GraphicsSystem.gfxSurface[(int) index8].dataStart;
            int index11 = GraphicsSystem.gfxSurface[(int) index8].texStartX + (GraphicsSystem.gfxSurface[(int) index8].texStartY << 10);
            for (int index12 = 0; index12 < GraphicsSystem.gfxSurface[(int) index8].height; ++index12)
            {
              for (int index13 = 0; index13 < GraphicsSystem.gfxSurface[(int) index8].width; ++index13)
              {
                GraphicsSystem.texBuffer[index11] = GraphicsSystem.graphicData[dataStart] <= (byte) 0 ? (ushort) 0 : GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int) GraphicsSystem.graphicData[dataStart]];
                ++index11;
                ++dataStart;
              }
              index11 += 1024 /*0x0400*/ - GraphicsSystem.gfxSurface[(int) index8].width;
            }
          }
        }
      }

      public static void UpdateTextureBufferWithSprites()
      {
        for (int index1 = 0; index1 < 24; ++index1)
        {
          if (GraphicsSystem.gfxSurface[index1].texStartY + GraphicsSystem.gfxSurface[index1].height <= 1024 /*0x0400*/ && GraphicsSystem.gfxSurface[index1].texStartX > -1)
          {
            int dataStart = (int) GraphicsSystem.gfxSurface[index1].dataStart;
            int index2 = GraphicsSystem.gfxSurface[index1].texStartX + (GraphicsSystem.gfxSurface[index1].texStartY << 10);
            for (int index3 = 0; index3 < GraphicsSystem.gfxSurface[index1].height; ++index3)
            {
              for (int index4 = 0; index4 < GraphicsSystem.gfxSurface[index1].width; ++index4)
              {
                GraphicsSystem.texBuffer[index2] = GraphicsSystem.graphicData[dataStart] <= (byte) 0 ? (ushort) 0 : GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int) GraphicsSystem.graphicData[dataStart]];
                ++index2;
                ++dataStart;
              }
              index2 += 1024 /*0x0400*/ - GraphicsSystem.gfxSurface[index1].width;
            }
          }
        }
      }

      public static void LoadBMPFile(char[] fileName, int surfaceNum)
      {
        FileData fData = new FileData();
        if (!FileIO.LoadFile(fileName, fData))
          return;
        FileIO.StrCopy(ref GraphicsSystem.gfxSurface[surfaceNum].fileName, ref fileName);
        FileIO.SetFilePosition(18U);
        byte num1 = FileIO.ReadByte();
        GraphicsSystem.gfxSurface[surfaceNum].width = (int) num1;
        byte num2 = FileIO.ReadByte();
        GraphicsSystem.gfxSurface[surfaceNum].width += (int) num2 << 8;
        byte num3 = FileIO.ReadByte();
        GraphicsSystem.gfxSurface[surfaceNum].width += (int) num3 << 16 /*0x10*/;
        byte num4 = FileIO.ReadByte();
        GraphicsSystem.gfxSurface[surfaceNum].width += (int) num4 << 24;
        byte num5 = FileIO.ReadByte();
        GraphicsSystem.gfxSurface[surfaceNum].height = (int) num5;
        byte num6 = FileIO.ReadByte();
        GraphicsSystem.gfxSurface[surfaceNum].height += (int) num6 << 8;
        byte num7 = FileIO.ReadByte();
        GraphicsSystem.gfxSurface[surfaceNum].height += (int) num7 << 16 /*0x10*/;
        byte num8 = FileIO.ReadByte();
        GraphicsSystem.gfxSurface[surfaceNum].height += (int) num8 << 24;
        FileIO.SetFilePosition((uint) ((ulong) fData.fileSize - (ulong) (GraphicsSystem.gfxSurface[surfaceNum].width * GraphicsSystem.gfxSurface[surfaceNum].height)));
        GraphicsSystem.gfxSurface[surfaceNum].dataStart = GraphicsSystem.gfxDataPosition;
        int index1 = (int) GraphicsSystem.gfxSurface[surfaceNum].dataStart + GraphicsSystem.gfxSurface[surfaceNum].width * (GraphicsSystem.gfxSurface[surfaceNum].height - 1);
        for (int index2 = 0; index2 < GraphicsSystem.gfxSurface[surfaceNum].height; ++index2)
        {
          for (int index3 = 0; index3 < GraphicsSystem.gfxSurface[surfaceNum].width; ++index3)
          {
            byte num9 = FileIO.ReadByte();
            GraphicsSystem.graphicData[index1] = num9;
            ++index1;
          }
          index1 -= GraphicsSystem.gfxSurface[surfaceNum].width << 1;
        }
        GraphicsSystem.gfxDataPosition += (uint) (GraphicsSystem.gfxSurface[surfaceNum].width * GraphicsSystem.gfxSurface[surfaceNum].height);
        if (GraphicsSystem.gfxDataPosition >= 4194304U /*0x400000*/)
          GraphicsSystem.gfxDataPosition = 0U;
        FileIO.CloseFile();
      }

      public static void LoadGIFFile(char[] fileName, int surfaceNum)
      {
        FileData fData = new FileData();
        byte[] byteP = new byte[3];
        bool interlaced = false;
        if (!FileIO.LoadFile(fileName, fData))
          return;
        FileIO.StrCopy(ref GraphicsSystem.gfxSurface[surfaceNum].fileName, ref fileName);
        FileIO.SetFilePosition(6U);
        byteP[0] = FileIO.ReadByte();
        int num1 = (int) byteP[0];
        byteP[0] = FileIO.ReadByte();
        int width = num1 + ((int) byteP[0] << 8);
        byteP[0] = FileIO.ReadByte();
        int num2 = (int) byteP[0];
        byteP[0] = FileIO.ReadByte();
        int height = num2 + ((int) byteP[0] << 8);
        byteP[0] = FileIO.ReadByte();
        byteP[0] = FileIO.ReadByte();
        byteP[0] = FileIO.ReadByte();
        for (int index = 0; index < 256 /*0x0100*/; ++index)
          FileIO.ReadByteArray(ref byteP, 3);
        byteP[0] = FileIO.ReadByte();
        while (byteP[0] != (byte) 44)
          byteP[0] = FileIO.ReadByte();
        if (byteP[0] == (byte) 44)
        {
          FileIO.ReadByteArray(ref byteP, 2);
          FileIO.ReadByteArray(ref byteP, 2);
          FileIO.ReadByteArray(ref byteP, 2);
          FileIO.ReadByteArray(ref byteP, 2);
          byteP[0] = FileIO.ReadByte();
          if (((int) byteP[0] & 64 /*0x40*/) >> 6 == 1)
            interlaced = true;
          if (((int) byteP[0] & 128 /*0x80*/) >> 7 == 1)
          {
            for (int index = 128 /*0x80*/; index < 256 /*0x0100*/; ++index)
              FileIO.ReadByteArray(ref byteP, 3);
          }
          GraphicsSystem.gfxSurface[surfaceNum].width = width;
          GraphicsSystem.gfxSurface[surfaceNum].height = height;
          GraphicsSystem.gfxSurface[surfaceNum].dataStart = GraphicsSystem.gfxDataPosition;
          GraphicsSystem.gfxDataPosition += (uint) (GraphicsSystem.gfxSurface[surfaceNum].width * GraphicsSystem.gfxSurface[surfaceNum].height);
          if (GraphicsSystem.gfxDataPosition >= 4194304U /*0x400000*/)
            GraphicsSystem.gfxDataPosition = 0U;
          else
            GifLoader.ReadGifPictureData(width, height, interlaced, ref GraphicsSystem.graphicData, (int) GraphicsSystem.gfxSurface[surfaceNum].dataStart);
        }
        FileIO.CloseFile();
      }

      public static void LoadStageGIFFile(int zNumber)
      {
        FileData fData = new FileData();
        byte[] byteP = new byte[3];
        bool interlaced = false;
        if (!FileIO.LoadStageFile("16x16Tiles.gif".ToCharArray(), zNumber, fData))
          return;
        FileIO.SetFilePosition(6U);
        byteP[0] = FileIO.ReadByte();
        int num1 = (int) byteP[0];
        byteP[0] = FileIO.ReadByte();
        int width = num1 + ((int) byteP[0] << 8);
        byteP[0] = FileIO.ReadByte();
        int num2 = (int) byteP[0];
        byteP[0] = FileIO.ReadByte();
        int height = num2 + ((int) byteP[0] << 8);
        byteP[0] = FileIO.ReadByte();
        byteP[0] = FileIO.ReadByte();
        byteP[0] = FileIO.ReadByte();
        for (int index = 128 /*0x80*/; index < 256 /*0x0100*/; ++index)
          FileIO.ReadByteArray(ref byteP, 3);
        for (int index = 128 /*0x80*/; index < 256 /*0x0100*/; ++index)
        {
          FileIO.ReadByteArray(ref byteP, 3);
          GraphicsSystem.tilePalette[index].red = byteP[0];
          GraphicsSystem.tilePalette[index].green = byteP[1];
          GraphicsSystem.tilePalette[index].blue = byteP[2];
          GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, index] = GraphicsSystem.RGB_16BIT5551(byteP[0], byteP[1], byteP[2], (byte) 1);
        }
        byteP[0] = FileIO.ReadByte();
        if (byteP[0] == (byte) 44)
        {
          FileIO.ReadByteArray(ref byteP, 2);
          FileIO.ReadByteArray(ref byteP, 2);
          FileIO.ReadByteArray(ref byteP, 2);
          FileIO.ReadByteArray(ref byteP, 2);
          byteP[0] = FileIO.ReadByte();
          if (((int) byteP[0] & 64 /*0x40*/) >> 6 == 1)
            interlaced = true;
          if (((int) byteP[0] & 128 /*0x80*/) >> 7 == 1)
          {
            for (int index = 128 /*0x80*/; index < 256 /*0x0100*/; ++index)
              FileIO.ReadByteArray(ref byteP, 3);
          }
          GifLoader.ReadGifPictureData(width, height, interlaced, ref GraphicsSystem.tileGfx, 0);
          byteP[0] = GraphicsSystem.tileGfx[0];
          for (int index = 0; index < 262144 /*0x040000*/; ++index)
          {
            if ((int) GraphicsSystem.tileGfx[index] == (int) byteP[0])
              GraphicsSystem.tileGfx[index] = (byte) 0;
          }
        }
        FileIO.CloseFile();
      }

      public static void Copy16x16Tile(int tDest, int tSource)
      {
        tSource <<= 2;
        tDest <<= 2;
        GraphicsSystem.tileUVArray[tDest] = GraphicsSystem.tileUVArray[tSource];
        GraphicsSystem.tileUVArray[tDest + 1] = GraphicsSystem.tileUVArray[tSource + 1];
        GraphicsSystem.tileUVArray[tDest + 2] = GraphicsSystem.tileUVArray[tSource + 2];
        GraphicsSystem.tileUVArray[tDest + 3] = GraphicsSystem.tileUVArray[tSource + 3];
      }

      public static void ClearScreen(byte clearColour)
      {
        GraphicsSystem.gfxVertexSize = (ushort) 0;
        GraphicsSystem.gfxIndexSize = (ushort) 0;
        
        Debug.Log($"ClearScreen called with color {clearColour}");
        
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = 0.0f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = 0.0f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = GraphicsSystem.tilePalette[(int) clearColour].red;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = GraphicsSystem.tilePalette[(int) clearColour].green;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = GraphicsSystem.tilePalette[(int) clearColour].blue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = 0.0f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = 0.0f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (GlobalAppDefinitions.SCREEN_XSIZE << 4);
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = 0.0f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = GraphicsSystem.tilePalette[(int) clearColour].red;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = GraphicsSystem.tilePalette[(int) clearColour].green;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = GraphicsSystem.tilePalette[(int) clearColour].blue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = 0.0f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = 0.0f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = 0.0f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = 3840f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = GraphicsSystem.tilePalette[(int) clearColour].red;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = GraphicsSystem.tilePalette[(int) clearColour].green;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = GraphicsSystem.tilePalette[(int) clearColour].blue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = 0.0f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = 0.0f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (GlobalAppDefinitions.SCREEN_XSIZE << 4);
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = 3840f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = GraphicsSystem.tilePalette[(int) clearColour].red;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = GraphicsSystem.tilePalette[(int) clearColour].green;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = GraphicsSystem.tilePalette[(int) clearColour].blue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = 0.0f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = 0.0f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxIndexSize += (ushort) 2;
      }

      public static void DrawSprite(
        int xPos,
        int yPos,
        int xSize,
        int ySize,
        int xBegin,
        int yBegin,
        int surfaceNum)
      {
        if (GraphicsSystem.gfxSurface[surfaceNum].texStartX <= -1 || GraphicsSystem.gfxVertexSize >= (ushort) 8192 /*0x2000*/ || xPos <= -100 || xPos >= 1380 || yPos <= -100 || yPos >= 820)
          return;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) xPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) yPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos + xSize);
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) yPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) xPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos + ySize);
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].position.x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.y;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxIndexSize += (ushort) 2;
      }

      public static void DrawSpriteFlipped(
        int xPos,
        int yPos,
        int xSize,
        int ySize,
        int xBegin,
        int yBegin,
        int direction,
        int surfaceNum)
      {
        if (GraphicsSystem.gfxSurface[surfaceNum].texStartX <= -1 || GraphicsSystem.gfxVertexSize >= (ushort) 8192 /*0x2000*/ || xPos <= -100 || xPos >= 1380 || yPos <= -100 || yPos >= 820)
          return;
        switch (direction)
        {
          case 0:
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) xPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) yPos;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
            ++GraphicsSystem.gfxVertexSize;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos + xSize << 4);
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos << 4);
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
            ++GraphicsSystem.gfxVertexSize;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos << 4);
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos + ySize << 4);
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
            ++GraphicsSystem.gfxVertexSize;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].position.x;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.y;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
            ++GraphicsSystem.gfxVertexSize;
            break;
          case 1:
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) xPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) yPos;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
            ++GraphicsSystem.gfxVertexSize;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos + xSize << 4);
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos << 4);
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
            ++GraphicsSystem.gfxVertexSize;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos << 4);
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos + ySize << 4);
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
            ++GraphicsSystem.gfxVertexSize;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].position.x;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.y;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
            ++GraphicsSystem.gfxVertexSize;
            break;
          case 2:
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) xPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) yPos;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
            ++GraphicsSystem.gfxVertexSize;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos + xSize << 4);
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos << 4);
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
            ++GraphicsSystem.gfxVertexSize;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos << 4);
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos + ySize << 4);
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
            ++GraphicsSystem.gfxVertexSize;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].position.x;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.y;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
            ++GraphicsSystem.gfxVertexSize;
            break;
          case 3:
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) xPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) yPos;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
            ++GraphicsSystem.gfxVertexSize;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos + xSize << 4);
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos << 4);
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
            ++GraphicsSystem.gfxVertexSize;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos << 4);
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos + ySize << 4);
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
            ++GraphicsSystem.gfxVertexSize;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].position.x;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.y;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
            GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
            ++GraphicsSystem.gfxVertexSize;
            break;
        }
        GraphicsSystem.gfxIndexSize += (ushort) 2;
      }

      public static void DrawBlendedSprite(
        int xPos,
        int yPos,
        int xSize,
        int ySize,
        int xBegin,
        int yBegin,
        int surfaceNum)
      {
        if (GraphicsSystem.gfxSurface[surfaceNum].texStartX <= -1 || GraphicsSystem.gfxVertexSize >= (ushort) 8192 /*0x2000*/ || xPos <= -100 || xPos >= 1380 || yPos <= -100 || yPos >= 820)
          return;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) xPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) yPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = (byte) 128 /*0x80*/;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos + xSize);
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) yPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = (byte) 128 /*0x80*/;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) xPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos + ySize);
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = (byte) 128 /*0x80*/;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].position.x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.y;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = (byte) 128 /*0x80*/;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxIndexSize += (ushort) 2;
      }

      public static void DrawAlphaBlendedSprite(
        int xPos,
        int yPos,
        int xSize,
        int ySize,
        int xBegin,
        int yBegin,
        int alpha,
        int surfaceNum)
      {
        if (GraphicsSystem.gfxSurface[surfaceNum].texStartX <= -1 || GraphicsSystem.gfxVertexSize >= (ushort) 8192 /*0x2000*/ || xPos <= -100 || xPos >= 1380 || yPos <= -100 || yPos >= 820)
          return;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) xPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) yPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = (byte) alpha;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos + xSize);
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) yPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = (byte) alpha;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) xPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos + ySize);
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = (byte) alpha;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].position.x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.y;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = (byte) alpha;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxIndexSize += (ushort) 2;
      }

      public static void DrawAdditiveBlendedSprite(
        int xPos,
        int yPos,
        int xSize,
        int ySize,
        int xBegin,
        int yBegin,
        int alpha,
        int surfaceNum)
      {
        if (GraphicsSystem.gfxSurface[surfaceNum].texStartX <= -1 || GraphicsSystem.gfxVertexSize >= (ushort) 8192 /*0x2000*/ || xPos <= -100 || xPos >= 1380 || yPos <= -100 || yPos >= 820)
          return;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) xPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) yPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = (byte) alpha;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos + xSize);
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) yPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = (byte) alpha;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) xPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos + ySize);
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = (byte) alpha;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].position.x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.y;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = (byte) alpha;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxIndexSize += (ushort) 2;
      }

      public static void DrawSubtractiveBlendedSprite(
        int xPos,
        int yPos,
        int xSize,
        int ySize,
        int xBegin,
        int yBegin,
        int alpha,
        int surfaceNum)
      {
        if (GraphicsSystem.gfxSurface[surfaceNum].texStartX <= -1 || GraphicsSystem.gfxVertexSize >= (ushort) 8192 /*0x2000*/ || xPos <= -100 || xPos >= 1380 || yPos <= -100 || yPos >= 820)
          return;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) xPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) yPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = (byte) alpha;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos + xSize);
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) yPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = (byte) alpha;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) xPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos + ySize);
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = (byte) alpha;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].position.x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.y;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = (byte) alpha;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxIndexSize += (ushort) 2;
      }

      public static void DrawRectangle(
        int xPos,
        int yPos,
        int xSize,
        int ySize,
        int r,
        int g,
        int b,
        int alpha)
      {
        if (alpha > (int) byte.MaxValue)
          alpha = (int) byte.MaxValue;
        if (GraphicsSystem.gfxVertexSize >= (ushort) 8192 /*0x2000*/)
          return;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) xPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) yPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = (byte) r;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = (byte) g;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = (byte) b;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = (byte) alpha;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = 0.0f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = 0.0f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos + xSize);
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) yPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = (byte) r;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = (byte) g;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = (byte) b;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = (byte) alpha;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = 0.01f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) xPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos + ySize);
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = (byte) r;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = (byte) g;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = (byte) b;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = (byte) alpha;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = 0.0f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = 0.01f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].position.x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.y;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = (byte) r;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = (byte) g;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = (byte) b;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = (byte) alpha;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = 0.01f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = 0.01f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxIndexSize += (ushort) 2;
      }

      public static void DrawTintRectangle(int xPos, int yPos, int xSize, int ySize)
      {
      }

      public static void DrawTintSpriteMask(
        int xPos,
        int yPos,
        int xSize,
        int ySize,
        int xBegin,
        int yBegin,
        int tableNo,
        int surfaceNum)
      {
      }

      public static void DrawScaledTintMask(
        byte direction,
        int xPos,
        int yPos,
        int xPivot,
        int yPivot,
        int xScale,
        int yScale,
        int xSize,
        int ySize,
        int xBegin,
        int yBegin,
        int surfaceNum)
      {
      }

      public static void DrawScaledSprite(
        byte direction,
        int xPos,
        int yPos,
        int xPivot,
        int yPivot,
        int xScale,
        int yScale,
        int xSize,
        int ySize,
        int xBegin,
        int yBegin,
        int surfaceNum)
      {
        if (GraphicsSystem.gfxVertexSize >= (ushort) 8192 /*0x2000*/ || xPos <= -100 || xPos >= 1380 || yPos <= -100 || yPos >= 820)
          return;
        xScale <<= 2;
        yScale <<= 2;
        xPos -= xPivot * xScale >> 11;
        xScale = xSize * xScale >> 11;
        yPos -= yPivot * yScale >> 11;
        yScale = ySize * yScale >> 11;
        if (GraphicsSystem.gfxSurface[surfaceNum].texStartX <= -1)
          return;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) xPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) yPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos + xScale);
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) yPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) xPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos + yScale);
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].position.x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.y;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxIndexSize += (ushort) 2;
      }

      public static void DrawScaledChar(
        byte direction,
        int xPos,
        int yPos,
        int xPivot,
        int yPivot,
        int xScale,
        int yScale,
        int xSize,
        int ySize,
        int xBegin,
        int yBegin,
        int surfaceNum)
      {
        if (GraphicsSystem.gfxVertexSize >= (ushort) 8192 /*0x2000*/ || xPos <= -2000 || xPos >= 3280 || yPos <= -200 || yPos >= 920)
          return;
        xPos -= xPivot * xScale >> 5;
        xScale = xSize * xScale >> 5;
        yPos -= yPivot * yScale >> 5;
        yScale = ySize * yScale >> 5;
        if (GraphicsSystem.gfxSurface[surfaceNum].texStartX <= -1 || GraphicsSystem.gfxVertexSize >= (ushort) 4096 /*0x1000*/)
          return;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) xPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) yPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos + xScale);
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) yPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) xPos;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos + yScale);
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].position.x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].position.y;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxIndexSize += (ushort) 2;
      }

      public static void DrawRotatedSprite(
        byte direction,
        int xPos,
        int yPos,
        int xPivot,
        int yPivot,
        int xBegin,
        int yBegin,
        int xSize,
        int ySize,
        int rotAngle,
        int surfaceNum)
      {
        xPos <<= 4;
        yPos <<= 4;
        rotAngle -= rotAngle >> 9 << 9;
        if (rotAngle < 0)
          rotAngle += 512 /*0x0200*/;
        if (rotAngle != 0)
          rotAngle = 512 /*0x0200*/ - rotAngle;
        int num1 = GlobalAppDefinitions.SinValue512[rotAngle];
        int num2 = GlobalAppDefinitions.CosValue512[rotAngle];
        if (GraphicsSystem.gfxSurface[surfaceNum].texStartX <= -1 || GraphicsSystem.gfxVertexSize >= (ushort) 8192 /*0x2000*/ || xPos <= -2000 || xPos >= 3280 || yPos <= -2000 || yPos >= 2720)
          return;
        if (direction == (byte) 0)
        {
          int num3 = -xPivot;
          int num4 = -yPivot;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos + (num3 * num2 + num4 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos + (num4 * num2 - num3 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
          ++GraphicsSystem.gfxVertexSize;
          int num5 = xSize - xPivot;
          int num6 = -yPivot;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos + (num5 * num2 + num6 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos + (num6 * num2 - num5 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
          ++GraphicsSystem.gfxVertexSize;
          int num7 = -xPivot;
          int num8 = ySize - yPivot;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos + (num7 * num2 + num8 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos + (num8 * num2 - num7 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
          ++GraphicsSystem.gfxVertexSize;
          int num9 = xSize - xPivot;
          int num10 = ySize - yPivot;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos + (num9 * num2 + num10 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos + (num10 * num2 - num9 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
          ++GraphicsSystem.gfxVertexSize;
          GraphicsSystem.gfxIndexSize += (ushort) 2;
        }
        else
        {
          int num11 = xPivot;
          int num12 = -yPivot;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos + (num11 * num2 + num12 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos + (num12 * num2 - num11 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
          ++GraphicsSystem.gfxVertexSize;
          int num13 = xPivot - xSize;
          int num14 = -yPivot;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos + (num13 * num2 + num14 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos + (num14 * num2 - num13 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
          ++GraphicsSystem.gfxVertexSize;
          int num15 = xPivot;
          int num16 = ySize - yPivot;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos + (num15 * num2 + num16 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos + (num16 * num2 - num15 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
          ++GraphicsSystem.gfxVertexSize;
          int num17 = xPivot - xSize;
          int num18 = ySize - yPivot;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos + (num17 * num2 + num18 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos + (num18 * num2 - num17 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
          ++GraphicsSystem.gfxVertexSize;
          GraphicsSystem.gfxIndexSize += (ushort) 2;
        }
      }

      public static void DrawRotoZoomSprite(
        byte direction,
        int xPos,
        int yPos,
        int xPivot,
        int yPivot,
        int xBegin,
        int yBegin,
        int xSize,
        int ySize,
        int rotAngle,
        int scale,
        int surfaceNum)
      {
        xPos <<= 4;
        yPos <<= 4;
        rotAngle -= rotAngle >> 9 << 9;
        if (rotAngle < 0)
          rotAngle += 512 /*0x0200*/;
        if (rotAngle != 0)
          rotAngle = 512 /*0x0200*/ - rotAngle;
        int num1 = GlobalAppDefinitions.SinValue512[rotAngle] * scale >> 9;
        int num2 = GlobalAppDefinitions.CosValue512[rotAngle] * scale >> 9;
        if (GraphicsSystem.gfxSurface[surfaceNum].texStartX <= -1 || GraphicsSystem.gfxVertexSize >= (ushort) 8192 /*0x2000*/ || xPos <= -2000 || xPos >= 3280 || yPos <= -2000 || yPos >= 2720)
          return;
        if (direction == (byte) 0)
        {
          int num3 = -xPivot;
          int num4 = -yPivot;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos + (num3 * num2 + num4 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos + (num4 * num2 - num3 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
          ++GraphicsSystem.gfxVertexSize;
          int num5 = xSize - xPivot;
          int num6 = -yPivot;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos + (num5 * num2 + num6 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos + (num6 * num2 - num5 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
          ++GraphicsSystem.gfxVertexSize;
          int num7 = -xPivot;
          int num8 = ySize - yPivot;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos + (num7 * num2 + num8 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos + (num8 * num2 - num7 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
          ++GraphicsSystem.gfxVertexSize;
          int num9 = xSize - xPivot;
          int num10 = ySize - yPivot;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos + (num9 * num2 + num10 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos + (num10 * num2 - num9 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
          ++GraphicsSystem.gfxVertexSize;
          GraphicsSystem.gfxIndexSize += (ushort) 2;
        }
        else
        {
          int num11 = xPivot;
          int num12 = -yPivot;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos + (num11 * num2 + num12 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos + (num12 * num2 - num11 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
          ++GraphicsSystem.gfxVertexSize;
          int num13 = xPivot - xSize;
          int num14 = -yPivot;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos + (num13 * num2 + num14 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos + (num14 * num2 - num13 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
          ++GraphicsSystem.gfxVertexSize;
          int num15 = xPivot;
          int num16 = ySize - yPivot;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos + (num15 * num2 + num16 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos + (num16 * num2 - num15 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
          ++GraphicsSystem.gfxVertexSize;
          int num17 = xPivot - xSize;
          int num18 = ySize - yPivot;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) (xPos + (num17 * num2 + num18 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) (yPos + (num18 * num2 - num17 * num1 >> 5));
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 2].texCoord.x;
          GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].texCoord.y;
          ++GraphicsSystem.gfxVertexSize;
          GraphicsSystem.gfxIndexSize += (ushort) 2;
        }
      }

      public static void DrawQuad(Quad2D face, int rgbVal)
      {
        if (GraphicsSystem.gfxVertexSize >= (ushort) 8192 /*0x2000*/)
          return;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) face.vertex[0].x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) face.vertex[0].y;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = (byte) (rgbVal >> 16 /*0x10*/ & (int) byte.MaxValue);
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = (byte) (rgbVal >> 8 & (int) byte.MaxValue);
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = (byte) (rgbVal & (int) byte.MaxValue);
        rgbVal = (rgbVal & 2130706432 /*0x7F000000*/) >> 23;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = rgbVal <= 253 ? (byte) rgbVal : byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = 0.01f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = 0.01f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) face.vertex[1].x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) face.vertex[1].y;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].color.r;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].color.g;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].color.b;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].color.a;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = 0.01f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = 0.01f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) face.vertex[2].x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) face.vertex[2].y;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].color.r;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].color.g;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].color.b;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].color.a;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = 0.01f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = 0.01f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) face.vertex[3].x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) face.vertex[3].y;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].color.r;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].color.g;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].color.b;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize - 1].color.a;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = 0.01f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = 0.01f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxIndexSize += (ushort) 2;
      }

      public static void DrawTexturedQuad(Quad2D face, int surfaceNum)
      {
        if (GraphicsSystem.gfxVertexSize >= (ushort) 8192 /*0x2000*/)
          return;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) face.vertex[0].x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) face.vertex[0].y;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + face.vertex[0].u) * 0.0009765625f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + face.vertex[0].v) * 0.0009765625f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) face.vertex[1].x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) face.vertex[1].y;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + face.vertex[1].u) * 0.0009765625f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + face.vertex[1].v) * 0.0009765625f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) face.vertex[2].x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) face.vertex[2].y;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + face.vertex[2].u) * 0.0009765625f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + face.vertex[2].v) * 0.0009765625f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.x = (float) face.vertex[3].x;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].position.y = (float) face.vertex[3].y;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.r = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.g = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.b = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].color.a = byte.MaxValue;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.x = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartX + face.vertex[3].u) * 0.0009765625f;
        GraphicsSystem.gfxPolyList[(int) GraphicsSystem.gfxVertexSize].texCoord.y = (float) (GraphicsSystem.gfxSurface[surfaceNum].texStartY + face.vertex[3].v) * 0.0009765625f;
        ++GraphicsSystem.gfxVertexSize;
        GraphicsSystem.gfxIndexSize += (ushort) 2;
      }

      public static void SetPaletteEntry(byte entryPos, byte cR, byte cG, byte cB)
      {
        GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int) entryPos] = entryPos <= (byte) 0 ? GraphicsSystem.RGB_16BIT5551(cR, cG, cB, (byte) 0) : GraphicsSystem.RGB_16BIT5551(cR, cG, cB, (byte) 1);
        GraphicsSystem.tilePalette[(int) entryPos].red = cR;
        GraphicsSystem.tilePalette[(int) entryPos].green = cG;
        GraphicsSystem.tilePalette[(int) entryPos].blue = cB;
      }

      public static void SetFade(byte clrR, byte clrG, byte clrB, ushort clrA)
      {
        GraphicsSystem.fadeMode = (byte) 1;
        if (clrA > (ushort) byte.MaxValue)
          clrA = (ushort) byte.MaxValue;
        GraphicsSystem.fadeR = clrR;
        GraphicsSystem.fadeG = clrG;
        GraphicsSystem.fadeB = clrB;
        GraphicsSystem.fadeA = (byte) clrA;
      }

      public static void SetLimitedFade(
        byte paletteNum,
        byte clrR,
        byte clrG,
        byte clrB,
        ushort clrA,
        int fStart,
        int fEnd)
      {
        byte[] numArray = new byte[3];
        GraphicsSystem.paletteMode = paletteNum;
        if (paletteNum >= (byte) 8)
          return;
        if (clrA > (ushort) byte.MaxValue)
          clrA = (ushort) byte.MaxValue;
        if (fEnd < 256 /*0x0100*/)
          ++fEnd;
        for (int index = fStart; index < fEnd; ++index)
        {
          numArray[0] = (byte) ((int) GraphicsSystem.tilePalette[index].red * ((int) byte.MaxValue - (int) clrA) + (int) clrA * (int) clrR >> 8);
          numArray[1] = (byte) ((int) GraphicsSystem.tilePalette[index].green * ((int) byte.MaxValue - (int) clrA) + (int) clrA * (int) clrG >> 8);
          numArray[2] = (byte) ((int) GraphicsSystem.tilePalette[index].blue * ((int) byte.MaxValue - (int) clrA) + (int) clrA * (int) clrB >> 8);
          GraphicsSystem.tilePalette16_Data[0, index] = GraphicsSystem.RGB_16BIT5551(numArray[0], numArray[1], numArray[2], (byte) 1);
        }
        GraphicsSystem.tilePalette16_Data[0, 0] = GraphicsSystem.RGB_16BIT5551(numArray[0], numArray[1], numArray[2], (byte) 0);
      }

      public static void SetActivePalette(byte paletteNum, int minY, int maxY)
      {
        if (paletteNum >= (byte) 8)
          return;
        GraphicsSystem.texPaletteNum = (int) paletteNum;
      }

      public static void CopyPalette(byte paletteSource, byte paletteDest)
      {
        if (paletteSource >= (byte) 8 || paletteDest >= (byte) 8)
          return;
        for (int index = 0; index < 256 /*0x0100*/; ++index)
          GraphicsSystem.tilePalette16_Data[(int) paletteDest, index] = GraphicsSystem.tilePalette16_Data[(int) paletteSource, index];
      }

      public static void RotatePalette(byte pStart, byte pEnd, byte pDirection)
      {
        switch (pDirection)
        {
          case 0:
            ushort num1 = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int) pStart];
            for (byte index = pStart; (int) index < (int) pEnd; ++index)
              GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int) index] = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int) index + 1];
            GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int) pEnd] = num1;
            break;
          case 1:
            ushort num2 = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int) pEnd];
            for (byte index = pEnd; (int) index > (int) pStart; --index)
              GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int) index] = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int) index - 1];
            GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int) pStart] = num2;
            break;
        }
      }

      public static void GenerateBlendLookupTable()
      {
        int index1 = 0;
        for (int index2 = 0; index2 < 256 /*0x0100*/; ++index2)
        {
          for (int index3 = 0; index3 < 32 /*0x20*/; ++index3)
          {
            GraphicsSystem.blendLookupTable[index1] = (ushort) (index3 * index2 >> 8);
            GraphicsSystem.subtractiveLookupTable[index1] = (ushort) ((31 /*0x1F*/ - index3) * index2 >> 8);
            ++index1;
          }
        }
      }
    }
}