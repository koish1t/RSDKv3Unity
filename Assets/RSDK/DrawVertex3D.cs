using UnityEngine;

namespace Retro_Engine
{
    public struct DrawVertex3D
    {
        public Vector3 position;
        public Vector2 texCoord;
        public Color color;

        public DrawVertex3D(Vector3 position, Vector2 texCoord, Color color)
        {
            this.position = position;
            this.texCoord = texCoord;
            this.color = color;
        }
    }
}
