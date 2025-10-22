namespace Retro_Engine
{

    public class TextMenu
    {
      public char[] textData = new char[10240];
      public int[] entryStart = new int[512 /*0x0200*/];
      public int[] entrySize = new int[512 /*0x0200*/];
      public byte[] entryHighlight = new byte[512 /*0x0200*/];
      public int textDataPos;
      public int selection1;
      public int selection2;
      public ushort numRows;
      public ushort numVisibleRows;
      public ushort visibleRowOffset;
      public byte alignment;
      public byte numSelections;
      public sbyte timer;
    }
}