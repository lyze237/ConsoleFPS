using System;
using Microsoft.Xna.Framework;

namespace ConsoleFPS
{
    public class PlayerEntity
    {
        public Vector2 Position { get; set; }
        public float Angle { get; set; }
        public float FOV { get; set; } = (float) Math.PI / 4;
            
        public float Speed { get; set; }= 5.0f;

        public PlayerEntity(float x, float y, float angle = 0)
        {
            Position = new Vector2(x, y);
            Angle = angle;
        }
    }
}