namespace Retro_Engine
{

    public class LineScrollParallax
    {
      public int[] parallaxFactor = new int[256 /*0x0100*/];
      public int[] scrollSpeed = new int[256 /*0x0100*/];
      public int[] scrollPosition = new int[256 /*0x0100*/];
      public int[] linePos = new int[256 /*0x0100*/];
      public byte[] deformationEnabled = new byte[256 /*0x0100*/];
      public byte numEntries;
    }
}