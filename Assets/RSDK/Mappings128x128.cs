namespace Retro_Engine
{

    public class Mappings128x128
    {
      public int[] gfxDataPos = new int[32768 /*0x8000*/];
      public ushort[] tile16x16 = new ushort[32768 /*0x8000*/];
      public byte[] direction = new byte[32768 /*0x8000*/];
      public byte[] visualPlane = new byte[32768 /*0x8000*/];
      public byte[,] collisionFlag = new byte[2, 32768 /*0x8000*/];
    }
}