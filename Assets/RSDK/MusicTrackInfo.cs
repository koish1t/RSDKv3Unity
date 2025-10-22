namespace Retro_Engine
{

    public class MusicTrackInfo
    {
      public char[] trackName = new char[64 /*0x40*/];
      public bool loop;
      public uint loopPoint;
    }
}