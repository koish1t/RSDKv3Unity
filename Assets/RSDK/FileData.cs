namespace Retro_Engine
{

    public class FileData
    {
      public char[] fileName = new char[64];
      public uint fileSize;
      public uint position;
      public uint bufferPos;
      public uint virtualFileOffset;
      public byte eStringPosA;
      public byte eStringPosB;
      public byte eStringNo;
      public bool eNybbleSwap;
    }
}