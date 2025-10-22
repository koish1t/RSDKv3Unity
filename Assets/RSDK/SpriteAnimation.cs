namespace Retro_Engine
{

    public class SpriteAnimation
    {
      public char[] name = new char[16 /*0x10*/];
      public byte numFrames;
      public byte animationSpeed;
      public byte loopPosition;
      public byte rotationFlag;
      public int frameListOffset;
    }
}