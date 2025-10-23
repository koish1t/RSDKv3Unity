using UnityEngine;

namespace Retro_Engine
{

    public static class RenderDevice
    {
      public const int NUM_TEXTURES = 6;
      private static Material renderMaterial;
      private static Mesh renderMesh;
      private static Texture2D[] gfxTexture = new Texture2D[6];
      public static int orthWidth;
      public static int viewWidth;
      public static int viewHeight;
      public static float viewAspect;
      public static int bufferWidth;
      public static int bufferHeight;
      public static int highResMode = 0;
      public static bool useFBTexture = true;
      private static Matrix4x4 projection2D;
      private static Matrix4x4 projection3D;
      private static Rect screenRect;

        public static void InitRenderDevice()
      {
        GraphicsSystem.SetupPolygonLists();
        GraphicsSystem.InitializeRenderTexture();
        
        renderMaterial = new Material(Shader.Find("Unlit/Transparent"));
        
        for (int index = 0; index < 6; ++index)
        {
          gfxTexture[index] = new Texture2D(1024, 1024, TextureFormat.RGBA4444, false);
          gfxTexture[index].filterMode = FilterMode.Point;
        }
        
        CreateRenderMesh();
      }
      
      private static void CreateRenderMesh()
      {
        renderMesh = new Mesh();
        renderMesh.name = "RetroEngineRenderMesh";
        
        Vector3[] vertices = new Vector3[8192];
        Vector2[] uvs = new Vector2[8192];
        Color32[] colors = new Color32[8192];
        int[] triangles = new int[49152];
        
        renderMesh.vertices = vertices;
        renderMesh.uv = uvs;
        renderMesh.colors32 = colors;
        renderMesh.triangles = triangles;
      }

      public static void UpdateHardwareTextures()
      {
        if (gfxTexture[0] == null)
        {
          Debug.LogWarning("UpdateHardwareTextures called before InitRenderDevice. Skipping texture update.");
          return;
        }
        
        GraphicsSystem.SetActivePalette((byte) 0, 0, 720);
        GraphicsSystem.UpdateTextureBufferWithTiles();
        GraphicsSystem.UpdateTextureBufferWithSortedSprites();
        
        Color32[] colors = new Color32[GraphicsSystem.texBuffer.Length];
        for (int i = 0; i < GraphicsSystem.texBuffer.Length; i++)
        {
          ushort pixel = GraphicsSystem.texBuffer[i];
          byte r = (byte)((pixel & 0x7C00) >> 10 << 3);
          byte g = (byte)((pixel & 0x03E0) >> 5 << 3);
          byte b = (byte)((pixel & 0x001F) << 3);
          byte a = (byte)((pixel & 0x8000) != 0 ? 255 : 0);
          colors[i] = new Color32(r, g, b, a);
        }
        
        gfxTexture[0].SetPixels32(colors);
        gfxTexture[0].Apply();
        
        for (byte paletteNum = 1; paletteNum < (byte) 6; ++paletteNum)
        {
          if (gfxTexture[paletteNum] == null)
            continue;
            
          GraphicsSystem.SetActivePalette(paletteNum, 0, 720);
          GraphicsSystem.UpdateTextureBufferWithTiles();
          GraphicsSystem.UpdateTextureBufferWithSprites();
          
          for (int i = 0; i < GraphicsSystem.texBuffer.Length; i++)
          {
            ushort pixel = GraphicsSystem.texBuffer[i];
            byte r = (byte)((pixel & 0x7C00) >> 10 << 3);
            byte g = (byte)((pixel & 0x03E0) >> 5 << 3);
            byte b = (byte)((pixel & 0x001F) << 3);
            byte a = (byte)((pixel & 0x8000) != 0 ? 255 : 0);
            colors[i] = new Color32(r, g, b, a);
          }
          
          gfxTexture[paletteNum].SetPixels32(colors);
          gfxTexture[paletteNum].Apply();
        }
        
        GraphicsSystem.SetActivePalette((byte) 0, 0, 720);
      }

