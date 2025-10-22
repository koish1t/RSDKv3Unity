
namespace Retro_Engine
{

    public class LayoutMap
    {
      public ushort[] tileMap = new ushort[65536 /*0x010000*/];
      public byte[] lineScrollRef = new byte[32768 /*0x8000*/];
      public int parallaxFactor;
      public int scrollSpeed;
      public int scrollPosition;
      public int angle;
      public int xPos;
      public int yPos;
      public int zPos;
      public int deformationPos;
      public int deformationPosW;
      public byte type;
      public byte xSize;
      public byte ySize;
    }
}