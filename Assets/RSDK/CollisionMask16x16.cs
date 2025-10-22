namespace Retro_Engine
{
    public class CollisionMask16x16
    {
      public sbyte[] floorMask = new sbyte[16384];
      public sbyte[] leftWallMask = new sbyte[16384];
      public sbyte[] rightWallMask = new sbyte[16384];
      public sbyte[] roofMask = new sbyte[16384];
      public uint[] angle = new uint[1024];
      public byte[] flags = new byte[1024];
    }
}