      public static void SetScreenDimensions(int width, int height)
      {
        InputSystem.touchWidth = width;
        InputSystem.touchHeight = height;
        RenderDevice.viewWidth = InputSystem.touchWidth;
        RenderDevice.viewHeight = InputSystem.touchHeight;
        RenderDevice.bufferWidth = (int) ((float) RenderDevice.viewWidth / (float) RenderDevice.viewHeight * 240f);
        RenderDevice.bufferWidth += 8;
        RenderDevice.bufferWidth = RenderDevice.bufferWidth >> 4 << 4;
        if (RenderDevice.bufferWidth > 400)
          RenderDevice.bufferWidth = 400;
        RenderDevice.viewAspect = 0.75f;
        GlobalAppDefinitions.HQ3DFloorEnabled = RenderDevice.viewHeight >= 480;
        if (RenderDevice.viewHeight >= 480)
        {
          GraphicsSystem.SetScreenRenderSize(RenderDevice.bufferWidth, RenderDevice.bufferWidth);
          RenderDevice.bufferWidth *= 2;
          RenderDevice.bufferHeight = 480;
        }
        else
        {
          RenderDevice.bufferHeight = 240 /*0xF0*/;
          GraphicsSystem.SetScreenRenderSize(RenderDevice.bufferWidth, RenderDevice.bufferWidth);
        }
        RenderDevice.orthWidth = GlobalAppDefinitions.SCREEN_XSIZE * 16 /*0x10*/;
        RenderDevice.projection2D = Matrix4x4.Ortho(4f, (float)(RenderDevice.orthWidth + 4), 3844f, 4f, 0.0f, 100f);
        RenderDevice.projection3D = Matrix4x4.Perspective(1.83259571f * Mathf.Rad2Deg, RenderDevice.viewAspect, 0.1f, 2000f) * Matrix4x4.Scale(new Vector3(1f, -1f, 1f)) * Matrix4x4.Translate(new Vector3(0.0f, -0.045f, 0.0f));
        RenderDevice.screenRect = new Rect(0f, 0f, RenderDevice.viewWidth, RenderDevice.viewHeight);
      }

      public static void FlipScreen()
      {
        if (GraphicsSystem.renderCamera == null || GraphicsSystem.renderTexture == null)
        {
          Debug.LogWarning("FlipScreen: Missing render camera or texture");
          return;
        }
        
        //Debug.Log($"FlipScreen: gfxVertexSize={GraphicsSystem.gfxVertexSize}, texPaletteNum={GraphicsSystem.texPaletteNum}");
        
        RenderTexture.active = GraphicsSystem.renderTexture;
        
        GL.Clear(true, true, Color.black);
        
        if (renderMaterial != null && gfxTexture[GraphicsSystem.texPaletteNum] != null)
        {
          renderMaterial.mainTexture = gfxTexture[GraphicsSystem.texPaletteNum];
          
          UpdateRenderMesh();
          
          GL.PushMatrix();
          GL.LoadOrtho();
          
          renderMaterial.SetPass(0);
          
          RenderGameVertices();
          
          GL.PopMatrix();
        }
        else
        {
          Debug.LogWarning($"FlipScreen: renderMaterial={renderMaterial != null}, gfxTexture={gfxTexture[GraphicsSystem.texPaletteNum] != null}");
        }
        
        RenderTexture.active = null;
      }
      
