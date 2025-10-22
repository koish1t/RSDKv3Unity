namespace Retro_Engine
{

    public class Quad2D
    {
      public Vertex2D[] vertex = new Vertex2D[4];

      public Quad2D()
      {
        for (int index = 0; index < 4; ++index)
          this.vertex[index] = new Vertex2D();
      }
    }
}