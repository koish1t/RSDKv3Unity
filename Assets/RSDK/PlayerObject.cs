
namespace Retro_Engine
{

    public class PlayerObject
    {
      public int objectNum;
      public int xPos;
      public int yPos;
      public int xVelocity;
      public int yVelocity;
      public int speed;
      public int screenXPos;
      public int screenYPos;
      public int angle;
      public int timer;
      public int lookPos;
      public int[] value = new int[8];
      public byte collisionMode;
      public byte skidding;
      public byte pushing;
      public byte collisionPlane;
      public sbyte controlMode;
      public byte controlLock;
      public PlayerStatistics movementStats = new PlayerStatistics();
      public byte visible;
      public byte tileCollisions;
      public byte objectInteraction;
      public byte left;
      public byte right;
      public byte up;
      public byte down;
      public byte jumpPress;
      public byte jumpHold;
      public byte followPlayer1;
      public byte trackScroll;
      public byte gravity;
      public byte water;
      public byte[] flailing = new byte[3];
      public AnimationFileList animationFile;
      public ObjectEntity objectPtr;
    }
}