      private static void RenderGameVertices()
      {
        if (GraphicsSystem.gfxVertexSize == 0)
        {
          //Debug.Log("RenderGameVertices: No vertices to render");
          return;
        }
          
       //Debug.Log($"RenderGameVertices: Rendering {GraphicsSystem.gfxVertexSize} vertices");
        
        float screenWidth = 1280f;
        float screenHeight = 720f;
        
        GL.Begin(GL.QUADS);
        
        for (int i = 0; i < GraphicsSystem.gfxVertexSize; i += 4)
        {
          if (i + 3 >= GraphicsSystem.gfxVertexSize)
            break;
            
          DrawVertex v0 = GraphicsSystem.gfxPolyList[i];
          DrawVertex v1 = GraphicsSystem.gfxPolyList[i + 1];
          DrawVertex v2 = GraphicsSystem.gfxPolyList[i + 2];
          DrawVertex v3 = GraphicsSystem.gfxPolyList[i + 3];
          
          float x0 = v0.position.x / screenWidth;
          float y0 = 1.0f - (v0.position.y / screenHeight);
          float x1 = v1.position.x / screenWidth;
          float y1 = 1.0f - (v1.position.y / screenHeight);
          float x2 = v2.position.x / screenWidth;
          float y2 = 1.0f - (v2.position.y / screenHeight);
          float x3 = v3.position.x / screenWidth;
          float y3 = 1.0f - (v3.position.y / screenHeight);
          
          GL.Color(v0.color);
          GL.TexCoord2(v0.texCoord.x, v0.texCoord.y);
          GL.Vertex3(x0, y0, 0);
          
          GL.Color(v1.color);
          GL.TexCoord2(v1.texCoord.x, v1.texCoord.y);
          GL.Vertex3(x1, y1, 0);
          
          GL.Color(v2.color);
          GL.TexCoord2(v2.texCoord.x, v2.texCoord.y);
          GL.Vertex3(x2, y2, 0);
          
          GL.Color(v3.color);
          GL.TexCoord2(v3.texCoord.x, v3.texCoord.y);
          GL.Vertex3(x3, y3, 0);
        }
        
        GL.End();
      }

      public static void FlipScreenHRes()
      {
        if (renderMaterial != null && gfxTexture[GraphicsSystem.texPaletteNum] != null)
        {
          renderMaterial.mainTexture = gfxTexture[GraphicsSystem.texPaletteNum];
          renderMaterial.SetFloat("_FilterMode", 1);
          
          UpdateRenderMesh();
          
          Graphics.DrawMesh(renderMesh, Matrix4x4.identity, renderMaterial, 0);
        }
      }
      
      private static void UpdateRenderMesh()
      {
        if (renderMesh == null || GraphicsSystem.gfxVertexSize == 0)
          return;
          
        Vector3[] vertices = new Vector3[GraphicsSystem.gfxVertexSize];
        Vector2[] uvs = new Vector2[GraphicsSystem.gfxVertexSize];
        Color32[] colors = new Color32[GraphicsSystem.gfxVertexSize];
        
        int triangleCount = GraphicsSystem.gfxIndexSize;
        if (triangleCount % 3 != 0)
        {
          triangleCount = (triangleCount / 3) * 3;
        }
        int[] triangles = new int[triangleCount];
        
        for (int i = 0; i < GraphicsSystem.gfxVertexSize; i++)
        {
          DrawVertex dv = GraphicsSystem.gfxPolyList[i];
          vertices[i] = new Vector3(dv.position.x, dv.position.y, 0f);
          uvs[i] = new Vector2(dv.texCoord.x, dv.texCoord.y);
          colors[i] = new Color32((byte)(dv.color.r * 255f), (byte)(dv.color.g * 255f), (byte)(dv.color.b * 255f), (byte)(dv.color.a * 255f));
        }
        
        if (triangleCount % 3 != 0)
        {
          triangleCount = (triangleCount / 3) * 3;
        }
        
        for (int i = 0; i < triangleCount; i++)
        {
          triangles[i] = GraphicsSystem.gfxPolyListIndex[i];
        }
        
        renderMesh.Clear();
        renderMesh.vertices = vertices;
        renderMesh.uv = uvs;
        renderMesh.colors32 = colors;
        renderMesh.triangles = triangles;
        renderMesh.RecalculateNormals();
      }
    }
}