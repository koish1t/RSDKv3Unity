using UnityEngine;

namespace Retro_Engine
{
    [System.Serializable]
    public struct DrawVertex
    {
        public Vector2 position;
        public Vector2 texCoord;
        public Color color;

        public DrawVertex(Vector2 position, Vector2 texCoord, Color color)
        {
            this.position = position;
            this.texCoord = texCoord;
            this.color = color;
        }

        public Vector3 ToVector3(float z = 0f)
        {
            return new Vector3(position.x, position.y, z);
        }
    }
}
