namespace Retro_Engine
{

    public class InputResult
    {
      public byte up;
      public byte down;
      public byte left;
      public byte right;
      public byte buttonA;
      public byte buttonB;
      public byte buttonC;
      public byte start;
      public byte[] touchDown = new byte[4];
      public int[] touchX = new int[4];
      public int[] touchY = new int[4];
      public int[] touchID = new int[4];
      public int touches;
    }
}