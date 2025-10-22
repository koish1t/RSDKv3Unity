namespace Retro_Engine
{

    public class GfxSurfaceDesc
    {
      public char[] fileName = new char[64 /*0x40*/];
      public int width;
      public int height;
      public int texStartX;
      public int texStartY;
      public uint dataStart;
    }
}